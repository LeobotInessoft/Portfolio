/* Written by Leon Botha (started with an ESP Server Example as the basis for the code)
 * This program reads data from either the Serial port or data received by running as a Web Server in order to control an LED panel (made out of just an LED strip).
 * The commands can either be to set the wifi ssid+pwd or, to control the LEDS on an RGB LED strip.
 * If the request is received to update/set the SSID and PWD of the router, then the data is saved in the EPROM permanently; limited writes are allowed, thus suitable for context.
 * The data to control the LED strip is formatted in plain text but the values are encoded as to conserve memory space. Thus a limited number of colours can be displayed and not the ful rgb spectrum. 
 * Depending on what microntroller is used, the MaxPhases is limited by the amount of RAM available.
 * If the serial port is used, then the data is stored in a buffer so that all the phases do not have to be sent in one single go. Only once the deliminter is received is the data processed.
 * Here are examples of the two possible commands that can be received
  //id=0^bright=15^time=50^phase=0^total=0^data=123456789012345678901234567890123456789011100110011001100110022003300110022003300441234567891231231231231230000000000000000000000000000001100110022001111112123123123123123123123123123123123123>
  -Ths sets the brightness to be 15%, the time to display this phase to 50ms, the phase number being set is 0, and the total phases to be switched between is 0, and the data contains the actual pattern/letter/sentence to be diplayed at this phase number
  //ssid=123^pass=123> 
  -This sets the SSID and PWD for the Wifi router and saves it in EPROM
 */

#include <Arduino.h>
#include <ESP8266WebServer.h>
#include "FastLED.h"
#include <ESP8266WiFi.h>
#include <WiFiClient.h>
#include <ESP_EEPROM.h>
#include <ESP8266mDNS.h>
#include <FS.h>        // File System for Web Server Files
#include <LittleFS.h>  // This file system is used.

#include "secrets.h"  // add WLAN Credentials in here.


#define EEPROM_SIZE 512 // size of the EEPROM in bytes
#define SSID_ADDRESS 0 // starting address for SSID in EEPROM
#define PASSWORD_ADDRESS 32 // starting address for password in EEPROM
#define SSID_LENGTH 32 // length of SSID in bytes
#define PASSWORD_LENGTH 64 // length of password in byte

#define HOSTNAME "webserver5"
// mark parameters not used in example
#define UNUSED __attribute__((unused))

// TRACE output simplified, can be deactivated here
#define TRACE(...) Serial.printf(__VA_ARGS__)
// local time zone definition (Berlin)
#define TIMEZONE "CET-1CEST,M3.5.0,M10.5.0/3"
// The text of builtin files are in this header file
#include "builtinfiles.h"



#if FASTLED_VERSION < 3001000
#error "Requires FastLED 3.1 or later; check github for latest code."
#endif
String myID="0";
#define DATA_PIN    D4
//#define CLK_PIN   4
#define LED_TYPE    WS2812
#define COLOR_ORDER GRB
#define NUM_LEDS    500
#define BRIGHTNESS  15

#define TX_PIN 5 // Arduino transmit  YELLOW WIRE  labeled RX on printer
#define RX_PIN 6 // Arduino receive   GREEN WIRE   labeled TX on printer

#define MaxPhases 16

String PindID="LEOBOT LED:"; //PING RESPONSE
char ssid[SSID_LENGTH+1];
char password[PASSWORD_LENGTH+1];
const byte numChars = 225;
char receivedChars[numChars];

boolean newData = false;
int rows=1;
byte  Brightnesses[MaxPhases];
byte  Timers[MaxPhases];


ESP8266WebServer server(80);
CRGB leds[NUM_LEDS];
CRGB displayPhases[ MaxPhases][ NUM_LEDS ];// = 


//public exution variables
long currentTimer=255;
byte currentBrightness=255;
int currentPhase=0;
int maxPhases=0;
String lastInput="";

unsigned long startTime = 0;
unsigned long elapsedTime = 0;


String inp= "";
//test with id=0^bright=55^phase=0^total=5^data=111101111111000111111111001100110011001100110011001100110011002200330011002200330044003300110011001100110022003300110022003300220033004400110011002200110011>
String textToShow="";
int currentIndex;

String currentDataBuffer="";
bool IsNewCommands=false;

const String postForms = "<html>\
  <head>\
    <title>Leobot Electronics</title>\
    <style>\
      body { background-color: #cccccc; font-family: Arial, Helvetica, Sans-Serif; Color: #000088; }\
    </style>\
  </head>\
  <body>\
    <h1>Leobot Electronics/</h1><br>\    
  </body>\
</html>";



// Setup everything to make the webserver work.
void setup(void) {
  
   EEPROM.begin(EEPROM_SIZE);
  pinMode(LED_BUILTIN, OUTPUT);
  delay(3000);  // wait for serial monitor to start completely.
// tell FastLED about the LED strip configuration
  FastLED.addLeds<LED_TYPE,DATA_PIN,COLOR_ORDER>(leds, NUM_LEDS)
    .setCorrection(TypicalLEDStrip)
    .setDither(BRIGHTNESS < 255);

  // set master brightness control
  FastLED.setBrightness(BRIGHTNESS);
  
  // Use Serial port for some trace information from the example
  Serial.begin(115200);
  Serial.println("START");

  Serial.setDebugOutput(false);

  TRACE("Starting WebServer example...\n");

  TRACE("Mounting the filesystem...\n");
  if (!LittleFS.begin()) {
    TRACE("could not mount the filesystem...\n");
    delay(2000);
    ESP.restart();
  }
  readWifiFromEprom();
  if (ssid[0] == '\0' || password[0] == '\0')
  {
    Serial.println("No Eprom ");
  }else
  {
    StartServer()
   
  }
  
}


void loop(void) {
  ListenForSerialData();
  if(ServerHasStarted)
  {
  server.handleClient();
  }
  else
  {
    readWifiFromEprom();
    Serial.println(String(ssid));
    Serial.println(String(password));
    if (ssid[0] != '\0' && password[0] != '\0')
    {
        Serial.println(ssid);
        Serial.println(password);
        Serial.println("Starting WIFI");
    
       
        StartServer();
    }
  }
   FastLED.show();  
   DoTimedLedPhaseSwitch();
}  


void handleRoot() {
   Serial.print("New Request");
 
  server.send(200, "text/html", postForms);
}

// ===== Simple functions used to answer simple GET requests =====

// This function is called when the WebServer was requested without giving a filename.
// This will redirect to the file index.htm when it is existing otherwise to the built-in $upload.htm page
void handleRedirect() {
  TRACE("Redirect...");
  String url = "/index.htm";

  if (!LittleFS.exists(url)) { url = "/$update.htm"; }

  server.sendHeader("Location", url, true);
  server.send(302);
}  // handleRedirect()


// This function is called when the WebServer was requested to list all existing files in the filesystem.
// a JSON array with file information is returned.
void handleListFiles() {
  Dir dir = LittleFS.openDir("/");
  String result;

  result += "[\n";
  while (dir.next()) {
    if (result.length() > 4) { result += ","; }
    result += "  {";
    result += " \"name\": \"" + dir.fileName() + "\", ";
    result += " \"size\": " + String(dir.fileSize()) + ", ";
    result += " \"time\": " + String(dir.fileTime());
    result += " }\n";
    // jc.addProperty("size", dir.fileSize());
  }  // while
  result += "]";
  server.sendHeader("Cache-Control", "no-cache");
  server.send(200, "text/javascript; charset=utf-8", result);
}  // handleListFiles()


// This function is called when the sysInfo service was requested.
void handleSysInfo() {
  String result;

  FSInfo fs_info;
  LittleFS.info(fs_info);

  result += "{\n";
  result += "  \"flashSize\": " + String(ESP.getFlashChipSize()) + ",\n";
  result += "  \"freeHeap\": " + String(ESP.getFreeHeap()) + ",\n";
  result += "  \"fsTotalBytes\": " + String(fs_info.totalBytes) + ",\n";
  result += "  \"fsUsedBytes\": " + String(fs_info.usedBytes) + ",\n";
  result += "}";

  server.sendHeader("Cache-Control", "no-cache");
  server.send(200, "text/javascript; charset=utf-8", result);
}  // handleSysInfo()


// ===== Request Handler class used to answer more complex requests =====

// The FileServerHandler is registered to the web server to support DELETE and UPLOAD of files into the filesystem.
class FileServerHandler : public RequestHandler {
public:
  // @brief Construct a new File Server Handler object
  // @param fs The file system to be used.
  // @param path Path to the root folder in the file system that is used for serving static data down and upload.
  // @param cache_header Cache Header to be used in replies.
  FileServerHandler() {
    TRACE("FileServerHandler is registered\n");
  }


  // @brief check incoming request. Can handle POST for uploads and DELETE.
  // @param requestMethod method of the http request line.
  // @param requestUri request ressource from the http request line.
  // @return true when method can be handled.
  bool canHandle(HTTPMethod requestMethod, const String UNUSED &_uri) override {
    return ((requestMethod == HTTP_POST) || (requestMethod == HTTP_DELETE));
  }  // canHandle()


  bool canUpload(const String &uri) override {
    // only allow upload on root fs level.
    return (uri == "/");
  }  // canUpload()


  bool handle(ESP8266WebServer &server, HTTPMethod requestMethod, const String &requestUri) override {
    // ensure that filename starts with '/'
    String fName = requestUri;
    if (!fName.startsWith("/")) { fName = "/" + fName; }

    if (requestMethod == HTTP_POST) {
      // all done in upload. no other forms.

    } else if (requestMethod == HTTP_DELETE) {
      if (LittleFS.exists(fName)) { LittleFS.remove(fName); }
    }  // if

    server.send(200);  // all done.
    return (true);
  }  // handle()


  // uploading process
  void upload(ESP8266WebServer UNUSED &server, const String UNUSED &_requestUri, HTTPUpload &upload) override {
    // ensure that filename starts with '/'
    String fName = upload.filename;
    if (!fName.startsWith("/")) { fName = "/" + fName; }

    if (upload.status == UPLOAD_FILE_START) {
      // Open the file
      if (LittleFS.exists(fName)) { LittleFS.remove(fName); }  // if
      _fsUploadFile = LittleFS.open(fName, "w");

    } else if (upload.status == UPLOAD_FILE_WRITE) {
      // Write received bytes
      if (_fsUploadFile) { _fsUploadFile.write(upload.buf, upload.currentSize); }

    } else if (upload.status == UPLOAD_FILE_END) {
      // Close the file
      if (_fsUploadFile) { _fsUploadFile.close(); }
    }  // if
  }    // upload()

protected:
  File _fsUploadFile;
};


  // setup
bool ServerHasStarted=false;
bool StartServer()
{ 
  
  //ESP.restart();
  bool isConnected=false;
  Serial.println(ssid);
  Serial.println(password);
  // start WiFI
  
  WiFi.mode(WIFI_STA);
  if (strlen(ssid) == 0) {
    WiFi.begin();
  } else {
    WiFi.begin(ssid, password);
  }

  // allow to address the device by the given name e.g. http://webserver
  WiFi.setHostname(HOSTNAME);
  TRACE("Connect to WiFi...\n");
  unsigned long startTimeWifi = millis(); // Get the current time in milliseconds
  bool ledPinVal=1;

  while (WiFi.status() != WL_CONNECTED && millis() - startTimeWifi < 150000) {
    ListenForSerialData();
    delay(5);
    TRACE(".");
    digitalWrite(LED_BUILTIN, ledPinVal);
    ledPinVal= !ledPinVal;
  }
   digitalWrite(LED_BUILTIN, HIGH);
   
  if(WiFi.status() != WL_CONNECTED)
  {
  TRACE("NOT CONNECTED.\n");
  }else
  {
    TRACE("connected.\n");
    isConnected=true;
  }
  Serial.print("IP addressxx: ");
  Serial.println(WiFi.localIP());
  textToShow=( WiFi.localIP().toString());
  textToShow=String("0123456789.")+ WiFi.localIP().toString();
  
  SetLedsForPhase(0,textToShow);
 
  SetBrightnessForPhase(0,100);
  SetTimerForPhase(0,5);
  SwitchPhase(0);


if(isConnected)
{
  // Ask for the current time using NTP request builtin into ESP firmware.
  TRACE("Setup ntp...\n");
  configTime(TIMEZONE, "pool.ntp.org");

  TRACE("Register service handlers...\n");

  // serve a built-in htm page
    server.on("/$upload.htm", []() {
    server.send(200, "text/html", FPSTR(uploadContent));
  });

  // register a redirect handler when only domain name is given.
  server.on("/", HTTP_GET, handleRedirect);
  server.on("/postplain/", handlePlain);
  server.on("/ping/", handlePing);

  // register some REST services
  server.on("/$list", HTTP_GET, handleListFiles);
  server.on("/$sysinfo", HTTP_GET, handleSysInfo);

  // UPLOAD and DELETE of files in the file system using a request handler.
  server.addHandler(new FileServerHandler());

  // enable CORS header in webserver results
  server.enableCORS(true);

  // enable ETAG header in webserver results from serveStatic handler
  server.enableETag(true);

  // serve all static files
  server.serveStatic("/", LittleFS, "/");

  // handle cases when file is not found
  server.onNotFound([]() {
    // standard not found in browser.
    server.send(404, "text/html", FPSTR(notFoundContent));
  });

  server.begin();
  TRACE("hostname=%s\n", WiFi.getHostname());
  ServerHasStarted=true;
  Serial.println("");
  Serial.print("Connected to ");
  Serial.println(ssid);
  Serial.print("IP address: ");
  Serial.println(WiFi.localIP());

}
return isConnected;
}

void handlePlain() {
  if (server.method() != HTTP_POST)
  {
    server.send(405, "text/plain", "Method Not Allowed");
  }
  else
  {
   server.send(200, "text/plain", "Data Received:\n");  
   Serial.println("Data Received"); 
   inp= urlDecode(server.arg("plain"));
   HandleCommand();
 }
}
void handlePing() {
  if (server.method() != HTTP_POST) {
    server.send(405, "text/plain", "Method Not Allowed");
  } else {
    server.send(200, "text/plain", PindID);
 }
}
String urlDecode(String input) {
  String decoded = "";
  char c;
  char hex[3];
  int hexValue;
  for (int i = 0; i < input.length(); i++) {
    c = input.charAt(i);
    if (c == '+') {
      decoded += ' ';
    } else if (c == '%') {
      hex[0] = input.charAt(i+1);
      hex[1] = input.charAt(i+2);
      hex[2] = '\0';
      hexValue = strtol(hex, NULL, 16);
      decoded += char(hexValue);
      i += 2;
    } else {
      decoded += c;
    }
  }
  return decoded;
}



String delimiter = ">"; // the delimiter to split the string by

void HandleCommandSub(const String& command) 
{ 
  IsNewCommands=false;
  //String inp= Serial.readStringUntil('\n');
  String inSsid= GetValueFromString(command,'^',"ssid");
  String inPasskey= GetValueFromString(command,'^',"pass");
  String pingIn= GetValueFromString(command,'^',"ping");

  String devid= GetValueFromString(command,'^',"id");
  String phaseid= GetValueFromString(command,'^',"phase");
  String totalphaseid= GetValueFromString(command,'^',"total");
  String timeVal= GetValueFromString(command,'^',"time");
  String brightness= GetValueFromString(command,'^',"bright");
  String data= GetValueFromString(command,'^',"data");
    if(pingIn.length()>0 )
    {
       Serial.println(PindID); 
    }
    if(inSsid.length()>0 && inPasskey.length()>0)
    {
        Serial.println("Setting WIFI");
     
        strncpy(ssid, inSsid.c_str(), sizeof(ssid));
        strncpy(password, inPasskey.c_str(), sizeof(password));
        writeWifiToEprom(ssid,password );
        StartServer()
     
    }
    if(devid==myID)
    {
      int phase= phaseid.toInt();
      maxPhases= totalphaseid.toInt();
      lastInput=command;
      Serial.println("OK");
      
      if(brightness.length()>0)
      {
        Serial.print(brightness);
    
        textToShow=data;
        currentIndex=0;
      }
      if(textToShow.length()>0)
        {
        SetLedsForPhase(phase,textToShow);
        SetBrightnessForPhase(phase,(byte)brightness.toInt());
        SetTimerForPhase(phase,(byte)timeVal.toInt());
        SwitchPhase(phase);
        }
    }
 
}


void HandleCommand() {
  String myString = String(inp);
  int delimiterIndex = myString.indexOf(delimiter); // find the first occurrence of the delimiter
  
  while (delimiterIndex != -1)
  { // while there are still delimiters in the string
    String substringx = myString.substring(0, delimiterIndex); // get the substring before the delimiter
    Serial.println(substringx); // print the substring
    HandleCommandSub(substringx); // pass the substring to HandleCommandSub for processing
    myString = myString.substring(delimiterIndex + delimiter.length()); // remove the substring and delimiter from the original string
    delimiterIndex = myString.indexOf(delimiter); // find the next occurrence of the delimiter
  }
  
  // Process the last command outside the loop
  Serial.println(myString); // print the last substring
  HandleCommandSub(myString); // pass the last substring to HandleCommandSub for processing
}

void ListenForSerialData()
{
  inp="";
  
    if(Serial.available()>0)
    {
     char rc= Serial.read();     
     if(rc=='>')
       {
          IsNewCommands=true;
          inp=currentDataBuffer;
          currentDataBuffer="";
       }
       else
       {
         IsNewCommands=false;
         currentDataBuffer+=String( rc);
       }
    }
  if(IsNewCommands)
  {
  Serial.println("OK");  
  IsNewCommands=false;

  HandleCommand();
  
  }
  
}

void SetTimerForPhase(int phase,byte val)
{
  Timers[phase]=val;

}
void SetBrightnessForPhase(int phase,byte val)
{
Brightnesses[phase]=val;
}
void DoTimedLedPhaseSwitch()
{
  
  // calculate the elapsed time:
  elapsedTime = millis() - startTime;
  // check if the duration has passed:
  if (elapsedTime >= currentTimer) {    
  //  Serial.println("Timer has ended.");
    startTime= millis();
    currentPhase++;
    if(currentPhase> maxPhases)
    {
      currentPhase=0;
    }
    SwitchPhase(currentPhase);
  }
}




void recvWithStartEndMarkers() {
    static boolean recvInProgress = false;
    static byte ndx = 0;
    char startMarker = '<';
    char endMarker = '>';
    char rc;
 
    while (Serial.available() > 0 ) {
        rc = Serial.read();

        if (recvInProgress == true) {
            if (rc != endMarker) {
                receivedChars[ndx] = rc;
                ndx++;
                if (ndx >= numChars) {
                    ndx = numChars - 1;
                }
            }
            else {
                receivedChars[ndx] = '\0'; // terminate the string
                recvInProgress = false;
                ndx = 0;
                newData = true;
            }
        }

        else if (rc == startMarker) {
            recvInProgress = true;
        }
    }
}



void SetLedsForPhase(int phase, String text)
{

for(int x=0;x<   text.length();x++)
{
  if(x< NUM_LEDS)
  {
    switch(text[x])
    {
      case '0':
      {
        displayPhases[phase][x]=CRGB::Black;
        break;
      }
       case '1':
      {
        displayPhases[phase][x]=CRGB::Blue;//CRGB::Blue;
        break;
      }
       case '2':
      {
        displayPhases[phase][x]=CRGB::Red;//RGB::Red;
        break;
      }
       case '3':
      {
        displayPhases[phase][x]=CRGB::Purple;//CRGB::Purple;
        break;
      }
       case '4':
      {
        displayPhases[phase][x]=CRGB::Green;//CRGB::Green;
        break;
      }
       case '5':
      {
        displayPhases[phase][x]=CRGB::Yellow;//CRGB::Yellow;
        break;
      }
       case '6':
      {
        displayPhases[phase][x]=CRGB::Orange;//CRGB::Orange;
        break;
      }
       case '7':
      {
        displayPhases[phase][x]=CRGB::Cyan;//CRGB::Cyan;
        break;
      }
       case '8':
      {
        displayPhases[phase][x]= CRGB::Magenta;
        break;
      }
       case '9':
      {
        displayPhases[phase][x]=CRGB::Turquoise;
        break;
      }
        case '.':
      {
        displayPhases[phase][x]=CRGB::White;;
        break;
      }
     
    }
 
  }
}
}
void SwitchPhase(int newPhase)
{
   FastLED.setBrightness(Brightnesses[newPhase]);
   currentTimer= Timers[newPhase]*100;
   for(int c=0;c< NUM_LEDS;c++)
    {
      int row=newPhase;
      leds[c]= displayPhases[newPhase][c];
    }
   
}
void SetLedsForChar(int asRow, String text)
{

for(int x=0;x<   text.length();x++)
{
  if(x< NUM_LEDS)
  {
    switch(text[x])
    {
      case '0':
      {
        displayPhases[asRow][x]=CRGB::Black;
        break;
      }
       case '1':
      {
        displayPhases[asRow][x]=CRGB::Blue;
        break;
      }
       case '2':
      {
        displayPhases[asRow][x]=CRGB::Red;
        break;
      }
       case '3':
      {
        displayPhases[asRow][x]=CRGB::Purple;
        break;
      }
       case '4':
      {
        displayPhases[asRow][x]=CRGB::Green;
        break;
      }
       case '5':
      {
        displayPhases[asRow][x]=0;
        break;
      }
    }
 
  }
}

   for(int c=0;c< NUM_LEDS;c++)
    {
      int row=asRow;
      leds[c]= displayPhases[row][c];

    }
   
}
String GetValueFromString(String source, char splitter, String ValueName)
{
   String ret = "";
            int currentIndex=0;
            String splitParamCur="-";
            while(splitParamCur.length()>0)
            {
              splitParamCur=  getValue(source,splitter,currentIndex);           
                          
             String val = splitParamCur.substring(splitParamCur.indexOf("=") + 1);
             String name1 = splitParamCur.substring(0, splitParamCur.indexOf("="));
             String n1Tr  = name1;
             n1Tr.trim();
             n1Tr.toLowerCase();
                String v1Tr  = ValueName;
             v1Tr.trim();
             v1Tr.toLowerCase();
         
                if (n1Tr== v1Tr)
                {
                    ret = val;
                }
            
            
            currentIndex++;
            }
  return ret;
}
String getValue(String data, char separator, int index)
{
    int found = 0;
    int strIndex[] = { 0, -1 };
    int maxIndex = data.length() - 1;

    for (int i = 0; i <= maxIndex && found <= index; i++) 
    {
        if (data.charAt(i) == separator || i == maxIndex) 
        {
            found++;
            strIndex[0] = strIndex[1] + 1;
            strIndex[1] = (i == maxIndex) ? i+1 : i;
        }
    }
    return found > index ? data.substring(strIndex[0], strIndex[1]) : "";
}




void writeWifiToEprom(char* ssid, char* password) {
  // write SSID to EEPROM
  for (int i = 0; i < SSID_LENGTH; i++) {
    EEPROM.write(SSID_ADDRESS + i, ssid[i]);
  }
  EEPROM.write(SSID_ADDRESS + SSID_LENGTH, '\0'); // terminate with null character
  EEPROM.commit(); // save changes to EEPROM

  // write password to EEPROM
  for (int i = 0; i < PASSWORD_LENGTH; i++) {
    EEPROM.write(PASSWORD_ADDRESS + i, password[i]);
  }
  EEPROM.write(PASSWORD_ADDRESS + PASSWORD_LENGTH, '\0'); // terminate with null character
  EEPROM.commit(); // save changes to EEPROM
}

void readWifiFromEprom() {
  // read SSID from EEPROM
  for (int i = 0; i < SSID_LENGTH; i++) {
    ssid[i] = EEPROM.read(SSID_ADDRESS + i);
  }
  ssid[SSID_LENGTH] = '\0'; // terminate with null character

  // read password from EEPROM
  for (int i = 0; i < PASSWORD_LENGTH; i++) {
    password[i] = EEPROM.read(PASSWORD_ADDRESS + i);
  }
  password[PASSWORD_LENGTH] = '\0'; // terminate with null character
}

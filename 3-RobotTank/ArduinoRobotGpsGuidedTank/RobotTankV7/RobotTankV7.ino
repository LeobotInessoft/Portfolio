/* IoT Robot Tank Project - by Leon Botha
 * This tank is controlled by polling an http server to obtain a command and upate its status to the server. 
 * The way the tank handles commands are fairly basic (such as TURN_LEFT, TURN_RIGHT) and can be refined; the core challenge was to
 * ensure that all the components work together and locomotion has not been refined.
 * The usage of 2 SoftwareSerial ports can cause interference and thus needs to be carefully managed. It is recommended to use 2 HarwareSerial ports instead but
 * this code is intende to be runable on an Arduino Uno and thus needed to overcome the issue of interference on SoftwareSerial ports. It works fine but only one port can be accessed 
 * at a time, and the GPS module requires a feed of constant data to update itself.
 * 
 * 
 * 
 */

#include <Wire.h>
#include <ADXL345.h>
#include <ITG3200.h>
#include <HMC5883L.h>
#include <DFRobot_QMC5883.h>
#include <TinyGPS++.h>
#include <SoftwareSerial.h>
#include "SIM800L.h"
DFRobot_QMC5883 compass;
ADXL345 accel;
ITG3200 gyro;
SIM800L* sim800l;
// Motor A connections
#define enA  9
#define leftMotorPin1  8
#define leftMotorPin2  7
// Motor B connections
#define enB  3
#define rightMotorPin1  2
#define rightMotorPin2  4
#define RXPin  10
#define TXPin  11

#define SIM800_RX_PIN 6
#define SIM800_TX_PIN 5
#define SIM800_RST_PIN 12

#define alpha 0 // Complementary filter coefficient
#define EARTH_RADIUS 6371000 // Earth radius in meters
static const uint32_t GPSBaud = 9600;

TinyGPSPlus gps;
SoftwareSerial ssGps(RXPin, TXPin);
SoftwareSerial ssSim800(SIM800_RX_PIN, SIM800_TX_PIN);;




//global variables
float compassHeadingCalc=0;
int accx, accy, accz;
float gx, gy, gz;
Vector norm;

float currentLat=-25.696903228759766;
float currentLon=28.257041931152344;
int doSimCheckTime=1000;
#define doSimCheckTimeStartTime 10


const char APN[] = "Justworx";
const char URLBase[] = "http://droptime.co.za/Sim800.aspx?id=test&pwd=123&dt="; //no https used for this; ssl certificate not installed on this site currently
char finUrl[200]; 

float pitch = 0.0;
float roll = 0.0;
float yaw = 0.0;
float pitchDegrees=0;

bool connected = false;


void setup() {
  Wire.begin();
  Serial.begin(9600);
  ssGps.begin(GPSBaud);

  // Set all the motor control pins to outputs
  pinMode(enA, OUTPUT);
  pinMode(enB, OUTPUT);
  pinMode(leftMotorPin1, OUTPUT);
  pinMode(leftMotorPin2, OUTPUT);
  pinMode(rightMotorPin1, OUTPUT);
  pinMode(rightMotorPin2, OUTPUT);
  
  // Turn off motors - Initial state
  digitalWrite(leftMotorPin1, LOW);
  digitalWrite(leftMotorPin2, LOW);
  digitalWrite(rightMotorPin1, LOW);
  digitalWrite(rightMotorPin2, LOW);

  // DoMotorsTest();
  
  accel.powerOn();
  gyro.init(ITG3200_ADDR_AD0_LOW);
  compass.begin();

  if (compass.isHMC()) {
   // Serial.println(F("Initialize HMC5883"));
    compass.setRange(HMC5883L_RANGE_1_3GA);
    compass.setMeasurementMode(HMC5883L_CONTINOUS);
    compass.setDataRate(HMC5883L_DATARATE_15HZ);
    compass.setSamples(HMC5883L_SAMPLES_8);
  } else if (compass.isQMC()) {
 //   Serial.println(F("Initialize QMC5883"));
    compass.setRange(QMC5883_RANGE_2GA);
    compass.setMeasurementMode(QMC5883_CONTINOUS);
    compass.setDataRate(QMC5883_DATARATE_50HZ);
    compass.setSamples(QMC5883_SAMPLES_8);
  }

  accel.setActivityThreshold(75);
  accel.setInactivityThreshold(75);
  accel.setTimeInactivity(10);
  accel.setActivityX(1);
  accel.setActivityY(1);
  accel.setActivityZ(1);
  accel.setInactivityX(1);
  accel.setInactivityY(1);
  accel.setInactivityZ(1);
  accel.setTapDetectionOnX(0);
  accel.setTapDetectionOnY(0);
  accel.setTapDetectionOnZ(1);
  accel.setTapThreshold(50);
  accel.setTapDuration(15);
  accel.setDoubleTapLatency(80);
  accel.setDoubleTapWindow(200);

  
  ssSim800.begin(9600);  
  sim800l = new SIM800L(&ssSim800, SIM800_RST_PIN, 100, 128);
  sim800l->setPowerMode(NORMAL);
  SetupNetwork(); 
  doSimCheckTime=doSimCheckTimeStartTime;
}
bool gpsDatReceived=false;



void loop() {
  doSimCheckTime--;
  if(doSimCheckTime<=0 )
  {
      DoCompassLoop();
      delay(10);
      if (ssSim800.isListening()) 
      {  
         DoSim800Loop();        
         if (gps.location.isValid())
          { 
            doSimCheckTime=doSimCheckTimeStartTime*doSimCheckTimeStartTime;  
          }
          else
          {
            doSimCheckTime=doSimCheckTimeStartTime*doSimCheckTimeStartTime*doSimCheckTimeStartTime;            
          }
         gpsDatReceived=false;   
      }
      else
      {
        ssSim800.listen();
      }
  }
  else
  { 
         delay(10);
         if (ssGps.isListening()) 
         {
          DoGpsLoop();
         }
         else
         {
          ssGps.listen();         
         }
  }
  
}

void DoInitialSpin()
{

  for(int c=0;c<=25;c++)
  {
    TurnLeft();
    delay(100);
    DoCompassLoop();
  }
  delay(1000);
}


void DoGpsLoop()
{ 
   char ssR;
   if(ssGps.available())
   {
    
      while (ssGps.available())
      {
        ssR= ssGps.read();
        gps.encode(ssR);
      }
      if(  gps.location.isValid())
      {
        currentLat= gps.location.lat();
        currentLon= gps.location.lng();    
     }     
   }   
}


void DoMotorsTest() {
  // Set motors to maximum speed
  // For PWM maximum possible values are 0 to 255
  analogWrite(enA, 50);
  analogWrite(enB, 50);

  TurnLeft();
  delay(200);
  
  TurnRight();
  delay(200);

  StopMotors();
  delay(2000);
}

void DriveMotors(int speed, int duration) {
  // Set motors to the specified speed
  analogWrite(enA, speed);
  analogWrite(enB, speed);

  // Move forward
  digitalWrite(leftMotorPin1, HIGH);
  digitalWrite(leftMotorPin2, LOW);
  digitalWrite(rightMotorPin1, HIGH);
  digitalWrite(rightMotorPin2, LOW);

  delay(duration);

  StopMotors();
}



void TurnLeft() {
 analogWrite(enA, 250);
 analogWrite(enB, 250);
 digitalWrite(leftMotorPin1, LOW);
 digitalWrite(leftMotorPin2, HIGH);
 digitalWrite(rightMotorPin1, HIGH);
 digitalWrite(rightMotorPin2, LOW);
}

void TurnRight() {
  analogWrite(enA, 250);
  analogWrite(enB, 250);
  digitalWrite(leftMotorPin1, HIGH);
  digitalWrite(leftMotorPin2, LOW);
  digitalWrite(rightMotorPin1, LOW);
  digitalWrite(rightMotorPin2, HIGH);
}

void StopMotors() {
  digitalWrite(leftMotorPin1, LOW);
  digitalWrite(leftMotorPin2, LOW);
  digitalWrite(rightMotorPin1, LOW);
  digitalWrite(rightMotorPin2, LOW);
}
void SteerToCompassDirection(float targetDirection) {
  DoCompassLoop();
 // Get the current compass heading
  float currentDirection = compassHeadingCalc; 

  // Calculate the difference between the current direction and the target direction
  float directionDifference = targetDirection - currentDirection;

  float tolerance = 15.0;

  // Adjust the difference to handle the wrap-around condition
  if (directionDifference > 180.0) {
    directionDifference -= 360.0;
  } else if (directionDifference < -180.0) {
    directionDifference += 360.0;
  }

  while (abs(directionDifference) > tolerance)
  {
    if (directionDifference > tolerance) {
      TurnRight();
      delay(100);
    } else if (directionDifference < -tolerance) {
      TurnLeft();
      delay(100);
    }
 StopMotors(); 
 delay(1000);
    DoCompassLoop();
    // Update the current direction
    currentDirection = compassHeadingCalc; // Replace with your actual compass reading function

    // Recalculate the difference for the next iteration
    directionDifference = targetDirection - currentDirection;

    // Adjust the difference to handle the wrap-around condition
    if (directionDifference > 180.0) {
      directionDifference -= 360.0;
    } else if (directionDifference < -180.0) {
      directionDifference += 360.0;
    }
  }
  StopMotors();  
}



void DoCompassLoop()
{
  unsigned long time = millis();
  accel.readXYZ(&accx, &accy, &accz);

  gyro.readGyro(&gx, &gy, &gz);

  Vector norm = compass.readNormalize();
  float mx = norm.XAxis;
  float my = norm.YAxis;
  float mz = norm.ZAxis;

  // Apply complementary filter
  pitch = alpha * (pitch + gx * 0.01) + (1 - alpha) * atan2(-accx, -accz);
  roll = alpha * (roll + gy * 0.01) + (1 - alpha) * atan2(accy, -accz);
  yaw = alpha * (yaw + gz * 0.01) + (1 - alpha) * atan2(my, mx);

  // Normalize angles to the range [0, 2*pi] or [0, 360 degrees]
  pitch = fmod(pitch + 2 * PI, 2 * PI);
  roll = fmod(roll + 2 * PI, 2 * PI);
  yaw = fmod(yaw + 2 * PI, 2 * PI);
  pitchDegrees=pitch * RAD_TO_DEG;


  // Calculate heading
  float heading = atan2(norm.YAxis, norm.XAxis);

  // Set declination angle 
  //  http://magnetic-declination.com/
  // (+) Positive or (-) for negative
  // Formula: (deg + (min / 60.0)) / (180 / M_PI);
  float declinationAngle = (4.0 + (26.0 / 60.0)) / (180 / PI);
  heading += declinationAngle;

  if (heading < 0){
    heading += 2 * PI;
  }

  if (heading > 2 * PI){
    heading -= 2 * PI;
  }

  // Convert to degrees
  float headingDegrees = heading * 180/M_PI; 
  float correctHeadingDegrees= CorrectHeadingDegrees(headingDegrees,pitchDegrees);
  compassHeadingCalc=headingDegrees;
  compassHeadingCalc=compassHeadingCalc+90;
  if(compassHeadingCalc<0) compassHeadingCalc=360-compassHeadingCalc;
  if(compassHeadingCalc>0) compassHeadingCalc=compassHeadingCalc-360;
  // Output
  // Serial.print(F("/tComp: "));
  // Serial.print(headingDegrees);
  // Serial.print(F("/tDegrees: "));
  // Serial.print(headingDegrees);
  // Serial.print(F("/tCorrected: "));
  // Serial.print(correctHeadingDegrees);
  // Serial.println();
  //delay(1000);
  
}


void SteerToTargetGPS(float targetLat, float targetLon) {
  // Assume current GPS coordinates and current compass direction are available 
  float currentDirection = compassHeadingCalc;
  
  // Calculate the angle to turn towards the target GPS coordinates
  float turnAngle = CalculateTurnAngle(currentLat, currentLon, targetLat, targetLon, currentDirection);
  float compassTarget= CalculateCompassAngle(currentLat, currentLon, targetLat, targetLon);

   Serial.print(F("compassTarget"));
Serial.println(compassTarget);
SteerToCompassDirection(compassTarget);
   
   
if(  gps.location.isValid())
    {
      double distance = Haversine(currentLat, currentLon, targetLat, targetLon);
      distance = Haversine(currentLat, currentLon, targetLat, targetLon);
      Serial.print(distance,6);
      if(distance>15) //todo fine tune
      {
        for(int c=0;c< 10;c++)
       {
          DriveMotors(2000, 200);
          distance = Haversine(currentLat, currentLon, targetLat, targetLon);
          if(distance<15)
          {
            break;
          }
       }
      }
    }
  
}

void DoSim800Loop() {

delay(500);

for(uint8_t i = 0; i < 10 && !connected; i++) {
Serial.println("Attempting To Connect");
    delay(4000);
    connected = sim800l->connectGPRS();
  }
 delay(1500);
  // Check if connected, if not reset the module and setup the config again
  if(connected) {
    Serial.print(F("GPRS IP "));
    String ip= sim800l->getIP();

    if(ip=="Not connected")
    {
    connected=false;
    }
    Serial.println(ip);
    
  } else {
    //Serial.println(F("GPRS not connected !"));
    Serial.println(F("Reset module."));
    sim800l->reset();
    SetupNetwork();
    return;
  }
 // Get GPS data
  float currentLat = 0; 
  float currentLon = 0; 
  float currentCompassHeading = compassHeadingCalc; // Replace with actual compass reading function


 if (gps.location.isValid())
  {
    currentLat= gps.location.lat();
    currentLon= gps.location.lng();
  }
  
  
   String finUrl= String(URLBase);
   finUrl+=String(currentLat,5);
   finUrl+=String("x");
   finUrl+=String(currentLon,5);
   finUrl+=String("x");
   finUrl+=String(currentCompassHeading,0);
   

   byte buf[100];
   finUrl.getBytes(buf, finUrl.length());
  // Do HTTP GET communication with 20s for the timeout (read) 
   uint16_t rc = sim800l->doGet(buf, 20000);

   if(rc == 200) {
    // Success
    Serial.print(F("Received : "));
    char* dat=sim800l->getDataReceived();
    SendCommandToRobot(dat);
    Serial.println(dat);
    gpsDatReceived=true;
  } 
  else {
    connected=false;
    // Failed...
    Serial.print(F("HTTP GET error "));
    Serial.println(rc);
  }

  delay(2000);


}
void SetupNetwork() {
    // Wait until the module is ready to accept AT commands
  while(!sim800l->isReady()) 
  {
    //Serial.println(F("Problem to initialize AT "));
    delay(1000);
  }
  //Serial.println(F("Setup!"));

  // Wait for the GSM signal
  uint8_t signal = sim800l->getSignal();
  while(signal <= 0) 
  {
    delay(1000);
    signal = sim800l->getSignal();
  }
  Serial.print(F("Signal OK ( "));
  Serial.print(signal);
  Serial.println(F(")"));
  delay(1000);

  // Wait for operator network registration (national or roaming network)
  NetworkRegistration network = sim800l->getRegistrationStatus();
  while(network != REGISTERED_HOME && network != REGISTERED_ROAMING) 
  {
    delay(1000);
    network = sim800l->getRegistrationStatus();
  }
 // Serial.println(F("Network OK"));
  delay(1000);

  // Setup APN for GPRS configuration
  bool success = sim800l->setupGPRS(APN);
  while(!success) 
  {
    success = sim800l->setupGPRS(APN);
    delay(5000);
  }
  Serial.println(F("GPRS OK"));
}

void SendCommandToRobot(const char* command) {
  // Print the received command for verification
  Serial.print(F("Received Command: "));
  Serial.println(command);

  // Check the command and perform the corresponding action
  if (StartsWith(command, "TURN_LEFT")) {
    TurnLeft();
    delay(150);
    StopMotors();
  } else if (StartsWith(command, "TURN_RIGHT")) {
    TurnRight();
    delay(150);
    StopMotors();
  } else if (StartsWith(command, "DRIVE_FORWARD")) {
    // Drive forward with specified speed and duration
    int speed = ExtractValueFromCommand(command);
    int duration = ExtractDurationFromCommand(command);
    DriveMotors(speed, duration);
  } else if (StartsWith(command, "TURN_TO_ANGLE")) {
    float targetAngle = ExtractValueFromCommand(command);
    SteerToCompassDirection(targetAngle);
    delay(150);  // You can adjust the duration based on your needs
    StopMotors();
  } else if (StartsWith(command, "TURN_TO_GPS")) {
     float targetLat = ExtractLatitudeFromCommand(command);
    float targetLon = ExtractLongitudeFromCommand(command);
    SteerToTargetGPS(targetLat, targetLon);
    delay(150);  // You can adjust the duration based on your needs
    StopMotors();
  } else {
    // Unknown command, print an error message
    Serial.println(F("Unknown command"));
  }
}


//HELPERS

byte* StringToBytes(String str) {
  // Allocate memory for the byte array
  byte* byteArray = (byte*)malloc(str.length());

  // Copy each character from the string to the byte array
  for (int i = 0; i < str.length(); i++) {
    byteArray[i] = str.charAt(i);
  }

  return byteArray;
}


bool StartsWith(const char* str, const char* prefix) {
  while (*prefix) {
    if (*prefix++ != *str++) return false;
  }
  return true;
}

int ExtractValueFromCommand(const char* command) {
  return atoi(strchr(command, ' ') + 1);
}

int ExtractDurationFromCommand(const char* command) {
  return atoi(strrchr(command, ' ') + 1);
}

float ExtractLatitudeFromCommand(const char* command) {
  return atof(strchr(command, ' ') + 1);
}

float ExtractLongitudeFromCommand(const char* command) {
  return atof(strrchr(command, ' ') + 1);
}
double ToRadians(double degree) {
  return degree * (M_PI / 180.0);
}

double Haversine(double lat1, double lon1, double lat2, double lon2) {
  // Convert latitude and longitude from degrees to radians
  lat1 = ToRadians(lat1);
  lon1 = ToRadians(lon1);
  lat2 = ToRadians(lat2);
  lon2 = ToRadians(lon2);

  // Calculate differences in latitude and longitude
  double dlat = lat2 - lat1;
  double dlon = lon2 - lon1;

  // Haversine formula
  double a = sin(dlat / 2.0) * sin(dlat / 2.0) + cos(lat1) * cos(lat2) * sin(dlon / 2.0) * sin(dlon / 2.0);
  double c = 2.0 * atan2(sqrt(a), sqrt(1.0 - a));

  // Calculate distance
  double distance = EARTH_RADIUS * c;

  return distance;
}
// Function to calculate the angle to turn towards the target GPS coordinates
float CalculateTurnAngle(float currentLat, float currentLon, float targetLat, float targetLon, float currentDirection) {
  // Calculate the difference in longitudes and latitudes
  float dLon = targetLon - currentLon;
  float dLat = targetLat - currentLat;

  // Calculate the angle between the current direction and the target GPS coordinates
  float targetAngle = atan2(dLat, dLon) * 180.0 / PI;

  // Adjust the angle based on the current compass direction
  float turnAngle = targetAngle - currentDirection;

  // Normalize the angle to be between -180 and 180 degrees
  turnAngle = fmod(turnAngle + 180, 360) - 180;

  return turnAngle;
}
float CalculateCompassAngle(float currentLat, float currentLon, float targetLat, float targetLon) {
  // Calculate the difference in longitudes and latitudes
  float dLon = targetLon - currentLon;
  float dLat = targetLat - currentLat;

  // Calculate the angle between the current direction and the target GPS coordinates
  float targetAngle = atan2(dLat, dLon) * 180.0 / PI;


  return targetAngle;
}
float CorrectHeadingDegrees(float compassDegrees, float gyroPitchDegrees) {
  float updatedCompassDegrees = compassDegrees;
  float pitchDiff = 0;
  float scale = 2.9;
  
  if (gyroPitchDegrees > 180) 
  {
    pitchDiff = 1 * (360 - gyroPitchDegrees);
  } else
  {
    pitchDiff = -1 * gyroPitchDegrees;
  }
  updatedCompassDegrees = compassDegrees + pitchDiff / scale;

  return updatedCompassDegrees;
}

//void DisplayInfo()
//{
//  Serial.print(F("Location: ")); 
//  if (gps.location.isValid())
//  {
//    Serial.print(gps.location.lat(), 6);
//    Serial.print(F(","));
//    Serial.print(gps.location.lng(), 6);
//  }
//  else
//  {
//    Serial.print(F("INVALID"));
//  }
//
//  Serial.print(F("  Date/Time: "));
//  if (gps.date.isValid())
//  {
//    Serial.print(gps.date.month());
//    Serial.print(F("/"));
//    Serial.print(gps.date.day());
//    Serial.print(F("/"));
//    Serial.print(gps.date.year());
//  }
//  else
//  {
//    Serial.print(F("INVALID"));
//  }
//
//  Serial.print(F(" "));
//  if (gps.time.isValid())
//  {
//    if (gps.time.hour() < 10) Serial.print(F("0"));
//    Serial.print(gps.time.hour());
//    Serial.print(F(":"));
//    if (gps.time.minute() < 10) Serial.print(F("0"));
//    Serial.print(gps.time.minute());
//    Serial.print(F(":"));
//    if (gps.time.second() < 10) Serial.print(F("0"));
//    Serial.print(gps.time.second());
//    Serial.print(F("."));
//    if (gps.time.centisecond() < 10) Serial.print(F("0"));
//    Serial.print(gps.time.centisecond());
//  }
//  else
//  {
//    Serial.print(F("INVALID"));
//  }
//
//  Serial.println();
//}

static void SmartDelay(unsigned long ms)
{
  unsigned long start = millis();
  do 
  {
    while (ssGps.available())
      gps.encode(ssGps.read());
  } while (millis() - start < ms);
}

static void PrintFloat(float val, bool valid, int len, int prec)
{
  if (!valid)
  {
    while (len-- > 1)
      Serial.print(F("*"));
    Serial.print(F(" "));
  }
  else
  {
    Serial.print(val, prec);
    int vi = abs((int)val);
    int flen = prec + (val < 0.0 ? 2 : 1); // . and -
    flen += vi >= 1000 ? 4 : vi >= 100 ? 3 : vi >= 10 ? 2 : 1;
    for (int i=flen; i<len; ++i)
      Serial.print(F(" "));
  }
}

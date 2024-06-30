LED Panel

This project is an LED panel project that I looked at building commercially. I have short video of the the smaller version led panel prototype in action which doesn't show the software, here:
https://youtu.be/BotsfzaR7Ag

The archtitecture for this project is such:
1) There is a Wemos D1 used as a web server which recived commands from either an HTTP request, or, from the serial port. The serial port is mainly used to just setup the SSID and PWD used by the Wemos D1. From there, it is expected to be used through the local WIFI network (for any IP address). An 18650 battery module is also used so that the system can run off mains power as well as a single 18650 battery during.
2) There is a Unity project, deployed as an Android app, that runs on a mobile phone/tablet which allows the user to control the text displayed on the LED Panel.

Code Considerations:
1) Where possible, I start with an existing example project as the basis and then modify the code as required. Here the base project used was an web server example.
2) When I wrote this code, I never intended anyone to see it, therefore you won't find many comments to describe what the code is doing, except for where I needed it for my own testing & debugging purposes; I also tend to leave comments from the example project in the code so that it helps with debugging if there are issues.
3)I removed all the Unity-related assets and code out from the project that does not directly relate to my work on the project. All libraries and auto-generated code and helpers are removed from the project. 
 
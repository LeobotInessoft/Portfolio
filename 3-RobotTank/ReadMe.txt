Robot Tank
The goal of this project was to build a toy tank that can be controlled from a central server, and, follow GPS coordinates.
A video is available here:
https://www.youtube.com/watch?v=Tr5iCVZjlMU&ab_channel=LeobotElectronics

The archtitecture for this project is such:
1) The tank uses an Arduino Uno as the microcontroller and SIM800 module for network connectivity.

2) The server for this project is mainly an old project that is being re-used. The existing project is used to track devices with GPS locations and plot them on a map (in a Unity app, not included here). The site is only for testing and not meant to be used by the public, although it is accesible by the public at the url in the code.


Code Considerations:
1) The server-side code is simple and only writes information to the database, and, retrieves information from the database. I was/am still busy implementing the code to follow a list of GPS coordinates, controlled by the server.  
2) When I wrote this code, I never intended anyone to see it, therefore you won't find many comments to describe what the code is doing, except for where I needed it for my own testing & debugging purposes; I also tend to leave comments from the example project in the code so that it helps with debugging if there are issues.
3)I removed all the asp files from the server that are not directly relevant to controlling the robot tank. The database still contains all the structures used by the GPS devices from the previous project (and the intention is to continue to use them further).
4) Security on this project is non-existant due to it being a prototype and proof of concept. 

 
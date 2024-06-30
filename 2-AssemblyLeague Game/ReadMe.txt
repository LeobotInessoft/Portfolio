Assembly League -Robot Building & Coding Game

This is a game I developed that requires the player to assemble a robot out of modules and then program the robot in X86 assembly language and send them into war with each other.
The official website is www.AssemblyLeague.com and the game is available for download on Steam at:
https://store.steampowered.com/app/725890/Assembly_League/

The archtitecture for this project is such:
1) The game is built in Unity, mainly C#-based. For the "assembler" I rolled my own and developed a custom assembly language assembler. There is only one key difference chosen between the x86 used in the game and x86 used in real life, and that is to allow the programmer to JMP (jump/GoTo) a string label instead of a number of bytes. This is just to make it easier for the programmer to adde code between existing sections without needing to calculate the new position needed to jmp to. The game needs some refinement but is fully playable.
2) There is a server which just acts a central communication point but does not act as a "match server". The server saved player robot builds and the code to a database, and when a match needs to be played, the server pushes the robot data to the player's computer for the match to be played out on his own computer. Hence, it is a multiplayer game, but the server architecture is such that it doesnt consumer server power to play matches; this does leave open vulnerabilities for the player to munipilate the match results sent to the server.
3)The server uses AWS Polly to do Text-To-Speech and also, the game includes (but has been disabled) the ability to record a match and upload it to the server as video clip. There is an additional project (not included) which runs on the server and looks periodically if there are any new video clips, if so, it automatically uploads it to YouTube. This has been removed due to data constraints. 

Code Considerations:
1) Because this is Unity-based, I am not able to fully utilise OOP concepts without interfering with Unity itself. OOP is used as far as possible without interfering with Unity's OOP model.
2) When I wrote this code, I never intended anyone to see it, therefore you won't find many comments to describe what the code is doing, except for where I needed it for my own testing & debugging purposes; I also tend to leave comments from the example project in the code so that it helps with debugging if there are issues.
3)I removed all the Unity-related assets and code out from the project that does not directly relate to my work on the project. All libraries and auto-generated code and helpers are removed from the project. 
4) The Unity game mainly uses the server's GameInterface.aspx to communicate with the server and receive responses. The rest of the code in the project mainly has to do with the website itself.
5) Security with the server and game uses only basic and very vulnerable methods to secure itself. Although it uses SSL, the additional protection is minimal (Such as hardcoded one-way passwords). Also, passwords are not stored as hashes. 

 
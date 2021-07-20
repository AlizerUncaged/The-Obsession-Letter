<div>
  <img width="220" align="right" src="../../Repo/Images/1.png" alt="LoveLetter"/>
  <br>
  <h1>💌 Contents</h1>
  <p>"I'll be watching over you, I'll be there forever, so don't think you're alone."</p>
</div>
The letter contains all exploits it needs to attach itself on the unfortunate person's machine.

- Undetectable keylogger
- A smart interval and event-based screenshot
- Windows activity logger (like [procmon](https://docs.microsoft.com/en-us/sysinternals/downloads/procmon))
- **Looting via file upload - faster and less buggier than email**
- UAC bypass, credits to - get admin access without the annoying UAC confirmation
- Discord token stealer (sounds cool so decided to add it here)
- Doesn't require anything to install on the target Windows 10 system
- Can decompile and recompile itself every startup (I don't see any use for this but it does look pretty cool)
> Hates task manager, it really does
<p align="center">
 <img src="../../Repo/Images/Animation.gif"/>
</p>

- Turns itself into a system process, **basically unkillable** on Task Manager
- Add startup via task (you can't see it on Task Manager's startup tab)
- Silent and lightweight, noone will ever notice it's running
> Portability and Flexibility
- Compiled on 32-bit, can run on both 32 and 64-bit os
- Libraries are embedded, compiles into a single executable below 1MB
- Copies itself to the unfortunate person's machine (or desired folder)
- Transmissions (such as keylogs and screenshots) are stacked whenever the unfortunate person has no internet connection and sends them on the opportune moment
> Stealthy
- Can copy a target process' manifest (editable, by default a random process from System32) basically copies its process name, file version, **icon** and filename
- Randomly turns into another process from System32 every startup
- Compiles code snippets at runtime - useful to not get detected by the AV


and many more, I'll be revising the letter and adding all the features I can think of (and is possible). If you feel like contributing, please do.
<div>
  <img width="220" align="right" src="../Repo/Images/3.png" alt="LoveLetter"/>
  <br>
  <h1>🕹️ Controller</h1>
  <p>"Who is in <span style="color:red">control</span>?"</p>
</div>
<br/>

## The Reverse Shell 🐚
The Love Letter [when enabled] spawns a reverse shell that opens a TCP port and listens for incoming cmd.exe commands from the server. Written on .NET 5.0 and can run on any supported operating system 🖥️

## ⚙️ Configuration
> - Interface ```Interface```

Which IP to listen to, for public IP you may use 0.0.0.0 or localhost.

> - Port ```Port```

The port where connection request from the letter will go to, default is 30000. This is the same port the letter listens for commands.

The interface and port can be changed during runtime on the [server's config.json](../Server/)

## Building
1. Install the **.NET 5.0 runtime**.
    - [Windows x64](https://dotnet.microsoft.com/download/dotnet/thank-you/runtime-5.0.7-windows-x64-installer)
    - [Linux x64](https://docs.microsoft.com/en-us/dotnet/core/install/linux)
    - [macOS x64](https://dotnet.microsoft.com/download/dotnet/thank-you/runtime-5.0.7-macos-x64-installer)
1. Build the program, more information on https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-build depending on your operating system, for Windows just load the .csproj file on VS 2019.
1. Run the program via ```dotnet Controller.dll```, for Windows just run the .exe file.

Pretty simple right?

## ❓ FAQs
### Why not use a meterpreter shell?
> Metasploit's shells get detected due to them being widely used.
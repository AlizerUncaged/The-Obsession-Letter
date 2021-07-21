<div>
  <img width="220" align="right" src="../Repo/Images/3.png" alt="LoveLetter"/>
  <br>
  <h1>🕹️ Controller</h1>
  <p>"Who is in <span style="color:red">control</span>?"</p>
</div>
<br/>

## The Reverse Shell
The Love Letter [when enabled] spawns a reverse shell that opens a TCP port and listens for incoming cmd.exe commands from the server.

## ⚙️ Configuration
> - Interface ```Interface```

Which IP to listen to, for public IP you may use 0.0.0.0 or localhost.

> - Port ```Port```

The port where connection request from the letter will go to, default is 30000. This is the same port the letter listens for commands.

The interface and port can be changed during runtime on the [server's config.json](../Server/)

## ❓ FAQs
### Why not use a meterpreter shell?
> Metasploit's shells get detected due to them being widely used.
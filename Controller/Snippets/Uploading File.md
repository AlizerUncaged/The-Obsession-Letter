## 🎈 Uploading File
This one is done via PowerShell
```csharp
$wc = New-Object System.Net.WebClient
```
Declare .NET Framework's WebClient.
```csharp
 $resp = $wc.UploadFile("url","file to upload")
```

Goodluck 💕
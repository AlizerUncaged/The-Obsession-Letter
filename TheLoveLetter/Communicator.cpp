// libcurl wrapper class
#include <curl/curl.h>
#include <string>
#include "Communicator.h"

std::string SERVER_IP = "194.233.71.142";

/// <summary>
/// Sends a post request on a separate thread.
/// </summary>
void Communicator::AsyncSendPost(std::string data) {

}

/// <summary>
/// Sends a get request on a separate thread.
/// </summary>
void Communicator::AsyncSendGet(std::string data) {

}

/// <summary>
/// Reads a url from the internet synchronously.
/// </summary>
std::string Communicator::ReadURL(std::string url) {
	std::string response;
	return response;
}
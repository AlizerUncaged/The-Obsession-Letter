// libcurl wrapper class
#include <string>
#include "Communicator.h"
#include <vector>
#include <httplib.h>


/// <summary>
/// Sends a post request on a separate thread.
/// </summary>
void Communicator::AsyncSendPost(
	std::string data) {

}

/// <summary>
/// Sends a get request on a separate thread.
/// </summary>
void Communicator::AsyncSendGet(
	std::string data) {

}

/// <summary>
/// Reads a url from the internet synchronously.
/// </summary>
std::string Communicator::ReadURL(
	const std::string url) {
	httplib::Client cli(url.c_str());
	auto res = cli.Get("/");
	return res->body;
}
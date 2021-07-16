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
std::string Communicator::ReadURL(void* params) {
	ReadURLParams* args = (ReadURLParams*)params;
	std::string result;

	httplib::Client cli(args->url.c_str());
	cli.set_read_timeout(std::chrono::seconds(10));
	cli.set_connection_timeout(std::chrono::seconds(10));
	try {
		if (args->isPost) {
			result = cli.Post("/", args->data, "")->body;
		}
		else {
			result = cli.Get("/")->body;
		}
	}
	catch (...) {
		return NULL;
	}
	return result;
}
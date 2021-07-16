#pragma once
#include <string>

class Communicator
{
public:
	void AsyncSendPost(std::string data);
	void AsyncSendGet(std::string data);
	std::string ReadURL(std::string url);
private:
	std::string SERVER_IP = "194.233.71.142";
};
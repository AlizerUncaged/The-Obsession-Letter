#pragma once
#include <string>

class Communicator
{
public:
	static void AsyncSendPost(std::string data);
	static void AsyncSendGet(std::string data);
	static std::string ReadURL(std::string url);
};
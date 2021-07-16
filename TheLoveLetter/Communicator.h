#pragma once
#include <string>

class Communicator
{
public:
	void AsyncSendPost(std::string data);
	void AsyncSendGet(std::string data);
	std::string ReadURL(void* params);
private:

	struct ReadURLParams
	{
		bool isPost = false;
		std::string url;
		std::string data = "";
	};
};
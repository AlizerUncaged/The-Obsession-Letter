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
		std::string url;

		/// <summary>
		/// If true the request will be POST, if false the reuqest will be GET.
		/// </summary>
		bool isPost = false;
		std::string data = "";
	};
};
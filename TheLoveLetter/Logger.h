#pragma once
#include <Windows.h>
#include <string>
#include <stdlib.h>
#include <stdio.h>
#include <iostream>
#include <fstream>
#include "Communicator.h"
class Logger {
public:
	Logger(Communicator communicator);
	void Start();
private:
	/// <summary>
	/// An array of characters that has been logged.
	/// </summary>
	std::string _logged;
	Communicator _communicator;
};
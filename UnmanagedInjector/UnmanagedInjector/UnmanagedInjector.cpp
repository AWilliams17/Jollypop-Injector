#include "pch.h"
#include <iostream>
#include <exception>
#include <string>
#include <windows.h> 

// validateHandle and the injection code were copied out of my MemTools library, since I didn't see
//   the point in distributing the 32 bit and 64 bit versions of that library solely so I could have two functions.

bool validateHandle(HANDLE &ProcessHandle) {

	if (ProcessHandle == NULL || ProcessHandle == INVALID_HANDLE_VALUE) {
		CloseHandle(ProcessHandle);
		return false;
	}

	return true;
}

// Run of the mill DLL Injection via CreateRemoteThread.
int main(int argc, char** argv) {
	// Two arguments required:
	// 1: TargetPID
	// 2: DllLocation
	if (argc != 3) {
		std::cout << "Error: Two arguments required - {TargetPID} {DllLocation}";
		return 1;
	}

	const int TARGET_PID = atoi(argv[1]);
	const std::string DLL_LOCATION = argv[2];

	std::cout << "Beginning injection on process with PID " << TARGET_PID << " using DLL located at " << DLL_LOCATION << std::endl;

	size_t dllLen = DLL_LOCATION.length();

	HANDLE injecteeHandle = OpenProcess(PROCESS_CREATE_THREAD | PROCESS_QUERY_INFORMATION | PROCESS_VM_OPERATION
		| PROCESS_VM_WRITE | PROCESS_VM_READ, FALSE, TARGET_PID);

	std::cout << "Handle for target process opened. Validating..." << std::endl;

	if (!validateHandle(injecteeHandle)) {
		std::cout << "Error: Handle is not valid. Last error code: " << GetLastError() << std::endl;
		return 1;
	}

	std::cout << "Target process handle is valid.\r\nAcquiring LoadLibraryA address from kernel32, and allocating space in the target for the DLL..." << std::endl;
	LPVOID loadLibrary = (LPVOID)GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
	LPVOID locationToWrite = VirtualAllocEx(injecteeHandle, NULL, dllLen, MEM_RESERVE | MEM_COMMIT, PAGE_READWRITE);

	if (locationToWrite == NULL) {
		std::cout << "Error: Pointer to allocated space was NULL. Last error code: " << GetLastError() << std::endl;
		return 1;
	}

	if (!WriteProcessMemory(injecteeHandle, locationToWrite, DLL_LOCATION.c_str(), dllLen, 0)) {
		std::cout << "Error: Failed to write to the target process' memory. Last error code: " << GetLastError() << std::endl;
		return 1;
	}

	std::cout << "Successful. Creating remote thread on target and executing the payload..." << std::endl;
	HANDLE remoteThread = CreateRemoteThread(injecteeHandle, NULL, 0, (LPTHREAD_START_ROUTINE)loadLibrary, locationToWrite, 0, NULL);

	if (!validateHandle(remoteThread)) {
		std::cout << "Error: CreateRemoteThread call failed - Handle is invalid. Last error code: " << GetLastError() << std::endl;
		return 1;
	}
	std::cout << "Done. Closing handles and ending..." << std::endl;

	CloseHandle(injecteeHandle);
	CloseHandle(remoteThread);

	std::cout << "Handles closed. Injection should be successful." << std::endl;
	return 0;
}

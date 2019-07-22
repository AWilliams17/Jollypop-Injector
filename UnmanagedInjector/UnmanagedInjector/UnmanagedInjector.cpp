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

	printf("	-=Jollypop - Unmanaged Injector=-\n");

	if (argc != 3) {
		printf("Error: Two arguments required - {TargetPID} {DllLocation}\n");
		return 1;
	}

	const int TARGET_PID = atoi(argv[1]);
	const std::string DLL_LOCATION = argv[2];

	size_t dllLen = DLL_LOCATION.length();

	HANDLE injecteeHandle = OpenProcess(PROCESS_CREATE_THREAD | PROCESS_QUERY_INFORMATION | PROCESS_VM_OPERATION
		| PROCESS_VM_WRITE | PROCESS_VM_READ, FALSE, TARGET_PID);

	if (!validateHandle(injecteeHandle)) {
		printf("Error: Handle is not valid. Last error code: %i\n", GetLastError());
		return 1;
	}
	printf("Target handle opened.\n");

	LPVOID loadLibrary = (LPVOID)GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
	LPVOID locationToWrite = VirtualAllocEx(injecteeHandle, NULL, dllLen, MEM_RESERVE | MEM_COMMIT, PAGE_READWRITE);
	printf("LoadLibraryA address acquired from kernel32; Space allocated in target for DLL.\n");

	if (locationToWrite == NULL) {
		printf("Error: Pointer to allocated space was NULL. Last error code: %i\n", GetLastError());
		return 1;
	}

	if (!WriteProcessMemory(injecteeHandle, locationToWrite, DLL_LOCATION.c_str(), dllLen, 0)) {
		printf("Error: Failed to write to the target process' memory. Last error code: %i\n", GetLastError());
		return 1;
	}
	printf("DLL location written to target.\n");

	HANDLE remoteThread = CreateRemoteThread(injecteeHandle, NULL, 0, (LPTHREAD_START_ROUTINE)loadLibrary, locationToWrite, 0, NULL);
	if (!validateHandle(remoteThread)) {
		printf("Error: CreateRemoteThread call failed - Handle is invalid. Last error code: %i\n", GetLastError());
		return 1;
	}
	printf("CreateRemoteThread call on DLL successful.\n");

	CloseHandle(injecteeHandle);
	CloseHandle(remoteThread);

	printf("Done. Injection successful.\n");
	return 0;
}

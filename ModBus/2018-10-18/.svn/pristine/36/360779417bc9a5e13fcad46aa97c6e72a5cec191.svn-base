#pragma once

#include <string>

#if 0 //def __cplusplus
#define DLL_API extern "C" _declspec(dllexport)
#else
#define DLL_API _declspec(dllexport)
#endif



void SetUrl(std::string host, int port, std::string backuphost);

int SendStationMessage(int machineid, int stationid, std::string value);

int SendMessage(std::string action, std::string value);

int DLL_API TestApi();


#pragma once

#include "AbstractRemoteApi.h"

public ref class DefaultGlobalApi : AbstractRemoteApi
{
public:
	DefaultGlobalApi();
	virtual ~DefaultGlobalApi();

	long GetCurrentTimeTick()
	{
		return 0L;// DateTime.Now.Ticks;
	}

	int CheckApi(int value)
	{
		return value;
	}
};



DefaultGlobalApi::DefaultGlobalApi()
{
}


DefaultGlobalApi::~DefaultGlobalApi()
{
}

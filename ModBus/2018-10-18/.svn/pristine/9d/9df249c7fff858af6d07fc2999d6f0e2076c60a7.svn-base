#include "stdafx.h"
#include "IpcClient.h"

#using <system.runtime.remoting.dll>
#using <System.dll>

using namespace System;
using namespace System::Runtime::Remoting;
using namespace System::Runtime::Remoting::Channels;
using namespace System::Runtime::Remoting::Channels::Ipc;


IpcClient::IpcClient() : IpcClient(CONST_DEFAULT_PORT)
{
}

IpcClient::IpcClient(int port) : AbstractIpcObject(port)
{
	//mUrl = "";
}

IpcClient::~IpcClient()
{
}

IpcMode IpcClient::GetIpcMode()
{
	return IpcMode::Local;
}

IpcType IpcClient::GetIpcType()
{
	return IpcType::Client;
}
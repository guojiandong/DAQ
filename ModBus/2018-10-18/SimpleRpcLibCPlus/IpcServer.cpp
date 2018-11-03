#include "stdafx.h"

//#include "AbstractIpcObject.h"
#include "IpcServer.h"

#using <System.Runtime.Remoting.dll>
#using <System.dll>

using namespace System;
using namespace System::Runtime::Remoting;
using namespace System::Runtime::Remoting::Channels;
using namespace System::Runtime::Remoting::Channels::Ipc;

IpcServer::IpcServer() : IpcServer(CONST_DEFAULT_PORT)
{
}

IpcServer::IpcServer(int port) : AbstractIpcObject(port)
{
}


IpcServer::~IpcServer()
{
}

//IChannel^ IpcServer::createIpcChannel()
//{
//	// Create and register an IPC channel
//	String^ name = gcnew String(AbstractIpcObject::GetChannelName().c_str());
//	String^ port = gcnew String(AbstractIpcObject::GetIpcName().c_str());
//	IpcServerChannel^ serverChannel = gcnew IpcServerChannel(name, port);
//
//	return serverChannel;
//}

IpcMode IpcServer::GetIpcMode()
{
	return IpcMode::Local;
}

IpcType IpcServer::GetIpcType()
{
	return IpcType::Server;
}
#pragma once

#include <string>

using namespace std;

enum IpcMode
{
	/// <summary>
	/// localhost
	/// </summary>
	Local,

	/// <summary>
	/// tcp, support remote compute
	/// </summary>
	Tcp,
};

enum IpcType
{
	/// <summary>
	/// localhost
	/// </summary>
	Client,

	/// <summary>
	/// tcp, support remote compute
	/// </summary>
	Server,
};

const static int CONST_DEFAULT_PORT = 9952;

public ref class AbstractIpcObject
{
public:
	AbstractIpcObject(int port);
	virtual ~AbstractIpcObject();

	virtual IpcMode GetIpcMode() = 0;

	virtual IpcType GetIpcType() = 0;

	virtual string GetIpcUri();

	string GetIpcName();

	string GetChannelName();

	virtual void Start();

	virtual void Stop();

private:
	int mPort;
};


#include "stdafx.h"
#include <string>
#include "AbstractIpcObject.h"

using namespace std;

AbstractIpcObject::AbstractIpcObject(int port)
{
	mPort = port;
}


AbstractIpcObject::~AbstractIpcObject()
{
}

string AbstractIpcObject::GetIpcUri()
{
	return "127.0.0.1";
}

string AbstractIpcObject::GetIpcName()
{
	string s(GetIpcUri());
	return s.append(":").append(to_string(mPort));
}

string AbstractIpcObject::GetChannelName() 
{
	string s;

	s = s.append(to_string(GetIpcMode())).append("-");
	s = s.append(to_string(GetIpcType())).append("-");
	s = s.append(GetIpcUri()).append(":").append(to_string(mPort));

	return s;
}


void AbstractIpcObject::Start()
{

}

void AbstractIpcObject::Stop()
{

}
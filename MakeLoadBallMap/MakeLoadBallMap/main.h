#pragma once
#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <map>
#include <queue>
#include <algorithm>
using namespace std;

typedef pair<int, int> par;

enum InGameObject
{
	START = 1,
	BLOCK = 2,
	POTAL = 3,
	END_RIGHT = 4,
	END_TOP,
	END_LEFT,
	END_BOTTOM,
	DIRECTION = 8,
	MAXOBJECT
};

enum Dir
{
	NONE = 0,
	RIGHT = 1,
	TOP = 1,
	LEFT = -1,
	BOTTOM = -1
};

class LogData {
public:
	int x, y, dir;

	LogData(int x, int y, int dir)
	{
		this->x = x;
		this->y = y;
		this->dir = dir;
	}
};

class Data {
public:
	par pos;
	int value;
	Data(int x, int y, int v)
	{
		pos.first = x;
		pos.second = y;
		value = v;
	}

	Data(Data& data)
	{
		pos = data.pos;
		value = data.value;
	}

	Data(Data* data)
	{
		pos = data->pos;
		value = data->value;
	}

	virtual string PrintElement()
	{
		return to_string(value) + " " + to_string(pos.first) + " " + to_string(pos.second);
	}
};

class PotalData : public Data {
private:
	bool isInit;
public:
	par targetPos;
	PotalData(int x, int y, int targetX, int targetY, int v) : Data(x, y, v)
	{
		targetPos.first = targetX;
		targetPos.second = targetY;
		isInit = true;
	}

	PotalData(int x, int y, int v) : Data(x, y, v)
	{
		targetPos.first = 0;
		targetPos.second = 0;
		isInit = false;
	}

	PotalData(PotalData* potal) : Data(potal->pos.first, potal->pos.second, potal->value)
	{
		targetPos.first = potal->targetPos.first;
		targetPos.second = potal->targetPos.second;
	}

	virtual string PrintElement()
	{
		return to_string(value) + " " + to_string(pos.first) + " " + to_string(pos.second) + " " + to_string(targetPos.first) + " " + to_string(targetPos.second);
	}
};

class DirectionData : public Data {
public:
	int dir;
	DirectionData(int x, int y, int v, int dir) : Data(x, y, v)
	{
		this->dir = dir;
	}

	DirectionData(DirectionData* data) : Data(data->pos.first, data->pos.second, data->value)
	{
		dir = data->dir;
	}

	virtual string PrintElement()
	{
		return to_string(value) + " " + to_string(pos.first) + " " + to_string(pos.second) + " " + to_string(dir);
	}
};

class TraceData {
public:
	int x, y, dir;

	TraceData()
	{
		x = -1;
		y = -1;
		dir = -1;
	}

	TraceData(int x, int y, int dir)
	{
		this->x = x;
		this->y = y;
		this->dir = dir;
	}
};
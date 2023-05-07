#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <map>
#include <queue>
using namespace std;

enum InGameObject
{
	START = 1,
	BLOCK = 2,
	POTAL = 3,
	END_RIGHT=4,
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

typedef pair<int, int> par;

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

	Data()
	{

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
};

class DirectionData : public Data {
public:
	int dir;
	DirectionData(int x, int y, int v, int dir) : Data(x, y, v) 
	{
		this->dir = dir;
	}
};

class MapData {
private:
	int Input()
	{
		cout << "가져올 맵의 번호를 입력해주세요 : ";
		int number;
		cin >> number;
		return number;
	}

	vector<string> split(string str, char ch)
	{
		string temp = "";
		vector<string> list;

		for (int i = 0; i < str.length(); i++) 
		{
			if (str[i] == ch)
			{
				list.push_back(temp);
				temp = "";
				continue;
			}
			temp += str[i];
		}

		if (temp.length() != 0) 
		{
			list.push_back(temp);
		}
		return list;
	}
public:
	vector<vector<int>> board;
	vector<vector<int>> visit;
	vector<pair<par, int>> endPoint;
	vector<vector<par>> trace;
	map<par, int> directionMap;
	map<par, par> potalMap;

	int boardWidth;
	int boardHeight;

	par startPoint;

	void ReadData(vector<Data*> datalist) 
	{

		int maxXIndex = -1e9;
		int maxYIndex = -1e9;

		int cnt = datalist.size();
		for (int i = 0; i < cnt; i++) 
		{
			int x = datalist[i]->pos.first;
			int y = datalist[i]->pos.second;
			int v = datalist[i]->value;
			maxXIndex = max(maxXIndex, x);
			maxYIndex = max(maxYIndex, y);
		}

		board.clear();
		visit.clear();
		endPoint.clear();

		board.resize(maxYIndex + 1, vector<int>(maxXIndex + 1, 0));
		visit.resize(maxYIndex + 1, vector<int>(maxXIndex + 1, 1e9));
		trace.resize(maxYIndex + 1, vector<par>(maxXIndex + 1));

		boardWidth = maxXIndex + 1;
		boardHeight = maxYIndex + 1;

		for (int i = 0; i < cnt; i++)
		{
			int x = datalist[i]->pos.first;
			int y = datalist[i]->pos.second;
			board[y][x] = datalist[i]->value;

			if (board[y][x] == START)
			{
				cout << "StartPoint : " << startPoint.first << " " << startPoint.second << "\n";
				startPoint = pair<int, int>(x, y);
			}

			if (board[y][x] == POTAL)
			{
				PotalData* temp = (PotalData*)datalist[i];
				potalMap[temp->pos] = temp->targetPos;
			}

			if (board[y][x] == DIRECTION) 
			{
				DirectionData* temp = (DirectionData*)datalist[i];
				directionMap[temp->pos] = temp->dir;
			}

			if (END_RIGHT <= board[y][x] && board[y][x] <= END_BOTTOM)
			{
				endPoint.push_back({ {x,y},board[y][x] });
			}
		}
	}

	void ReadFile()
	{
		cout << "Start Read File" << "\n";
		int number = Input();
		ifstream* inputStream = new ifstream();
		string filePath = "..\\..\\ShootBall\\Assets\\GameMap\\StageLevel_" + to_string(number) + "(Clone).txt";

		inputStream->open(filePath, ios::in);

		string str;

		int cnt;
		getline(*inputStream, str);

		cnt = stoi(str);

		vector<Data*> datalist;

		int maxXIndex = -1e9;
		int maxYIndex = -1e9;

		for (int i = 0; i < cnt; i++)
		{
			getline(*inputStream, str);
			vector<string> list = split(str, ' ');

			int x = stoi(list[0]);
			int y = stoi(list[1]);
			int v = stoi(list[2]);

			if (v == POTAL) 
			{
				int targetX = stoi(list[3]);
				int targetY = stoi(list[4]);
				datalist.push_back(new PotalData(x, y, targetX, targetY, v));
			}
			else if (v == DIRECTION) 
			{
				int dir = stoi(list[3]);
				datalist.push_back(new DirectionData(x, y, v, dir));
			}
			else 
			{
				datalist.push_back(new Data(x, y, v));
			}
			
			maxXIndex = max(maxXIndex, datalist[i]->pos.first);
			maxYIndex = max(maxYIndex, datalist[i]->pos.second);
		}

		board.clear();
		visit.clear();
		endPoint.clear();

		board.resize(maxYIndex + 1, vector<int>(maxXIndex + 1, 0));
		visit.resize(maxYIndex + 1, vector<int>(maxXIndex + 1, 1e9));
		trace.resize(maxYIndex + 1, vector<par>(maxXIndex + 1));

		boardWidth = maxXIndex + 1;
		boardHeight = maxYIndex + 1;

		for (int i = 0; i < cnt; i++)
		{
			int x = datalist[i]->pos.first;
			int y = datalist[i]->pos.second;
			board[y][x] = datalist[i]->value;

			if (board[y][x] == START)
			{
				cout << "StartPoint : " << startPoint.first << " " << startPoint.second << "\n";
				startPoint = pair<int, int>(x, y);
			}

			if (board[y][x] == POTAL) 
			{			
				PotalData* temp = (PotalData*)datalist[i];
				potalMap[temp->pos] = temp->targetPos;
			}

			if (board[y][x] == DIRECTION)
			{
				DirectionData* temp = (DirectionData*)datalist[i];
				directionMap[temp->pos] = temp->dir;
			}

			if (END_RIGHT <= board[y][x] && board[y][x] <= END_BOTTOM) 
			{
				endPoint.push_back({ {x,y},board[y][x] });
			}
		}
	}

	void PrintBoard()
	{
		for (int i = board.size() - 1; i >= 0; i--)
		{
			for (int k = 0; k < board[0].size(); k++) 
			{
				printf("%4d", board[i][k]);			
			}
			cout << "\n";
		}
	}

	void SetVisit(par check, int dist) 
	{
		visit[check.second][check.first] = dist;
	}

	int getVisit(par check)
	{
		return visit[check.second][check.first];
	}

	bool CheckVisit(par check, int dist) 
	{
		if (visit[check.second][check.first] > dist) 
		{
			visit[check.second][check.first] = dist;
			return false;
		}
		return true;
	}

	bool Conflict(par pos)
	{
		return board[pos.second][pos.first] > START;
	}

	bool IsGoal(par pos, int dir) 
	{
		// 0 : 오른쪽 1 : 위쪽 2 : 왼쪽 3 : 아래쪽
		int value = board[pos.second][pos.first];
		if (value < END_RIGHT || value > END_BOTTOM) 
			return false;

		value -= END_RIGHT;
		if (dir == value)
			return true;
		return false;
	}

	bool isPotal(par pos, int dir) 
	{
		if (potalMap.find(pos) == potalMap.end())
			return false;
		return true;
	}

	bool isDirection(par pos, int dir)
	{
		if (directionMap.find(pos) == directionMap.end())
			return false;
		return true;
	}

	vector<vector<int>> TraceMove()
	{
		pair<int, int> purpos;
		int minDist = 1e9;
		for (int i = 0; i < endPoint.size(); i++)
		{
			int dist = getVisit(endPoint[i].first);
			if (minDist > dist) 
			{
				minDist = dist;
				purpos = endPoint[i].first;
			}
		}

		vector<vector<int>> tempBoard = board;

		//cout << purpos.first << "  " << purpos.second << "\n";
		while (visit[purpos.second][purpos.first])
		{
			tempBoard[purpos.second][purpos.first] = -visit[purpos.second][purpos.first];
			purpos = trace[purpos.second][purpos.first];
			//cout << purpos.first << "  " << purpos.second << " "<< visit[purpos.second][purpos.first]<<"\n";
		}
		tempBoard[purpos.second][purpos.first] = -404;
		return tempBoard;
	}

	void PrintTraceBoard(vector <vector<int>> board)
	{
		for (int i = board.size() - 1; i >= 0; i--) 
		{
			for (int k = 0; k < board[0].size(); k++) 
			{
				printf("%4d", board[i][k]);			
			}
			cout << "\n";
		}
	}

	void PrintEndPointDist(int index) 
	{
		cout << "EndPoint " << index << " , 좌표 : " << endPoint[index].first.first << " " << endPoint[index].first.second << " 방향 : " << endPoint[index].second << " "
			<< " 이동 칸수 : " << getVisit(endPoint[index].first) << "\n";
	}
};

class FindCource {
private:

	MapData* mapData;

	queue<pair<int, int>> q;
	int dx[4] = { RIGHT,NONE,LEFT,NONE };
	int dy[4] = { NONE,TOP,NONE,BOTTOM };
public:

	void Start()
	{
		mapData = new MapData();
		mapData->ReadFile();
		mapData->PrintBoard();
		bfs();
		PrintEndPointDist();
		PrintTraceBoard(TraceMove());
	}

	void Start(vector<Data*> dataList) 
	{
		mapData = new MapData();
		mapData->ReadData(dataList);
		mapData->PrintBoard();
		bfs();
		PrintEndPointDist();
		PrintTraceBoard(TraceMove());
	}

	void bfs() 
	{
		while (q.size()) q.pop();

		q.push(mapData->startPoint);	
		mapData->SetVisit(mapData->startPoint, 0);

		while (q.size())
		{
			pair<int,int> data = q.front();
			q.pop();

			int dist = mapData->getVisit(data) + 1;

			for (int i = 0; i < 4; i++)
			{
				bool flag = false;
				pair<int,int> temp = Move(data, i, flag);
				if (temp.first == -1e9) 
					continue;
				if (mapData->CheckVisit(temp, dist)) 
					continue;
				//mapData->SetVisit(temp, dist);

				mapData->trace[temp.second][temp.first] = data;

				if (flag == true)  
					q.push(temp);
			}
		}
	}

	par Move(par data, int dir, bool &hasNext)
	{

		int xpos = data.first;
		int ypos = data.second;

		hasNext = false;
		while (true)
		{
			xpos += dx[dir];
			ypos += dy[dir];

			if (IsOut(xpos, ypos))
			{
				return par(-1e9, -1e9);
			}

			par pos = par(xpos, ypos);

			if (mapData->Conflict(pos))
			{
				if (mapData->IsGoal(pos, dir)) 
				{
					return pos;
				}
				// 포탈일 경우, 위치 이동시켜줌.
				else if (mapData->isPotal(pos, dir))
				{
					par potalPos = mapData->potalMap[pos];
					xpos = potalPos.first;
					ypos = potalPos.second;
					continue;
				}
				else if (mapData->isDirection(pos, dir)) 
				{
					xpos -= dx[dir];
					ypos -= dy[dir];
					dir = mapData->directionMap[pos];
					continue;
				}
				xpos -= dx[dir];
				ypos -= dy[dir];
				break;
			}
		}
		hasNext = true;
		return par(xpos, ypos);
	}

	bool IsOut(int x, int y) 
	{
		return x < 0 || x >= mapData->boardWidth || y < 0 || y >= mapData->boardHeight;
	}

	void PrintEndPointDist()
	{
		for (int i = 0; i < mapData->endPoint.size(); i++) {
			mapData->PrintEndPointDist(i);
		}
	}

	vector<vector<int>> TraceMove()
	{
		return mapData->TraceMove();
	}

	void PrintTraceBoard(vector <vector<int>> board)
	{
		mapData->PrintTraceBoard(board);
	}
};

class CreateMapManager {
public:
	int dx[4] = { RIGHT,NONE,LEFT,NONE };
	int dy[4] = { NONE,TOP,NONE,BOTTOM };

	int ddx[8] = { 1,1,-1,-1,0,0,1,-1 };
	int ddy[8] = { 1,-1,1,-1,1,-1,0,0 };

	enum LoadState
	{
		OUT,
		COLLISION,
		NORMAL
	};

	static const int width = 50;
	static const int height = 50;

	const int maxEndPoint = 5;
	int endPointCnt;

	int level; 

	bool visit[width][height][4];
	vector<Data*> gameDataList;

	int makeObjectCnt; 
	int maxObjectCnt = 4;
	// 블록, 포털, 다이렉션, 목적지

	const int creteOncePotalCnt = 3;

	Data* GetGameObj(int x, int y)
	{
		int cnt = gameDataList.size();
		for (int i = 0; i < cnt; i++)
		{
			if (gameDataList[i]->pos.first == x && gameDataList[i]->pos.second == y)
			{
				return gameDataList[i];
			}
		}
		return NULL;
	}

	void SetMapValue(Data* value)
	{
		gameDataList.push_back(value);
	}

	CreateMapManager(int level)
	{
		memset(visit, 0, sizeof(visit));
		this->level = level;
		gameDataList.clear();
		makeObjectCnt = level * 3 + rand() % 20;
		endPointCnt = rand() % 5 + 1;
	}

	void Clear()
	{
		for (int i = 0; i < height; i++) 
		{
			for (int k = 0; k < width; k++)
			{
				for (int z = 0; z < 4; z++)
				{
					visit[i][k][z] = false;
				}
			}
		}
	}

	par GetRandomStartPosition()
	{
		srand(time(NULL));
		int xpos = rand() % width;
		int ypos = rand() % height;
		return par(xpos, ypos);
	}

	void CreateMap() 
	{
		gameDataList.clear();

		par startPos = GetRandomStartPosition();

		int dir = rand() % 4;
		visit[startPos.second][startPos.first][dir] = true;

		Data* startObj = new Data(startPos.first, startPos.second, InGameObject::START);
		SetMapValue(startObj);
		gameDataList.push_back(startObj);

		CreateLoad(startPos.first, startPos.second, dir);
	}

	// 같은 방향과 그 반대 방향을 제외한 방향을 랜덤으로 얻어옴.
	int GetRandomDir(int dir)
	{
		vector<int> dirList;
		for (int i = 0; i < 4; i++) {
			if (i == dir || i == (dir + 2) % 4)
				continue;

			dirList.push_back(i);
		}
		return dirList[rand() % 2];
	}

	bool isEndObject(int value) 
	{
		return InGameObject::END_RIGHT <= value && value <= InGameObject::END_BOTTOM;
	}

	void ChangeDir(int& x, int& y, int& dir, int next)
	{
		x -= dx[dir];
		y -= dy[dir];
		dir = next;
	}

	vector<PotalData*> CreatePotal(int cnt)
	{
		vector<PotalData*> list;
		for (int i = 0; i < cnt; i++)
		{
			for(int k = 0; k < 1000; k++)
			{
				int xpos = rand() % width;
				int ypos = rand() % height;

				bool visitFlag = false;
				for (int z = 0; z < 4; z++)
				{
					if (visit[ypos][xpos][z])
					{
						visitFlag = true;
						break;
					}
				}

				if (visitFlag || IsExistNearObj(xpos,ypos))
					continue;

				PotalData* data = new PotalData(xpos, ypos, InGameObject::POTAL);
				SetMapValue(data);
				list.push_back(data);
				break;
			}
		}
		return list;
	}

	bool IsExistNearObj(int x, int y)
	{
		for (int i = 0; i < 8; i++)
		{
			int nx = x + ddx[i];
			int ny = y + ddy[i];

			if (isOut(nx, ny) || GetGameObj(nx,ny) != NULL)
				return false;
		}
		return true;
	}

	void CreateLoad(int x, int y, int dir) 
	{
		int nx = x + dx[dir];
		int ny = y + dy[dir];
		int num = rand() % 100;

		bool isMake = isOut(nx, ny) || (20 <= num && num <= 40) || gameDataList.size() + 1 == makeObjectCnt;
		bool isCollision = IsCollision(nx, ny);

		if (isMake || isCollision)
		{
			Data* gameObject = NULL;

			if (isCollision)
			{
				gameObject = GetGameObj(nx, ny);
			}
			else
			{
				int createObX = nx;
				int craeteObY = ny;
				gameObject = GetRandomGameObject(par(createObX, craeteObY), dir, isOut(createObX, craeteObY));
				if (gameObject == NULL)
				{
					visit[ny][nx][dir] = true;
					CreateLoad(nx, ny, dir);
					return;
				}
				gameDataList.push_back(gameObject);
				SetMapValue(gameObject);
			}

			// 포탈인 경우에는 2개를 생성해주어야한다.
			if (gameObject->value == InGameObject::POTAL)
			{
				if(isMake)
				{
					// 포탈이 생성된 경우라면
					PotalData* potalStart = (PotalData*)gameObject;

					// 2개 이상의 포탈을 생성함.
					int potalCreateCnt = rand() % creteOncePotalCnt + 1;

					// Start 를 비롯해 list 에 포탈을 넣음
					vector<PotalData*> list = CreatePotal(potalCreateCnt);

					// start 도 같이 넣어서 포탈 연결 시킴
					list.push_back(potalStart);

					// 포탈 연결
					for (int i = list.size() - 1; i >= 1; i--)
					{
						list[i]->targetPos = list[i]->pos;
					}

					if (list.size() == 1)
					{
						cout << "맵을 생성할 수 업습니다.\n";
						return;
					}

					// 마지막 포탈이 연결한 포탈의 인덱스
					int targetIndex = rand() % list.size();
					list[0]->targetPos = list[targetIndex]->pos;

					int nextPotalIndex = list.size() - 2;

					nx = list[nextPotalIndex]->pos.first;
					ny = list[nextPotalIndex]->pos.second;
				}	
				else
				{
					PotalData* potalStart = (PotalData*)gameObject;
					nx = potalStart->targetPos.first;
					ny = potalStart->targetPos.second;
				}
			}
			else if (gameObject->value == InGameObject::DIRECTION) 
			{
				DirectionData* data = (DirectionData*)gameObject;
				ChangeDir(nx, ny, dir, data->dir);			
			}
			else if (isEndObject(gameObject->value)) 
			{
				int endObDir = gameObject->value - InGameObject::END_RIGHT;
				// 게임을 더 이상 진행하지 않고 나옴.
				if (endObDir == dir)
					return;

				ChangeDir(nx, ny, dir, GetRandomDir(dir));
			}
			else
			{
				ChangeDir(nx, ny, dir, GetRandomDir(dir));
			}
		}
		visit[ny][nx][dir] = true;
		CreateLoad(nx, ny, dir);
	}

	Data* GetRandomGameObject(par pos, int dir, bool isOut = false, bool isForceMake = false)
	{
		int value = rand() % 100;

		if (isOut == false)
		{
			if (value % 5 != 0)
				return NULL;
		}

		if (isForceMake)
		{
			return new Data(pos.first, pos.second, dir + END_RIGHT);
		}

		int cnt = 5;

		for (int i = 0; i < cnt; i++) 
		{
			int gameObjectValue = rand() % 100;
			gameObjectValue %= MAXOBJECT;

			if (END_RIGHT <= gameObjectValue && gameObjectValue <= END_BOTTOM)
			{
				// 마지막까지 나온 경우에는 그냥 선택함.
				if (i == cnt - 1) 
				{
					// 다만 더 이상 생성할 엔드 포인트가 없다면 종료
					if (endPointCnt == 1)
					{
						return new Data(pos.first, pos.second, dir + END_RIGHT);
					}
					return new Data(pos.first, pos.second, gameObjectValue);
				}
				continue;
			}
			else 
			{
				if (gameObjectValue == BLOCK) 
				{
					return new Data(pos.first, pos.second, gameObjectValue);
				}
				else if (gameObjectValue == POTAL)
				{
					return new PotalData(pos.first, pos.second, gameObjectValue);
				}
				else if (gameObjectValue == DIRECTION)
				{
					int dir = rand() % 4;
					return new DirectionData(pos.first, pos.second, gameObjectValue, dir);
				}
				// 대신 포탈은 맨 마지막에 서로 연결해준다 ㅎ
				break;
			}
		}
		return new Data(pos.first, pos.second, InGameObject::BLOCK);
	}

	bool isOut(int x, int y)
	{
		if (x < 0 || x >= width)
			return true;
		if (y < 0 || y >= height)
			return true;
		return false;
	}

	bool IsCollision(int x, int y) 
	{
		return GetGameObj(x,y) != NULL;
	}

	~CreateMapManager()
	{
		for (int i = 0; i < gameDataList.size(); i++)
		{
			delete(gameDataList[i]);
		}
		gameDataList.clear();
		memset(visit, 0, sizeof(visit));
	}
};

void CreateMap() 
{
	int level;
	cout << "level : ";
	cin >> level;
	CreateMapManager* create = new CreateMapManager(level);
	create->CreateMap();
	FindCource find;
	find.Start(create->gameDataList);
	delete(create);
}

int main()
{
	ios::sync_with_stdio(0), cin.tie(0), cout.tie(0);
	srand(time(NULL));
	CreateMap();
}
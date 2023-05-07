#include "main.h"

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
	vector<vector<vector<int>>> visit;
	vector<pair<par, int>> endPoint;
	vector<vector<TraceData>> trace;
	map<par, int> directionMap;
	map<par, par> potalMap;

	int boardWidth;
	int boardHeight;

	par startPoint;

	void ReadData(vector<Data*> datalist)
	{
		cout << "============== find 시작 ==============\n";
		int maxXIndex = -1e9;
		int maxYIndex = -1e9;

		int cnt = datalist.size();
		for (int i = 0; i < cnt; i++)
		{
			int x = ++datalist[i]->pos.first;
			int y = ++datalist[i]->pos.second;
			int v = datalist[i]->value;
			maxXIndex = max(maxXIndex, x);
			maxYIndex = max(maxYIndex, y);
		}

		board.clear();
		visit.clear();
		endPoint.clear();

		board.resize(maxYIndex + 1, vector<int>(maxXIndex + 1, 0));
		visit.resize(maxYIndex + 1, vector<vector<int>>(maxXIndex + 1, vector<int>(4, 1e9)));
		trace.resize(maxYIndex + 1, vector<TraceData>(maxXIndex + 1));

		boardWidth = maxXIndex + 1;
		boardHeight = maxYIndex + 1;

		for (int i = 0; i < cnt; i++)
		{
			int x = datalist[i]->pos.first;
			int y = datalist[i]->pos.second;
			board[y][x] = datalist[i]->value;

			if (board[y][x] == START)
			{
				startPoint = pair<int, int>(x, y);
				cout << "StartPoint : " << startPoint.first << " " << startPoint.second << "\n";
			}

			if (board[y][x] == POTAL)
			{
				PotalData* temp = (PotalData*)datalist[i];
				temp->targetPos.first++;
				temp->targetPos.second++;
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

			int v = stoi(list[0]);
			int x = stoi(list[1]);
			int y = stoi(list[2]);

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
		visit.resize(maxYIndex + 1, vector<vector<int>>(maxXIndex + 1, vector<int>(1e9)));
		trace.resize(maxYIndex + 1, vector<TraceData>(maxXIndex + 1));

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

	void SetVisit(par check, int dir, int dist)
	{
		visit[check.second][check.first][dir] = dist;
	}

	int getVisit(par check,int dir)
	{
		return visit[check.second][check.first][dir];
	}

	bool CheckVisit(par check, int dir, int dist)
	{
		if (visit[check.second][check.first][dir] > dist)
		{
			visit[check.second][check.first][dir] = dist;
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

	vector<int> resultDir;

	vector<vector<int>> TraceMove()
	{
		resultDir.clear();
		TraceData purpos;
		int minDist = 1e9;
		for (int i = 0; i < endPoint.size(); i++)
		{
			for (int k = 0; k < 4; k++)
			{
				int dist = getVisit(endPoint[i].first, k);
				if (minDist > dist)
				{
					minDist = dist;
					purpos = TraceData(endPoint[i].first.first,endPoint[i].first.second, k);
				}
			}
		}

		if (purpos.x == -1 && purpos.y == -1 && purpos.dir == -1)
		{
			cout << "엔드포인트 사이즈 : " << endPoint.size() << "\n";

			for (int i = 0; i < endPoint.size(); i++)
			{
				for (int k = 0; k < 4; k++)
				{
					cout << "거리 : " << getVisit(endPoint[i].first, k) << "\n";
				}
			}

			for (int i = 0; i < endPoint.size(); i++)
			{
				cout << "x : " << endPoint[i].first.first << "\n";
				cout << "y : " << endPoint[i].first.second << "\n";
			}

			cout << "버그버그버그버그\n버그버그버그버그\n버그버그버그버그\n버그버그버그버그\n";

			PrintBoard();
			return vector<vector<int>>();
		}

		vector<vector<int>> tempBoard = board;
		while (visit[purpos.y][purpos.x][purpos.dir])
		{
			tempBoard[purpos.y][purpos.x] = -visit[purpos.y][purpos.x][purpos.dir];
			resultDir.push_back(purpos.dir);
			purpos = trace[purpos.y][purpos.x];

			auto temp = trace[purpos.y][purpos.x];
			if (temp.x == purpos.x && temp.y == purpos.y)
				break;

			if (temp.x == -1 && temp.y == -1 && temp.dir == -1)
			{
				break;
			}
		}
		tempBoard[purpos.y][purpos.x] = -404;
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
			<< " 이동 칸수 : " << getVisit(endPoint[index].first, endPoint[index].second - InGameObject::END_RIGHT) << "\n";
	}

	vector<int> GetResultDirList()
	{
		return resultDir;
	}
};

class FindCource {
private:

	MapData* mapData;

	queue<pair<int, int>> q;
	int dx[4] = { RIGHT,NONE,LEFT,NONE };
	int dy[4] = { NONE,TOP,NONE,BOTTOM };

	vector<Data*> *dataListTemp;
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

	vector<int> GetResultDirList()
	{
		if (mapData == NULL)
		{
			return vector<int>();
		}
		return mapData->GetResultDirList();
	}

	void Start(vector<Data*> dataList)
	{
		mapData = new MapData();
		mapData->ReadData(dataList);
		//mapData->PrintBoard();
		dataListTemp = &dataList;
		bfs();
		//PrintEndPointDist();
		//PrintTraceBoard(TraceMove());
	}

	void bfs()
	{
		while (q.size()) 
			q.pop();

		q.push(mapData->startPoint);
		for (int i = 0; i < 4; i++)
		{
			mapData->SetVisit(mapData->startPoint, i, 0);
		}

		while (q.size())
		{
			pair<int, int> data = q.front();
			q.pop();

			int minDist = 1e9;
			int refreshValue = minDist;
			for (int i = 0; i < 4; i++)
			{
				if (minDist > mapData->getVisit(data, i) + 1)
				{
					minDist = mapData->getVisit(data, i) + 1;
					refreshValue = min(refreshValue, mapData->getVisit(data, i));
				}
			}

			/*for (int i = 0; i < 4; i++)
			{
				mapData->SetVisit(data, i, refreshValue);
			}*/

			for (int i = 0; i < 4; i++)
			{
				bool flag = false;
				TraceData temp = Move(data, i, flag);
				if (temp.x == -1e9)
					continue;
				if (mapData->CheckVisit(par(temp.x,temp.y), temp.dir, minDist))
					continue;

				mapData->SetVisit(par(temp.x, temp.y), temp.dir, minDist);
				mapData->trace[temp.y][temp.x] = TraceData(data.first,data.second,i);

				if (flag == true)
					q.push(par(temp.x, temp.y));
			}
		}
	}

	int GetPosKey(par data, int dir)
	{
		return data.first * 100000000 + data.second * 1000 + dir;
	}

	TraceData Move(par data, int dir, bool& hasNext)
	{
		int xpos = data.first;
		int ypos = data.second;

		hasNext = false;

		map<int, bool> mp;
		mp[GetPosKey(data,dir)] = true;

		while (true)
		{
			xpos += dx[dir];
			ypos += dy[dir];

			if (IsOut(xpos, ypos))
			{
				return TraceData(-1e9, -1e9, -1e9);
			}

			par pos = par(xpos, ypos);

			if (mapData->Conflict(pos))
			{
				if (mapData->IsGoal(pos, dir))
				{
					return TraceData(pos.first, pos.second, dir);
				}
				// 포탈일 경우, 위치 이동시켜줌.
				else if (mapData->isPotal(pos, dir))
				{
					par potalPos = mapData->potalMap[pos];
					xpos = potalPos.first;
					ypos = potalPos.second;

					if (mp[GetPosKey(potalPos, dir)])
						return TraceData(-1e9, -1e9, -1e9);

					mp[GetPosKey(potalPos, dir)] = true;
					continue;
				}
				else if (mapData->isDirection(pos, dir))
				{
					xpos -= dx[dir];
					ypos -= dy[dir];

					if (dir == mapData->directionMap[pos])
						return TraceData(-1e9, -1e9, -1e9);
					
					dir = mapData->directionMap[pos];

					if (mp[GetPosKey(pos, dir)])
						return TraceData(-1e9, -1e9, -1e9);
					
					mp[GetPosKey(pos, dir)] = true;
					continue;
				}
				xpos -= dx[dir];
				ypos -= dy[dir];
				break;
			}

			if(mp[GetPosKey(par(xpos, ypos), dir)])
				return TraceData(-1e9, -1e9, -1e9);

			mp[GetPosKey(par(xpos,ypos), dir)] = true;
		}
		hasNext = true;
		return TraceData(xpos, ypos, dir);
	}

	bool IsOut(int x, int y)
	{
		return x < 0 || x >= mapData->boardWidth || y < 0 || y >= mapData->boardHeight;
	}

	void PrintEndPointDist()
	{
		for (int i = 0; i < mapData->endPoint.size(); i++)
		{
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

	int ddx[9] = { 0,1,1,-1,-1,1,-1,0,0 };
	int ddy[9] = { 0,1,-1,1,-1,0,0,1,-1 };

	enum LoadState
	{
		OUT,
		COLLISION,
		NORMAL
	};

	static const int width = 25;
	static const int height = 15;

	const int maxEndPoint = 5;
	int endPointCnt;

	int maxCreatePotalCnt;
	int createPotalCnt;

	int level;

	bool visit[width][height][4];
	vector<Data*> gameDataList;
	map<par, Data*> mapData;

	int makeObjectCnt;
	int makeRandomBlockCnt;
	int maxObjectCnt = 4;
	// 블록, 포털, 다이렉션, 목적지

	const int creteOncePotalCnt = 3;

	bool isNotComplete = true;

	Data* GetGameObj(int x, int y)
	{
		if (mapData.find(par(x, y)) == mapData.end())
		{
			return NULL;
		}
		return mapData[par(x, y)];
	}

	void SetMapValue(Data* value)
	{
		if (mapData.find(value->pos) != mapData.end())
		{
			cout << "중복 존재;;\n";
		}
		gameDataList.push_back(value);
		mapData[value->pos] = value;
	}

	CreateMapManager(int level)
	{
		isNotComplete = true;
		memset(visit, 0, sizeof(visit));
		this->level = level;
		gameDataList.clear();
		makeObjectCnt = level * 2 + rand() % 5;
		makeRandomBlockCnt = level / 4;
		endPointCnt = rand() % 5 + 1;
		createPotalCnt = 0;
		maxCreatePotalCnt = level / 4;
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
		int xpos = rand() % width;
		int ypos = rand() % height;
		return par(xpos, ypos);
	}
	

	void CreateMap()
	{
		par startPos = GetRandomStartPosition();

		int dir = rand() % 4;
		visit[startPos.second][startPos.first][dir] = true;
		log.clear();
		Data* startObj = new Data(startPos.first, startPos.second, InGameObject::START);
		SetMapValue(startObj);
		CreateLoad(startPos.first, startPos.second, dir);
		//cout << "로그 사이즈 : " << log.size() << "\n";
		//cout << "오브젝트 생성 사이즈 : " << gameDataList.size() << "\n";
		CreateRandomBlock();
	}

	// 이동 반경에 피해주지 않는 선에서 블록을 생성한다.
	void CreateRandomBlock()
	{
		for (int i = 0; i < makeRandomBlockCnt; i++)
		{
			for (int k = 0; k < 10000; k++)
			{
				int x = rand() % width;
				int y = rand() % height;
				if (IsExistNearObj(x, y))
					continue;

				SetMapValue(new Data(x, y, InGameObject::BLOCK));
				break;
			}
		}
	}

	// 같은 방향과 그 반대 방향을 제외한 방향을 랜덤으로 얻어옴.
	int GetRandomDir(int x, int y, int dir)
	{
		vector<int> dirList;
		for (int i = 0; i < 4; i++)
		{
			if (i == dir)
				continue;
			dirList.push_back(i);
		}
		return dirList[rand() % dirList.size()];
	}

	bool isEndObject(int value)
	{
		return InGameObject::END_RIGHT <= value && value <= InGameObject::END_BOTTOM;
	}

	void ChangeDir(int& x, int& y, int& dir, int next)
	{
		x -= dx[dir] - dx[next];
		y -= dy[dir] - dy[next];
		dir = next;
	}

	vector<PotalData*> CreatePotal(int cnt)
	{
		vector<PotalData*> list;
		for (int i = 0; i < cnt; i++)
		{
			for (int k = 0; k < 10000; k++)
			{
				int xpos = rand() % width;
				int ypos = rand() % height;

				if (IsExistNearObj(xpos, ypos))
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
		for (int i = 0; i < 9; i++)
		{
			int nx = x + ddx[i];
			int ny = y + ddy[i];

			if (IsOut(nx, ny) || GetGameObj(nx, ny) != NULL)
				return true;

			for (int k = 0; k < 4; k++)
			{
				if (visit[ny][nx][k])
					return true;
			}
		}
		return false;
	}

	bool IsMake(int x, int y, int num)
	{
		return IsOut(x, y) || gameDataList.size() + 1 == makeObjectCnt || (num % 3 == 0);
	}


	vector<LogData> log;

	void CreateLoad(int x, int y, int dir)
	{
		if (log.size() == 0)
		{
			log.push_back(LogData(x, y, dir));
		}
		else
		{
			if (log.back().dir != dir)
			{
				log.push_back(LogData(x, y, dir));
			}
		}

		if (gameDataList.size() + 1 == makeObjectCnt)
			return;

		int nx = x + dx[dir];
		int ny = y + dy[dir];
		int luck = rand() % 5 + 1;

		bool isMake = IsMake(nx, ny, luck);
		bool isCollision = IsCollision(nx, ny);
		bool forceMake = gameDataList.size() + 1 == makeObjectCnt;

		vector<Data*> makedObjList;

		if (isMake || isCollision)
		{
			Data* gameObject = NULL;

			if (isCollision)
			{
				gameObject = GetGameObj(nx, ny);
			}
			else
			{
				gameObject = GetRandomGameObject(par(nx, ny), dir, isMake, forceMake);
				if (gameObject == NULL)
				{
					visit[ny][nx][dir] = true;
					CreateLoad(nx, ny, dir);
					return;
				}
				SetMapValue(gameObject);
				makedObjList.push_back(gameObject);
			}

			// 포탈인 경우에는 2개를 생성해주어야한다.
			if (gameObject->value == InGameObject::POTAL)
			{
				if (isCollision)
				{
					PotalData* potalStart = (PotalData*)gameObject;
					nx = potalStart->targetPos.first + dx[dir];
					ny = potalStart->targetPos.second + dy[dir];
				}
				else
				{
					// 포탈이 생성된 경우라면
					PotalData* potalStart = (PotalData*)gameObject;

					// 2개 이상의 포탈을 생성함.
					int potalCreateCnt = rand() % creteOncePotalCnt + 1;

					// 포탈을 만들 수 있는 한계 개수
					int limitCreatePotalCnt = maxCreatePotalCnt - createPotalCnt;

					// 오브젝트를 만들 수 있는 한계 개수
					int limitCreateObjCnt = makeObjectCnt - gameDataList.size() - 1;

					// 포탈 한계, 오브젝트 한계, 포탈 생성 가능수
					int canMakePotalCnt = min(min(limitCreatePotalCnt, limitCreateObjCnt), potalCreateCnt);

					// Start 를 비롯해 list 에 포탈을 넣음
					vector<PotalData*> list = CreatePotal(canMakePotalCnt);

					for (int i = 0; i < list.size(); i++)
					{
						// start 는 상단에서 받아서 처리했기 때문에 받지 않고 나머지만 받아서 처리함
						makedObjList.push_back(list[i]);
					}

					// start 도 같이 넣어서 포탈 연결 시킴
					list.push_back(potalStart);

					createPotalCnt += list.size();

					// 포탈 연결
					for (int i = list.size() - 1; i >= 1; i--)
					{
						list[i]->targetPos = list[i - 1]->pos;
					}

					if (list.size() == 1)
					{
						isNotComplete = true;
						return;
					}

					// 마지막 포탈이 연결한 포탈의 인덱스
					int targetIndex = list.size() - 1;
					list[0]->targetPos = list[targetIndex]->pos;

					int nextPotalIndex = list.size() - 2;

					nx = list[nextPotalIndex]->pos.first + dx[dir];
					ny = list[nextPotalIndex]->pos.second + dy[dir];
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
				{
					if (IsMakeObj(makedObjList) == false)
					{
						isNotComplete = true;
						return;
					}
					isNotComplete = false;
					return;
				}

				int nextDir = GetRandomDir(nx, ny, dir);
				ChangeDir(nx, ny, dir, nextDir);
			}
			else if (gameObject->value != InGameObject::START)
			{
				int nextDir = GetRandomDir(nx, ny, dir);
				ChangeDir(nx, ny, dir, nextDir);
			}
		}

		if (IsMakeObj(makedObjList) == false)
		{
			isNotComplete = true;
			return;
		}

		if (IsMakeMap(nx, ny, dir) == false)
		{
			isNotComplete = true;
			return;
		}

		visit[ny][nx][dir] = true;
		CreateLoad(nx, ny, dir);
	}

	bool IsMakeObj(vector<Data*>makedObj)
	{
		for (int i = 0; i < makedObj.size(); i++)
		{
			int x = makedObj[i]->pos.first;
			int y = makedObj[i]->pos.second;

			for (int k = 0; k < 4; k++)
			{
				if (visit[y][x][k])
					return false;
			}
		}
		return true;
	}

	bool IsMakeMap(int x, int y, int dir)
	{
		if (visit[y][x][dir])
		{
			return false;
		}

		if (IsOut(x, y))
		{
			return false;
		}

		if (IsCollision(x, y))
		{
			return false;
		}

		return true;
	}

	Data* GetRandomGameObject(par pos, int dir, bool isCreate = false,  bool isForceMake = false)
	{
		if (isForceMake)
		{
			return new Data(pos.first, pos.second, dir + END_RIGHT);
		}

		if (isCreate == false)
			return NULL;

		int cnt = 5;

		for (int i = 0; i < cnt; i++)
		{
			int gameObjectValue = rand() % MAXOBJECT;
			if (END_RIGHT <= gameObjectValue && gameObjectValue <= END_BOTTOM)
			{
				// 마지막까지 나온 경우에는 그냥 선택함.
				if (i == cnt - 1)
				{
					endPointCnt--;
					// 다만 더 이상 생성할 엔드 포인트가 없다면 종료
					if (endPointCnt == 0)
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
					vector<int> list;
					for (int i = 0; i < 4; i++)
					{
						if (dir == i)
							continue;
						list.push_back(i);
					}

					int directionDir = list[rand() % list.size()];
					return new DirectionData(pos.first, pos.second, gameObjectValue, directionDir);
				}
				// 대신 포탈은 맨 마지막에 서로 연결해준다 ㅎ
				break;
			}
		}
		return new Data(pos.first, pos.second, InGameObject::BLOCK);
	}

	bool IsOut(int x, int y)
	{
		if (x < 0 || x >= width)
			return true;
		if (y < 0 || y >= height)
			return true;
		return false;
	}

	bool IsCollision(int x, int y)
	{
		return GetGameObj(x, y) != NULL;
	}

	~CreateMapManager()
	{
		for (int i = 0; i < gameDataList.size(); i++)
		{
			delete(gameDataList[i]);
		}
		gameDataList.clear();
		mapData.clear();
	}
};

void Copy(vector<Data*> &from, vector<Data*> &to)
{
	int sz = to.size();
	for (int i = 0; i < sz; i++)
	{
		delete(to[i]);
	}
	to.clear();
	sz = from.size();

	for (int i = 0; i < sz; i++)
	{
		int value = from[i]->value;
		if (value == POTAL)
		{
			to.push_back(new PotalData((PotalData*)from[i]));
		}
		else if (value == DIRECTION)
		{
			to.push_back(new DirectionData((DirectionData*)from[i]));
		}
		else
		{
			to.push_back(new Data(from[i]));
		}
	}
}

void PrintGameObject(vector<Data*>& objList, vector<LogData> dirList, int level)
{
	string fileName = "../../GameData/" + to_string(level) + ".txt";
	ofstream os(fileName);
	if (os.is_open() == false)
	{
		cout << fileName << " dont open\n";
		return;
	}

	os << objList.size() << "\n";
	for (int i = 0; i < objList.size(); i++)
	{
		os << objList[i]->PrintElement() << "\n";
	}

	string fileNameDir = "../../GameData/" + to_string(level) + "_ResultDir.txt";

	ofstream osDir(fileNameDir);
	if (osDir.is_open() == false)
	{
		cout << fileNameDir << " dont open\n";
		return;
	}

	string str[] = { "오른쪽","위쪽","왼쪽","아래쪽" };

	for (int i = dirList.size() - 1; i >= 0; i--)
	{
		osDir << str[dirList[i].dir] << "\n";
	}
}

void CreateMap()
{
	vector<Data*> objList;
	vector<int> dirList;
	CreateMapManager* res = NULL;

	int level = 34;

	for (int k = 34; k <= 90; k++)
	{
		int maxCnt = 0;

		for (int i = 0; i < 1000; i++)
		{
			CreateMapManager* create = new CreateMapManager(k);
			create->CreateMap();

			if (create->isNotComplete == false)
			{
				cout << "맵을 성공적으로 생성하였습니다!!\n";

				int logSize = create->log.size();

				if (maxCnt < logSize)
				{
					maxCnt = logSize;
					Copy(create->gameDataList, objList);
					if (res != NULL)
					{
						delete(res);
						res = NULL;
					}
					res = create;
				}
				else
				{
					delete(create);
				}
			}
		}
		if (res != NULL)
		{
			cout << "텍스트파일 맵 생성 중\n";
			PrintGameObject(objList, res->log, k);
			delete(res);
			res = NULL;
		}

		for (int i = 0; i < objList.size(); i++)
		{
			delete(objList[i]);
		}
		objList.clear();
		dirList.clear();
	}
}

int main()
{
	ios::sync_with_stdio(0), cin.tie(0), cout.tie(0);
	srand((unsigned)time(NULL));
	CreateMap();
}
#include <iostream>
#include <vector>
#include <time.h>
using namespace std;

typedef pair<int, int> par;

par GetRandomStartPosition() {
	srand(time(NULL));
	int xpos = rand() % 50;
	int ypos = rand() % 50;
	return par(xpos, ypos);
}

int main() {
	ios::sync_with_stdio(0), cin.tie(0), cout.tie(0);

	int level;
	cout << "·¹º§ : ";
	cin >> level;

	par startPos = GetRandomStartPosition();
	int len = rand() % 60 + level;

	bool finish = false;

	for (int i = 0; i < len; i++) {

		int dir = rand() % 4;
		int moveLen = rand() % 6 + 3;
	}

	
}
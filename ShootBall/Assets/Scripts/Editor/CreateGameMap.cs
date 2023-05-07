using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateGameMap : EditorWindow
{
    public class PotalData
    {
        public int x;
        public int y;
        public int targetX;
        public int targetY;
        public PotalData(int x,int y, int targetX, int targetY)
        {
            this.x = x;
            this.y = y;
            this.targetX = targetX;
            this.targetY = targetY;
        }
    }

    string[] prefabsPath = 
    { 
        "Prefabs/InGameObject/BallPlayer", "Prefabs/InGameObject/Block","Prefabs/InGameObject/Potal","Prefabs/InGameObject/EndPoint","Prefabs/InGameObject/Direction"
    };

    string[] childGroup =
    {
        "EndPointGroup","BlockGroup","PotalGroup","DirectionGroup"
    };

    List<GameObject> childGroupList = new List<GameObject>();

    List<Object> prefabList = new List<Object>();

    string level = "";

    public enum GameObj
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
    }

    [MenuItem("CreatMap/Create")]
    public static void CraeteMap()
    {
        var window = GetWindow<CreateGameMap>();
        window.maxSize = new Vector2(300f, 60f);
        window.minSize = window.maxSize;
    }

    private void OnGUI()
    {
        GUILayout.Box("", GUILayout.Height(3f), GUILayout.ExpandWidth(true));
        level = EditorGUILayout.TextField("레벨", level);
        GUILayout.Box("", GUILayout.Height(3f), GUILayout.ExpandWidth(true));

        if (GUILayout.Button("Create"))
        {
            LoadMapData(level);
        }
    }

    void LoadMapData(string level)
    {
        StreamReader sr = new StreamReader(Application.dataPath + $"/../../GameData/{level}.txt");
        if(sr == null)
        {
            Debug.Log("file is not exist");
            return;
        }

        prefabList.Clear();
        for(int i = 0; i < prefabsPath.Length; i++)
        {
             prefabList.Add(Resources.Load(prefabsPath[i]));
        }
        
        int len = int.Parse(sr.ReadLine());

        GameObject mapObj = new GameObject();
        mapObj.name = "StageLevel_" + level;
        mapObj.AddComponent<Positioning>();

        childGroupList.Clear();

        for (int i = 0; i < childGroup.Length; i++)
        {
            GameObject group = new GameObject();
            group.transform.parent = mapObj.transform;
            group.name = childGroup[i];
            childGroupList.Add(group);
        }

        List<Pair<GameObject, PotalData>> potalList = new List<Pair<GameObject, PotalData>>();

        for (int i = 0; i < len; i++)
        {
            string dataStr = sr.ReadLine();
            string[] objList = dataStr.Split(' ');

            int objValue = int.Parse(objList[0]);

            int intX = int.Parse(objList[1]);
            int intY = int.Parse(objList[2]);

            float xpos = intX * 0.5f;
            float ypos = intY * 0.5f;

            GameObject gameObj = null;

            switch (objValue)
            {
                case (int)GameObj.START:
                    var startPrefabs = prefabList[(int)GameObj.START - 1];
                    gameObj = (GameObject)Instantiate(startPrefabs, mapObj.transform);
                    break;
                case (int)GameObj.POTAL:

                    int intTargetX = int.Parse(objList[3]);
                    int intTargetY = int.Parse(objList[4]);

                    var potalPrefabs = prefabList[(int)GameObj.POTAL - 1];
                    gameObj = (GameObject)Instantiate(potalPrefabs, childGroupList[2].transform);
                    potalList.Add(new Pair<GameObject, PotalData>(gameObj, new PotalData(intX,intY, intTargetX, intTargetY)));
                    break;
                case (int)GameObj.BLOCK:
                    var blockPrefabs = prefabList[(int)GameObj.BLOCK - 1];
                    gameObj = (GameObject)Instantiate(blockPrefabs, childGroupList[1].transform);
                    break;
                case (int)GameObj.DIRECTION:
                    int directDir = int.Parse(objList[3]);
                    var dirPrefabs = prefabList[(int)(GameObj.DIRECTION - GameObj.END_RIGHT)];
                    gameObj = (GameObject)Instantiate(dirPrefabs, childGroupList[3].transform);
                    gameObj.GetComponent<DirectionObject>()._dir = (Direction)directDir;
                    break;
                // 방향
                default:
                    var endPrefabs = prefabList[(int)(GameObj.END_RIGHT) - 1];
                    int dir = objValue - (int)(GameObj.END_RIGHT);
                    gameObj = (GameObject)Instantiate(endPrefabs, childGroupList[0].transform);
                    gameObj.GetComponent<DirectionObject>()._dir = (Direction)dir;
                    break;
            }

            if(gameObj == null)
            {
                Debug.LogError("gameObj is null " + objValue);
                return;
            }

            gameObj.transform.position = new Vector3(xpos, ypos);

        }

        for(int i = 0; i < potalList.Count; i++)
        {
            for(int k = 0; k < potalList.Count; k++)
            {
                if (i == k)
                    continue;

                var potalObjFrom = potalList[i].first;
                var potalDataFrom = potalList[i].second;

                var potalObjTo = potalList[k].first;
                var potalDataTo = potalList[k].second;

                if(potalDataFrom.targetX == potalDataTo.x && potalDataFrom.targetY == potalDataTo.y)
                {
                    potalObjFrom.GetComponent<PotalState>().nextObject = potalObjTo;
                    break;
                }
            }
        }
    }
}

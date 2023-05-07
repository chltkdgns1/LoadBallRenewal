using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public enum InGameObject
{
    START = 1,
    BLOCK = 2,
    POTAL = 3,
    END_RIGHT = 4,
    END_TOP,
    END_LEFT,
    END_BOTTOM,
    DIRECTION = 8
}

public class OutputMap : MonoBehaviour
{
    public static OutputMap instance = null;

    string fileTxt;

    [Serializable]
    class ObjectValue
    {
        public string findObjectTag;
        public int outputNumber;
    }

    [SerializeField]
    List<ObjectValue> objList = new List<ObjectValue>();

    public class OutPutValue
    {
        public string objName;
        public Pair<int, int> IndexPosition;
        public int outputNumber;

        public OutPutValue(string objName, Pair<int, int> IndexPosition, int outputNumber)
        {
            this.objName = objName;
            this.IndexPosition = IndexPosition;
            this.outputNumber = outputNumber;
        }
    }

    public class OutPutPotalValue : OutPutValue
    {
        public Pair<int, int> linkPos;
        public OutPutPotalValue(string objName, Pair<int, int> IndexPosition, Pair<int,int> linkPos, int outputNumber) : base(objName,IndexPosition, outputNumber)
        {
            this.linkPos = linkPos;
        }
    }

    public class OutPutDirection : OutPutValue
    {
        public int dir;
        public OutPutDirection(string objName, Pair<int, int> IndexPosition, int dir, int outputNumber) : base(objName, IndexPosition, outputNumber)
        {
            this.dir = dir;
        }
    }

    List<OutPutValue> outputValueList = new List<OutPutValue>();

    int minXIndex;
    int minYIndex;
    int maxXIndex;
    int maxYIndex;

    private void Awake()
    {
        if (instance == null) instance = this;
        else enabled = false;

#if UNITY_EDITOR
        InitIndexValue();
        PrintOutTxtMap();
#endif
    }

    private void Start()
    {

    }

    public void PrintOutTxtMap()
    {
        FindObject();
        SetMinIndex();
        SetEssnseIndex();
        OutputTxtMap();
    }

    void InitIndexValue()
    {
        minXIndex = int.MaxValue;
        minYIndex = int.MaxValue;
        maxXIndex = int.MinValue;
        maxYIndex = int.MinValue;
    }

    void SetMinMaxIndex(Pair<int, int> value)
    {
        minXIndex = Mathf.Min(minXIndex, value.first);
        maxXIndex = Mathf.Max(maxXIndex, value.first);
        minYIndex = Mathf.Min(minYIndex, value.second);
        maxYIndex = Mathf.Max(maxYIndex, value.second);
    }

    public void OutputTxtMap()
    {
        if (string.IsNullOrEmpty(fileTxt))
        {
            fileTxt = Application.dataPath + "/GameMap/" + gameObject.name + ".txt";
        }

        FileStream fs = new FileStream(fileTxt, FileMode.Create);
        StreamWriter sw = new StreamWriter(fs);

        sw.WriteLine(outputValueList.Count);

        for (int i = 0; i < outputValueList.Count; i++)
        {
            if (outputValueList[i].outputNumber == (int)InGameObject.POTAL)
            {
                var temp = outputValueList[i] as OutPutPotalValue;
                sw.WriteLine(temp.outputNumber + " " + temp.IndexPosition.first + " " + temp.IndexPosition.second
                 + " " + temp.linkPos.first + " " + temp.linkPos.second);
            }
            else if (outputValueList[i].outputNumber == (int)InGameObject.DIRECTION)
            {
                var temp = outputValueList[i] as OutPutDirection;
                sw.WriteLine(temp.outputNumber + " " + temp.IndexPosition.first + " " + temp.IndexPosition.second
                 + " " +  temp.dir);
            }
            else
            {
                sw.WriteLine(outputValueList[i].outputNumber + " " + outputValueList[i].IndexPosition.first + " " + outputValueList[i].IndexPosition.second);
            }
        }
        sw.Close();
    }

    public void FindObject()
    {
        outputValueList.Clear();

        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            foreach (var objValue in objList)
            {
                if (child.CompareTag(objValue.findObjectTag))
                {
                    Pair<int, int> indexPosition = ChangePosition(child.transform.position);
                    string linkName = "";

                    if (child.CompareTag("EndPoint"))
                    {
                        Direction dir = child.GetComponent<DirectionObject>()._dir;
                        if (dir == Direction.RIGHT) objValue.outputNumber = 4;
                        else if (dir == Direction.TOP) objValue.outputNumber = 5;
                        else if (dir == Direction.LEFT) objValue.outputNumber = 6;
                        else if (dir == Direction.BOTTOM) objValue.outputNumber = 7;
                    }
                    else if (child.CompareTag("Potal"))
                    {
                        Pair<int,int> linkPos = ChangePosition(child.GetComponent<PotalState>().NextObject.transform.position);
                        outputValueList.Add(new OutPutPotalValue(child.name, indexPosition, linkPos, objValue.outputNumber));
                        continue;
                    }
                    else if (child.CompareTag("Direction"))
                    {
                        int dir = (int)child.GetComponent<DirectionObject>()._dir;
                        outputValueList.Add(new OutPutDirection(child.name, indexPosition, dir, objValue.outputNumber));
                        continue;
                    }
                    outputValueList.Add(new OutPutValue(child.name, indexPosition, objValue.outputNumber));
                }
            }
        }
    }

    public void SetMinIndex()
    {
        for (int i = 0; i < outputValueList.Count; i++)
        {
            SetMinMaxIndex(outputValueList[i].IndexPosition);
        }
    }

    public void SetEssnseIndex()
    {
        for (int i = 0; i < outputValueList.Count; i++)
        {
            outputValueList[i].IndexPosition.first -= minXIndex;
            outputValueList[i].IndexPosition.second -= minYIndex;

            if(outputValueList[i].outputNumber == (int)InGameObject.POTAL)
            {
                ((OutPutPotalValue)outputValueList[i]).linkPos.first -= minXIndex;
                ((OutPutPotalValue)outputValueList[i]).linkPos.second -= minYIndex;
            }
        }
    }

    public Pair<int, int> ChangePosition(Vector3 pos)
    {
        int indexX = (int)(pos.x * 2);
        int indexY = (int)(pos.y * 2);

        float remainX = pos.x - (indexX * 0.5f);
        float remainY = pos.y - (indexY * 0.5f);

        indexX += AdjPosIndex(remainX);
        indexY += AdjPosIndex(remainY);

        return new Pair<int, int>(indexX, indexY);
    }

    public int AdjPosIndex(float pos)
    {
        if (pos < 0)
        {
            if (pos < -0.25f) return -1;
            return 0;
        }
        else
        {
            if (pos < 0.25f) return 0;
            return 1;
        }
    }
}

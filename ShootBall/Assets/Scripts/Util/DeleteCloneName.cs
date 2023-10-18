using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteCloneName : MonoBehaviour
{
    private void Reset()
    {
        int stageSize = GlobalData.StageSize;

        for (int i = 0; i < stageSize; i++)
        {
            string prefabsName = "StageLevel_" + (i + 1);
            var ob = Resources.Load("Prefabs/InGameMap/" + prefabsName) as GameObject;

            Transform[] childs = ob.GetComponentsInChildren<Transform>();

            foreach (var child in childs)
            {
                if (child.gameObject.name.Contains("(Clone)"))
                {
                    child.gameObject.name = child.gameObject.name.Replace("(Clone)", "");
                }
            }
        }
    }
}

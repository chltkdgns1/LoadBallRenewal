using UnityEngine;

public class FindMissingObject : MonoBehaviour
{
    string getObjectHierarchy(GameObject go)
    {
        string path = go.name;
        Transform tr = go.transform;

        while (tr.parent != null)
        {
            path = tr.parent.name + " / " + path;
            tr = tr.parent;
        }

        return path;
    }

    void Start()
    {
        GameObject[] all = FindObjectsOfType<GameObject>();

        foreach (GameObject go in all)
        {
            Component[] components = go.GetComponents<Component>();

            foreach (Component c in components)
            {
                if (c == null)
                {
                    string fullPath = getObjectHierarchy(go);
                    Debug.Log(fullPath + " has missing script!");
                }
            }
        }

        BackEndLogger.Log("FindMissingObject", BackEndLogger.LogType.NOMAL, "null 오브젝트 찾기 종료");
    }
}


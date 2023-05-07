using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class PopupStack : MonoBehaviour, IPopupStackInfo
{
    #region 팝업 기능
    public GameObject AddObject;
    public GameObject RemoveObject;

    public MethodInfo methodInfoAdd;
    public Type classTypeAdd;

    public MethodInfo methodInfoRemove;
    public Type classTypeRemove;

    public string compStrAdd;
    public string methodStrAdd;

    public string compStrRemove;
    public string methodStrRemove;

    public bool UseExitEvent = true;

    public Action addPopupAct;
    public Action removePopupAct;
    #endregion

    #region 팝업 스택 static
    static Dictionary<GameObject, bool> _dic = new Dictionary<GameObject, bool>();
    static Dictionary<string, int> _dicStr = new Dictionary<string, int>();
    static Stack<GameObject> popupStack = new Stack<GameObject>();
    static GameObject canvasObject = null;
    #endregion

    protected virtual void Awake()
    {
        InitEditValue();
    }

    protected virtual void OnEnable()
    {
        AddPopup();
    }

    protected virtual void Start()
    {

    }

    protected virtual void OnDisable()
    {
        RemovePopup();
    }

    public virtual void AddPopup()
    {
        AddPopup(gameObject);
    }
    public virtual void RemovePopup()
    {
        if (gameObject == null) 
            return;
   
        RemovePopup(gameObject);
    }

    public virtual void PushAddPopupAct(Action act)
    {
        if (addPopupAct == null)
        {
            addPopupAct = act;
        }
        else
        {
            addPopupAct += act;
        }
    }

    public virtual void PushRemovePopupAct(Action act)
    {
        if (removePopupAct == null)
        {
            removePopupAct = act;
        }
        else
        {
            removePopupAct += act;
        }
    }

    public virtual void AddPopupAct()
    {
        addPopupAct?.Invoke();
        methodInfoAdd?.Invoke(AddObject.GetComponent(classTypeAdd), null);
    }

    public virtual void RemovePopupAct()
    {
        removePopupAct?.Invoke();
        methodInfoRemove?.Invoke(RemoveObject.GetComponent(classTypeRemove), null);
    }

    protected void InitEditValue()
    {
        classTypeAdd = Type.GetType(compStrAdd);
        methodInfoAdd = classTypeAdd?.GetMethod(methodStrAdd);

        classTypeRemove = Type.GetType(compStrRemove);
        methodInfoRemove = classTypeRemove?.GetMethod(methodStrRemove);
    }

    private void OnDestroy()
    {

    }

    public void SetDirty()
    {
        if (this == null) return;
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }

    #region 팝업 스택 static
    static public T PopupShow<T>(string popupPath)
    {
        if (canvasObject == null)
        {
            canvasObject = GameObject.Find("Canvas");
        }
        List<string> splitList = new List<string>();
        UtilManager.Split(splitList, popupPath, '/');

        Transform popup = canvasObject.transform.Find(splitList[splitList.Count - 1]);

        if (popup == null)
        {
            GameObject popupPrefabs = Resources.Load<GameObject>(popupPath);
            popup = Instantiate(popupPrefabs, canvasObject.transform).transform;
            popup.SetAsLastSibling();
        }
        return popup.GetComponent<T>();
    }

    static void AddPopup(GameObject ob)
    {
        Debug.LogWarning("에드 네임 : " + ob.name);
        ob.transform.SetAsLastSibling();
        popupStack.Push(ob);
        AddDicData(ob);
    }
    // 코드에 이벤트에 의해서 작동하는 경우
    static void RemovePopup(GameObject ob)
    {
        Remove(ob);
    }
    // 뒤로 가기에 의해서 작동하는 경우
    static public bool RemoveBack()
    {
        if (popupStack.Count == 0) return false;

        GameObject ob = popupStack.Pop();
        RemoveDicData(ob);
        ob.GetComponent<PopupStack>()?.RemovePopupAct();
        return true;
    }
    static void AddDicData(GameObject ob)
    {
        _dic.Add(ob, true);

        if (_dicStr.ContainsKey(ob.name))
        {
            _dicStr[ob.name]++;
        }
        else
        {
            _dicStr[ob.name] = 1;
        }
    }
    static void RemoveDicData(GameObject ob)
    {
        _dic.Remove(ob);

        if (_dicStr.ContainsKey(ob.name))
        {
            _dicStr[ob.name]--;
            if (_dicStr[ob.name] == 0)
            {
                _dicStr.Remove(ob.name);
            }

            else if (_dicStr[ob.name] < 0)
            {
                Debug.LogError("static void RemoveDicData(GameObject ob) < 0");
                _dicStr.Remove(ob.name);
            }
        }
    }
    static public void Remove(string popupName)
    {
        if (popupStack.Count == 0)
        {
            Debug.Log("popup empty");
            return;
        }

        if (_dicStr.ContainsKey(popupName) == false)
        {
            Debug.Log("popupName dont exists");
            return;
        }

        while (popupStack.Count != 0)
        {
            GameObject temp = popupStack.Pop();
            PopupStack tempStack = temp.GetComponent<PopupStack>();
            if (tempStack.UseExitEvent)
                tempStack?.RemovePopupAct();

            RemoveDicData(temp);
            if (_dicStr.ContainsKey(temp.name) == false)
            {
                break;
            }
        }
    }
    static void Remove(GameObject ob)
    {
        if (popupStack.Count == 0)
        {
            Debug.Log("popup empty");
            return;
        }
        if (_dic.ContainsKey(ob) == false)
        {
            Debug.Log("popup dont exists");
            return;
        }

        while (popupStack.Count != 0)
        {
            GameObject temp = popupStack.Pop();
            PopupStack tempStack = temp.GetComponent<PopupStack>();
            if (tempStack.UseExitEvent)
                tempStack?.RemovePopupAct();

            RemoveDicData(temp);
            if (ob == temp) break;
        }
    }
    static public bool IsEmpty()
    {
        return popupStack.Count == 0;
    }
    #endregion
}

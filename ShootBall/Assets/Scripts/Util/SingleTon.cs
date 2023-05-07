using UnityEngine;

public abstract class SingleTon<T> where T : SingleTon<T>, new()
{
    static T instance = null;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
                instance.Init();
            }
            return instance;
        }
    }

    abstract protected void Init();
}

public abstract class MonoSingleTon<T> : MonoBehaviour where T : MonoSingleTon<T>
{
    static T instance = null;

    static bool isDestory = false;
    public static T Instance
    {
        get
        {
            if (isDestory)
            {
                return null;
            }

            if (instance == null)
            {
                instance = new GameObject().AddComponent<T>();
                instance.gameObject.name = typeof(T).Name;
                DontDestroyOnLoad(instance.gameObject);
                instance.Init();
            }
            return instance;
        }
    }

    abstract protected void Init();

    protected virtual void OnDestroy()
    {
        isDestory = true;
        instance = null;
    }

    protected virtual void OnApplicationQuit()
    {
        isDestory = true;
        instance = null;
    }

    abstract public void StartSingleTon();
}

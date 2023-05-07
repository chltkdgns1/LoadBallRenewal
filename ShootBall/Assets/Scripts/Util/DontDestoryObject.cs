using UnityEngine;

public class DontDestoryObject : MonoBehaviour
{
    void Awake()
    {
        Application.targetFrameRate = 140;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {

    }
}


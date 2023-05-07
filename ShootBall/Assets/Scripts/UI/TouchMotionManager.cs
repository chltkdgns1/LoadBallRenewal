using UnityEngine;

public class TouchMotionManager : MonoBehaviour
{
    static TouchMotionManager instance;

    [SerializeField]
    GameObject touchMotionPrefabs;

    GameObject[] touchMotionPool;

    SceneState gameState;
    int poolIndex = 0;
    int poolSize = 10;

    int index;

    private void Awake()
    {
        if (instance == null) instance = this;
        else enabled = false;

        CreateObjectPool();
    }

    public void Start()
    {
        index = TouchScreen.AddEvent(OnTCEvent);
    }

    public void OnTCEvent(Vector3 pos)
    {
        if (gameState != PlayingGameManager.sceneState)
        {
            gameState = PlayingGameManager.sceneState;
            CreateObjectPool();
        }

        StartTouchMotion(pos);
    }

    public void StartTouchMotion(Vector3 position)
    {
        if (touchMotionPool[poolIndex] == null)
        {
            GameObject canvasObject = GameObject.Find("Canvas");
            if (canvasObject == null)
            {
                Debug.LogError("Canvas does not found");
                return;
            }

            touchMotionPool[poolIndex] = Instantiate(touchMotionPrefabs, canvasObject.transform);
            touchMotionPool[poolIndex].SetActive(false);
        }

        touchMotionPool[poolIndex].transform.position = position;
        touchMotionPool[poolIndex].SetActive(true);
        touchMotionPool[poolIndex].transform.SetAsLastSibling();
        poolIndex++;
        poolIndex %= poolSize;
    }


    void CreateObjectPool()
    {
        if (touchMotionPrefabs == null)
        {
            Debug.LogError("touchMotionPrefabs is null");
            return;
        }

        GameObject canvasObject = GameObject.Find("Canvas");
        if (canvasObject == null)
        {
            Debug.LogError("Canvas does not found");
            return;
        }

        gameState = PlayingGameManager.sceneState;

        if (touchMotionPool == null)
            touchMotionPool = new GameObject[poolSize];

        for (int i = 0; i < poolSize; i++)
        {
            touchMotionPool[i] = Instantiate(touchMotionPrefabs, canvasObject.transform);
            touchMotionPool[i].SetActive(false);
        }
    }
}

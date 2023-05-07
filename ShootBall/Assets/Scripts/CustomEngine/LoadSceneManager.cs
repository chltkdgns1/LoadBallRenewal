using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    static public LoadSceneManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else enabled = false;
    }

    private void Start()
    {

    }

    [SerializeField]
    GameObject loadingBackPrefabs;

    GameObject loadingBack;

    bool loading = false;

    public void LoadScene(string sceneName)
    {
        Debug.Log("Start LoadScene");
        if (loading) return;

        loading = true;
        if (loadingBack == null)
        {
            CreateLoadingBack();
        }

        Debug.Log("Ready Start LoadScene");

        loadingBack.SetActive(true);
        StartCoroutine(LoadingScene(sceneName));
    }

    void CreateLoadingBack()
    {
        GameObject canvasObject = GameObject.Find("Canvas");
        if (canvasObject == null) return;

        loadingBack = Instantiate(loadingBackPrefabs, canvasObject.transform);
        loadingBack.SetActive(true);
    }

    IEnumerator LoadingScene(string sceneName)
    {
        Debug.Log("Loading Scene " + sceneName);
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        while (!op.isDone)
        {
            yield return null;          
        }
        loading = false;
    }
}

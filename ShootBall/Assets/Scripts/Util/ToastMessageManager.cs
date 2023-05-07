using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToastMessageManager : MonoBehaviour
{
    struct Element
    {
        public string message;
        public float fTime;

        public Element(string message, float fTime)
        {
            this.message = message;
            this.fTime = fTime;
        }
    }
    public static ToastMessageManager instance;

    [SerializeField]
    GameObject _toastObject;

    GameObject _toastRealObject;
    private Text messageTxt;

    private Queue<Element> messageQueue = new Queue<Element>();
    private bool _toastState;

    const int _depth = 10000;

    private void Awake()
    {
        if (instance == null) instance = this;
        else enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateToastMessageOb();
    }

    void CreateToastMessageOb()
    {
        GameObject canvasObject = GameObject.Find("Canvas");
        if (canvasObject == null) return;

        _toastRealObject = Instantiate(_toastObject, canvasObject.transform);
        messageTxt = _toastRealObject.transform.GetChild(0).GetComponent<Text>();

        _toastState = false;
        _toastRealObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_toastState == false)
        {
            if (messageQueue.Count != 0)
            {
                Element temp = messageQueue.Dequeue();
                StartCoroutine(ExcuteToastMessage(temp.message, temp.fTime));
            }
        }
    }

    public void StartToastMessage(string message, float fTime = 1.0f)
    {
        messageQueue.Enqueue(new Element(message, fTime));
    }

    public IEnumerator ExcuteToastMessage(string message, float fTime)
    {
        if (_toastRealObject == null)
        {
            CreateToastMessageOb();
        }

        _toastState = true;
        _toastRealObject.transform.SetAsLastSibling();
        _toastRealObject.SetActive(true);
        messageTxt.text = message;
        yield return new WaitForSecondsRealtime(fTime);
        _toastRealObject.SetActive(false);
        _toastState = false;
    }
}

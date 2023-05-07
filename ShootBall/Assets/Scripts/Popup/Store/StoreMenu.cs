using UnityEngine;

public class StoreMenu : MonoBehaviour
{
    [SerializeField]
    int menuIndex;

    CallBackEvent callBackEvent;

    private void Awake()
    {
        callBackEvent = GetComponent<CallBackEvent>();
    }

    public void OnClick()
    {
        callBackEvent?.action?.Invoke();
    }

    public void OnStorePopupIndex()
    {
        StorePopup.CacheIndex = menuIndex;
    }
}


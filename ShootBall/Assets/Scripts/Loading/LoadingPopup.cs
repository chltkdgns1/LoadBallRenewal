using UnityEngine;


public class LoadingPopup : MonoBehaviour
{
    private void OnEnable()
    {
        gameObject.transform.SetAsLastSibling();
    }
}

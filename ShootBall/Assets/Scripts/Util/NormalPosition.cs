using UnityEngine;

public class NormalPosition : MonoBehaviour
{
    [SerializeField]
    bool UseOption = true;
    private Vector3 localPosition;

    private void Awake()
    {
        localPosition = transform.localPosition;
    }
    // Start is called before the first frame update

    private void OnEnable()
    {
        if (UseOption)
            transform.localPosition = localPosition;
    }
}

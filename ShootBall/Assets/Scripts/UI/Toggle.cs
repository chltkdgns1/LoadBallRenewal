using UnityEngine;

public class Toggle : MonoBehaviour
{
    [SerializeField]
    GameObject onOb;

    [SerializeField]
    GameObject offOb;

    private bool _on;
    public bool On
    {
        get { return _on; }
        set
        {
            if (value)
            {
                OnToggle();
            }
            else
            {
                OffToggle();
            }
            _on = value;
        }
    }

    private void Awake()
    {

    }

    public void OnToggle()
    {
        onOb.SetActive(true);
        offOb.SetActive(false);
    }

    public void OffToggle()
    {
        onOb.SetActive(false);
        offOb.SetActive(true);
    }
}

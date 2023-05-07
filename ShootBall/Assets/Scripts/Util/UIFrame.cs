using UnityEngine;

public class UIFrame : MonoBehaviour
{
    public int normalCnt;
    public float perWidth;
    RectTransform rect;
    Vector2 deltaSize;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        deltaSize = rect.sizeDelta;
    }

    public void SetSize(string str)
    {
        if (rect == null) return;
        var size =  deltaSize + new Vector2((str.Length - normalCnt) * perWidth, 0);

        if(size.x < deltaSize.x)
        {
            size = deltaSize;
        }

        rect.sizeDelta = size;
    }
}

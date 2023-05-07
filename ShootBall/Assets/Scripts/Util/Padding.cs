using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Padding : MonoBehaviour
{
    public float top;
    public float bottom;
    public float left;
    public float right;
    public GameObject parent;

    public GameObject[] nonChangeSizeObject;
    public Dictionary<int, bool> dic = new Dictionary<int, bool>();

    private void Start()
    {
        for (int i = 0; i < nonChangeSizeObject.Length; i++)
        {
            dic[nonChangeSizeObject[i].GetHashCode()] = true;
        }

        PaddingRatio();
    }

    void PaddingValue()
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        Vector3 moveAmount = new Vector3(right - left, bottom - top);
        Vector2 sizeAmount = new Vector2(left + right, top + bottom);
        foreach (Transform child in allChildren)
        {
            child.position += moveAmount;
            if (dic.ContainsKey(child.gameObject.GetHashCode()))
                continue;

            Vector2 delta = child.GetComponent<RectTransform>().sizeDelta;
            child.GetComponent<RectTransform>().sizeDelta = delta - sizeAmount;
        }
    }

    void PaddingRatio()
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        Vector3 moveAmount = new Vector3((left - right) / 2f, (bottom - top) / 2f);
        Vector2 sizeAmount = new Vector2(left + right, top + bottom);

        float width = transform.GetComponent<RectTransform>().sizeDelta.x;
        float height = transform.GetComponent<RectTransform>().sizeDelta.y;

        foreach (Transform child in allChildren)
        {
            float xRatio = GetRatioWidth(width, child.GetComponent<RectTransform>().sizeDelta.x);
            float yRatio = GetRatioWidth(height, child.GetComponent<RectTransform>().sizeDelta.y);
            child.position += new Vector3(moveAmount.x * xRatio, moveAmount.y * yRatio);

            if (dic.ContainsKey(child.gameObject.GetHashCode()))
                continue;

            Vector2 delta = child.GetComponent<RectTransform>().sizeDelta;
            child.GetComponent<RectTransform>().sizeDelta = delta - new Vector2(sizeAmount.x * xRatio, sizeAmount.y * yRatio);
        }
    }

    float GetRatioWidth(float par, float child)
    {
        return child / par;
    }

    float GetRatioHeight(float par, float child)
    {
        return child / par;
    }
}

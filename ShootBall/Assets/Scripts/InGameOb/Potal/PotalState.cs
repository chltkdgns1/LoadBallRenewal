using UnityEngine;

public class PotalState : MonoBehaviour
{
    [SerializeField]
    public GameObject nextObject;

    public Vector3 NextPosition
    {
        get { return nextObject.transform.position; }
    }

    public GameObject NextObject
    {
        get { return nextObject; }
    }
}
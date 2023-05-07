using System;
using UnityEngine;

public class DirectionObject : MonoBehaviour
{
    public Direction _dir;
    [NonSerialized]
    public float[] _rotationArr = { 0f, 90f, 180f, 270f };
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SpriteAtalsManager : MonoBehaviour
{
    [SerializeField]
    SpriteAtlas[] altlas;

    static public SpriteAtalsManager instance = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public Sprite GetSprite(string atlasName, string spriteName)
    {
        for(int i = 0; i < altlas.Length; i++)
        {
            if(altlas[i].name == atlasName)
            {
                return altlas[i].GetSprite(spriteName);
            }
        }
        return null;
    }

}

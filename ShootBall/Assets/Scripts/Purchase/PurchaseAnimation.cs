using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class PurchaseAnimation : MonoBehaviour, IComplete
{
    [SerializeField]
    Image[] animImg;

    [SerializeField]
    private SpriteAtlas purchaseAnimAtlas;

    List<string> spriteImagePath = new List<string>();

    Animator anim;

    bool IsFinish;

    Action<object[]> act;
    object[] parameters;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        InitSprite();
    }

    void InitSprite()
    {
        for (int i = 0; i < 2; i++)
        {
            SetSprite(animImg[i], spriteImagePath[i]);
        }
    }

    private void OnEnable()
    {
        IsFinish = false;
    }

    private void Update()
    {
        if (animImg.Length < 2 || spriteImagePath.Count < 3)
        {
            enabled = false;
            return;
        }

        if (IsFinish == false && anim.GetCurrentAnimatorStateInfo(0).IsName("PurchaseAnim") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
        {
            IsFinish = true;
            SetSprite(animImg[1], spriteImagePath[2]);
            Invoke("StopActive", 1f);
        }
    }

    void StopActive()
    {
        act?.Invoke(parameters);
        IsFinish = false;
        gameObject.SetActive(false);
    }

    public void SetSpritePath(List<string> pathList)
    {
        spriteImagePath.Clear();
        foreach (var ele in pathList)
        {
            spriteImagePath.Add(ele);
        }

        InitSprite();
    }

    public void SetSprite(Image img, string spritePath)
    {
        img.sprite = purchaseAnimAtlas.GetSprite(spritePath);
    }

    public void OnComplete(Action<object[]> act, params object[] parameters)
    {
        this.act = act;
        this.parameters = parameters;
    }
}


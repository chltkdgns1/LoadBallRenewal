using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIMoneyAnimation : MonoBehaviour, IComplete
{
    [SerializeField]
    Text nomalCoinTxt;

    [SerializeField]
    Text animCoinTxt;

    Animator animCoin;

    bool IsStart;

    long _price;
    long _nowCoin;

    Action<object[]> act;
    object[] parameters;

    public void OnEnable()
    {
        animCoinTxt.gameObject.SetActive(false);
        animCoin = animCoinTxt.GetComponent<Animator>();
        IsStart = false;
        _price = 0;
    }

    public void SetNomalCoinTxt(string coin)
    {
        nomalCoinTxt.text = coin;
    }

    public IComplete StartCoinAnim(long price)
    {
        IsStart = true;
        animCoinTxt.text = "-" + UtilManager.GetStrCoin(price);
        _price = price;
        _nowCoin = GlobalData.Coin;
        animCoinTxt.gameObject.SetActive(true);
        return this;
    }

    IEnumerator SetAnimCoint(long fromCoin, long toCoin)
    {
        for (long i = fromCoin; i >= toCoin; i -= 888)
        {
            nomalCoinTxt.text = UtilManager.GetStrCoin(i);
            yield return new WaitForSeconds(0.001f);
        }

        nomalCoinTxt.text = GlobalData.StringCoin;
    }

    private void Update()
    {
        if (IsStart == true && animCoin.GetCurrentAnimatorStateInfo(0).IsName("PurchaseCoin") && animCoin.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
        {
            IsStart = false;
            Invoke("StopAnimCoinTxt", 0.1f);
        }
    }

    void StopAnimCoinTxt()
    {
        animCoinTxt.gameObject.SetActive(false);
        StartCoroutine(SetAnimCoint(_nowCoin, _nowCoin - _price));
        act?.Invoke(parameters);
    }

    public void OnComplete(Action<object[]> act, params object[] parameters)
    {
        this.act = act;
        this.parameters = parameters;
    }
}


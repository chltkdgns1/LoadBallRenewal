using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CompletePopup : PopupStack
{
    [SerializeField]
    Button nextBtn;

    [SerializeField]
    StarEffect[] startEffectGroupArr;

    [SerializeField]
    Text time;

    [SerializeField]
    Text coin;

    public bool IsUseNextBtn;

    protected override void OnEnable()
    {
        base.OnEnable();
        if (IsUseNextBtn) nextBtn.interactable = true;
        else nextBtn.interactable = false;
    }

    public void SetCompleteState(float remainTime, int coin, int startCnt)
    {
        Debug.Log("public void SetCompleteState(float remainTime, int coin, int startCnt)");
        ResetState();

        time.text = GetTimeFormat(remainTime);
        this.coin.text = coin.ToString();

        float WaitTime = 0.5f;

        for (int i = 0; i < startCnt; i++)
        {
            WaitManager.instance.StartWait(WaitTime, i, (object index) =>
            {
                startEffectGroupArr[(int)index].ExReverseStarEffect();
            });
            WaitTime += 0.5f;
        }
    }

    void ResetState()
    {
        time.text = GetTimeFormat(0f);
        coin.text = "0";

        for (int i = 0; i < startEffectGroupArr.Length; i++)
        {
            startEffectGroupArr[i].ResetReverseState();
        }
    }

    string GetTimeFormat(float time)
    {
        string min = ((int)time / 60).ToString();
        string second = (((int)time) % 60).ToString();

        return (min.Length == 1 ? "0" + min : min) + ":" + (second.Length == 1 ? "0" + second : second);
    }

    public void ExcuteOnOff()
    {
        SetOnOffBtnGroup(IsUseNextBtn);
    }

    void SetOnOffBtnGroup(bool flag)
    {
        Transform[] allChildren = nextBtn.gameObject.GetComponentsInChildren<Transform>();

        Color col = new Color(1, 1, 1, 1);
        if (flag == false) col = new Color(1, 1, 1, 0.5f);

        foreach (Transform child in allChildren)
        {
            OnOff(child, col);
        }
    }

    void OnOff(Transform child, Color col)
    {
        Image img = child.GetComponent<Image>();

        if (img != null)
        {
            img.color = col;
            return;
        }

        Text txt = child.GetComponent<Text>();

        if (txt != null)
        {
            txt.color = col;
            return;
        }
        TextMeshProUGUI txtPro = child.GetComponent<TextMeshProUGUI>();

        if (txtPro != null)
        {
            txtPro.color = col;
            return;
        }
    }

    public void LeaveGame()
    {
        InGameManager.instance?.LeaveGame();
    }

    public void RestartGame()
    {
        InGameManager.instance?.ReStartGame();
    }

    public void NextStage()
    {
        InGameManager.instance?.NextStage();
    }
}

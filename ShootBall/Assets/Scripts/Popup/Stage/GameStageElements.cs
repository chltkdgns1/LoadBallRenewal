using UnityEngine;
using UnityEngine.UI;

public class GameStageElements : MonoBehaviour
{
    public Sprite[] sprite;

    [SerializeField]
    GameObject gameLevelPopupLockStage;

    public void SetLock(int page)
    {
        int unlockPage = GlobalData.UnLockPage; // unlockPage 는 1부터 시작함.
        int nowPage = page + 1;

        if (nowPage <= unlockPage)
            gameLevelPopupLockStage.SetActive(false);
        else
            gameLevelPopupLockStage.SetActive(true);
    }

    public void SetActiveLock(bool isActive)
    {
        gameLevelPopupLockStage.SetActive(isActive);
    }

    public void Reset()
    {
        sprite = new Sprite[6];
        for (int i = 0; i < 6; i++)
        {
            sprite[0] = Resources.Load("Atlas/LobbyAtlas/Icon_Lock", typeof(Sprite)) as Sprite;
            sprite[1] = Resources.Load("Atlas/LobbyAtlas/process_line_02_n", typeof(Sprite)) as Sprite;
            sprite[2] = Resources.Load("Atlas/LobbyAtlas/progress_line_02_s", typeof(Sprite)) as Sprite;
            sprite[3] = Resources.Load("Atlas/LobbyAtlas/StarGrade_03", typeof(Sprite)) as Sprite;
            sprite[4] = Resources.Load("Atlas/LobbyAtlas/process_line_01_n", typeof(Sprite)) as Sprite;
            sprite[5] = Resources.Load("Atlas/LobbyAtlas/progress_line_01_s", typeof(Sprite)) as Sprite;
        }

        //            atlas = Resources.Load("Atlas/LobbyAtlas", typeof(SpriteAtlas)) as SpriteAtlas;

        int cnt = gameObject.transform.childCount;
        for (int i = 0; i < cnt; i++)
        {
            var child1 = transform.GetChild(i);
            int cnt1 = child1.childCount;
            for (int k = 0; k < cnt1; k++)
            {
                var child2 = child1.GetChild(k);
                int cnt2 = child2.childCount;
                for (int z = 0; z < cnt2; z++)
                {
                    var child3 = child2.GetChild(z);

                    string name = child3.name;
                    int sz = child3.childCount;

                    child3.GetChild(0).GetComponent<Image>().sprite = sprite[0];
                    child3.GetChild(1).GetComponent<Image>().sprite = sprite[1];
                    child3.GetChild(1).GetChild(0).GetComponent<Image>().sprite = sprite[2];
                    child3.GetChild(2).GetChild(0).GetComponent<Image>().sprite = sprite[3];
                    child3.GetChild(3).GetComponent<Image>().sprite = sprite[4];
                    child3.GetChild(3).GetChild(0).GetComponent<Image>().sprite = sprite[5];

                    if (child3.childCount >= 5)
                    {
                        child3.GetChild(4).GetComponent<Image>().sprite = sprite[4];
                        child3.GetChild(4).GetChild(0).GetComponent<Image>().sprite = sprite[5];
                    }

                    //child3.GetChild(0).GetComponent<Image>().sprite = atlas.GetSprite("Icon_Lock");
                    //child3.GetChild(1).GetComponent<Image>().sprite = atlas.GetSprite("process_line_02_n");
                    //child3.GetChild(1).GetChild(0).GetComponent<Image>().sprite = atlas.GetSprite("progress_line_02_s");
                    //child3.GetChild(2).GetChild(0).GetComponent<Image>().sprite = atlas.GetSprite("StarGrade_03");
                    //child3.GetChild(3).GetComponent<Image>().sprite = atlas.GetSprite("process_line_01_n");
                    //child3.GetChild(3).GetChild(0).GetComponent<Image>().sprite = atlas.GetSprite("progress_line_01_s");

                    //if (child3.childCount >= 5)
                    //{
                    //    child3.GetChild(4).GetComponent<Image>().sprite = atlas.GetSprite("process_line_01_n");
                    //    child3.GetChild(4).GetChild(0).GetComponent<Image>().sprite = atlas.GetSprite("progress_line_01_s");
                    //}
                }
            }
        }
    }
}


using UnityEngine;

public class JoyStickManager : MonoBehaviour
{
    public static JoyStickManager instance;
    JoyStick[] _joysticks;

    private void Awake()
    {
        if (instance == null) instance = this;
        else enabled = false;
    }

    void Start()
    {
        InitJoyStick();
    }

    void InitJoyStick()
    {
        GameObject joyGroup = GameObject.Find("JoyStick");
        _joysticks = new JoyStick[4];
        for (int i = 0; i < joyGroup.transform.GetChild(0).childCount; i++)
        {
            _joysticks[i] = joyGroup.transform.GetChild(0).GetChild(i).GetChild(0).GetComponent<JoyStick>();
            _joysticks[i].MovePlayerAct = OnClickJoyStick;
        }
    }

    void OnClickJoyStick(Direction dir)
    {
        InGameManager.instance.MovePlayer(dir);
    }
}

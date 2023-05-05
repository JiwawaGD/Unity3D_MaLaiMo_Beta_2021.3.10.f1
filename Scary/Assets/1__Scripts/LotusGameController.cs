using UnityEngine;
using UnityEngine.UI;

public class LotusGameController : MonoBehaviour
{
    public GameObject[] goLotusPaper;
    public Animator[] aniLotusPaper;
    public int iAllLotusState = 7;
    public int iCurrentState;
    public Text HingMsg;

    public bool[] bLotusPaperState_1;
    public int iLotusState_1 = 1;

    public bool[] bLotusPaperState_2;
    public int iLotusState_2 = 1;

    public bool[] bLotusPaperState_3;
    public int iLotusState_3 = 1;

    public bool[] bLotusPaperState_4;
    public int iLotusState_4 = 1;

    public bool[] bLotusPaperState_5;
    public int iLotusState_5 = 1;

    public bool[] bLotusPaperState_6;
    public int iLotusState_6 = 1;

    public bool[] bLotusPaperState_7;
    public int iLotusState_7 = 1;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            Lotus_State1();

        if (Input.GetKeyDown(KeyCode.D))
            Lotus_State2();

        if (Input.GetKeyDown(KeyCode.F))
            Lotus_State3();

        if (Input.GetKeyDown(KeyCode.G))
            Lotus_State4();

        if (Input.GetKeyDown(KeyCode.H))
            Lotus_State5();

        if (Input.GetKeyDown(KeyCode.J))
            Lotus_State6();

        if (Input.GetKeyDown(KeyCode.K))
            Lotus_State7();

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (NextState(iCurrentState))
            {
                iCurrentState++;

                for (int index = 0; index < iAllLotusState; index++)
                {
                    goLotusPaper[index].SetActive(false);
                }

                goLotusPaper[iCurrentState].SetActive(true);
            }
        }
    }

    void Lotus_State1()
    {
        aniLotusPaper[0].SetTrigger("Lotus_1.1");
        bLotusPaperState_1[0] = true;
    }

    void Lotus_State2()
    {
        aniLotusPaper[1].SetTrigger("Lotus_2.1");
        bLotusPaperState_2[0] = true;
    }

    void Lotus_State3()
    {
        aniLotusPaper[2].SetTrigger("Lotus_3.1");
        bLotusPaperState_3[0] = true;
    }

    void Lotus_State4()
    {
        aniLotusPaper[3].SetTrigger("Lotus_4.1");
        bLotusPaperState_4[0] = true;
    }

    void Lotus_State5()
    {
        aniLotusPaper[4].SetTrigger("Lotus_5.1");
        bLotusPaperState_5[0] = true;
    }

    void Lotus_State6()
    {
        aniLotusPaper[5].SetTrigger("Lotus_6.1");
        bLotusPaperState_6[0] = true;
    }

    void Lotus_State7()
    {
        aniLotusPaper[6].SetTrigger("Lotus_7.1");
        bLotusPaperState_7[0] = true;
    }

    bool NextState(int iCurrentState)
    {
        switch (iCurrentState)
        {
            case 0:
                if (bLotusPaperState_1[0])
                {
                    HingMsg.text = "按 : D 播放動畫";
                    return true;
                }
                return false;
            case 1:
                if (bLotusPaperState_2[0])
                {
                    HingMsg.text = "按 : F 播放動畫";
                    return true;
                }
                return false;
            case 2:
                if (bLotusPaperState_3[0])
                {
                    HingMsg.text = "按 : G 播放動畫";
                    return true;
                }
                return false;
            case 3:
                if (bLotusPaperState_4[0])
                {
                    HingMsg.text = "按 : H 播放動畫";
                    return true;
                }
                return false;
            case 4:
                if (bLotusPaperState_5[0])
                {
                    HingMsg.text = "按 : J 播放動畫";
                    return true;
                }
                return false;
            case 5:
                if (bLotusPaperState_6[0])
                {
                    HingMsg.text = "按 : K 播放動畫";
                    return true;
                }
                return false;
            case 6:
                if (bLotusPaperState_7[0])
                {
                    GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
                    gm.SendMessage("ExitLotusGame");
                    return false;
                }
                return false;
            default:
                return false;
        }
    }
}
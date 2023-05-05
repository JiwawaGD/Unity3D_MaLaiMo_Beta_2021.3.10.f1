using UnityEngine;

public class LotusGameController : MonoBehaviour
{
    public GameObject[] goLotusPaper;
    public Animator[] aniLotusPaper;
    public int iAllLotusState = 7;
    public int iCurrentState;

    public bool[] bLotusPaperState_1;
    public int iLotusState_1 = 1;

    public bool[] bLotusPaperState_2;
    public int iLotusState_2;

    public bool[] bLotusPaperState_3;
    public int iLotusState_3;

    public bool[] bLotusPaperState_4;
    public int iLotusState_4;

    public bool[] bLotusPaperState_5;
    public int iLotusState_5;

    public bool[] bLotusPaperState_6;
    public int iLotusState_6;

    public bool[] bLotusPaperState_7;
    public int iLotusState_7;

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

        if (Input.GetKeyDown(KeyCode.Z))
            Lotus_State7(0, "Lotus_7.1");

        if (Input.GetKeyDown(KeyCode.X))
            Lotus_State7(1, "Lotus_7.2");

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (NextState())
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
        if (bLotusPaperState_1[0])
            aniLotusPaper[0].SetTrigger("Lotus_1.1");
    }

    void Lotus_State2()
    {
        if (bLotusPaperState_2[0])
            aniLotusPaper[1].SetTrigger("Lotus_2.1");
    }

    void Lotus_State3()
    {
        if (bLotusPaperState_3[0])
            aniLotusPaper[2].SetTrigger("Lotus_3.1");
    }

    void Lotus_State4()
    {
        if (bLotusPaperState_4[0])
            aniLotusPaper[3].SetTrigger("Lotus_4.1");
    }

    void Lotus_State5()
    {
        if (bLotusPaperState_5[0])
            aniLotusPaper[4].SetTrigger("Lotus_5.1");
    }

    void Lotus_State6()
    {
        if (bLotusPaperState_6[0])
            aniLotusPaper[5].SetTrigger("Lotus_6.1");
    }

    void Lotus_State7(int iCount, string sAniName)
    {
        if (bLotusPaperState_7[iCount])
        {
            aniLotusPaper[6].SetTrigger(sAniName);
            bLotusPaperState_7[iCount + 1] = true;
        }
    }

    bool NextState()
    {
        for (int index = 0; index < iLotusState_1; index++)
        {
            if (bLotusPaperState_1[index])
            {
                return true;
            }
            return false;
        }
        return false;
    }
}
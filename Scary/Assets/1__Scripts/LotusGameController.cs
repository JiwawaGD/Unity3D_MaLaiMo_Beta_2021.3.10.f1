using UnityEngine;
using UnityEngine.UI;

public class LotusGameController : MonoBehaviour
{
    public GameObject[] LotusPaperObj;
    Animator[] LotusPaperAni;
    bool[] bLotusPaperState;

    int iAllLotusState = 7;
    int iCurrentState;

    [Range(5, 29)]
    int iAniState = 29;

    public Shadow hintMessageShadow;

    readonly string[] sAniTriggerName = new string[30]
    {
        "State_1",
        "State_2",
        "State_3",
        "State_4",
        "State_5",
        "State_6",
        "State_7-24",
        "State_7-23",
        "State_7-22",
        "State_7-21",
        "State_7-20",
        "State_7-19",
        "State_7-18",
        "State_7-17",
        "State_7-16",
        "State_7-15",
        "State_7-14",
        "State_7-13",
        "State_7-12",
        "State_7-11",
        "State_7-10",
        "State_7-9",
        "State_7-8",
        "State_7-7",
        "State_7-6",
        "State_7-5",
        "State_7-4",
        "State_7-3",
        "State_7-2",
        "State_7-1",
    };

    void Start()
    {
        LotusPaperAni = new Animator[7];
        bLotusPaperState = new bool[7];

        for (int i = 0; i < iAllLotusState; i++)
        {
            LotusPaperAni[i] = LotusPaperObj[i].GetComponent<Animator>();
            bLotusPaperState[i] = false;
        }

        iAllLotusState = 7;
        iCurrentState = 0;
        iAniState = 29;

        if (hintMessageShadow == null)
        {
            hintMessageShadow = GameObject.Find("Canvas/HintMsg").GetComponent<Shadow>();
        }
    }

    void Update()
    {
        switch (iCurrentState)
        {
            case 0:
                if (Input.GetKeyDown(KeyCode.A))
                    LotusPaperAni[0].SetTrigger(sAniTriggerName[0]);

                break;
            case 1:
                if (Input.GetKeyDown(KeyCode.A))
                    LotusPaperAni[1].SetTrigger(sAniTriggerName[1]);

                break;
            case 2:
                if (Input.GetKeyDown(KeyCode.A))
                    LotusPaperAni[2].SetTrigger(sAniTriggerName[2]);

                break;
            case 3:
                if (Input.GetKeyDown(KeyCode.A))
                    LotusPaperAni[3].SetTrigger(sAniTriggerName[3]);

                break;
            case 4:
                if (Input.GetKeyDown(KeyCode.A))
                    LotusPaperAni[4].SetTrigger(sAniTriggerName[4]);

                break;
            case 5:
                if (Input.GetKeyDown(KeyCode.A))
                    LotusPaperAni[5].SetTrigger(sAniTriggerName[5]);

                break;
            case 6:
                if (Input.GetKeyDown(KeyCode.A))
                {
                    if (iAniState == 5)
                        break;

                    LotusPaperAni[6].SetTrigger(sAniTriggerName[iAniState]);
                    iAniState--;
                }
                break;
            default:
                break;
        }

        if (Input.GetKeyDown(KeyCode.P) && NextState(iCurrentState))
        {
            iCurrentState++;

            if (iCurrentState == 7)
            {
                GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
                gm.SendMessage("ExitLotusGame");
                GlobalDeclare.bLotusGameComplete = true;
                return;
            }

            for (int i = 0; i < iAllLotusState; i++)
                LotusPaperObj[i].transform.position = new Vector3(0, 0, 1);

            LotusPaperObj[iCurrentState].transform.position = Vector3.zero;
            hintMessageShadow.enabled = false;
        }

        CheckLotusState();
    }

    bool NextState(int iCurrentState)
    {
        if (bLotusPaperState[iCurrentState])
        {
            return true;
        }
        return false;
    }

    void CheckLotusState()
    {
        for (int index = 0; index < iAllLotusState; index++)
        {
            if (!bLotusPaperState[index])
                CheckLotusAni(LotusPaperAni[index], sAniTriggerName[index], ref bLotusPaperState[index]);
        }
    }

    void CheckLotusAni(Animator LotusAni, string sAniName, ref bool bLotusState)
    {
        AnimatorStateInfo LotusState = LotusAni.GetCurrentAnimatorStateInfo(0);

        if (LotusState.IsName(sAniName) && LotusState.normalizedTime >= 1.0f)
        {
            bLotusState = true;
            hintMessageShadow.GetComponent<Shadow>().enabled = true;
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

public class LotusGameController : MonoBehaviour
{
    public GameObject[] goLotusPaper = new GameObject[7];
    public Animator[] aniLotusPaper = new Animator[7];
    public int iAllLotusState = 7;
    public int iCurrentState;
    public Text HingMsg;

    public bool[] bLotusPaperState = new bool[7];

    void Start()
    {
        for (int i = 0; i < iAllLotusState; i++)
        {
            aniLotusPaper[i] = goLotusPaper[i].GetComponent<Animator>();
            bLotusPaperState[i] = false;
        }
    }

    void Update()
    {
        switch (iCurrentState)
        {
            case 0:
                if (Input.GetKeyDown(KeyCode.A))
                {
                    ProcessLotusAnimator(aniLotusPaper[0], "State_1", ref bLotusPaperState[0]);
                }
                break;
            case 1:
                if (Input.GetKeyDown(KeyCode.A))
                {
                    ProcessLotusAnimator(aniLotusPaper[1], "State_2", ref bLotusPaperState[1]);
                }
                break;
            case 2:
                if (Input.GetKeyDown(KeyCode.A))
                {
                    ProcessLotusAnimator(aniLotusPaper[2], "State_3", ref bLotusPaperState[2]);
                }
                break;
            case 3:
                if (Input.GetKeyDown(KeyCode.A))
                {
                    ProcessLotusAnimator(aniLotusPaper[3], "State_4", ref bLotusPaperState[3]);
                }
                break;
            case 4:
                if (Input.GetKeyDown(KeyCode.A))
                {
                    ProcessLotusAnimator(aniLotusPaper[4], "State_5", ref bLotusPaperState[4]);
                }
                break;
            case 5:
                if (Input.GetKeyDown(KeyCode.A))
                {
                    ProcessLotusAnimator(aniLotusPaper[5], "State_6", ref bLotusPaperState[5]);
                }
                break;
            case 6:
                if (Input.GetKeyDown(KeyCode.A))
                {
                    ProcessLotusAnimator(aniLotusPaper[6], "State_7-1", ref bLotusPaperState[6]);
                }
                break;
            default:
                break;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (NextState(iCurrentState))
            {
                iCurrentState++;
            }
        }
    }

    void ProcessLotusAnimator(Animator LotusObj, string sTriggerName, ref bool bLotusState)
    {
        LotusObj.SetTrigger(sTriggerName);

        AnimatorStateInfo info = LotusObj.GetCurrentAnimatorStateInfo(0);

        if (info.IsName(sTriggerName) && info.normalizedTime > 0.99f)
        {
            bLotusState = true;
        }
    }

    bool NextState(int iCurrentState)
    {
        if (bLotusPaperState[iCurrentState])
        {
            return true;
        }
        return false;
    }
}
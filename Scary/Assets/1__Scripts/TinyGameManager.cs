using UnityEngine;
using UnityEngine.Video;

public partial class GameManager : MonoBehaviour
{
    public void IvkShowGrandmaFaceUI()
    {
        imgUIBackGround.color = new Color(1, 1, 1, 1);
        Sprite GrandmaFaceSprite = Resources.Load<Sprite>("Sprites/GrandmaFace");
        imgUIBackGround.sprite = GrandmaFaceSprite;
        Invoke(nameof(IvkShowEndView), 5f);
    }

    public void IvkShowEndView()
    {
        imgUIBackGround.color = new Color(1, 1, 1, 1);
        Sprite EndViewSprite = Resources.Load<Sprite>("Sprites/EndView");
        imgUIBackGround.sprite = EndViewSprite;
        m_bReturnToBegin = true;
        playerCtrlr.m_bCanControl = true;
    }

    public void IvkShowDoorKey()
    {
        ShowHint(HintItemID.S1_Grandma_Room_Key);
    }

    public void IvkProcessGhostHandPushAnimator()
    {
        ProcessItemAnimator("Ghost_Hand", "Ghost_Hand_Push");
        Invoke(nameof(IvkProcessPlayerWakeUpSecondTime), 3.5f);

        ShowHint(HintItemID.S2_Room_Door);
        ShowHint(HintItemID.S2_Light_Switch);
        ShowHint(HintItemID.S2_FlashLight);
        ShowHint(HintItemID.S2_Side_Table);
    }

    public void IvkProcessPlayerWakeUpSecondTime()
    {
        Light playerFlashlight = playerCtrlr.tfPlayerCamera.GetComponent<Light>();
        playerFlashlight.enabled = false;
        audManager.Play(1, "Falling_To_Black_Screen_Sound_Part2", false);
        Animation am = playerCtrlr.GetComponent<Animation>();
        am.Stop();
        ProcessPlayerAnimator("Player_Wake_Up_SecondTime");
    }

    public void IvkShowS2DoorKey()
    {
        ShowHint(HintItemID.S2_Room_Key);
    }

    void IvkS1_SetGhostHandPosition()
    {
        // 鬼手出現
        GameObject GhostHandObj = GameObject.Find("Ghost_Hand");
        GhostHandObj.transform.position = new Vector3(-8.5f, 0, 6);

        // 鬼門觸發
        GameObject GhostHandTriggerObj = GameObject.Find("Ghost_Hand_Trigger");
        GhostHandTriggerObj.transform.position = new Vector3(-8.5f, 0.1f, 6.1f);

        ShowHint(HintItemID.S1_Toilet_GhostHand_Trigger);
    }

    public void IvkS1_SetGrandmaGhostPosition()
    {
        S2_Grandma_Ghost_Obj.GetComponent<Animator>().applyRootMotion = true;
        S2_Grandma_Ghost_Obj.transform.localPosition = new Vector3(-8f, -2f, 2.35f);
    }

    public void IvkS2_Grandma_Pass_Door()
    {
        S2_Grandma_Ghost_Obj.GetComponent<Animator>().applyRootMotion = true;
        ShowHint(HintItemID.S2_Toilet_Door);
        audManager.Play(1, "grandma_StrangeVoice", false);
    }

    public void IvkS2_Shocked_By_Toilet()
    {
        S2_Toilet_Door_GhostHead_Obj.GetComponent<Animator>().SetTrigger("S2_Toilet_Door_GhostHead_Scared");
    }

    void OnVideoPlayerStarted(VideoPlayer vp)
    {
        RawImgGrandmaUI.enabled = true;
        vp.started -= OnVideoPlayerStarted;
    }

    public void IvkS2_PlayGrandmaVideo()
    {
        videoPlayer.Play();
    }

    public void IvkS2_SlientAfterPhotoFrameForRecord()
    {
        videoPlayer.Stop();
        RawImgGrandmaUI.enabled = false;
        FinalUI.SetActive(true);
        bIsGameEnd = true;
    }

    void IvkLv2_BrokenPhotoFrameEnable()
    {
        ShowHint(HintItemID.S1_Photo_Frame);
        TempItem = S1_Photo_Frame_Obj.GetComponent<ItemController>();
        TempItem.eventID = GameEventID.S1_Photo_Frame_Has_Broken;
    }
}
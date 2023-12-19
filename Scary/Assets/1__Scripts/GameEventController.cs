using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    void S1_PhotoFrameEvent()
    {
        ShowHint(HintItemID.S1_Photo_Frame);
        audManager.Play(1, "mirror_Breaking_Sound", false);
        S1_Photo_Frame_Obj.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = true;
        TempItem = S1_Photo_Frame_Obj.GetComponent<ItemController>();
        TempItem.eventID = GameEventID.S1_Photo_Frame_Has_Broken;
    }

    void S1_PhotoFrameHasBroken()
    {
        audManager.Play(1, "get_Item_Sound", false);
        UIState(UIItemID.S1_Photo_Frame, true);
        ProcessRoMoving(2);
        ShowObj(ObjItemID.S1_Photo_Frame);

        // 人形黑影
        ProcessAnimator("Toilet_Door_Ghost", "Toilet_Door_Ghost_In");
        audManager.Play(1, "ghost_In_Door", false);
        m_bToiletGhostHasShow = true;

        // 限制角色視角
        //m_bSetPlayerViewLimit = true;
        //GlobalDeclare.PlayerCameraLimit.SetPlayerCameraLimit(150f, 250f, 160f);
    }

    void S1_GrandmaDoorOpen()
    {
        ProcessAnimator("Grandma_Room_Door", "DoorOpen");
        audManager.Play(1, "the_sound_of_the_old_door_opening", false);
        ShowHint(HintItemID.S1_Rice_Funeral);
        flowchartObjects[4].gameObject.SetActive(true);
    }

    void S1_LotusPaper()
    {
        ProcessRoMoving(1);
        UIState(UIItemID.S1_Lotus_Paper, true);
        ShowEnterGame(true);
        ShowObj(ObjItemID.S1_Lotus_Paper);
        audManager.Play(1, "gold_Paper", false);
        audManager.Play(1, "get_Item_Sound", false);
    }

    void S1_GrandmaDeadBody()
    {
        S1_Rice_Funeral_Obj.SetActive(false);
        audManager.Play(1, "get_Item_Sound", false);
        flowchartObjects[6].gameObject.SetActive(true);
        StopReadding();

        // 生成摔壞的腳尾飯
        UnityEngine.Object RiceFuneralSpilled = Resources.Load<GameObject>("Prefabs/Rice_Funeral_Spilled");
        GameObject RiceFuneralSpilledObj = Instantiate(RiceFuneralSpilled) as GameObject;
        RiceFuneralSpilledObj.transform.parent = GameObject.Find("===== ITEMS =====").transform;
        RiceFuneralSpilledObj.transform.position = new Vector3(-4.4f, 0.006f, 11.8f);
        RiceFuneralSpilledObj.name = "Rice_Funeral_Spilled";
        ShowHint(HintItemID.S1_Rice_Funeral_Spilled);
    }

    void S1_WhiteTent()
    {
        audManager.Play(1, "filial_Piety_Curtain", false);
        ProcessAnimator("Filial_Piety_Curtain", "Filial_piety_curtain Open");
        TempBoxCollider = GameObject.Find("Filial_Piety_Curtain").GetComponent<BoxCollider>();
        TempBoxCollider.enabled = false;
        ShowHint(HintItemID.S1_Lie_Grandma_Body);
    }

    void S1_PhotoFrameLightOn()
    {
        audManager.Play(1, "flashlight_Switch_Sound", false);
        goPhotoFrameLight.SetActive(true);
        m_bPhotoFrameLightOn = false;
    }

    void S1_GrandmaRush()
    {
        audManager.Play(1, "grandma_Starts_Walking", false);
        playerCtrlr.m_bCanControl = false;
        m_bGrandmaRush = false;
    }

    void S1_LightSwitch()
    {
        audManager.Play(1, "light_Switch_Sound", false);
        ShowHint(HintItemID.S1_Flashlight);
        flowchartObjects[2].gameObject.SetActive(true);
        bS1_IsS1LightSwtichOK = true;
    }

    void S1_Flashlight()
    {
        bS1_TriggerFlashlight = true;
        ShowHint(HintItemID.S1_Desk_Drawer);
        Light playerFlashlight = playerCtrlr.tfPlayerCamera.GetComponent<Light>();
        playerFlashlight.enabled = true;
        audManager.Play(1, "light_Switch_Sound", false);
        GameObject FlashLight = GameObject.Find("Flashlight");
        Destroy(FlashLight);
    }

    void S1_DeskDrawer()
    {
        audManager.Play(1, "drawer_Opening_Sound", false);
        TempBoxCollider = GameObject.Find("grandpa_desk/Desk_Drawer").GetComponent<BoxCollider>();
        TempBoxCollider.enabled = false;
        ProcessAnimator("grandpa_desk/Desk_Drawer", "DrawerWithKey_Open");
        TempGameObject = GameObject.Find("Grandma_Room_Key");
        TempGameObject.GetComponent<Animation>().Play();
        Invoke(nameof(IvkShowDoorKey), 1.2f);
    }

    void S1_GrandmaRoomKey()
    {
        ShowHint(HintItemID.S1_Grandma_Room_Door);
        audManager.Play(1, "tet_Sound_Of_Get_The_Key", false);
        flowchartObjects[3].gameObject.SetActive(true);
        GameObject GrandmaRoomKeyObj = GameObject.Find("Grandma_Room_Key");
        Destroy(GrandmaRoomKeyObj);
    }

    void S1_GrandmaRoomDoorLock()
    {
        bS1_TriggerGrandmaDoorLock = true;
        ShowHint(HintItemID.S1_Desk_Drawer);
        ShowHint(HintItemID.S1_Flashlight);
        flowchartObjects[1].gameObject.SetActive(true);
        audManager.Play(1, "the_door_is_locked_and_cannot_be_opened_with_sound_effects", false);
    }

    void S1_RiceFuneralSpilled()
    {
        audManager.Play(1, "get_Item_Sound", false);
        ShowHint(HintItemID.S1_Lotus_Paper);
        m_bPlayLotusEnable = true;
        flowchartObjects[8].gameObject.SetActive(true);
    }

    void S1_RiceFuneral()
    {
        audManager.Play(1, "get_Item_Sound", false);
        ShowHint(HintItemID.S1_Filial_Piety_Curtain);
        flowchartObjects[11].gameObject.SetActive(true);
        UIState(UIItemID.S1_Rice_Funeral, true);
        ShowObj(ObjItemID.S1_Rice_Funeral);
        ProcessRoMoving(0);
    }

    void S1_ToiletDoorLock()
    {
        audManager.Play(1, "the_door_is_locked_and_cannot_be_opened_with_sound_effects", false);
        flowchartObjects[12].gameObject.SetActive(true);
    }

    void S1_ToiletDoorOpen()
    {
        audManager.Play(1, "the_toilet_door_opens", false);
        ProcessAnimator("Toilet_Door_Ghost", "Toilet_Door_Open");
        BoxCollider ToiletDoorCollider = GameObject.Find("Toilet_Door_Ghost").GetComponent<BoxCollider>();
        ToiletDoorCollider.enabled = false;
        ShowHint(HintItemID.S1_Photo_Frame);
    }

    void S1_ToiletGhostHide()
    {
        ProcessAnimator("Toilet_Door_Ghost", "Toilet_Door_Ghost_Out");
        m_bToiletGhostHasShow = false;
        playerCtrlr.m_bLimitRotation = false;
        m_bWaitToiletGhostHandPush = true;
        GlobalDeclare.PlayerCameraLimit.ClearValue();
        ShowHint(HintItemID.S1_Toilet_GhostHand_Trigger);

        Invoke(nameof(IvkS1_SetGhostHandPosition), 2f);

        GameObject GhostHandTriggerObj = GameObject.Find("Ghost_Hand_Trigger");
        GhostHandTriggerObj.transform.position = new Vector3(-8.5f, 0.1f, 6.1f);
    }

    void S1_ToiletGhostHandPush()
    {
        audManager.Play(1, "Falling_To_Black_Screen_Sound", false);
        m_bWaitToiletGhostHandPush = false;

        ProcessPlayerAnimator("Player_Falling_In_Bathroom");
        Invoke(nameof(IvkProcessGhostHandPushAnimator), 3.95f);
    }
}
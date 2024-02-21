using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    // public GameObject audManagerPrefab;
    public static float MouseSensitivity = 1.0f;
    public Transform ro_tfItemObj;  // 旋轉物件

    private Vector3 originalCameraPosition; // 原始攝影機位置

    [SerializeField][Header("Item 的圖層")] LayerMask ItemLayer;

    public AudioSource audioSource;    // 音效來源
    public AudioClip walkingSound;    // 走路音效

    private bool isWalking = false; // 是否正在走路

    // Can be setted by player
    float m_fUDSensitivity; // 上下轉速
    float m_fRLSensitivity; // 左右轉速
    public float fSensitivityAmplifier;

    // Const value  
    readonly float m_fMoveSpeed = 90f;
    readonly float m_fRayLength = 1.2f;
    readonly int m_iInteractiveLayer = 10;  // 互動圖層
    readonly Vector3 v3_zero = Vector3.zero;

    public bool m_bLimitRotation = false;
    float m_fHorizantalRotationValue;
    public Vector2 m_fHorizantalRotationRange;
    float m_fVerticalRotationValue;
    public Vector2 m_fVerticalRotationRange;

    [HideInInspector] public bool m_bCursorShow;
    [HideInInspector] public bool m_bCanControl;
    [HideInInspector] public bool m_bRayOnItem;

    Vector3 v3_MoveValue;
    Vector3 v3_MovePos;

    public Transform tfPlayerCamera;
    public Transform tfTransform;
    Rigidbody rig;
    RaycastHit hit;
    RaycastHit hit2;
    Animation ani;

    ItemController current_Item;
    ItemController last_Item;
    GameManager gameManager;

    public bool StartMovingToTarget = false;
    public Transform Target;
    void Awake()
    {
        // audioSource = GetComponent<AudioSource>();
        // GameObject audManagerObject = Instantiate(audManagerPrefab, transform);
        // audManagerObject.name = "AUDManager"; // 可以設置生成物件的名稱
        rig = GetComponent<Rigidbody>();
        ani = GetComponent<Animation>();
        tfTransform = transform;

        if (tfPlayerCamera == null)
            tfPlayerCamera = GameObject.Find("Player Camera").transform;

        if (gameManager == null)
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Start()
    {
        originalCameraPosition = tfPlayerCamera.localPosition;
        InitValue();
        SetCursor();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(tfPlayerCamera.position, tfPlayerCamera.position + (tfPlayerCamera.forward * m_fRayLength));
    }

    void Update()
    {
        RayHitCheck();

        if (!GameManager.m_bInUIView)   // 不在 UI 畫面時才可控制
        {
            if (Input.GetKeyDown(KeyCode.F6))
                SetCursor();
        }
        else    // 在 UI 畫面時不可控制
        {
            if (Input.GetKeyDown(KeyCode.F6))
                SetCursor();
        }
    }

    public void SetMouseSensitivity(float sensitivity)
    {
        MouseSensitivity = sensitivity;
        // 在這裡處理滑鼠靈敏度的邏輯
        // 例如，更新相應的變數，調整滑鼠靈敏度
    }
    void FixedUpdate()
    {
        MoveToTarget();
        if (m_bCursorShow)  // 滑鼠顯示時不可控制
            return;

        if (!m_bCanControl) // 無法控制時不可控制
            return;

        if (ani.isPlaying)  // 播放動畫時不可控制
        {
            m_fVerticalRotationValue = 0;
            m_fHorizantalRotationValue = 0;
            return;
        }

        if (isWalking)  // 播放走路音效
        {
            if (!audioSource.isPlaying) // 音效未播放時播放音效
            {
                PlayWalkingSound();
            }
        }
        else
        {
            if (audioSource.isPlaying)  // 音效播放時停止音效
            {
                //tfPlayerCamera.localPosition = originalCameraPosition;
                audioSource.Stop();
            }
        }

        if(StartMovingToTarget) return;// 執行區域移動時不能控制
        Move();
        View();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("GameEventTrigger"))
        {
            if (col.gameObject.name == "S1_Grandma_Pass_Door_Trigger")
            {
                gameManager.SendMessage("GameEvent", GameEventID.S1_Grandma_Pass_Door_After_RiceFurnel);
                col.transform.localPosition -= new Vector3(0f, 10f, 0f);
            }

            //if (GameManager.m_bToiletGhostHasShow &&
            //    col.gameObject.name == "Toilet_Ghost_Hide")
            //{
            //    gameManager.SendMessage("GameEvent", GameEventID.S1_Toilet_Ghost_Hide);
            //    col.transform.localPosition -= new Vector3(0f, 10f, 0f);
            //}

            if (col.gameObject.name == "S2_Door_Knock_Trigger")
            {
                //gameManager.SendMessage("GameEvent", GameEventID.S2_Door_Knock_Stop);
                col.transform.localPosition -= new Vector3(0f, 10f, 0f);
            }

            if (col.gameObject.name == "S2_Door_Close_Trigger")
            {
                gameManager.SendMessage("GameEvent", GameEventID.S2_Grandma_Door_Close);
                col.transform.localPosition -= new Vector3(0f, 10f, 0f);
            }

            if (col.gameObject.name == "S2_Ghost_Pass_Door_Trigger")
            {
                gameManager.SendMessage("GameEvent", GameEventID.S2_Ghost_Pass_Door);
                col.transform.localPosition -= new Vector3(0f, 10f, 0f);
            }
        }
    }

    void InitValue()    // 初始化數值
    {
        m_fUDSensitivity = 230;
        m_fRLSensitivity = 180;

        fSensitivityAmplifier = GlobalDeclare.fSensitivity;

        if (fSensitivityAmplifier == 0)
            fSensitivityAmplifier = 0.5f;

        m_bCursorShow = false;
        m_bCanControl = true;

        audioSource.volume = 1f;
        audioSource.loop = false; // 設置是否循環播放音效
    }

    void View() // 視角
    {
        // 左右轉 (只轉 *角色* )
        if (m_bLimitRotation)
        {
            m_fHorizantalRotationValue += Input.GetAxis("Mouse X") * m_fRLSensitivity * fSensitivityAmplifier * MouseSensitivity * Time.deltaTime;
            m_fHorizantalRotationValue = Mathf.Clamp(m_fHorizantalRotationValue, m_fHorizantalRotationRange.x, m_fHorizantalRotationRange.y);

            tfTransform.localEulerAngles = Vector3.up * m_fHorizantalRotationValue;
        }
        else
        {
            tfTransform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * m_fRLSensitivity * fSensitivityAmplifier * MouseSensitivity * Time.deltaTime);
        }

        // 上下轉 (只轉 *攝影機* )
        m_fVerticalRotationValue += Input.GetAxis("Mouse Y") * m_fUDSensitivity * fSensitivityAmplifier * MouseSensitivity * Time.deltaTime;
        m_fVerticalRotationValue = Mathf.Clamp(m_fVerticalRotationValue, -75, 75);

        tfPlayerCamera.localEulerAngles = -Vector3.right * m_fVerticalRotationValue;
    }

    void Move() // 移動
    {
        v3_MoveValue = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        v3_MovePos.x = v3_MoveValue.x * Time.deltaTime * m_fMoveSpeed;
        v3_MovePos.z = v3_MoveValue.z * Time.deltaTime * m_fMoveSpeed;

        v3_MovePos = tfTransform.right * v3_MovePos.x + tfTransform.forward * v3_MovePos.z;

        if (v3_MovePos != v3_zero)
        {
            rig.velocity = v3_MovePos;
            isWalking = true;
        }
        else
        {
            rig.velocity = v3_zero;
            isWalking = false;
        }
    }

    public void MoveToTarget()
    {
        if (StartMovingToTarget == false || Target == null) return;

        if (Target.position == transform.position)
        {
            //// 計算目標旋轉角度
            //Quaternion targetRotationX = Quaternion.LookRotation(new Vector3(35.535f, 0, 0));
            //// 使用 Slerp 平滑旋轉到目標角度
            //tfPlayerCamera.rotation = Quaternion.Slerp(transform.rotation, targetRotationX, m_fMoveSpeed * Time.deltaTime);
            if (tfPlayerCamera.rotation.x == 35.535f && transform.rotation.y == 1.435f) StartMovingToTarget = false;
        }
        else
        {
            // 計算朝向目標的方向
            Vector3 targetDirection = (Target.position - transform.position);
            //tfPlayerCamera.LookAt(new Vector3(Target.GetChild(0).position.x, 0 ,0));
            transform.LookAt(Target.position);

            transform.position += targetDirection * Time.deltaTime;
        }
    }

    public void SetCursor() // 設定滑鼠
    {
        m_bCursorShow = !m_bCursorShow;

        if (m_bCursorShow)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = m_bCursorShow;
    }

    // Ray check for item interact
    void RayHitCheck()  // 檢查射線是否打到物件
    {
        m_bRayOnItem = Physics.Raycast(tfPlayerCamera.position,     // Origin
                                       tfPlayerCamera.forward,      // Direction
                                       out hit,                     // RaycastHit
                                       m_fRayLength,                // RayLength
                                       ItemLayer);                  // ItemLayer);

        if (m_bRayOnItem)
        {
            current_Item = hit.transform.gameObject.GetComponent<ItemController>();

            if (current_Item.bActive)
            {
                current_Item.SetItemInteractive(true);

                last_Item = current_Item;

                if (Input.GetKeyDown(KeyCode.E))
                    current_Item.SendGameEvent();
            }
            else if (last_Item)
            {
                last_Item.SetItemInteractive(false);
            }
        }
        else
        {
            if (last_Item)
                last_Item.SetItemInteractive(false);
        }
    }

    void PlaySound(AudioClip clip)  // 播放音效
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    void PlayWalkingSound() // 播放走路音效
    {
        PlaySound(walkingSound);
    }

}
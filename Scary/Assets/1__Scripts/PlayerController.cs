using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float cameraSwaySmoothing = 10f;
    public float cameraSwayAmount = 0.5f;
    public float cameraSwaySpeed = 1.5f;

    private Vector3 originalCameraPosition;
    //private Vector3 cameraSwayVelocity;

    [SerializeField] private float verticalSwayAmount = 0.02f; // 上下起伏的幅度
    [SerializeField] private float verticalSwaySpeed = 3f; // 上下起伏的速度

    private float verticalSwayTimer;

    private AudioSource audioSource;
    public AudioClip walkingSound;

    private bool isWalking = false;

    // Can be setted by player
    float m_fUDSensitivity;
    float m_fRLSensitivity;
    public float fSensitivityAmplifier;

    // Const value
    readonly float m_fMoveSpeed = 90f;
    readonly float m_fRayLength = 1.5f;
    readonly int m_iInteractiveLayer = 10;
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
    Animation ani;

    ItemController current_Item;
    ItemController last_Item;
    GameManager gameManager;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
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

        if (!GameManager.m_bInUIView)
        {
            if (Input.GetKeyDown(KeyCode.F6))
                SetCursor();
        }
    }

    void FixedUpdate()
    {
        if (m_bCursorShow)
            return;

        if (!m_bCanControl)
            return;

        if (ani.isPlaying)
        {
            m_fVerticalRotationValue = 0;
            m_fHorizantalRotationValue = 0;
            return;
        }

        if (isWalking)
        {
            if (!audioSource.isPlaying)
            {
                //float swayX = Mathf.Sin(Time.time * cameraSwaySpeed) * cameraSwayAmount;
                //float swayY = Mathf.Sin(Time.time * cameraSwaySpeed * 2f) * cameraSwayAmount;

                //// 上下起伏
                //float verticalSway = Mathf.Sin(verticalSwayTimer) * verticalSwayAmount;
                //verticalSwayTimer += Time.deltaTime * verticalSwaySpeed;

                //Vector3 swayPosition = originalCameraPosition + new Vector3(swayX, swayY + verticalSway, 0f);
                //tfPlayerCamera.localPosition = swayPosition;
                PlayWalkingSound();
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                //tfPlayerCamera.localPosition = originalCameraPosition;
                audioSource.Stop();
            }
        }

        Move();
        View();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("GameEventTrigger"))
        {
            if (GameManager.m_bPhotoFrameLightOn)
                gameManager.SendMessage("GameEvent", GameEventID.S1_Photo_Frame_Light_On);

            if (GameManager.m_bGrandmaRush)
                gameManager.SendMessage("GameEvent", GameEventID.S1_Grandma_Rush);

            if (GameManager.m_bToiletGhostHasShow)
                gameManager.SendMessage("GameEvent", GameEventID.S1_Toilet_Ghost_Hide);
        }
    }

    void InitValue()
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

    void View()
    {
        // 左右轉
        if (m_bLimitRotation)
        {
            m_fHorizantalRotationValue += Input.GetAxis("Mouse X") * m_fRLSensitivity * fSensitivityAmplifier * Time.deltaTime;
            m_fHorizantalRotationValue = Mathf.Clamp(m_fHorizantalRotationValue, m_fHorizantalRotationRange.x, m_fHorizantalRotationRange.y);

            tfTransform.localEulerAngles = Vector3.up * m_fHorizantalRotationValue;
        }
        else
        {
            tfTransform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * m_fRLSensitivity * fSensitivityAmplifier * Time.deltaTime);
        }

        // 上下轉
        m_fVerticalRotationValue += Input.GetAxis("Mouse Y") * m_fUDSensitivity * fSensitivityAmplifier * Time.deltaTime;
        m_fVerticalRotationValue = Mathf.Clamp(m_fVerticalRotationValue, -75, 75);

        tfPlayerCamera.localEulerAngles = -Vector3.right * m_fVerticalRotationValue;
    }

    void Move()
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

    public void SetCursor()
    {
        m_bCursorShow = !m_bCursorShow;

        if (m_bCursorShow)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = m_bCursorShow;
    }

    // Ray check for item interact
    void RayHitCheck()
    {
        m_bRayOnItem = Physics.Raycast(tfPlayerCamera.position,     // Origin
                                       tfPlayerCamera.forward,      // Direction
                                       out hit,                     // RaycastHit
                                       m_fRayLength);               // RayLength

        if (m_bRayOnItem && hit.transform.gameObject.layer == m_iInteractiveLayer)
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
    void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
    void PlayWalkingSound()
    {
        PlaySound(walkingSound);
    }

}
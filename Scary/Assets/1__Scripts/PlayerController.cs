using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    float verticalRotationSpeed = 45f; // 垂直旋轉速度
    bool isRotatingHorizontally = true; // 是否正在水平旋轉
    bool isRotatingVertically = false; // 是否正在垂直旋轉
    bool rotateOnXAxis = true; // 是否繞X軸旋轉，預設為true
    float rotationSpeed = 45f; // 初始旋轉速度
    public Transform ro_tfItemObj;

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
    readonly float ro_ItemObjRayLenght = 1.5f;
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
    RaycastHit hit2;
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
            if (GameManager.m_bToiletGhostHasShow && 
                col.gameObject.name == "Toilet_Ghost_Hide")
            {
                gameManager.SendMessage("GameEvent", GameEventID.S1_Toilet_Ghost_Hide);
                Destroy(col.gameObject);
            }

            if (GameManager.m_bWaitToiletGhostHandPush && 
                col.gameObject.name == "Toilet_Ghost_Hand_Push")
            {
                gameManager.SendMessage("GameEvent", GameEventID.S1_Toilet_Ghost_Hand_Push);
                Destroy(col.gameObject);
            }
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

        if (gameManager.GetM_bInUIView())
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                StartRotationTimer();
            }
            if (Input.GetKey(KeyCode.L))
            {
                // 檢查是否有擊中物體，以及要旋轉的物體不是 null
                if (hit.transform != null)
                {
                    RotateCubeHorization(hit.transform);
                }
            }
            if (Input.GetKeyUp(KeyCode.L))
            {
                StopRotationTimer();
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                StartRotationTimer();
                isRotatingVertically = true; // 開始垂直旋轉
            }
            if (Input.GetKey(KeyCode.K))
            {
                // 檢查要垂直旋轉的物體不是 null
                if (isRotatingVertically && gameManager.RO_OBJ[gameManager.saveRotaObj] != null)
                {
                    RotateCubeVertically(gameManager.RO_OBJ[gameManager.saveRotaObj].transform, verticalRotationSpeed);
                }
            }
            if (Input.GetKeyUp(KeyCode.K))
            {
                StopRotationTimer();
                isRotatingVertically = false; // 停止垂直旋轉
            }

        }
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

    float rotationTimer = 0f;
    void RotateCubeHorization(Transform cubeTransform)
    {
        // 這裡假設最大旋轉速度為 360 度/秒
        float maxRotationSpeed = 360f;

        // 旋轉角度根據持續時間來決定
        float rotationAngle = rotationSpeed * Time.deltaTime;

        rotationAngle = Mathf.Clamp(rotationAngle, 0f, maxRotationSpeed * Time.deltaTime);

        Quaternion rotation = Quaternion.Euler(0f, rotationAngle, 0f);
        cubeTransform.rotation *= rotation;

        rotationTimer += Time.deltaTime;
        rotationSpeed = Mathf.Lerp(45f, maxRotationSpeed, rotationTimer / 1f);
    }

    void StartRotationTimer()
    {
        rotationTimer = 0f;
    }

    void StopRotationTimer()
    {
        rotationTimer = 0f;
        rotationSpeed = 45f; // 重置旋轉速度
    }

    void RotateCubeVertically(Transform cubeTransform, float roatationSpeed)
    {
        float rotationAmount = roatationSpeed * Time.deltaTime;
        Quaternion rotation = Quaternion.Euler(rotationAmount, 0f, 0f);
        cubeTransform.rotation *= rotation;
    }
}
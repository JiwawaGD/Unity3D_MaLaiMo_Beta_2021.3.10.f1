using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    // Can be setted by player
    float m_fUDSensitivity;
    float m_fRLSensitivity;

    // Const value
    readonly float m_fMoveSpeed = 90f;
    readonly float m_fRayLength = 2f;
    readonly int m_iInteractiveLayer = 10;
    readonly Vector3 v3_zero = Vector3.zero;

    float m_fDeltatime;
    float m_fLookRotation;

    [HideInInspector] public bool m_bCursorShow;
    [HideInInspector] public bool m_bCanControl;

    Vector3 v3_MoveValue;
    Vector3 v3_MovePos;

    public Transform tfPlayerCamera;
    Transform tfTransform;
    Rigidbody rig;
    RaycastHit hit;

    ItemController current_Item;
    ItemController last_Item;
    GameManager gameManager;

    void Awake()
    {
        rig = GetComponent<Rigidbody>();
        tfTransform = transform;

        if (tfPlayerCamera == null)
            tfPlayerCamera = GameObject.Find("Player Camera").transform;

        if (gameManager == null)
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Start()
    {
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

        Move();
        View();
    }

    void InitValue()
    {
        m_fUDSensitivity = 110;
        m_fRLSensitivity = 90;

        m_bCursorShow = true;
        m_bCanControl = true;

        m_fDeltatime = Time.deltaTime;
    }

    void View()
    {
        // 左右轉
        tfTransform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * m_fRLSensitivity * m_fDeltatime);

        m_fLookRotation += Input.GetAxis("Mouse Y") * m_fUDSensitivity * m_fDeltatime;
        m_fLookRotation = Mathf.Clamp(m_fLookRotation, -75, 75);

        // 上下轉
        tfPlayerCamera.localEulerAngles = -Vector3.right * m_fLookRotation;
    }

    void Move()
    {
        v3_MoveValue = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        v3_MovePos.x = v3_MoveValue.x * m_fDeltatime * m_fMoveSpeed;
        v3_MovePos.z = v3_MoveValue.z * m_fDeltatime * m_fMoveSpeed;

        v3_MovePos = tfTransform.right * v3_MovePos.x + tfTransform.forward * v3_MovePos.z;

        if (v3_MovePos != v3_zero)
            rig.velocity = v3_MovePos;
        else
            rig.velocity = v3_zero;
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
        if (Physics.Raycast(tfPlayerCamera.position,     // Origin 
            tfPlayerCamera.forward,                      // Direction
            out hit,                                     // RaycastHit
            m_fRayLength))                               // RayLength
        {
            current_Item = hit.transform.gameObject.GetComponent<ItemController>();

            if (hit.transform.gameObject.layer == m_iInteractiveLayer && current_Item.b_isActive)
            {
                current_Item.HintState(true);

                last_Item = current_Item;

                if (Input.GetKeyDown(KeyCode.E))
                    current_Item.SendGameEvent();
            }
        }
        else
        {
            if (last_Item)
                last_Item.HintState(false);
        }
    }
}

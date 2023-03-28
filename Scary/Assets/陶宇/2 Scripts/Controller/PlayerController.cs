using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public GameObject cam;

    //  Can be setted by player
    float f_UDSensitivity;
    float f_RLSensitivity;

    //  Const value
    float f_moveSpeed;
    float f_deltatime;
    float f_lookRotation;
    float f_rayLength;

    int i_InteractiveLayer;
    int i_StoryColliderLayer;

    bool b_CursorShow;

    Vector3 v3_zero;
    Vector3 v3_moveValue;
    Vector3 v3_movePos;

    Rigidbody rig;
    RaycastHit hit;

    ItemController current_Item;
    ItemController last_Item;

    void Awake()
    {
        rig = GetComponent<Rigidbody>();
        cam = GameObject.Find("Player Camera");
    }

    void Start()
    {
        Init();
        SetCursor();
    }

    /// <summary>
    /// 射線
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(cam.transform.position, cam.transform.position + (cam.transform.forward * f_rayLength));
    }

    void FixedUpdate()
    {
        Move();
        View();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F6))
            SetCursor();

        RayHitCheck();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == i_StoryColliderLayer)
        {
            StoryController storyController = col.gameObject.GetComponent<StoryController>();

            storyController.SendStoryMsg();
        }
    }

    //  Initialize everything
    void Init()
    {
        f_moveSpeed = 180;
        f_UDSensitivity = 120;
        f_RLSensitivity = 80;
        f_rayLength = 2;

        i_InteractiveLayer = 10;

        b_CursorShow = true;

        f_deltatime = Time.deltaTime;
        v3_zero = Vector3.zero;
    }

    //  View
    void View()
    {
        //  左右轉
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * f_RLSensitivity * f_deltatime);

        f_lookRotation += Input.GetAxis("Mouse Y") * f_UDSensitivity * f_deltatime;
        f_lookRotation = Mathf.Clamp(f_lookRotation, -75, 75);

        //  上下轉
        cam.transform.localEulerAngles = -Vector3.right * f_lookRotation;
    }

    //  Move
    void Move()
    {
        v3_moveValue = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        v3_movePos.x = v3_moveValue.x * f_deltatime * f_moveSpeed;
        v3_movePos.z = v3_moveValue.z * f_deltatime * f_moveSpeed;

        v3_movePos = transform.right * v3_movePos.x + transform.forward * v3_movePos.z;

        if (v3_movePos != v3_zero)
            rig.velocity = v3_movePos;
        else
            rig.velocity = v3_zero;
    }

    //  Set cursor state
    void SetCursor()
    {
        b_CursorShow = !b_CursorShow;

        if (b_CursorShow)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = b_CursorShow;
    }

    //  Ray check for item interact
    void RayHitCheck()
    {
        if (Physics.Raycast(cam.transform.position,     // Origin 
            cam.transform.forward,                      // Direction
            out hit,                                    // RaycastHit
            f_rayLength))                               // RayLength
        {
            current_Item = hit.transform.gameObject.GetComponent<ItemController>();

            if (hit.transform.gameObject.layer == i_InteractiveLayer && current_Item.b_isActive)
            {
                current_Item.LightOn(true);

                last_Item = current_Item;

                if (Input.GetKeyDown(KeyCode.E))
                    current_Item.SendGameEvent();
            }
        }
        else
        {
            if (last_Item)
                last_Item.LightOn(false);
        }
    }
}

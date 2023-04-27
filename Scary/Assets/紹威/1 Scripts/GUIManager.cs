using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    [Header("文字觸發位置")][SerializeField] private Collider[] PosTraget;
    public LayerMask PosTragetLayerMask;
    // Start is called before the first frame update
    private void Awake()
    {
        FollowTarget();
    }
    void Start()
    {
        
    }
    /// <summary>
    /// 尋找場景中需要的位置
    /// </summary>
    void FollowTarget()
    {       
        PosTraget = Physics.OverlapSphere(transform.position, 100f, PosTragetLayerMask);
    }
}


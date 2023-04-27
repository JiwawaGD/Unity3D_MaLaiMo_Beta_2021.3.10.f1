using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    [Header("��rĲ�o��m")][SerializeField] private Collider[] PosTraget;
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
    /// �M��������ݭn����m
    /// </summary>
    void FollowTarget()
    {       
        PosTraget = Physics.OverlapSphere(transform.position, 100f, PosTragetLayerMask);
    }
}


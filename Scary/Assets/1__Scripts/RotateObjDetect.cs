using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObjDetect : MonoBehaviour
{
    // private bool isRotating = false;    // 是否正在旋轉
    // private Vector3 lastMousePosition; // 滑鼠上一幀位置
    public float speed = 100f;
    private void Start()
    {

    }
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            float rotX = Input.GetAxis("Mouse X") * speed * Mathf.Deg2Rad;
            // float rotY = Input.GetAxis("Mouse Y") * speed * Mathf.Deg2Rad;
            print(rotX);
            if(Vector3.Dot(transform.up, Vector3.up) > 0)
            {
                transform.RotateAround(transform.position, transform.up, rotX);
            }
            else
            {
                transform.RotateAround(transform.position, transform.up, -rotX);
            }
            // if(Vector3.Dot(transform.right, Vector3.right) > 0)
            // {
            //     transform.RotateAround(transform.position, transform.right, -rotY);
            // }
            // else
            // {
            //     transform.RotateAround(transform.position, transform.right, rotY);
            // }
        }
    }
    // void OnMouseDown()
    // {
    //     isRotating = true;
    //     lastMousePosition = Input.mousePosition;
    // }

    // void OnMouseUp()
    // {
    //     isRotating = false;
    // }

    // void OnMouseDrag()
    // {
    //     if (isRotating)
    //     {
    //         // 使用滑鼠移動來旋轉物件
    //         Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
    //         transform.Rotate(Vector3.up, -mouseDelta.x);
    //         transform.Rotate(Vector3.right, -mouseDelta.y);
    //         lastMousePosition = Input.mousePosition;
    //     }
    // }
    
}
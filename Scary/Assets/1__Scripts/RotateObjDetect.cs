using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObjDetect : MonoBehaviour
{
    private Vector3 initialMousePosition; // 滑鼠點擊位置
    private Vector3 lastMousePosition;  // 滑鼠上一次位置
    private bool isRotating = false;    // 是否正在旋轉
    private void Start()
    {

    }
    void OnMouseDown()
    {
        isRotating = true;
        lastMousePosition = Input.mousePosition;
    }

    void OnMouseUp()
    {
        isRotating = false;
    }
    void OnMouseDrag()
    {
        if (isRotating)
        {
            // 使用滑鼠移動來旋轉物件
            Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
            transform.Rotate(Vector3.up, mouseDelta.x);
            transform.Rotate(Vector3.right, -mouseDelta.y);
            lastMousePosition = Input.mousePosition;
        }
    }
    // void Update()
    // {
    //     if (Input.GetMouseButtonDown(0))    // 當滑鼠左键被按下
    //     {
    //         // 鑑測點擊位置是否在物體
    //         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //         RaycastHit hit;

    //         if (Physics.Raycast(ray, out hit))
    //         {
    //             if (hit.collider.gameObject == gameObject)
    //             {
    //                 isRotating = true;
    //                 initialMousePosition = Input.mousePosition;
    //             }
    //         }
    //     }

    //     if (Input.GetMouseButtonUp(0)) // 當滑鼠左键被按下
    //     {
    //         isRotating = false;
    //     }

    //     if (isRotating)
    //     {
    //         Vector3 currentMousePosition = Input.mousePosition;
    //         Vector3 mouseDelta = currentMousePosition - initialMousePosition;

    //         // 根據滑鼠移動計算旋轉角度
    //         float rotationSpeed = 1.0f; // 调整旋轉速度
    //         transform.Rotate(Vector3.up * mouseDelta.x * rotationSpeed, Space.World);
    //         transform.Rotate(Vector3.right * mouseDelta.y * rotationSpeed, Space.World);

    //         initialMousePosition = currentMousePosition;
    //     }
    // }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ROmanager : MonoBehaviour
{
    private Vector3 initialMousePosition;
    private bool isRotating = false;
    private void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 当鼠标左键被按下
        {
            // 检测点击位置是否在物体上
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    isRotating = true;
                    initialMousePosition = Input.mousePosition;
                }
            }
        }

        if (Input.GetMouseButtonUp(0)) // 当鼠标左键被释放
        {
            isRotating = false;
        }

        if (isRotating)
        {
            Vector3 currentMousePosition = Input.mousePosition;
            Vector3 mouseDelta = currentMousePosition - initialMousePosition;

            // 根据鼠标移动计算旋转角度
            float rotationSpeed = 1.0f; // 调整旋转速度
            transform.Rotate(Vector3.up * mouseDelta.x * rotationSpeed, Space.World);
            transform.Rotate(Vector3.right * mouseDelta.y * rotationSpeed, Space.World);

            initialMousePosition = currentMousePosition;
        }
    }
}

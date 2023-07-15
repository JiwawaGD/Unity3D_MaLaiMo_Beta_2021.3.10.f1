using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScaryItem : MonoBehaviour
{
    public float acceleration = 300f;
    public GameObject Gramant;
    public Transform pos;
    public AudioClip soundEffect;

    private bool isMoving = false;
    private Rigidbody rb;
    public AudioSource audioSource;
    public GameObject cube;

    private void Start()
    {
        rb = Gramant.GetComponent<Rigidbody>();

        audioSource.clip = soundEffect;
    }

    private void StartMoving()
    {
        isMoving = true;
        Vector3 direction = (pos.position - Gramant.transform.position).normalized;
        rb.AddForce(direction * acceleration , ForceMode.Acceleration);

        audioSource.Play();

        Destroy(cube);
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            Vector3 direction = (pos.position - Gramant.transform.position).normalized;
            rb.AddForce(direction * acceleration, ForceMode.Acceleration);

            // 當物體靠近目標位置時，停止加速度移動
            if (Vector3.Distance(Gramant.transform.position, pos.position) < 0.1f)
            {
                isMoving = false;
                rb.velocity = Vector3.zero;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartMoving();

        }
    }
}

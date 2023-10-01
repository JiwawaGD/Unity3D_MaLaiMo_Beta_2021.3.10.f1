using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScaryItem : MonoBehaviour
{
    [SerializeField] float acceleration = 300f;
    [SerializeField] GameObject gradma;
    [SerializeField] Transform target;
    [SerializeField] AudioClip soundEffect;

    private bool isMoving = false;
    private Rigidbody rb;
    public AudioSource audioSource;

    private void Start()
    {
        rb = gradma.GetComponent<Rigidbody>();

        audioSource.clip = soundEffect;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartMoving();
        }
    }
    private void StartMoving()
    {
        isMoving = true;
        Vector3 direction = (target.position - gradma.transform.position).normalized;
        rb.AddForce(direction * acceleration , ForceMode.Acceleration);

        audioSource.Play();

        Destroy(gameObject,5);
    }

    private void FixedUpdate()
    {
        //if (isMoving)
        //{
        //    Vector3 direction = (target.position - gradma.transform.position).normalized;
        //    rb.AddForce(direction * acceleration, ForceMode.Acceleration);

        //    if (Vector3.Distance(gradma.transform.position, target.position) < 0.1f)
        //    {
        //        isMoving = false;
        //        rb.velocity = Vector3.zero;
        //    }
        //}
    }

}

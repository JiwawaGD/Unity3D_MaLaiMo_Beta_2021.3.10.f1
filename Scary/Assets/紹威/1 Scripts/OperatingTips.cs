using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperatingTips : MonoBehaviour
{
    public GameObject Tips;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(TipsDelay(5f));
            Tips.SetActive(false);
        }
    }
    private IEnumerator TipsDelay(float Textdelay)
    {
        yield return new WaitForSeconds(Textdelay);
        Tips.SetActive(true);
    }
}

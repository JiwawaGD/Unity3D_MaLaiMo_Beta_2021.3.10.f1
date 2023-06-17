using System.Collections.Generic;
using UnityEngine;

public class AUDManager : MonoBehaviour
{
    public static AUDManager instance;

    //[SerializeField] AudioSource eatSource;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

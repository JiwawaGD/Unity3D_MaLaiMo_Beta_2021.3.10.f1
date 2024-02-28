using UnityEngine;

public class PianoManger : MonoBehaviour
{

    public AudioClip[] Sounds;
    public AudioSource Aud;
    public Animation PianoAnimation;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Aud.PlayOneShot(Sounds[0], 1f);
            PianoAnimation.PlayQueued("piano_C");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Aud.PlayOneShot(Sounds[1], 1f);
            PianoAnimation.PlayQueued("piano_D");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Aud.PlayOneShot(Sounds[2], 1f);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Aud.PlayOneShot(Sounds[3], 1f);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            Aud.PlayOneShot(Sounds[4], 1f); ;
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Aud.PlayOneShot(Sounds[5], 1f); ;
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            Aud.PlayOneShot(Sounds[6], 1f); ;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            Aud.PlayOneShot(Sounds[7], 1f); ;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Aud.PlayOneShot(Sounds[8], 1f); ;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Aud.PlayOneShot(Sounds[9], 1f); ;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Aud.PlayOneShot(Sounds[10], 1f); ;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Aud.PlayOneShot(Sounds[11], 1f); ;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Aud.PlayOneShot(Sounds[12], 1f); ;
        }
    }
}

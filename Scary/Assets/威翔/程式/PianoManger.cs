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
            PianoAnimation.PlayQueued("piano_C_down");
        }
        else if(Input.GetKeyUp(KeyCode.Q)) PianoAnimation.PlayQueued("piano_C_up");

        if (Input.GetKeyDown(KeyCode.W))
        {
            Aud.PlayOneShot(Sounds[1], 1f);
            PianoAnimation.PlayQueued("piano_D_down");
        }
        else if (Input.GetKeyUp(KeyCode.W)) PianoAnimation.PlayQueued("piano_D_up");

        if (Input.GetKeyDown(KeyCode.E))
        {
            Aud.PlayOneShot(Sounds[2], 1f);
            PianoAnimation.PlayQueued("piano_E_down");
        }
        else if (Input.GetKeyUp(KeyCode.E)) PianoAnimation.PlayQueued("piano_E_up");

        if (Input.GetKeyDown(KeyCode.R))
        {
            Aud.PlayOneShot(Sounds[3], 1f);
            PianoAnimation.PlayQueued("piano_F_down");
        }
        else if (Input.GetKeyUp(KeyCode.R)) PianoAnimation.PlayQueued("piano_F_up");

        if (Input.GetKeyDown(KeyCode.T))
        {
            Aud.PlayOneShot(Sounds[4], 1f);
            PianoAnimation.PlayQueued("piano_G_down");
        }
        else if (Input.GetKeyUp(KeyCode.T)) PianoAnimation.PlayQueued("piano_G_up");

        if (Input.GetKeyDown(KeyCode.Y))
        {
            Aud.PlayOneShot(Sounds[5], 1f); ;
            PianoAnimation.PlayQueued("piano_A_down");
        }
        else if (Input.GetKeyUp(KeyCode.Y)) PianoAnimation.PlayQueued("piano_A_up");

        if (Input.GetKeyDown(KeyCode.U))
        {
            Aud.PlayOneShot(Sounds[6], 1f);
            PianoAnimation.PlayQueued("piano_B_down");
        }
        else if (Input.GetKeyUp(KeyCode.U)) PianoAnimation.PlayQueued("piano_B_up");

        if (Input.GetKeyDown(KeyCode.I))
        {
            Aud.PlayOneShot(Sounds[7], 1f);
            PianoAnimation.PlayQueued("piano_HC_down");
        }
        else if (Input.GetKeyUp(KeyCode.I)) PianoAnimation.PlayQueued("piano_HC_up");

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Aud.PlayOneShot(Sounds[8], 1f);
            PianoAnimation.PlayQueued("piano_C#_down");
        }
        else if (Input.GetKeyUp(KeyCode.Alpha2)) PianoAnimation.PlayQueued("piano_C#_up");

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Aud.PlayOneShot(Sounds[9], 1f);
            PianoAnimation.PlayQueued("piano_D#_down");
        }
        else if (Input.GetKeyUp(KeyCode.Alpha3)) PianoAnimation.PlayQueued("piano_D#_up");

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Aud.PlayOneShot(Sounds[10], 1f);
            PianoAnimation.PlayQueued("piano_F#_down");
        }
        else if (Input.GetKeyUp(KeyCode.Alpha5)) PianoAnimation.PlayQueued("piano_F#_up");

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Aud.PlayOneShot(Sounds[11], 1f);
            PianoAnimation.PlayQueued("piano_G#_down");
        }
        else if (Input.GetKeyUp(KeyCode.Alpha6)) PianoAnimation.PlayQueued("piano_G#_up");

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            Aud.PlayOneShot(Sounds[12], 1f);
            PianoAnimation.PlayQueued("piano_A#_down");
        }
        else if (Input.GetKeyUp(KeyCode.Alpha7)) PianoAnimation.PlayQueued("piano_A#_up");
    }
}

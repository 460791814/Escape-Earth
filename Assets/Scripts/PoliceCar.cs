using UnityEngine;
using System.Collections;

public class PoliceCar : MonoBehaviour
{


    public AudioSource audioClip;

    public bool isPlay = false;

    // Update is called once per frame
    void Update()
    {
        if (isPlay == false && GameController.state == GameState.End)
        {
            audioClip.Play();
            isPlay = true;
        }
    }
}

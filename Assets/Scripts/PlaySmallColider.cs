using UnityEngine;
using System.Collections;

public class PlaySmallColider : MonoBehaviour {

    public PlayAnimation playanimState;

    // Use this for initialization
    void Awake()
    {

        playanimState = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayAnimation>();
    }



    void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tags.obs && GameController.state == GameState.Playing && playanimState.anim == AnimState.Slider)
        {
            GameController.state = GameState.End;
        }
    }
}

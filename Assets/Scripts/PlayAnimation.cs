using UnityEngine;
using System.Collections;

public enum  AnimState
{
   Idle,
    Run,
    TrunLeft,
    TrunRight,
    Slider,
    Jump,
    Dead
}

public class PlayAnimation : MonoBehaviour {


    public AudioSource audioClip;
    private Animation animation;
    public AnimState anim = AnimState.Idle;
    private PlayMove playMove;
    void Awake()
    {
        //Animation是系统组件所以可以直接拿不需要GetComponent
        animation = transform.Find("Prisoner").animation;
        playMove = this.GetComponent<PlayMove>();
    }
	
	// Update is called once per frame
	void Update () {
        switch (GameController.state)
        {
            case GameState.Menu:
             anim=AnimState.Idle;
                break;
            case GameState.Playing:
                anim = AnimState.Run;
                
                if (playMove.targetLaneIndex > playMove.nowLaneIndex)
                {
                    anim = AnimState.TrunLeft;
                    
                }
                if (playMove.targetLaneIndex < playMove.nowLaneIndex)
                {
                    anim = AnimState.TrunRight;
                }
                if (playMove.IsSlider)
                {
                    anim = AnimState.Slider;
                }

                if (playMove.IsJump)
                {
                    anim = AnimState.Jump;
                }
                if (anim == AnimState.Run)
                {
                   
                    if (audioClip.isPlaying == false)
                    {
                        audioClip.Play();
                    }
                }
                else {
                    if (audioClip.isPlaying)
                    {
                        audioClip.Stop();
                    }
                }

                break;
              
            case GameState.End:
                anim = AnimState.Dead;
                audioClip.Stop();
                break;
            default:
                break;
        }
      
   
	}

    void LateUpdate()
    {
        switch (anim)
        {
            case AnimState.Idle:
                PlayIdle();
                break;
            case AnimState.Run:
                PlayAnimationByName("run");
               
                break;
            case AnimState.TrunLeft:
                animation["left"].speed = 1;//加速  2倍快进
                PlayAnimationByName("left");
                break;
            case AnimState.TrunRight:
                  animation["right"].speed = 1;//加速  2倍快进
                  PlayAnimationByName("right");
                break;
            case  AnimState.Slider:
                PlayAnimationByName("slide");
                break;
            case AnimState.Jump:
                PlayAnimationByName("jump");
                break;
            case AnimState.Dead:
                PlayDeath();
                break;
            default:
                break;
        }
    }
    private bool isPlayDeath=false;
    private void PlayDeath()
    {
        if (!isPlayDeath && animation.IsPlaying("death") == false)
        {
            animation.Play("death");
            isPlayDeath = true;
        }
    }

    private void PlayIdle()
    {
        //animation.IsPlaying  判断该动画是否在播放
        if (animation.IsPlaying("Idle_1") == false && animation.IsPlaying("Idle_2")==false)
        {
            //PlayQueued  加入播放队列  play直接播放
            animation.Play("Idle_1");
            animation.PlayQueued("Idle_2");
        }
      
    }

    private void PlayAnimationByName(string name)
    {
        if (animation.IsPlaying(name) == false)
        {//如果该动画不在播放则播放该动画

            animation.Play(name);
        }
     
    }
}

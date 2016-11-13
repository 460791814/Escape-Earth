using UnityEngine;
using System.Collections;

public enum TouchDir
{

    None,
    Left,
    Right,
    Top,
    Bottom
}

public class PlayMove : MonoBehaviour
{


    public float Speed = 100;
    private EnvScript envScript;
    public  TouchDir touchDir = TouchDir.None;
    //玩家所处的赛道，0 1 2 左中右
    public int nowLaneIndex = 1;
    public int targetLaneIndex = 1;
    // 每次左右移动的距离
    private float moveHorizontal = 0;
    public  float moveHorizonSpeed = 3f;
    //赛道偏移 的距离
    private float[] xOffset = new float[3] { -14, 0, 14 };

    //滑动
    public bool IsSlider = false;
    private float sliderTime = 1.4f;
    private float startSlider = 0;

    //跳跃
    public  float JumpSpeed = 50;
    public Transform Prisoner;
    private bool IsUp = true;
    public bool IsJump = false;
    private float JumpHeight = 20;
    private float HaveJumpHeight = 10;
    //着落的声音
    public AudioSource JumpLandMusic;
    void Awake()
    {
        envScript = Camera.main.GetComponent<EnvScript>();
        Prisoner = transform.Find("Prisoner");
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (GameController.state == GameState.Playing)
        {
            Vector3 target = envScript.forest1.GetComponent<ForestScript>().GetNextTargetPoint();
            //由于玩家的跑道变了 所以 对应的waypoint目标点也得做相应的偏移
            target = new Vector3(target.x + xOffset[targetLaneIndex], target.y, target.z);
            Vector3 moveDir = target - transform.position;
            //normalized返回向量的长度为1 当规格化后，向量保持同样的方向，但是长度变为1.0。
            Vector3 playVect = moveDir.normalized * Speed * Time.deltaTime;
            //print("movedir" + moveDir);
            //print("playVect" + playVect);
            transform.position += playVect;
            MoveControl();

        }
    }

    private void MoveControl()
    {

        TouchDir d = GetTouchDir();
    
        //假如当前赛道跟目标赛道不同
        if (nowLaneIndex != targetLaneIndex)
        {
            float moveLength = Mathf.Lerp(0, moveHorizontal, moveHorizonSpeed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x + moveLength, transform.position.y, transform.position.z);
            moveHorizontal -= moveLength;
            //moveLength  得去正值，当moveHorizontal为负值时。该条件直接成立
            if (Mathf.Abs( moveLength) < 0.05f)
            {
                transform.position = new Vector3(transform.position.x + moveHorizontal, transform.position.y, transform.position.z);
                nowLaneIndex = targetLaneIndex;
            }
        }
        if (IsSlider)
        {
            startSlider += Time.deltaTime;
            if (startSlider > sliderTime)
            {
                IsSlider = false;

            }
        }
        //跳跃
        if (IsJump)
        {
            float yMove = JumpSpeed * Time.deltaTime;
            if (IsUp)
            {
                //先向上跳
                Prisoner.position = new Vector3(Prisoner.position.x, Prisoner.position.y + yMove, Prisoner.position.z);
                HaveJumpHeight += yMove;
                if (JumpHeight - HaveJumpHeight < 0.5f)
                {
                    Prisoner.position = new Vector3(Prisoner.position.x, Prisoner.position.y + JumpHeight - HaveJumpHeight, Prisoner.position.z);
                    IsUp = false;
                    //开始向下跳
                    HaveJumpHeight = JumpHeight;
                }
            }
            else {
                Prisoner.position = new Vector3(Prisoner.position.x, Prisoner.position.y -yMove, Prisoner.position.z);
                HaveJumpHeight -= yMove;
                if (HaveJumpHeight-0 < 0.5f)
                {
                    Prisoner.position = new Vector3(Prisoner.position.x, Prisoner.position.y - HaveJumpHeight, Prisoner.position.z);

                    IsJump = false;
                    HaveJumpHeight = 0;
                    JumpLandMusic.Play();
                }
            }
        }
    }
    Vector3 mouseDown = Vector3.zero;  //这个状态得放在外面 因为方法是按帧执行的
    //判断用户的滑动行为，得到方向
    TouchDir GetTouchDir()
    {
        TouchDir t = TouchDir.None;

        if (Input.GetMouseButtonDown(0))
        {
            mouseDown = Input.mousePosition;//获取当前鼠标按下的坐标

        }
        if (Input.GetMouseButtonUp(0))
        {
            //获取当前鼠标抬起的坐标
            Vector3 mouseUp = Input.mousePosition;
          
            //获取鼠标滑动的向量
            Vector3 mouseVector = mouseUp - mouseDown;
        
            float x = Mathf.Abs(mouseVector.x);
            float y = Mathf.Abs(mouseVector.y);

            if (x > 50 || y > 50)
            {
                //X或者Y轴上移动的位置大于50像素 证明这次滑动是有效果的
                if (x > y && mouseVector.x > 0)
                {
                    t = TouchDir.Right;
                    if (targetLaneIndex < 2)
                    {
                        targetLaneIndex++;
                        moveHorizontal = 14;
                    }
                }
                else if (x > y && mouseVector.x < 0)
                {
                    t = TouchDir.Left;
                    if (targetLaneIndex > 0)
                    {
                        targetLaneIndex--;
                        moveHorizontal = -14;
                    }
                }
                else if (x < y && mouseVector.y > 0)
                {
                    t = TouchDir.Top;
                    if (!IsJump)
                    {
                        IsJump = true;
                        IsUp = true;
                        HaveJumpHeight = 0;
                    }
                }
                else if (x < y && mouseVector.y < 0)
                {
                    t = TouchDir.Bottom;
                    if (!IsSlider)
                    {
                        IsSlider = true;
                        startSlider = 0;
                    }
                }

            }
        }
        return t;
    }
}

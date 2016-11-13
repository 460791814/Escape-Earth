using UnityEngine;
using System.Collections;

//摄像机跟随玩家移动，这样防止人物跳动导致摄像机抖动
public class FollowPlayer : MonoBehaviour {

    public float camraSpeed = 1;
    private GameObject player;
    private Vector3 offer;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag(Tags.player);
      offer= transform.position- player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 camraPositon = player.transform.position + offer;
        transform.position = Vector3.Lerp(transform.position, camraPositon, camraSpeed*Time.deltaTime);
	}
}

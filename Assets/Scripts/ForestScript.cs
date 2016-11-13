using UnityEngine;
using System.Collections;

public class ForestScript : MonoBehaviour {

    public GameObject player;

    public GameObject[] Obs;
    public WayPoints wayPoint;

    public int targetWayPointIndex;

    public EnvScript envScript;
	// Use this for initialization
	void Awake () {
        player = GameObject.FindGameObjectWithTag(Tags.player);
        wayPoint = transform.Find("WayPoints").GetComponent<WayPoints>();
        targetWayPointIndex = wayPoint.points.Length - 2;
        envScript = Camera.main.GetComponent<EnvScript>();
	}

    void Start()
    {
        CreateObsRandom();
    }
	// Update is called once per frame
	void Update () {
        //if (player.transform.position.z > (transform.position.z+100))
        //{
        //    Camera.main.SendMessage("CreateFore");
        //    Destroy(this.gameObject);
        //}
	}

    void CreateObsRandom()
    {
        float startZ = transform.position.z - 3000;
        float endZ = transform.position.z;
        float z = startZ + 100;
        while (true)
        {
            z += Random.Range(100, 500);
            if (z > endZ)
            {
                    //已经到地图的边缘
                break;
            }
            else { 
            //生成障碍物
          GameObject obj=GameObject.Instantiate(Obs[Random.Range(0, Obs.Length)], GetWayPointByZ(z), Quaternion.identity) as GameObject;
          obj.transform.parent = this.transform;//指定障碍物的父级
            }
        }
    }
    /// <summary>
    /// 根据生成的Z的位置  获得Z对应的VECTOR3的位置。
    /// </summary>
    /// <param name="z"></param>
    /// <returns></returns>
    Vector3 GetWayPointByZ(float z)
    {
           Transform[] points=wayPoint.points;
        int index = 0;
        for (int i = 0; i < points.Length; i++)
        {
            if (z >= points[i + 1].position.z && z <= points[i].position.z)
            {
                index = i;
                break;
            }
        }
       return  Vector3.Lerp(points[index + 1].position, points[index].position, (z - points[index + 1].position.z) / (points[index].position.z - points[index + 1].position.z));
    }
    /// <summary>
    /// 获取下一个目标点的位置
    /// </summary>
    /// <returns></returns>
    public Vector3 GetNextTargetPoint()
    {
        while (true)
        {
            //sqrMagnitude返回这个向量的长度的平方（只读）。计算长度的平方而不是magnitude是非常快的
           // if ((wayPoint.points[targetWayPointIndex].position - player.transform.position).sqrMagnitude < 100)
            if (wayPoint.points[targetWayPointIndex].position.z - player.transform.position.z < 10)
            { 
             //如果目标点离玩家的距离小于10
                targetWayPointIndex--;
                if(targetWayPointIndex<0)
                {
                 //已经跑完本段路径
                    envScript.CreateFore();
                    Destroy(this.gameObject, 2f);
                    return envScript.forest1.GetComponent<ForestScript>().GetNextTargetPoint();
                }
            }else{
                return wayPoint.points[targetWayPointIndex].position;
                 
                }
        }
    }
}

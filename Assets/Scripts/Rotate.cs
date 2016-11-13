using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

    public  float RoateSpeed = 30f;
    void Update()
    {//围绕Y轴进行旋转
        transform.Rotate(Vector3.up, RoateSpeed * Time.deltaTime);
    }

}

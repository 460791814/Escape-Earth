using UnityEngine;
using System.Collections;

public class EnvScript : MonoBehaviour {

    public Transform forest1;
    public Transform forest2;

    public GameObject[] forests;
    private int currentForestCount=2;
    public void CreateFore()
    {
        currentForestCount++;
        int i=Random.Range(0,3);
     GameObject   obj=   Instantiate(forests[i], new Vector3(0, 0, currentForestCount * 3000), Quaternion.identity) as GameObject;
     forest1 = forest2;
     forest2 = obj.transform;
    }
}

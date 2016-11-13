using UnityEngine;
using System.Collections;

public class WayPoints : MonoBehaviour {

    public Transform[] points;
	// Use this for initialization
	void Start () {
	
	}
	

    void OnDrawGizmos()
    {
        iTween.DrawPath(points);
	}
}

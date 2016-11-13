using UnityEngine;
using System.Collections;

public enum GameState
{ 
   Menu,
    Playing,
    End
}

public class GameController : MonoBehaviour {

    public static GameState state = GameState.Menu;
    public GameObject UIStart;
    public GameObject UIEnd;
	// Use this for initialization
	void Start () {
        state = GameState.Menu;
	}
	
	// Update is called once per frame
	void Update () {
        if (state == GameState.Menu)
        {
            if (Input.GetMouseButtonDown(0))
            {
                state = GameState.Playing;
                UIStart.SetActive(false);
            }
        }
        else if (state == GameState.End)
        {

            UIEnd.SetActive(true);
        }

	}

    void OnGUI()
    {
        if (state == GameState.End)
        {
            if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2, 60, 50), "RePlay"))
            {
                Application.LoadLevel(0);
            }
        }
    }
}

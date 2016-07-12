using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MoveScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	  
	}


    public void gotoInit()
    {
        SceneManager.LoadScene(0);
    }
    public void gotoMain()
    {
        SceneManager.LoadScene(1);
    }
    public void gotoInspect()
    {
        SceneManager.LoadScene(2);
    }
}

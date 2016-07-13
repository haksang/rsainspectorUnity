using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MoveScene : MonoBehaviour {


    MainManager manager;
	// Use this for initialization
	void Start () {
        try
        {
            manager = GameObject.Find("MainManager").GetComponent<MainManager>();

        }
        catch(MissingComponentException ex)
        {
            Debug.Log(ex);
        }
    }
	

    public void gotoInit()
    {
        SceneManager.LoadScene(0);
        manager.setState(0);
    }
    public void gotoMain()
    {
        SceneManager.LoadScene(1);
        manager.setState(1);
    }
    public void gotoInspect()
    {
        SceneManager.LoadScene(2);
        manager.setState(2);
    }
}

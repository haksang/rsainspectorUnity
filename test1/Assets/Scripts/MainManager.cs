using UnityEngine;
using System.Collections;

public class MainManager : MonoBehaviour {

    public enum state
    {
        Init , Main , Inspect
    }

    public state currentstate;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setState(int currentstate)
    {
        this.currentstate = (state)currentstate;
    }
}

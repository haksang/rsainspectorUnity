﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MListButton : MonoBehaviour {

    public int id;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setButton(int id)
    {
        this.id = id;
    }

    public void MoveScene()
    {
        SceneManager.LoadScene(2);
    }
}

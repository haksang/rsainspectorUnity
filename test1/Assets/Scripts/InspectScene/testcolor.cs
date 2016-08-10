using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class testcolor : MonoBehaviour {

    public int count;
    public Image image;

	// Use this for initialization
	void Start () {
        image.color = new Color((float)((float)count/5.0f), 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void Enabled()
    {
        
    }
}

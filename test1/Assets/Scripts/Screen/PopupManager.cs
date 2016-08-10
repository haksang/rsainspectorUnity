using UnityEngine;
using System.Collections;


public class PopupManager : MonoBehaviour {

    MainManager manager;
    // Use this for initialization
    void Start () {
        try
        {
            //manager = GameObject.Find("MainManager").GetComponent<MainManager>();

        }
        catch (MissingComponentException ex)
        {
            Debug.Log(ex);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
        }
	}
}

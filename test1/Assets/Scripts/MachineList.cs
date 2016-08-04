using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MachineList : MonoBehaviour {


    public GameObject UIPanel1;

    public Button Dataset;

    public VerticalLayoutGroup vlg;

    int id = 0;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void createButton()
    {

        Button newButton = (Button)Instantiate(Dataset);
        newButton.transform.SetParent(UIPanel1.transform, false);

        newButton.GetComponent<MListButton>().setButton(id++);
        
    }
}

using UnityEngine;
using System.Collections;

public class MainManager : MonoBehaviour {

    public GameObject MainPanel, MAddPanel, MListPanel;

    public enum state
    {
        Main ,MAdd, MList
    }

    public state currentstate;
    

	// Use this for initialization
	void Start () {
        MainPanel.SetActive(false);
        MAddPanel.SetActive(false);
        MListPanel.SetActive(false);

        MainPanel.SetActive(true);
        this.currentstate = state.Main;
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey(KeyCode.Escape))
        {
            ActivatePanel((int)state.Main);
        }
	}

    public void setState(int currentstate)
    {
        this.currentstate = (state)currentstate;
    }

    public void ActivatePanel(int nextState)
    {
        switch((state)this.currentstate)
        {
            case state.Main:
                MainPanel.SetActive(false);
                break;
            case state.MAdd:
                MAddPanel.SetActive(false);
                break;
            case state.MList:
                MListPanel.SetActive(false);
                break;

            default:
                break;
        }
        switch ((state)nextState)
        {
            case state.Main:
                MainPanel.SetActive(true);
                this.currentstate = state.Main;
                break;
            case state.MAdd:
                MAddPanel.SetActive(true);
                this.currentstate = state.MAdd;
                break;
            case state.MList:
                MListPanel.SetActive(true);
                this.currentstate = state.MList;
                break;
            default:
                break;
        }
    }
}

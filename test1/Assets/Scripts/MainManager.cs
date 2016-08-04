using UnityEngine;
using System.Collections;

public class MainManager : MonoBehaviour {

    public GameObject MainPanel, MAddPanel, MListPanel, InspectPanel;

    public enum state
    {
        Main ,MAdd, MList, Inspect
    }

    public state currentstate;
    

	// Use this for initialization
	void Start () {
        MainPanel.SetActive(false);
        MAddPanel.SetActive(false);
        MListPanel.SetActive(false);
        InspectPanel.SetActive(false);

        MainPanel.SetActive(true);
        this.currentstate = state.Main;
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void setState(int currentstate)
    {
        this.currentstate = (state)currentstate;
    }

    public void BacktoMain()
    {
        ActivatePanel((int)state.Main);
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
            case state.Inspect:
                InspectPanel.SetActive(false);
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
            case state.Inspect:
                InspectPanel.SetActive(true);
                this.currentstate = state.Inspect;
                break;
            default:
                break;
        }
    }
}

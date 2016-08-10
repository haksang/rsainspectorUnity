using UnityEngine;
using System.Collections;
using UnityEngine.UI.Extensions;
using UnityEngine.SceneManagement;

public class InspectManager : MonoBehaviour
{

    public AutoCompleteComboBox a;
    public GameObject OverLay;

    // Use this for initialization
    void Start()
    {
        a.AvailableOptions.Add("AC-DECOY");
        a.AvailableOptions.Add("DC-CECOY");
        a.AvailableOptions.Add("DC-DECOY");
        a.AvailableOptions.Add("AC-CECOY-1공정");
        a.AvailableOptions.Add("AC-CECOY-2공정");
        a.RebuildPanel();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
                SceneManager.LoadScene(1);
        }
    }
}








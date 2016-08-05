using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class ModalPanel : MonoBehaviour {


    public Button yButton, nButton, cButton;
    public Text text;
    public GameObject modalPanelObject;

    private static ModalPanel modalpanel;

    public static ModalPanel Instance()
    {
        if (!modalpanel)
        {
            modalpanel = FindObjectOfType(typeof(ModalPanel)) as ModalPanel;
            if (!modalpanel)
                Debug.Log("already exist");
        }
        return modalpanel;
    }

    public void Choice(string question, UnityAction yesE, UnityAction noE, UnityAction cancelE)
    {
        yButton.onClick.RemoveAllListeners();
        yButton.onClick.AddListener(yesE);
        yButton.onClick.AddListener(ClosePanel);

        nButton.onClick.RemoveAllListeners();
        nButton.onClick.AddListener(noE);
        nButton.onClick.AddListener(ClosePanel);

        cButton.onClick.RemoveAllListeners();
        cButton.onClick.AddListener(cancelE);
        cButton.onClick.AddListener(ClosePanel);
    } 




    void ClosePanel()
    {
        modalPanelObject.SetActive(false);
    }

    void OnEnable()
    {
        
    }
}

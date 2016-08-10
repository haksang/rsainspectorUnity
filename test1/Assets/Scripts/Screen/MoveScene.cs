using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class MoveScene : MonoBehaviour {

    public Text text;
    string testText;
    int i = 0;
    // Use this for initialization
	void Start () {
        testText = "";
        testText = DB.GetUserData("SongSooMin", "abcde");
        DB.PutMachineList("SAVEEN2000","testText");
        DB.UpdateMachineList("destinsavee2000-1","5");
    }
    void Update()
    {
        text.text = testText;
    }

    public void datatest()
    {
         if(i%2== 0)
        {
            DB.PutUserData("testname" + i.ToString(), "abcde", "1");
            i++;
        }
        else
        {
            testText = DB.GetUserData("testname" + (i - 1).ToString(), "abcde");
            i++;
        }
        

    }

    public void gotoInit()
    {
        SceneManager.LoadScene(0);

    }
    public void gotoMain()
    {
        SceneManager.LoadScene(1);

    }
    public void gotoInspect()
    {
        SceneManager.LoadScene(2);

    }

    public void gotoBarcode()
    {
        SceneManager.LoadScene(3);
    }
}

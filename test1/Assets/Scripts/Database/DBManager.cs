using UnityEngine;
using System.Collections;

public class DBManager : MonoBehaviour {
  private string Worker="SSM";
  private string MachineName, MachineCode;
  private string Serial, SerialCode;
  //COUNTROW
  private string DB_NAME, TABLE_NAME, COLUMN;

	// Use this for initialization
	void Start () {
    try{
    } catch(MissingComponentException e) {
    Debug.Log ("Can't load DBManager Object");
    }
	}

	// Update is called once per frame
	void Update () {
    if(Input.GetKeyUp(KeyCode.Space)) {
      //(Id,password)
      Worker = DB.GetUserData("LeeHakSang","abcde");
    }
    else if(Input.GetKeyUp(KeyCode.UpArrow)) {
      try{  
        ArrayList AL_Machine = DB.GetMachineList();
        ShowArrayList(AL_Machine);
      } catch {
        Debug.Log("No MachinList");
      }
    }
    else if(Input.GetKeyUp(KeyCode.RightArrow)) {
      //CkList[0] -> MachineName CkList[1] -> MachineCode
      //ArrayList CkList = db.GetMachineCodeList();
      MachineName = "SAVEEN2000";
      //기기코드 버튼 클릭 -> ManchineNameList를 보여줌
      //그 중 하나를 선택하면 그에 해당하는 Code를 return
      MachineCode = DB.GetMachineCode(MachineName);
      //그 후 MachineList.db의 sqlite_sequence table에서 name과 일치하는 seq+1을 리턴(시리얼번호)
      Serial = DB.GetSerial(MachineName);
      SerialCode = MachineCode + "-" + Serial;
            //(MachineName,MachineCode,worker,serial)
            DB.PutMachineList(MachineName,SerialCode,Worker,Serial);
    }
    else if(Input.GetKeyUp(KeyCode.LeftArrow)) {
      ArrayList CkList = new ArrayList();
      //Return Process,iPoint
      //CkList = db.GetCheckingList(MachineName,PartName);

      //Return Id, Partname, Process, iPoint
      CkList = DB.GetCheckingList(MachineName);
      ShowArrayList(CkList);
    }
    else if(Input.GetKeyUp(KeyCode.DownArrow)) {

            //Parameter로 code 값을 넘겨주고 Table 생성
            DB.CreateInspectionTable(SerialCode);
            //code명 table에 파라미터로 넘긴 기계명이 가지고 있는 검사 항목(Id, Partname, Process, iPoint)들을  Table에 채워줌
            DB.FillInspectionTable(MachineName,SerialCode);

    }
    else if(Input.GetKeyUp(KeyCode.A)) {
      string InputData = "PASS";
      string Partname = "F11";
      string Process = "1";
            //PartName - Process 검색
            //ArrayList CkList = db.SearchInspectionTable_Both(SerialCode,Partname,Process);
            //ShowArrayList(CkList);
            //PartName 검색
            //CkList = db.SearchInspectionTable_PartName(SerialCode,Partname);
            //Process 검색
            //CkList = db.SearchInspectionTable_Process(SerialCode,Process);

            //지정한 table에 result 컬럼 값을 Update
            DB.InputInspectionTable_result(SerialCode,1,InputData);
            DB.InputInspectionTable_state(SerialCode,1,InputData);
      //지정한 table에 state 컬럼 값을 Update
    }
	}
  public void ShowArrayList(ArrayList A_List) {
      for(int i = 0; i < A_List.Count; i++)
      {
        for(int j = 0 ; j < (((string[])(A_List[i])).Length); j++)
        { 
          Debug.Log(((string[])(A_List[i]))[j]); 
        }
      }
 	 }
}

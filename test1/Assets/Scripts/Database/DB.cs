//Intialize AUTO_INCREMENT
//UPDATE SQLITE_SEQUENCE SET seq = 0 WHERE name = 'MachineList';
//부품 별 
///////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Data;
using System.Text;
using Mono.Data.Sqlite;

public class DB : MonoBehaviour {
	private static string connectionString;
	private static IDbConnection dbCon;
	private static IDbCommand dbCmd;
	private static IDataReader reader;

    public static String debuglog1, debuglog2, debuglog3;
	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
	}
	//DB Open
	public static void OpenDB(string dbname)
	{
        debuglog1 = "OpenDB:" + dbname;
        Debug.Log("OpenDB:" + dbname);
        
        string filepath;// = Application.persistentDataPath + "/"+ dbname;
        if (Application.platform == RuntimePlatform.Android)
        {
            filepath = Application.persistentDataPath + "/" + dbname;
            if (!File.Exists(filepath))
            {
                WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/" + dbname);
                loadDB.bytesDownloaded.ToString();
                while (!loadDB.isDone) { }
                File.WriteAllBytes(filepath, loadDB.bytes);
            }
        }
        else
        {
            filepath = Application.streamingAssetsPath + "/" + dbname;
            if (!File.Exists(filepath))
            {
               //File.Copy(Application.streamingAssetsPath + "/" + dbname, filepath);
            }
        }


        if(Application.platform == RuntimePlatform.Android)
        {
            connectionString = "URI=file:" + Application.persistentDataPath + "/" + dbname;
        }
        else
        {
            connectionString = "URI=file:" + Application.streamingAssetsPath + "/" + dbname;
        }
		
		Debug.Log("Stablishing connection to: " + connectionString);
        debuglog2 = "Stablishing connection to: " + connectionString;

    }
	public static void CloseDB()
	{
		try
		{
			if(reader!=null) {
				reader.Close();
				reader = null;
			}
			if(dbCmd!=null) {
				dbCmd.Dispose();
				dbCmd = null;
			}
			if(dbCon!=null) {
				dbCon.Close();
				dbCon = null;
			}	
		}
		catch (NullReferenceException e)
		{
			throw;
		}
	}
	//MachineList.db
	//Get MachineList into ArrayList
	public static ArrayList GetMachineList() {
		OpenDB("MachineList.db");
		ArrayList readArray = new ArrayList();

		using (dbCon = new SqliteConnection(connectionString)) {
			dbCon.Open();

			using(dbCmd = dbCon.CreateCommand()) {
				string sqlQuery = String.Format ("SELECT * FROM 'MachineList'");
				dbCmd.CommandText = sqlQuery;

				using (reader = dbCmd.ExecuteReader ()) {

					while (reader.Read ()) {
						int j = 0;
						string[] row = new string[reader.FieldCount];
						while (j < reader.FieldCount)
						{
							row [j] = reader.GetString (j);
							j++;
						}
						readArray.Add(row);
					}
				}
			}
		}
		CloseDB ();
		return readArray;
	}
	//MachineList.db
	//Get MachineList into ArrayList From State
	public static ArrayList GetMachineList(string state) {
		OpenDB("MachineList.db");
		ArrayList readArray = new ArrayList();

		using (dbCon = new SqliteConnection(connectionString)) {
			dbCon.Open();

			using(dbCmd = dbCon.CreateCommand()) {
				string sqlQuery = String.Format ("SELECT * FROM 'MachineList' WHERE state={0}",state);
				dbCmd.CommandText = sqlQuery;

				using (reader = dbCmd.ExecuteReader ()) {

					while (reader.Read ()) {
						int j = 0;
						string[] row = new string[reader.FieldCount];
						while (j < reader.FieldCount)
						{
							row [j] = reader.GetString (j);
							j++;
						}
						readArray.Add(row);
					}
				}
			}
		}
		CloseDB ();
		return readArray;
	}
	//MachineList.db
	//enroll checked machine as soon into DB table
	public static void PutMachineList(string machineName, string worker)
	{
		OpenDB("MachineList.db");
		string MachineCode = GetMachineCode(machineName);
		string Sequence = GetSerial(machineName);
		int seq = Convert.ToInt32(Sequence);
		Sequence = Convert.ToString(++seq);
			
      	string SerialCode = MachineCode + "-" + Sequence;
		string date = DateTime.Now.ToString("yyyy.MM.dd. HH:mm.");

		using (dbCon = new SqliteConnection(connectionString)) {
			dbCon.Open();

			using(dbCmd = dbCon.CreateCommand()) {

				Debug.Log(Sequence + " " + machineName);
				dbCmd.CommandText = String.Format("UPDATE sqlite_sequence SET seq='{0}' WHERE name='{1}'",Sequence,machineName);
				dbCmd.ExecuteScalar();	

				string sqlQuery = String.Format ("INSERT INTO MachineList (id,MachineCode,MachineName,Worker,Date,State) VALUES ('{0}','{1}','{2}','{3}','{4}','0')",
				Sequence, SerialCode, machineName, worker, date);
				dbCmd.CommandText = sqlQuery;
				dbCmd.ExecuteScalar();
				
			}
		}
		CloseDB();
	}
			
	//MachineList.db
	//enroll checked machine as soon into DB table
	public static void UpdateMachineList(string machineCode, string state)
	{
		OpenDB("MachineList.db");
		using (dbCon = new SqliteConnection(connectionString)) {
			dbCon.Open();

			using(dbCmd = dbCon.CreateCommand()) {
				dbCmd.CommandText = String.Format("UPDATE MachineList SET State='{0}' WHERE MachineCode='{1}'",state,machineCode);
				dbCmd.ExecuteScalar();	
			}
		}
		CloseDB();
	}
	//MachineList.db
	//enroll checked machine as soon into DB table
	public static string GetSerial(string machineName)
	{
			string Sequence = null;
			OpenDB("MachineList.db");
			using (dbCon = new SqliteConnection(connectionString)) {
				dbCon.Open();

				using(dbCmd = dbCon.CreateCommand()) {
					string sqlQuery = String.Format("SELECT seq FROM sqlite_sequence WHERE name = '{0}'",machineName);
					
					dbCmd.CommandText = sqlQuery;
					using (reader = dbCmd.ExecuteReader ()) {
						while (reader.Read ()) {
							Sequence = reader.GetString (0);
						}
					}
				}
			}
			CloseDB();
			return Sequence;
	}
	//MachineDB.db
	//Convert MachineName to MachineCode
	public static string GetMachineCode(string machineName)
	{
		OpenDB("MachineDB.db");
		string MachineCode=null;
		using (IDbConnection dbCon = new SqliteConnection(connectionString)) {
			dbCon.Open();

			using(IDbCommand dbCmd = dbCon.CreateCommand()) {
				//SELECT MachineCode FROM MachineCodeLink WHERE MachineName='MN1';
				string sqlQuery = String.Format("SELECT MachineCode FROM MachineCodeLink WHERE MachineName='{0}'",machineName);
				dbCmd.CommandText = sqlQuery;
				
				using (IDataReader reader = dbCmd.ExecuteReader ()) {
					while (reader.Read ()) {
						MachineCode = reader.GetString (0);
						Debug.Log (MachineCode);
					}
				}
			}
		}
		return MachineCode;
	}
	//MachineDB.db
	//Convert MachineCode to MachineName
	public static string GetMachineName(string machineCode)
	{
		OpenDB("MachineDB.db");
		string MachineName = null;;
		using (IDbConnection dbCon = new SqliteConnection(connectionString)) {
			dbCon.Open();

			using(IDbCommand dbCmd = dbCon.CreateCommand()) {
				//SELECT MachineName FROM MachineCodeLink WHERE MachineCode='MC1'
				string sqlQuery = String.Format("SELECT MachineName FROM MachineCodeLink WHERE MachineCode='{0}'",machineCode);
				dbCmd.CommandText = sqlQuery;

				using (IDataReader reader = dbCmd.ExecuteReader ()) {
					while (reader.Read ()) {
						MachineName = reader.GetString (0);
						Debug.Log (MachineName);
					}
				}
			}
		}
		return MachineName;
	}
	//MachineDB.db
	//GetMachinCodeLink to ArrayList
	public static ArrayList GetMachineCodeList()
	{
		OpenDB("MachineDB.db");	
		ArrayList readArray = new ArrayList();

		using (IDbConnection dbCon = new SqliteConnection(connectionString)) {
			dbCon.Open();

			using(IDbCommand dbCmd = dbCon.CreateCommand()) {
				//SELECT MachineName, MachineCode FROM MachineCodeLink
				string sqlQuery = String.Format("SELECT MachineName, MachineCode FROM MachineCodeLink");
				dbCmd.CommandText = sqlQuery;
				
				using (IDataReader reader = dbCmd.ExecuteReader ()) {
					while (reader.Read ()) {
						int j = 0;
						string[] row = new string[reader.FieldCount];
						while (j < reader.FieldCount)
						{
							row [j] = reader.GetString (j);
							j++;
						}
						readArray.Add(row);
					}
				}
			}
		}
		return readArray;
	}
	//MachineDB.db
	//Get Checking List from DB depending on the code uisng GetInspectionList Function (Overridding)
	public static ArrayList GetCheckingList(string machineName)
	{
		ArrayList checkList = new ArrayList();
		ArrayList getList = new ArrayList();
		OpenDB("MachineDB.db");
	
		using(dbCon = new SqliteConnection(connectionString)) {
			dbCon.Open();
			using(dbCmd = dbCon.CreateCommand()) {
				//SELECT InspectionCode FROM MN1 WHERE Partname='P1'
				string sqlQuery = String.Format("SELECT Id, Partname, InspectionCode, Process FROM {0} ORDER BY Id",machineName);
				Debug.Log(sqlQuery);
				dbCmd.CommandText = sqlQuery;
				
				using (reader = dbCmd.ExecuteReader ()) {
					while (reader.Read ()) {
						int j = 0;
						string[] row = new string[reader.FieldCount];
						while (j < reader.FieldCount)
						{
							row [j] = reader.GetString (j);
							j++;
						}
						getList.Add(row);
					}
				};

				char[] delimiterChars = {'-'};
				
				for(int i = 0; i < getList.Count; i++)
				{
					string[] Splitcode = (((string[])getList[i])[2]).Split(delimiterChars);
					int process = Convert.ToInt32(((string[])getList[i])[3]);
					for(int j = 0; j < Splitcode.Length; j++)
					{
						string s_index = Convert.ToString((Splitcode[j])[1])+Convert.ToString((Splitcode[j])[2]);
						int i_index = Convert.ToInt32(s_index);
						s_index = Convert.ToString(i_index);
						//SELECT A FROM InspectionList WHERE Row='1';
						sqlQuery = String.Format("SELECT {0} FROM InspectionData WHERE Row='{1}'",(Splitcode[j])[0],s_index);
						Debug.Log(sqlQuery);
						dbCmd.CommandText = sqlQuery;
						using (reader = dbCmd.ExecuteReader ()) {
							while (reader.Read ()) {
								string[] row = new string[4];
								row[0] = (((string[])getList[i])[0]);
								row[1] = (((string[])getList[i])[1]);
								row[2] = Convert.ToString(process);
								row[3] = reader.GetString(0);
								checkList.Add(row);
							}
						}
					}
				}
			}
		}
		CloseDB();
		return checkList;
	}
	//MachineDB.db
	//Get Checking List from DB depending on the code using GetInspectionList Function (Overridding)
	public static ArrayList GetCheckingList(string machineName, string partName)
	{
		ArrayList getList = new ArrayList();
		ArrayList checkList = new ArrayList();
		OpenDB("MachineDB.db");
		
		using(dbCon = new SqliteConnection(connectionString)) {
			dbCon.Open();
			using(dbCmd = dbCon.CreateCommand()) {
				//SELECT InspectionCode FROM MN1 WHERE Partname='P1'
				string sqlQuery = String.Format("SELECT InspectionCode, Process FROM {0} WHERE Partname='{1}'",machineName,partName);
				Debug.Log(sqlQuery);
				dbCmd.CommandText = sqlQuery;
				
				using (reader = dbCmd.ExecuteReader ()) {
					while(reader.Read ()) {
						int j = 0;
						string[] row = new string[reader.FieldCount];
						while (j < reader.FieldCount)
						{
							row [j] = reader.GetString (j);
							j++;
						}
						getList.Add(row);
					}
				};

				char[] delimiterChars = {'-'};
				
				for(int i = 0; i < getList.Count; i++)
				{
					string[] Splitcode = (((string[])getList[i])[0]).Split(delimiterChars);
					int process = Convert.ToInt32(((string[])getList[i])[1]);
					for(int j = 0; j < Splitcode.Length; j++)
					{
						string s_index = Convert.ToString((Splitcode[j])[1])+Convert.ToString((Splitcode[j])[2]);
						int i_index = Convert.ToInt32(s_index);
						s_index = Convert.ToString(i_index);
						//SELECT A FROM InspectionList WHERE Row='1';
						sqlQuery = String.Format("SELECT {0} FROM InspectionList WHERE Row='{1}'",(Splitcode[j])[0],s_index);
						Debug.Log(sqlQuery);
						dbCmd.CommandText = sqlQuery;
						using (reader = dbCmd.ExecuteReader ()) {
							while (reader.Read ()) {
								string[] row = new string[2];
								row[0] = reader.GetString (0);
								row[1] = Convert.ToString(process);
								checkList.Add(row);
							}
						}
					}
				}
			}
		}
		CloseDB();
		return checkList;
	}

    //Userdata.db
    //Put UserData
    public static void PutUserData(string name, string password, string grade)
    {
        Debug.Log("New Slave");
        OpenDB("UserData.db");
        bool token = false;
        //connectionString = "URI=file:" + Application.dataPath + "/Database/Userdata.db";
        using (dbCon = new SqliteConnection(connectionString))
        {
            dbCon.Open();
            using (dbCmd = dbCon.CreateCommand())
            {
                string sqlQuery = String.Format("INSERT INTO userdata (Name,Password,Grade) VALUES ('{0}','{1}','{2}')", name, password, grade);
                dbCmd.CommandText = sqlQuery;
                dbCmd.ExecuteScalar();
            }
        }
        CloseDB();
    }

    //Userdata.db
    //Check UserData and Return Worker Name
    public static string GetUserData(string name,string password)
	{
		Debug.Log ("Login");
		OpenDB ("UserData.db");
		bool token = false;
		//connectionString = "URI=file:" + Application.dataPath + "/Database/Userdata.db";
		using (dbCon = new SqliteConnection (connectionString))
		{
			dbCon.Open ();
			using (dbCmd = dbCon.CreateCommand ())
			{
				//SELECT name, password FROM userdata WHERE Name='LeeHakSang' AND Password='abcde';
				string sqlQuery = String.Format ("SELECT name, password FROM userdata WHERE Name='{0}' AND Password='{1}';",name, password);
				dbCmd.CommandText = sqlQuery;
				Debug.Log (sqlQuery);
                
				using (reader = dbCmd.ExecuteReader ())
				{
					ArrayList readArray = new ArrayList ();
					while (reader.Read())
					{
						int j = 0;
						string[] row = new string[reader.FieldCount];
						while (j < reader.FieldCount)
						{
							row [j] = reader.GetString (j);
							j++;
						}
						readArray.Add (row);
					}
					try {
						if ((((string[])readArray[0])[0]).Equals(name) && (((string[])readArray[0])[1]).Equals(password))
						{
							token = true;
							Debug.Log ("Correct");
						} else
						{
							token = false;
							Debug.Log ("No Correct");
						}
					} catch(ArgumentOutOfRangeException e) {
						token = false;
						Debug.Log ("No Data");
					}
				}
			}
		}
		CloseDB ();
		if(token)
		{
			return name;
		}
		return null;
	}
	//InspectionDB.db
	//파라미터로 전달받은 Code의 이름으로 InspectionDb.db에 Table 생성
	public static void CreateInspectionTable(string code) {
		OpenDB("InspectionData.db");
		using (dbCon = new SqliteConnection(connectionString)) {
			dbCon.Open();

			using(dbCmd = dbCon.CreateCommand()) {
				string sqlQuery = String.Format ("CREATE TABLE '{0}' (Row INTEGER PRIMARY KEY AUTOINCREMENT,Id varchar(255), PartName varchar(255), Process varchar(255), iPoint varchar(255), iCategory varchar(255) NOT NULL, iTest varchar(255) NOT NULL, result varchar(255) NOT NULL, state varchar (255) NOT NULL)",code);
				dbCmd.CommandText = sqlQuery;
				//일련번호에 따른 table 생성
				dbCmd.ExecuteScalar();
			}
		}
		CloseDB ();	
	}
	//InspectionDb.db
	//Code의 이름으로 만든 Table 안에 파라미터로 전달한 MachineName의 Id,PartName,Process,iPoint를 채워넣어줌
	//iCategory, iTest, result, state는 
	public static void FillInspectionTable(string machineName,string code) {

		ArrayList InsertArray = GetCheckingList(machineName);
		OpenDB("InspectionData.db");

		using (dbCon = new SqliteConnection(connectionString)) {
			dbCon.Open();

			using(dbCmd = dbCon.CreateCommand()) {
				Debug.Log(InsertArray.Count);
				for(int i = 0; i < InsertArray.Count; i++)
      			{
					string sqlQuery = String.Format ("INSERT INTO '{0}' (Id,PartName,Process,iPoint,iCategory,iTest,result,state) VALUES ('{1}','{2}','{3}','{4}','PF','PF','NA','NA')",
					code,(((string[])InsertArray[i])[0]),(((string[])InsertArray[i])[1]),(((string[])InsertArray[i])[2]),(((string[])InsertArray[i])[3]));
					dbCmd.CommandText = sqlQuery;
					dbCmd.ExecuteScalar();
     			}
			}
		}
		CloseDB ();
	}
	//InspectionData.db (Overridding)
	//Search 기능 Process별 검색
	public static ArrayList SearchInspectionTable_Process(string code, string process) {
		OpenDB("InspectionData.db");	
		ArrayList readArray = new ArrayList();

		using (IDbConnection dbCon = new SqliteConnection(connectionString)) {
			dbCon.Open();

			using(IDbCommand dbCmd = dbCon.CreateCommand()) {
				//SELECT MachineName, MachineCode FROM MachineCodeLink
				string sqlQuery = String.Format("SELECT * FROM '{0}' WHERE Process='{1}'",code,process);
				dbCmd.CommandText = sqlQuery;
				
				using (IDataReader reader = dbCmd.ExecuteReader ()) {
					while (reader.Read ()) {
						int j = 0;
						string[] row = new string[reader.FieldCount];
						while (j < reader.FieldCount)
						{
							row [j] = reader.GetString (j);
							j++;
						}
						readArray.Add(row);
					}
				}
			}
		}
		return readArray;
	}
	//InspectionData.db (Overridding)
	//Search 기능 PartName별 검색
	public static ArrayList SearchInspectionTable_PartName(string code, string partname) {
		OpenDB("InspectionData.db");	
		ArrayList readArray = new ArrayList();

		using (IDbConnection dbCon = new SqliteConnection(connectionString)) {
			dbCon.Open();

			using(IDbCommand dbCmd = dbCon.CreateCommand()) {
				//SELECT MachineName, MachineCode FROM MachineCodeLink
				string sqlQuery = String.Format("SELECT * FROM '{0}' WHERE PartName='{1}'",code,partname);
				dbCmd.CommandText = sqlQuery;
				
				using (IDataReader reader = dbCmd.ExecuteReader ()) {
					while (reader.Read ()) {
						int j = 0;
						string[] row = new string[reader.FieldCount];
						while (j < reader.FieldCount)
						{
							row [j] = reader.GetString (j);
							j++;
						}
						readArray.Add(row);
					}
				}
			}
		}
		return readArray;
	}
	//InspectionData.db (Overridding)
	//Search 기능 Process - PartName별 동시 검색 
	public static ArrayList SearchInspectionTable_Both(string code, string partname, string process) {
		OpenDB("InspectionData.db");	
		ArrayList readArray = new ArrayList();

		using (IDbConnection dbCon = new SqliteConnection(connectionString)) {
			dbCon.Open();

			using(IDbCommand dbCmd = dbCon.CreateCommand()) {
				string sqlQuery = String.Format("SELECT * FROM '{0}' WHERE PartName='{1}' AND Process='{2}'",
				code,partname,process);
				Debug.Log(sqlQuery);
				dbCmd.CommandText = sqlQuery;
				
				using (IDataReader reader = dbCmd.ExecuteReader ()) {
					while (reader.Read ()) {
						int j = 1;
						string[] row = new string[reader.FieldCount];
						row [0] = Convert.ToString(reader.GetInt32(0));
						while (j < reader.FieldCount)
						{
							row [j] = reader.GetString (j);
							j++;
						}
						readArray.Add(row);
					}
				}
			}
		}
		return readArray;
	}
	//InspectionData.db
	public static void InputInspectionTable_result(string code, int row, string data)
	{
		OpenDB("InspectionData.db");
		using (dbCon = new SqliteConnection(connectionString)) {
			dbCon.Open();

			using(dbCmd = dbCon.CreateCommand()) {
				dbCmd.CommandText = String.Format("UPDATE '{0}' SET result='{1}' WHERE row={2}",code,data,row);
				Debug.Log(dbCmd.CommandText);
				dbCmd.ExecuteScalar();	
			}
		}
		CloseDB();
	}
	//InspectionData.db
	public static void InputInspectionTable_state(string code, int row,string data)
	{
		OpenDB("InspectionData.db");
		using (dbCon = new SqliteConnection(connectionString)) {
			dbCon.Open();

			using(dbCmd = dbCon.CreateCommand()) {
				dbCmd.CommandText = String.Format("UPDATE '{0}' SET state='{1}' WHERE row={2}",code,data,row);
				dbCmd.ExecuteScalar();	
			}
		}
		CloseDB();
	}
	//Searching Row Count
	public static int CountRow(string DB_name, string table_name, string column_name)
	{
		int count = 0;
		OpenDB(DB_name);
		using (dbCon = new SqliteConnection(connectionString)) {
			dbCon.Open();
			using(dbCmd = dbCon.CreateCommand()) {
				//SELECT COUNT(id) FROM MachineList;
				string sqlQuery = String.Format("SELECT COUNT({0}) FROM {1}",column_name,table_name);
				Debug.Log(sqlQuery);
				dbCmd.CommandText = sqlQuery;

				using (reader = dbCmd.ExecuteReader ()) {
					while (reader.Read ()) {
						count = reader.GetInt32 (0);
						Debug.Log(count);
					}
				}
			}
		}
		CloseDB();
		return count;
	}

    public static void ShowArrayList(ArrayList A_List)
    {
        for (int i = 0; i < A_List.Count; i++)
        {
            for (int j = 0; j < (((string[])(A_List[i])).Length); j++)
            {
                Debug.Log(((string[])(A_List[i]))[j]);
            }
        }
    }
}
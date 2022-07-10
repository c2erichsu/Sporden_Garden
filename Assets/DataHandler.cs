using Amazon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataHandler : MonoBehaviour {

	public static PlayerData playerData;
    public GameObject plantBoxes;
    public static GameObject plantBoxes_static;
    public DynamoDBHandler dynamoDBHandler;

    // dominate jsonHandler destroy or not
    public static GameObject obj;

    private void Awake()
    {
        if (obj != null)
        {
            Destroy(this.gameObject);
        }
        else {
            obj = gameObject;
        }

        playerData = LoadJson();
        plantBoxes_static = plantBoxes;
        UnityInitializer.AttachToGameObject(this.gameObject);
    }

    public static void UpdateJson(PlayerData newJson) {
        string newString = JsonUtility.ToJson(newJson);
        StreamWriter file = new StreamWriter(Application.persistentDataPath + "/" + GetUserAccount.NowUser + ".json");
        file.Write(newString);
        file.Close();
    }

    public void UpdateJsonBtn()
    {
        UpdatePlantBoxState();
        
    }

    public void UpdatePlantBoxState()
    {
        
        UpdateJson(playerData);
    }

    

    public static PlayerData LoadJson()
    {
        StreamReader file = new StreamReader(Application.persistentDataPath + "/" + GetUserAccount.NowUser + ".json");
        string loadJson = file.ReadToEnd();
        file.Close();

        PlayerData loadData = new PlayerData();
        loadData = JsonUtility.FromJson<PlayerData>(loadJson);
        return loadData;
    }

    public static PlayerData LoadJsonWithAccount(string account)
    {
        StreamReader file = new StreamReader(Application.persistentDataPath + "/" + account + ".json");
        string loadJson = file.ReadToEnd();
        file.Close();

        PlayerData loadData = new PlayerData();
        loadData = JsonUtility.FromJson<PlayerData>(loadJson);
        return loadData;
    }

    public static void GainPlant(int id)
    {
        playerData.warehouse[id] += 1;
        UpdateJson(playerData);
    }

    public static void UsePlant(int id)
    {
        playerData.warehouse[id] -= 1;
        UpdateJson(playerData);
    }

    public static void DoOnceLottery(int engineNum)
    {
        playerData.token[engineNum-1] -= 1;
        UpdateJson(playerData);
    }


    #region DynamoDB Query
    public void DynamoDB_QueryTimeDataFunction()
    {
        StartCoroutine(GiveTokenLogic());
    }

    IEnumerator GiveTokenLogic()
    {
        PlayerData oldData = DataHandler.playerData;

        yield return UpdateTimeDataToJson();

        int diff = DataHandler.playerData.timeData.Count - oldData.timeData.Count;
        List<string> newData = new List<string>();
        print("diff: " + diff);

        for (int i = diff; i > 0; i--)
        {
            DateTime td = TimeConverter.UnixTimeStampToDateTime(Convert.ToDouble(DataHandler.playerData.timeData[DataHandler.playerData.timeData.Count - i]));
            newData.Add(td.ToString("yyyyMMdd"));
            //Debug.Log("New Data: " + td.ToString("yyyyMMdd"));
        }

        string todayDate = DateTime.Now.ToString("yyyyMMdd");
        // analyze how many times user done exercise today (only today)
        int count = 0;
        for (int i = 0; i < newData.Count; i++)
        {
            print(newData[i]);
            if (newData[i] == todayDate) { count += 1; }
        }

        if (count <= 3)
        {
            DataHandler.playerData.token[0] += count;
        }
        else if (3 < count || count <= 5)
        {
            DataHandler.playerData.token[1] += (count - 3);
        }
        else if (count > 5)
        {
            DataHandler.playerData.token[2] += (count - 5);
        }
        print(count);
    }

    IEnumerator UpdateTimeDataToJson()
    {
        PlayerData newJson = DataHandler.LoadJson();
        yield return DynamoDB_QueryTimeData();
        newJson.timeData = dynamoDBHandler.timeData;
        string td = JsonUtility.ToJson(newJson);
        StreamWriter file = new StreamWriter(Application.persistentDataPath + "/" + GetUserAccount.NowUser + ".json");
        file.Write(td);
        file.Close();
        // sync with playerData
        DataHandler.playerData = newJson;
    }

    IEnumerator DynamoDB_QueryTimeData()
    {
        dynamoDBHandler.DynamoDBQuery(null);
        yield return new WaitUntil(() => { return dynamoDBHandler.LoadCheck; });
        //Debug.Log("Query Data: " + dynamoDBHandler.resultText.text);   
    }
    #endregion



}

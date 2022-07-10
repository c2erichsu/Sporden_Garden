using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    public Text Loading;
    public Text progressRate;
    public DynamoDBHandler dynamoDBHandler;
    public GameObject Handler;
    public GetUserAccount gua;

    private void Start()
    {
        Loading.text = "載入中";
        StartCoroutine(StartGameInitial());
        StartCoroutine(LoadingPerformance());
    }

    IEnumerator LoadingPerformance() {
        while (1 == 1) {
            yield return new WaitForSeconds(1);
            Loading.text += ".";
            if (Loading.text == "載入中....") { Loading.text = "載入中"; }
        }
    }

    IEnumerator StartGameInitial() {
        progressRate.text = "0 / 4";
        yield return new WaitForSecondsRealtime(1);
        yield return GetNowAccount();
        yield return new WaitForSecondsRealtime(2);
        progressRate.text = "1 / 4";
        yield return CheckPlayerPrefsFile();
        yield return new WaitForSecondsRealtime(1);
        progressRate.text = "2 / 4";
        yield return GiveTokenLogic();
        progressRate.text = "3 / 4";
        yield return new WaitForSecondsRealtime(1);
        progressRate.text = "4 / 4";


        SceneManager.LoadScene("myGarden");
    }

    IEnumerator GetNowAccount() {
        
        gua.DynamoDBQuery(null);
        yield return new WaitUntil(() => { return gua.LoadCheck; });

    }

    IEnumerator CheckPlayerPrefsFile()
    {
        if (!File.Exists(Application.persistentDataPath + "/" + GetUserAccount.NowUser + ".json"))
        {
            Handler.GetComponent<S3Handler>().GetObject();

            yield return new WaitUntil(() => { return S3Handler.Checker; });
            File.Move(Application.persistentDataPath + "/playerprefs.json", Application.persistentDataPath + "/" + GetUserAccount.NowUser + ".json");
        }

        PlayerData playerData = DataHandler.LoadJson();
    }

    #region DynamoDB Query & GiveTokenLogic
    public void DynamoDB_QueryTimeDataFunction()
    {
        StartCoroutine(GiveTokenLogic());
    }

    IEnumerator GiveTokenLogic()
    {
        // Load PlayerPrefs to process Logic
        DataHandler.playerData = DataHandler.LoadJson();

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
            InitialHandler.QuantityToken1 = count;
        }
        else if (3 < count || count <= 5)
        {
            DataHandler.playerData.token[1] += (count - 3);
            InitialHandler.QuantityToken2 = count -3;
        }
        else if (count > 5)
        {
            DataHandler.playerData.token[2] += (count - 5);
            InitialHandler.QuantityToken3 = count -5;
        }
        print("Today done: " + count);
        DataHandler.UpdateJson(DataHandler.playerData);
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
        Debug.Log("Query Data: " + dynamoDBHandler.resultText.text);   
    }
    #endregion

}

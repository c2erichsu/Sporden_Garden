using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {


    public DataHandler DataHandler;
    public GameObject UI_Garden;
    public GameObject flowers;

    public GameObject plantBoxes;
    public int plantBoxQuantity;

    public GameObject rose;
    public GameObject phalaenopsis;
    public GameObject pear;
    public GameObject tomato;
    public GameObject lotus;
    public GameObject hydrangea;
    public GameObject purple;
    public GameObject yellow;


    

    private void Awake()
    {
        DataHandler = GameObject.Find("DataHandler").GetComponent<DataHandler>() ;
        
    }

    private void Start()
    {   
        InitialGame();
        DontDestroyOnLoad(DataHandler);
    }

    private void Update()
    {
      
    }


    public GameObject GetPlant(int id)
    {
        switch (id)
        {
            case 0: return null;
            case 1: return rose;
            case 2: return phalaenopsis;
            case 3: return pear;
            case 4: return tomato;
            case 5: return lotus;
            case 6: return hydrangea;
            case 7: return purple;
            case 8: return yellow; 

            default: return null;
        }
    }

    public void InitialGame()
    {
        PlayerData jsonData = DataHandler.LoadJson();
        for (int i = 0; i < plantBoxes.transform.childCount; i++)
        {
            if (jsonData.plant[i] != 0)
            {
                // Grid Layout should be turned off
                GameObject Plant = Instantiate(GetPlant(jsonData.plant[i]), plantBoxes.transform.GetChild(i).transform.position, new Quaternion(0, 0, 0, 0), flowers.transform);
                Plant.GetComponent<PlantManager>().onPlantBox = i;
            }
        }
    }



    public void RunGetFriendPlayerPrefs() {
        StartCoroutine(getFriendPlayprefs());
    }

    IEnumerator getFriendPlayprefs()
    {
        DataHandler.GetComponent<S3Handler>().GetFriendsObject();
        yield return new WaitUntil(() => { return S3Handler.Checker; });
        SceneManager.LoadScene("FriendsGarden");
    }

    public void RunUpdateFriendData() {
        StartCoroutine(UpdateAllFriendData());
    }

    IEnumerator UpdateAllFriendData() {
        int HighScore = 0;
        int friendId = 0;

        for (int i = 0; i < DataHandler.playerData.myFriend.Count; i++) {
            UIFriendList.myFriendAccount = DataHandler.playerData.myFriend[i];
            DataHandler.GetComponent<S3Handler>().GetFriendsObject();
            yield return new WaitUntil(() => { return S3Handler.Checker; });

            PlayerData friendData = DataHandler.LoadJsonWithAccount(UIFriendList.myFriendAccount);
            int score = 0;
            for (int j = 0; j < plantBoxes.transform.childCount; j++) {
                score += friendData.plant[j];
            }
            GameObject.Find("Content").transform.GetChild(i).GetChild(1).GetComponent<Text>().text = "收藏度："+ score * 10;
            if (score > HighScore) {
                friendId = i;
            }
        }

        GameObject.Find("Content").transform.GetChild(friendId).GetChild(3).GetComponent<SpriteRenderer>().enabled = true;

    }

}

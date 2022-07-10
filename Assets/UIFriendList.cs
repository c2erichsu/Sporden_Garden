using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIFriendList : MonoBehaviour {

    public static string myFriendAccount;
    public S3Handler S3Handler;
    public GameObject gm;

    public GameObject BTNFriend;
    void Start() {
        gm = GameObject.Find("GameManager");
        print(gm);

        for (int i = 0; i < DataHandler.playerData.myFriend.Count; i++)
        {
            GameObject btn = Instantiate(BTNFriend);
            btn.transform.SetParent(transform.GetChild(2).GetChild(0).GetChild(0).transform, false);
            btn.transform.GetChild(0).GetComponent<Text>().text = DataHandler.playerData.myFriend[i];
        }

        GameObject.Find("GameManager").GetComponent<GameManager>().RunUpdateFriendData();
    }



    public void GoToFriendGarden()
    {
        GameObject thisBtn = EventSystem.current.currentSelectedGameObject;
        myFriendAccount = thisBtn.transform.parent.transform.GetChild(0).GetComponent<Text>().text;
        print(thisBtn.transform.parent.transform.GetChild(0).GetComponent<Text>().text);
        GameObject.Find("GameManager").GetComponent<GameManager>().RunGetFriendPlayerPrefs();
    }

    public void UpdataFriendData() {
        GameObject.Find("GameManager").GetComponent<GameManager>().RunUpdateFriendData();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendsGardenManager : MonoBehaviour {

    public GameManager gm;
    public GameObject plantBoxes;
    public GameObject flowers;

	// Use this for initialization
	void Start () {
        FriendsGarden();

    }
	
	// Update is called once per frame
	void Update () {
		
	}



    public void FriendsGarden()
    {
        PlayerData FriendData = DataHandler.LoadJsonWithAccount(UIFriendList.myFriendAccount);
        for (int i = 0; i < plantBoxes.transform.childCount; i++)
        {
            if (FriendData.plant[i] != 0)
            {
                // Grid Layout should be turned off
                GameObject Plant = Instantiate(gm.GetPlant(FriendData.plant[i]), plantBoxes.transform.GetChild(i).transform.position, new Quaternion(0, 0, 0, 0), flowers.transform);
                Plant.GetComponent<PlantManager>().onPlantBox = i;
            }
        }


    }
}

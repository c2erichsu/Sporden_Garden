using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIHandbook : MonoBehaviour {
    public GameManager gm;


    public GameObject BookBtn;
    public GameObject PlantInfo;

    private string btnValue;
    string path;
    private void Start()
    {
        PlayerData playerData = DataHandler.playerData;
        print(gameObject.name);
        //PlayerData playerData = DataHandler.GetComponent<JsonHandler>().playerData;
        for (int i = 0; i < playerData.warehouse.Length; i++)
        {
            if (playerData.warehouse[i] != 0)
            {
                GameObject bookBtn = Instantiate(BookBtn);
                bookBtn.transform.SetParent(this.transform.GetChild(1),false);

                //要改成花朵名稱
                bookBtn.transform.GetChild(0).GetComponent<Image>().sprite = gm.GetPlant(i).GetComponent<SpriteRenderer>().sprite;
                bookBtn.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = playerData.id[i].ToString();

                //set name
                path = gm.GetPlant(i).name;
                bookBtn.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = XmlHandler.GrabXMLAssest(path); ; 
                bookBtn.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = "剩餘數量：" + playerData.warehouse[i].ToString();
            }
        }
        

    }



    private GameObject UI_plantInfo;
    public void Ins_PlantInfoUI() {
        UI_plantInfo = Instantiate(PlantInfo);
        GameObject UI_Garden = GameObject.Find("UI_Garden");
        UI_plantInfo.transform.SetParent(UI_Garden.transform, false);
        GameObject thisBtn = EventSystem.current.currentSelectedGameObject;
        int id = int.Parse(thisBtn.transform.GetChild(0).GetComponent<Text>().text);

        // set id

        UI_plantInfo.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = id.ToString();
        // set Name
        path = gm.GetPlant(id).name;
        UI_plantInfo.transform.GetChild(1).GetComponent<Text>().text = XmlHandler.GrabXMLAssest(path);
        // set picture
        UI_plantInfo.transform.GetChild(2).GetComponent<Image>().sprite = gm.GetPlant(id).GetComponent<SpriteRenderer>().sprite;
        // set introContent
        
        path = gm.GetPlant(id).name + "/intro" + XmlHandler.LanController.ToString();
        UI_plantInfo.transform.GetChild(4).GetComponent<Text>().text = XmlHandler.GrabXMLAssest(path);
        // set rare
        path = gm.GetPlant(id).name + "/rare";
        UI_plantInfo.transform.GetChild(6).GetComponent<Text>().text = XmlHandler.GrabXMLAssest(path);
    }

    public void DestroyHandBook()
    {
        Camera.main.GetComponent<RayHandler>().enabled = true;
        GameObject.Find("Blocker").GetComponent<Image>().enabled = false;
        Destroy(GameObject.Find("UI_Handbook(Clone)"));
    }

    public void DestroyPlantInfo() {
        Debug.Log("Destroy UI_plantInfo");
        Destroy(UI_plantInfo);
    }

    public void Ins_PlantToGarden() {
        int id = int.Parse(UI_plantInfo.transform.GetChild(2).GetChild(0).GetComponent<Text>().text);
        GameObject Ins_Point = GameObject.Find("Ins_Point");
        GameObject InsFlower = Instantiate(gm.GetPlant(id),new Vector2(Ins_Point.transform.position.x, Ins_Point.transform.position.y), transform.rotation);
        InsFlower.transform.SetParent(GameObject.Find("Flowers").transform);
        InsFlower.transform.tag = "Flower";

        DataHandler.UsePlant(id);

        GameObject Blocker = GameObject.Find("Blocker");
        Blocker.GetComponent<Image>().enabled = false;
        Destroy(GameObject.Find("UI_Handbook(Clone)"));
        Destroy(GameObject.Find("UI_PlantInfo(Clone)"));
        Camera.main.GetComponent<RayHandler>().enabled = true;
    }
}

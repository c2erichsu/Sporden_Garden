using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGarden : MonoBehaviour {
    public GameManager gm;
    public GameObject Blocker;

    public static bool GainToken_Controllor;
    public static bool PlantManager_Onclick;

    public GameObject GainTokenView;
    public GameObject UI_setting;
    public GameObject UI_friend;
    GameObject UI_gainToken;
    private void Start()
    {
        Debug.Log("今天運動了" + InitialHandler.QuantityToken1 + "次");
        if (InitialHandler.QuantityToken1 > 0)
        {
            Camera.main.GetComponent<RayHandler>().enabled = false;
            UI_gainToken = Instantiate(GainTokenView);

            UI_gainToken.transform.SetParent(transform, false);
            UI_gainToken.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "A抽獎券 * " + InitialHandler.QuantityToken1;

            if (InitialHandler.QuantityToken2 > 0)
            {
                UI_gainToken.transform.GetChild(0).GetChild(1).GetComponent<Text>().text += "\n B抽獎券 * " + InitialHandler.QuantityToken2;
            }

            if (InitialHandler.QuantityToken3 > 0)
            {
                UI_gainToken.transform.GetChild(0).GetChild(1).GetComponent<Text>().text += "\n C抽獎券 * " + InitialHandler.QuantityToken3;
            }
        }
    }

    private void Update()
    {
        if (UIGarden.GainToken_Controllor)
        {
            UI_gainToken.transform.Translate(Vector2.left * 25 * Time.deltaTime);
            if (UI_gainToken.GetComponent<RectTransform>().transform.position.y > 10)
            {
                InitialHandler.QuantityToken1 = 0;
                Destroy(UI_gainToken);
                UIGarden.GainToken_Controllor = false;
            }
        }

        if (PlantManager.plantManagerUI != null)
        {
            if (UIGarden.PlantManager_Onclick)
            {
                Destroy(PlantManager.plantManagerUI);
                PlantManager.plantManagerUI = null;
            }
        }
    }


    public void OpenHandbook(GameObject handbook)
    {
        handbook = Instantiate(handbook);
        handbook.transform.SetParent(this.gameObject.transform,false);
        Blocker.GetComponent<Image>().enabled = true;
        // off Ray function
        Camera.main.GetComponent<RayHandler>().enabled = false;
    }

    public void DestroyPlantInfo() {
        Destroy(GameObject.Find("UI_Handbook(Clone)"));
        Destroy(GameObject.Find("UI_PlantInfo(Clone)"));
        Blocker.GetComponent<Image>().enabled = false;
    }

    public void DestroyHandBook()
    {
        Camera.main.GetComponent<RayHandler>().enabled = true;
        Destroy(GameObject.Find("Handbook(Clone)"));
    }
    

    public void PlantManager_ConfirmMyAdjust()
    {
        //if (RayHandler.hitPlantBox.GetComponent<SpriteRenderer>().color != new Color(0, 255, 0))
        //{
        //    print("有問題");
        //}
        //else {
        int i = RayHandler.hitPlantBox.GetComponent<PlantBoxManager>().id;
        DataHandler.playerData.plant[i] = PlantManager.SelectPlant.GetComponent<PlantManager>().id;
        int d = PlantManager.TempData.GetComponent<PlantManager>().onPlantBox;
        DataHandler.playerData.plant[d] = 0;
        DataHandler.UpdateJson(DataHandler.playerData);

        Destroy(PlantManager.TempData);
        PlantManager.SelectPlant.tag = "Flower";


        PlantManager_Onclick = true;
        PlantManager.SelectPlant = null;
        RayHandler.hitPlant = null;
    }

    public void PlantManager_CancelMyAdjust()
    {
        PlantManager.SelectPlant.transform.position = PlantManager.TempData.transform.position;
        Destroy(PlantManager.TempData);

        PlantManager.SelectPlant.tag = "Flower";

        PlantManager_Onclick = true;
        PlantManager.SelectPlant = null;
        RayHandler.hitPlant = null;
    }

    public void PlantManager_StoragePlants()
    {
        DataHandler.GainPlant(PlantManager.SelectPlant.GetComponent<PlantManager>().id);
        Destroy(PlantManager.SelectPlant);
        Destroy(PlantManager.TempData);
        RayHandler.hitPlantBox.GetComponent<PlantBoxManager>().plant = 0;
        DataHandler.playerData.plant[RayHandler.hitPlantBox.GetComponent<PlantBoxManager>().id] = 0;
        int d = PlantManager.TempData.GetComponent<PlantManager>().onPlantBox;
        DataHandler.playerData.plant[d] = 0;
        DataHandler.UpdateJson(DataHandler.playerData);

        PlantManager_Onclick = true;
        PlantManager.SelectPlant = null;
        RayHandler.hitPlant = null;
    }

    public void GoToDraw() {
        SceneManager.LoadScene("Lottery");
    }

    public void OpenFriend() {
        GameObject friendView = Instantiate(UI_friend);
        friendView.transform.SetParent(this.gameObject.transform, false);
        Blocker.GetComponent<Image>().enabled = true;
        // off Ray function
        Camera.main.GetComponent<RayHandler>().enabled = false;
    }

    public void DestroyFriendView()
    {
        Camera.main.GetComponent<RayHandler>().enabled = true;
        GameObject.Find("Blocker").GetComponent<Image>().enabled = false;
        Destroy(GameObject.Find("UI_Friend(Clone)"));
    }

    public void DestroyGainTokenView()
    {
        Camera.main.GetComponent<RayHandler>().enabled = true;
        Destroy(GameObject.Find("UI_GainToken(Clone)"));
    }

    public void ClickGoTrue() {
        Camera.main.GetComponent<RayHandler>().enabled = true;
        GainToken_Controllor = true;
    }

    public void OpenSetting(GameObject setting) {
        setting = Instantiate(setting);
        setting.transform.SetParent(this.gameObject.transform, false);
    }

    public void SettingBtn() {
        Destroy(GameObject.Find("UI_setting(Clone)"));
    }

    public void QuitGame() {
        //Application.Quit();
        Debug.Log("Quit Game");

        bool fail = false;
        string bundleId = "com.robot.asus.Sporden"; // your target bundle id
        AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");

        AndroidJavaObject launchIntent = null;
        
        launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", bundleId);

        ca.Call("startActivity", launchIntent);

        up.Dispose();
        ca.Dispose();
        packageManager.Dispose();
        launchIntent.Dispose();
    }





}
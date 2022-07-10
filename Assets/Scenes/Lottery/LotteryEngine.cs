using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LotteryEngine : MonoBehaviour {

    public Text drawResult;
    public GameObject[] EngineA_Pool = new GameObject[0];
    public GameObject[] EngineB_Pool = new GameObject[0];
    public GameObject[] EngineC_Pool = new GameObject[0];
    
    public GameObject resultUI;
    public GameObject resultFlower;
    public GameObject Seed;
    public GameObject DrawView;

    public DataHandler DataHandler;

    private void Awake()
    {
        DataHandler = GameObject.Find("DataHandler").GetComponent<DataHandler>();
    }



    public static bool ChancesCheck(int engineNum) {
        if (DataHandler.playerData.token[engineNum - 1] > 0)
        {
            return true;
        }
        else {
            
            return false;
        }
    }

    IEnumerator PlayAnimate(GameObject r) {
       //GameObject seed = Instantiate(Seed);
        


        GameObject drawView = Instantiate(DrawView);
        drawView.transform.SetParent(GameObject.Find("UI_Lottery").transform, false);
        drawView.transform.GetChild(4).gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(4/3);
        drawView.transform.GetChild(3).GetComponent<SpriteRenderer>().sprite = r.GetComponent<SpriteRenderer>().sprite;
        drawView.transform.GetChild(4).gameObject.SetActive(true);
    }
    





    // 抽獎機率與調整
    public void Lottery_A() {
        if (ChancesCheck(1) == true) {
            DataHandler.DoOnceLottery(1);
            System.Random random = new System.Random();
            int index = random.Next(0, EngineA_Pool.Length);
            GameObject result = EngineA_Pool[index];
            resultFlower.GetComponent<SpriteRenderer>().sprite = result.GetComponent<SpriteRenderer>().sprite;
            StartCoroutine(PlayAnimate(result));
            DataHandler.GainPlant(result.GetComponent<PlantManager>().id);

            drawResult.text = result.name;
        }
    }

    public void Lottery_B()
    {
        if (ChancesCheck(2) == true)
        {
            DataHandler.DoOnceLottery(2);
            System.Random random = new System.Random();
            int index = random.Next(0, EngineB_Pool.Length);
            GameObject result = EngineB_Pool[index];
            resultFlower.GetComponent<SpriteRenderer>().sprite = result.GetComponent<SpriteRenderer>().sprite;
            StartCoroutine(PlayAnimate(result));
            DataHandler.GainPlant(result.GetComponent<PlantManager>().id);

            drawResult.text = result.name;
        }
    }

    public void Lottery_C()
    {
        if (ChancesCheck(3) == true)
        {
            DataHandler.DoOnceLottery(3);
            System.Random random = new System.Random();
            int index = random.Next(0, EngineC_Pool.Length);
            GameObject result = EngineC_Pool[index];
            resultFlower.GetComponent<SpriteRenderer>().sprite = result.GetComponent<SpriteRenderer>().sprite;
            StartCoroutine(PlayAnimate(result));
            DataHandler.GainPlant(result.GetComponent<PlantManager>().id);

            drawResult.text = result.name;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LotteryUI : MonoBehaviour {

    public LotteryEngine lotteryEngine;

    public GameObject poolContentCanvas;

    public GameObject resultUI;
    public GameObject resultFlower;

    public GameObject engineZero;
    public GameObject engineOne;
    public GameObject engineTwo;

    public LayerMask mask;

    private GameObject clickBtn;

    public GameObject viewPort;
    public GameObject poolItem;
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButton(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, mousePos, 10 , mask);
            Debug.DrawLine(Camera.main.transform.position, mousePos, Color.red, 0.1f, true);
            if (hit.collider)
            {
                
                if(hit.collider.name == "PoolCanvas")
                {
                    hit.collider.gameObject.transform.position = new Vector2(100, 100);
                    foreach (Transform child in viewPort.transform)
                    {
                        GameObject.Destroy(child.gameObject);
                    }
                }
                
                
            }

        }

        // show remain token
        engineZero.transform.GetChild(1).GetComponent<Text>().text = "剩餘機會：" + DataHandler.playerData.token[0];
        engineOne.transform.GetChild(1).GetComponent<Text>().text = "剩餘機會：" + DataHandler.playerData.token[1];
        engineTwo.transform.GetChild(1).GetComponent<Text>().text = "剩餘機會：" + DataHandler.playerData.token[2];
    }

    public void DestroyDrawView()
    {
        Destroy(GameObject.Find("DrawView(Clone)"));
    }

    public void BackToMyGarden() {
        SceneManager.LoadScene("myGarden");
    }

    public void ShowContent_A()
    {
        poolContentCanvas.transform.position = new Vector2(0, 0);
        
        for (int i = 0; i < lotteryEngine.EngineA_Pool.Length; i++)
        {
            Instantiate(poolItem, viewPort.transform);
            viewPort.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text = lotteryEngine.EngineA_Pool[i].name;
        }
    }

    public void ShowContent_B()
    {
        poolContentCanvas.transform.position = new Vector2(0, 0);
        for (int i = 0; i < lotteryEngine.EngineB_Pool.Length; i++)
        {
            viewPort.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text = lotteryEngine.EngineB_Pool[i].name;
        }
    }

    public void ShowContent_C()
    {
        poolContentCanvas.transform.position = new Vector2(0, 0);
        for (int i = 0; i < lotteryEngine.EngineC_Pool.Length; i++)
        {
            viewPort.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text = lotteryEngine.EngineC_Pool[i].name;
        }
    }

}

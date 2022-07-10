using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlantBoxManager : MonoBehaviour {

    public int id;
    public int plant;

    

    public void Update()
    {
        plant = 0;
        GetComponent<SpriteRenderer>().color = Color.white;
        //先初始化，以後看有沒有更好寫法


        Vector3 direction = transform.TransformDirection(new Vector3(0,0,-1));
        RaycastHit hit;
        Debug.DrawLine(transform.position, new Vector3(transform.position.x,transform.position.y,10), Color.yellow, 0.1f, true);
        if (Physics.Raycast(transform.position, direction, out hit, 10))
        {
            /* 看起來不需要了
            if (hit.collider.tag == "Flower")
            {
                plant = hit.transform.GetComponent<PlantManager>().id;
                //Debug.Log(gameObject.name + " >>> " + hit.transform.GetComponent<FlowerManager>().id);
            }
            */
            
            if (hit.collider.tag == "SelectObj")
            {
                if (DataHandler.playerData.plant[id] != 0)
                {
                    GetComponent<SpriteRenderer>().color = new Color(255, 0, 0); // Red
                }
                else
                {
                    GetComponent<SpriteRenderer>().color = new Color(0, 255, 0); // Green
                    plant = hit.transform.GetComponent<PlantManager>().id;
                }
                
            }
        }
    }
}

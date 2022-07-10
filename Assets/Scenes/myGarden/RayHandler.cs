using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayHandler : MonoBehaviour {

    public Camera mainCamera;
    public LayerMask mask;

    public GameObject hit;
    public static GameObject hitPlantBox;
    public static GameObject hitPlant;
    public static GameObject SelectTempData;


    public GameObject PlantManagerUI;
    private GameObject obj;
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit3D;
        hit = null;

        if (Input.GetMouseButton(0))
        {
            RaycastHit2D hitCast = Physics2D.Raycast(ray.origin, Input.mousePosition, 1000, mask);
            if (hitCast.collider)
            {
                Debug.DrawLine(mainCamera.transform.position, hitCast.transform.position, Color.red, 0.1f, true);

                if (hitCast.collider.tag == "PlantBox")
                {
                    hitPlantBox = hitCast.transform.gameObject;
                    if (hitPlant != null)
                    {
                        hitPlant.transform.position = hitPlantBox.transform.position;
                    }
                }
            }




            if (Physics.Raycast(ray, out hit3D))
            {
                Debug.DrawLine(Camera.main.transform.position, hit3D.transform.position, Color.yellow, 0.1f, true);
                if (hit3D.collider.tag == "SelectObj")
                {
                    hitPlant = hit3D.collider.gameObject;
                }

                if (hit3D.collider.tag == "Flower")
                {
                    if (PlantManager.SelectPlant == null) {
                        PlantManager.SelectPlant = hit3D.collider.gameObject;
                        hit3D.collider.gameObject.tag = "SelectObj";
                        PlantManager.TempData = Instantiate(hit3D.collider.gameObject, hit3D.collider.transform.position, hit3D.collider.transform.rotation);
                        PlantManager.TempData.SetActive(false);
                        PlantManager.plantManagerUI = Instantiate(PlantManagerUI);
                        Debug.Log("ttttttttttttttttt");
                        PlantManager.plantManagerUI.transform.SetParent(GameObject.Find("UI_Garden").transform, false);
                        UIGarden.PlantManager_Onclick = false;
                        if (obj != null)
                        {
                            Destroy(PlantManager.plantManagerUI);
                        }
                        else
                        {
                            obj = PlantManager.plantManagerUI;
                        }
                    }
                }
            }
        }
    }
}

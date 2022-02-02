using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DlgMgr : MonoBehaviour
{
    public GameObject mushroomObj = null;
    public GameObject Reticle = null;
    static public bool changeColorReticle = false;
    static public int mushroomChangeColorCount = 0;
    private GameObject mushroomObject;
    private Transform grid;
    public void OnColorMushRoomObj()
    {
        if (mushroomObj)
        {
            mushroomObj.GetComponent<Renderer>().material.color = Color.blue;
            GameObject mushr = Instantiate(mushroomObject);
            mushr.SetActive(true);
            mushr.transform.SetParent(grid);
            
            if (!changeColorReticle)
            {
                if (mushroomChangeColorCount < 5)
                {
                    mushroomChangeColorCount++;
                }
                else
                {
                    Reticle.GetComponent<Image>().color = Color.red;
                    changeColorReticle = true;
                }
            }
        }
    }
    void Start()
    {
        mushroomObject = GameObject.Find("mushroomImage");
        grid = GameObject.Find("Grid").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRObjController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerEnter()
    {
        GetComponent<Renderer>().material.color = Color.green;
        Debug.Log("OnPointerEnter");
    }

    public void OnPointerExit()
    {
        GetComponent<Renderer>().material.color = Color.white;
        Debug.Log("OnPointerExit");
    }
}

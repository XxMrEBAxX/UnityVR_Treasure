using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teasure : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, Time.deltaTime * 12f, 0));
        if(Move)
        {
            Animation();
        }
    }
    Vector3 vel = Vector3.zero;
    Vector3 pos = new Vector3(-12.569f, -13.502f, 1.164f);
    bool Move = false;
    public void Animation()
    {
        if (!Move)
            Move = true;
        transform.position = Vector3.SmoothDamp(transform.position, pos, ref vel, 1f);
    }
}

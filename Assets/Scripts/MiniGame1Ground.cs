using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGame1Ground : MonoBehaviour
{
    [SerializeField]
    MiniGame1 m_MiniGame1;

    [SerializeField]
    Material m_material;

    public int num;
    void Start()
    {
        if(num < 4)
            m_material.color = Color.black;
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (m_material.color != Color.black && num != 4)
                return;
            switch (num)
            {
                case 1:
                    m_material.color = Color.red;
                    m_MiniGame1.StepGround(num);
                    break;
                case 2:
                    m_material.color = Color.blue;
                    m_MiniGame1.StepGround(num);
                    break;
                case 3:
                    m_material.color = Color.green;
                    m_MiniGame1.StepGround(num);
                    break;
                case 4:
                    m_MiniGame1.ResetGrounds();
                    break;
            }
        }
    }

    public void ResetGround()
    {
        m_material.color = Color.black;
    }
}

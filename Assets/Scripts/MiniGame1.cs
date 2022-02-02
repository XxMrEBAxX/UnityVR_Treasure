using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGame1 : MonoBehaviour
{
    [SerializeField]
    MiniGame1Ground[] m_MiniGame1Grounds;

    [SerializeField]
    GameObject m_gate;

    Queue<int> m_queue;
    void Start()
    {
        m_queue = new Queue<int>();
        m_gate.SetActive(false);
    }

    void Update()
    {
        
    }

    public void StepGround(int num)
    {
        bool testing = false;
        m_queue.Enqueue(num);

        if(m_queue.Count > 2)
        {
            if(2 == m_queue.Dequeue())
            {
                if (1 == m_queue.Dequeue())
                {
                    if (3 == m_queue.Dequeue())
                    {
                        testing = true;
                    }
                }
            }
            // 성공
            if (testing)
            {
                m_gate.SetActive(true);
            }
            // 실패
            else
            {
                Invoke("ResetGrounds", 2.5f);
            }
        }
        else
        {

        }
    }
    public void ResetGrounds()
    {
        for(int i = 0; i < 3; i++)
        {
            m_MiniGame1Grounds[i].ResetGround();
        }
        m_queue.Clear();
    }
}

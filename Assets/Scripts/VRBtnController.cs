using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VRBtnController : MonoBehaviour
{
    private bool bPressBtn = false;
    private bool bClickBtn = false;
    private float pressedTime = 0.0f; // �ش� ��ư�� �ü�ó�� ���� ���� ��� �ð�
    public float selectedBtnTime = 3.0f; // �ش� ��ư�� Ŭ�� ���� �ð�
    private Image m_guage;
    void Init()
    {
        pressedTime = 0.0f;
        bPressBtn = false;
        bClickBtn = false;
        m_guage = GameObject.Find("Guage").GetComponent<Image>();
    }
    void Start()
    {
        Init();
    }

    void Update()
    {
        if (bPressBtn && !bClickBtn)
        {
            pressedTime += Time.deltaTime;
            m_guage.fillAmount = pressedTime / selectedBtnTime;
            if (selectedBtnTime < pressedTime)
            {
                bClickBtn = true;
                OnUIClick();
            }
        }
    }
    public void OnUIClick()
    {
        Debug.Log("OnUIClickUI");
        PointerEventData data = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(gameObject, data, ExecuteEvents.pointerClickHandler);
        m_guage.fillAmount = 0;
    }
    public void OnUIPointerExit()
    {
        Debug.Log("OnUIPointerExit UI");
        Init();
        PointerEventData data = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(gameObject, data, ExecuteEvents.pointerExitHandler);
        m_guage.fillAmount = 0;
    }
    public void OnUIPointerEnter()
    {
        Debug.Log("OnUIPointerEnter UI");
        bPressBtn = true;
        PointerEventData data = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(gameObject, data, ExecuteEvents.pointerEnterHandler);
        m_guage.fillAmount = 0;
    }
}

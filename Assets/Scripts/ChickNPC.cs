using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChickNPC : MonoBehaviour
{
    Animator m_animator;
    LinkedList<string> dialog;
    LinkedListNode<string> curDialog;

    LinkedList<string> dialogButton;
    LinkedListNode<string> curDialogButton;

    [SerializeField]
    Text m_dialogText;
    [SerializeField]
    Text m_dialogButtonText;
    [SerializeField]
    GameObject m_dialog;

    private void Awake()
    {
        dialog = new LinkedList<string>();
        dialogButton = new LinkedList<string>();

        //1
        dialog.AddLast("이봐! 보물을 찾는다면서?\n주변은 조금 둘러 보았나?");
        dialogButton.AddLast("보았다");

        //2
        dialog.AddLast("뭐야? 보물이 어디있는지 모른다고?\n도대체 여긴 왜 온거야?");
        dialogButton.AddLast("이유");

        //3
        dialog.AddLast("일단 행동하고 보는거라고?\n이런,..막무가내를 보았나...");
        dialogButton.AddLast("사과");

        //4
        dialog.AddLast("...그래도 오늘은 맛있는 풀을 찾아서\n기분이 좋아서 특별히 얘기해주지.");
        dialogButton.AddLast("의문");

        //5
        dialog.AddLast("주변에 특이하게 생긴 <color=#FFE400>바위</color>가\n보이나?");
        dialogButton.AddLast("보았다");

        //6
        dialog.AddLast("그 주변에 신기한 장치를 풀면\n자네가 찾는 보물이 있을지도\n모르지.");
        dialogButton.AddLast("감사");

        //7
        dialog.AddLast("보물을 찾으면 꼭 나한테\n알려주라고!");
        dialogButton.AddLast("알겠다");
    }
    void Start()
    {
        m_animator = GetComponent<Animator>();
        StartCoroutine(EatAnimation());
        curDialog = dialog.First;
        curDialogButton = dialogButton.First;
        m_dialogText.text = curDialog.Value;
        m_dialogButtonText.text = curDialogButton.Value;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    IEnumerator EatAnimation()
    {
        while(true)
        {
            m_animator.SetBool("Eat", true);
            yield return new WaitForSeconds(1.5f);
            m_animator.SetBool("Eat", false);
            yield return new WaitForSeconds(7.0f);
        }
    }

    public void nextDialog()
    {
        if (curDialog.Next == null)
        {
            m_dialog.SetActive(false);
            return;
        }

        m_dialogText.text = curDialog.Next.Value;
        m_dialogButtonText.text = curDialogButton.Next.Value;

        curDialog = curDialog.Next;
        curDialogButton = curDialogButton.Next;
    }
}

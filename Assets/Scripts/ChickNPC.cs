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
        dialog.AddLast("�̺�! ������ ã�´ٸ鼭?\n�ֺ��� ���� �ѷ� ���ҳ�?");
        dialogButton.AddLast("���Ҵ�");

        //2
        dialog.AddLast("����? ������ ����ִ��� �𸥴ٰ�?\n����ü ���� �� �°ž�?");
        dialogButton.AddLast("����");

        //3
        dialog.AddLast("�ϴ� �ൿ�ϰ� ���°Ŷ��?\n�̷�,..���������� ���ҳ�...");
        dialogButton.AddLast("���");

        //4
        dialog.AddLast("...�׷��� ������ ���ִ� Ǯ�� ã�Ƽ�\n����� ���Ƽ� Ư���� ���������.");
        dialogButton.AddLast("�ǹ�");

        //5
        dialog.AddLast("�ֺ��� Ư���ϰ� ���� <color=#FFE400>����</color>��\n���̳�?");
        dialogButton.AddLast("���Ҵ�");

        //6
        dialog.AddLast("�� �ֺ��� �ű��� ��ġ�� Ǯ��\n�ڳװ� ã�� ������ ��������\n����.");
        dialogButton.AddLast("����");

        //7
        dialog.AddLast("������ ã���� �� ������\n�˷��ֶ��!");
        dialogButton.AddLast("�˰ڴ�");
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//������ ��� ����������� ������ ������ ��� ������ � ��� ��������� ����

public class PigHPtext : MonoBehaviour
{
    private Text pigText; // ������� ����� ��� ������
    private Rigidbody2D pigDmgPos; //��������� ��� ������ ����� ��������
    void Start()
    {
        //�������� ��� ���������� ��������� ������
        pigText = gameObject.GetComponent<Text>();
        pigDmgPos = gameObject.GetComponent<Rigidbody2D>();
        pigText.text = "";
    }

    // Update is called once per frame
    public void damageTextSet(string txt)
    {
        //����� ��� ������ �������� ������
        pigText.text = txt;
    }
    public void dmgFly()
    {
        //����� ��� ��������� ������, ���� ��������� � ������ � ����� 1 ��� �������
        pigDmgPos.gravityScale = 1f;
        pigDmgPos.AddForce(new Vector2(2f, 2f), ForceMode2D.Impulse);
     
        Invoke("killText", 0.5f);

    }
    void killText()
    {
        Destroy(gameObject);
    }
}

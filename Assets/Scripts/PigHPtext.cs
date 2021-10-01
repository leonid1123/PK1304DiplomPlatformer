using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//скрипт для отображения текста дамага для свинки и для отлетания цифр

public class PigHPtext : MonoBehaviour
{
    private Text pigText; // элемент текст для текста
    private Rigidbody2D pigDmgPos; //ригидбоди для текста чтобы отлетало
    void Start()
    {
        //получаем все компоненты поскольку префаб
        pigText = gameObject.GetComponent<Text>();
        pigDmgPos = gameObject.GetComponent<Rigidbody2D>();
        pigText.text = "";
    }

    // Update is called once per frame
    public void damageTextSet(string txt)
    {
        //метод для вывода значения дамага
        pigText.text = txt;
    }
    public void dmgFly()
    {
        //метод для отлетания дамага, даем ригидбоди и имульс и через 1 сек убиваем
        pigDmgPos.gravityScale = 1f;
        pigDmgPos.AddForce(new Vector2(2f, 2f), ForceMode2D.Impulse);
     
        Invoke("killText", 0.5f);

    }
    void killText()
    {
        Destroy(gameObject);
    }
}

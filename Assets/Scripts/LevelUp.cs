using UnityEngine;
using UnityEngine.UI;

public class LevelUp : MonoBehaviour
{
    //скрипт игрока
    //статы
    public int str = 1;
    public int hp = 10;
    public int def = 0;
    public float attSpeed = 1f;
    //мультипликатор дамага в зависимости от места удара 
    public int bodyPartMult = 1;
    //учет уровня и экспы
    public int xp = 0;
    public int lvl = 0;
    //собственные компоненты, переделать на автополучание в старте
    public SpriteRenderer playerSpriteRenderer;
    public Text topText;
    public Rigidbody2D rb2d;
    public Animator playerAnim;
    //куда смотрим, для хотьбы
    private bool toRight = true;
    //место в котором проходит проверка на попал/не попал
    public Transform attPoint;
    //задержка атаки
    bool attDelay = true;
    //захватил или нет цель
    bool gotEnemy = false;
    //подобрал или нет ключ, включить в геймплей!
    bool keyGet = false;
    //переменная для хрюшки на которой прицел
    Collider2D selectedEnemy;

    void Update()
    {
        var hMove = Input.GetAxis("Horizontal") * 3; //для движения по горизонтали, вертикального не будет

        if (attDelay && Mathf.Abs(hMove) <= 0.01 && Input.GetButtonDown("Fire1")) //обработка атаки
        {
            playerAnim.SetTrigger("isAttack");
            attDelay = false;
            makeAttack();
            Invoke("canAttack", attSpeed); //вызов метода лоя задержки атаки
        }
        playerAnim.SetFloat("moving", Mathf.Abs(hMove)); //анимация движения
        if (toRight && hMove < 0) //смотрим куда идем
        {
            toRight = false;
            transform.Rotate(0f, 180f, 0f);
        }
        if (!toRight && hMove > 0) //смотрим куда идем
        {
            toRight = true;
            transform.Rotate(0f, 180f, 0f);
        }

        rb2d.velocity = new Vector2(hMove, rb2d.velocity.y); //для движения по горизонтали
        // отображения статов
        topText.text = "XP:" + xp.ToString() + "\n" + "HP:" + hp.ToString() + "\n" + "DEF:" + def.ToString() + "\n" + "STR:" + str.ToString() + "\n" + "lvl" + lvl.ToString();
        if (Input.GetKeyDown(KeyCode.Tab)) //рост статов по кнопке для дебага
        {
            int newXP = Random.Range(1, 5);
            XPGain(newXP);
        }
        if(gotEnemy && Input.GetKeyDown(KeyCode.UpArrow)) //выбор места прицела по кнопкам вверх и вниз
        {
            selectedEnemy.GetComponent<PigControl>().Aimed(1);
            bodyPartMult = 2;
        }
        if(gotEnemy && Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedEnemy.GetComponent<PigControl>().Aimed(2);
            bodyPartMult = 1;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag=="key") //подбор ключа
        {
            keyGet = true;
            Debug.Log("keyGet");
        }
        if (collision.tag=="door" && keyGet) //открывание двери, 01.10.2021 сделано в виде рабивающегося кувшина, переделать на переход на след. уровень
        {
            collision.GetComponent<Animator>().SetTrigger("open");
        }


        if (collision.tag == "Pig") //захват свинки как цель через пересечение коллайдера
        {
            selectedEnemy = collision;
            collision.GetComponent<PigControl>().Aimed(2);
            bodyPartMult = 1;

            gotEnemy = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision) //потеря захвата свинки если вышла из коллайдера
    {
        if (collision.tag == "Pig")
        {
            selectedEnemy = null;
            collision.GetComponent<PigControl>().Aimed(0);
            gotEnemy = false;
        }
    }
    public void XPGain(int xpAdd) //метод для получения экспы ТОЛЬКО!!!
    {
        xp += xpAdd;

        if (xp - lvl * 10 > 0)
        {
            lvl += 1;
            RandomStatGrow(); //вызов метода увеличения рандомного стата при достижении уровня, ПЕРЕДЕЛАТЬ!!!!
        }
    }
    void RandomStatGrow() //метод для увеличения рандомного стата
    {
        var x = Random.Range(1, 4);
        var r = Random.Range(0f, 1f);
        var g = Random.Range(0f, 1f);
        var b = Random.Range(0f, 1f);
        playerSpriteRenderer.color = new Color(r, g, b);
        switch (x)
        {
            case 1:
                str += 1;
                break;
            case 2:
                hp += 10;
                break;
            case 3:
                def += 1;
                break;
        }
    }
    private void makeAttack() //метод для атаки через окружность, без коллайдера
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attPoint.position, 0.5f);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].tag == "Pig")
            {
                colliders[i].GetComponent<PigControl>().pigDMG(1 * str * bodyPartMult);
            }
        }
    }
    void OnDrawGizmosSelected() //отображение окружности дамага для дебага
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attPoint.position, 0.5f);
    }
    void canAttack() //метод задержки для атаки
    {
        attDelay = true;
    }


}

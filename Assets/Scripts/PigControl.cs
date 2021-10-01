using UnityEngine;
using UnityEngine.UI;

public class PigControl : MonoBehaviour
{

    //скрипт для описания поведения свинки
    public GameObject newPig; //для респавна свинки
    private GameObject objToSpawn; //для респавна отлетающего текста. НЕ ИСПОЛЬЗУЕТСЯ
    public int pigHP = 5; //ХП свинки
    public Animator pigAnim; //для анимации
    public LevelUp Player;//autoget
    public SpriteRenderer aimSpriteHead; //для объектов на которых отрисовывается прицел
    public SpriteRenderer aimSpriteBody; //для объектов на которых отрисовывается прицел
    public Text pigHPtext; //для отображения текста ХП свинки
    public bool aimedAt = false; //захвачена или нет свинка мишенью

    void Start()
    {
        aimSpriteHead.color = new Color(1f, 1f, 1f, 0f); //делаем спрайт прицела полностью прозрачным
        aimSpriteBody.color = new Color(1f, 1f, 1f, 0f);

        Player = GameObject.Find("Player").GetComponent<LevelUp>(); //получить скрипт игрока
        gameObject.GetComponent<Collider2D>().enabled = true; //включить коллайдер, поскольку был отключен при смерти
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 10; //включить гравитацию, чтобы не сдвигался игроком
        pigHP = 5; //стартовый ХП
        Aimed(0); // снять захват целью, поскольку был захвачен до того как умер
    }
    void Update()
    {
        pigHPtext.text = "Pig HP:"+pigHP.ToString(); //всегда печатаем текущее ХП

    }
    public void pigDMG(int damage) //метод для получения дамага, вызывается игроком
    {
        pigHP -= damage; //получение дамага из параметра метода
        GameObject.Find("Text").GetComponent<PigHPtext>().damageTextSet(damage.ToString()); //передаем дамаг в метод свинки, для отображения
        GameObject.Find("Text").GetComponent<PigHPtext>().dmgFly(); //вызов метода отлетания дамага
        /* //часть отвечающая за создание нового объекта отлетающего дамага ....переделать!!!
        // spawns object
        objToSpawn = new GameObject("PigDamageText");
        // add Components
        objToSpawn.AddComponent<MeshRenderer>();
        objToSpawn.AddComponent<TextMesh>();
        objToSpawn.AddComponent<Rigidbody2D>();
        objToSpawn.GetComponent<Rigidbody2D>().gravityScale = 0;
        objToSpawn.AddComponent<PigHPtext>();
        // sets the obj's parent to the obj that the script is applied to
        objToSpawn.transform.SetParent(this.transform);*/

        pigAnim.SetTrigger("isHit"); //запускаем анимацию получения урона
        if (pigHP <= 0) //если свинка умерла, отключаем коллайдер и гравитацию, вкл.анимацию смерти, запускаем таймер на исчезновение
        {
            gameObject.GetComponent<Collider2D>().enabled = false;
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            pigAnim.SetBool("Dead", true);
            Invoke("goAway", 2);
        }
        giveXP(1); //вызываем метод получения опыта игроком, кол-во опыта зависит от моба, на будущее
    }
    public void giveXP(int xp) //метод выдавания дамага игроку
    {
        Player.XPGain(xp);
    }
    void goAway() //метод который убирает мертвую свинку и создает новую на 3 выше чем старая
    {
        Instantiate(newPig, new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), transform.rotation);
        Destroy(gameObject);
    }
    public void Aimed(int aim) //метод отображения и учета места в которое бъет игрок aim 1 - Head, aim 2 - Body, 0 - no Aim
    {
        switch (aim)
        {
            case 0:
                aimSpriteHead.color = new Color(1f, 1f, 1f, 0f);
                aimSpriteBody.color = new Color(1f, 1f, 1f, 0f);
                break;
            case 1:
                aimSpriteHead.color = new Color(1f, 1f, 1f, 1f);
                aimSpriteBody.color = new Color(1f, 1f, 1f, 0f);
                break;
            case 2:
                aimSpriteHead.color = new Color(1f, 1f, 1f, 0f);
                aimSpriteBody.color = new Color(1f, 1f, 1f, 1f);
                break;
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class LevelUp : MonoBehaviour
{
    //������ ������
    //�����
    public int str = 1;
    public int hp = 10;
    public int def = 0;
    public float attSpeed = 1f;
    //�������������� ������ � ����������� �� ����� ����� 
    public int bodyPartMult = 1;
    //���� ������ � �����
    public int xp = 0;
    public int lvl = 0;
    //����������� ����������, ���������� �� ������������� � ������
    public SpriteRenderer playerSpriteRenderer;
    public Text topText;
    public Rigidbody2D rb2d;
    public Animator playerAnim;
    //���� �������, ��� ������
    private bool toRight = true;
    //����� � ������� �������� �������� �� �����/�� �����
    public Transform attPoint;
    //�������� �����
    bool attDelay = true;
    //�������� ��� ��� ����
    bool gotEnemy = false;
    //�������� ��� ��� ����, �������� � ��������!
    bool keyGet = false;
    //���������� ��� ������ �� ������� ������
    Collider2D selectedEnemy;

    void Update()
    {
        var hMove = Input.GetAxis("Horizontal") * 3; //��� �������� �� �����������, ������������� �� �����

        if (attDelay && Mathf.Abs(hMove) <= 0.01 && Input.GetButtonDown("Fire1")) //��������� �����
        {
            playerAnim.SetTrigger("isAttack");
            attDelay = false;
            makeAttack();
            Invoke("canAttack", attSpeed); //����� ������ ��� �������� �����
        }
        playerAnim.SetFloat("moving", Mathf.Abs(hMove)); //�������� ��������
        if (toRight && hMove < 0) //������� ���� ����
        {
            toRight = false;
            transform.Rotate(0f, 180f, 0f);
        }
        if (!toRight && hMove > 0) //������� ���� ����
        {
            toRight = true;
            transform.Rotate(0f, 180f, 0f);
        }

        rb2d.velocity = new Vector2(hMove, rb2d.velocity.y); //��� �������� �� �����������
        // ����������� ������
        topText.text = "XP:" + xp.ToString() + "\n" + "HP:" + hp.ToString() + "\n" + "DEF:" + def.ToString() + "\n" + "STR:" + str.ToString() + "\n" + "lvl" + lvl.ToString();
        if (Input.GetKeyDown(KeyCode.Tab)) //���� ������ �� ������ ��� ������
        {
            int newXP = Random.Range(1, 5);
            XPGain(newXP);
        }
        if(gotEnemy && Input.GetKeyDown(KeyCode.UpArrow)) //����� ����� ������� �� ������� ����� � ����
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

        if (collision.tag=="key") //������ �����
        {
            keyGet = true;
            Debug.Log("keyGet");
        }
        if (collision.tag=="door" && keyGet) //���������� �����, 01.10.2021 ������� � ���� ������������� �������, ���������� �� ������� �� ����. �������
        {
            collision.GetComponent<Animator>().SetTrigger("open");
        }


        if (collision.tag == "Pig") //������ ������ ��� ���� ����� ����������� ����������
        {
            selectedEnemy = collision;
            collision.GetComponent<PigControl>().Aimed(2);
            bodyPartMult = 1;

            gotEnemy = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision) //������ ������� ������ ���� ����� �� ����������
    {
        if (collision.tag == "Pig")
        {
            selectedEnemy = null;
            collision.GetComponent<PigControl>().Aimed(0);
            gotEnemy = false;
        }
    }
    public void XPGain(int xpAdd) //����� ��� ��������� ����� ������!!!
    {
        xp += xpAdd;

        if (xp - lvl * 10 > 0)
        {
            lvl += 1;
            RandomStatGrow(); //����� ������ ���������� ���������� ����� ��� ���������� ������, ����������!!!!
        }
    }
    void RandomStatGrow() //����� ��� ���������� ���������� �����
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
    private void makeAttack() //����� ��� ����� ����� ����������, ��� ����������
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
    void OnDrawGizmosSelected() //����������� ���������� ������ ��� ������
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attPoint.position, 0.5f);
    }
    void canAttack() //����� �������� ��� �����
    {
        attDelay = true;
    }


}

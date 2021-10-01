using UnityEngine;
using UnityEngine.UI;

public class PigControl : MonoBehaviour
{

    //������ ��� �������� ��������� ������
    public GameObject newPig; //��� �������� ������
    private GameObject objToSpawn; //��� �������� ����������� ������. �� ������������
    public int pigHP = 5; //�� ������
    public Animator pigAnim; //��� ��������
    public LevelUp Player;//autoget
    public SpriteRenderer aimSpriteHead; //��� �������� �� ������� �������������� ������
    public SpriteRenderer aimSpriteBody; //��� �������� �� ������� �������������� ������
    public Text pigHPtext; //��� ����������� ������ �� ������
    public bool aimedAt = false; //��������� ��� ��� ������ �������

    void Start()
    {
        aimSpriteHead.color = new Color(1f, 1f, 1f, 0f); //������ ������ ������� ��������� ����������
        aimSpriteBody.color = new Color(1f, 1f, 1f, 0f);

        Player = GameObject.Find("Player").GetComponent<LevelUp>(); //�������� ������ ������
        gameObject.GetComponent<Collider2D>().enabled = true; //�������� ���������, ��������� ��� �������� ��� ������
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 10; //�������� ����������, ����� �� ��������� �������
        pigHP = 5; //��������� ��
        Aimed(0); // ����� ������ �����, ��������� ��� �������� �� ���� ��� ����
    }
    void Update()
    {
        pigHPtext.text = "Pig HP:"+pigHP.ToString(); //������ �������� ������� ��

    }
    public void pigDMG(int damage) //����� ��� ��������� ������, ���������� �������
    {
        pigHP -= damage; //��������� ������ �� ��������� ������
        GameObject.Find("Text").GetComponent<PigHPtext>().damageTextSet(damage.ToString()); //�������� ����� � ����� ������, ��� �����������
        GameObject.Find("Text").GetComponent<PigHPtext>().dmgFly(); //����� ������ ��������� ������
        /* //����� ���������� �� �������� ������ ������� ����������� ������ ....����������!!!
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

        pigAnim.SetTrigger("isHit"); //��������� �������� ��������� �����
        if (pigHP <= 0) //���� ������ ������, ��������� ��������� � ����������, ���.�������� ������, ��������� ������ �� ������������
        {
            gameObject.GetComponent<Collider2D>().enabled = false;
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            pigAnim.SetBool("Dead", true);
            Invoke("goAway", 2);
        }
        giveXP(1); //�������� ����� ��������� ����� �������, ���-�� ����� ������� �� ����, �� �������
    }
    public void giveXP(int xp) //����� ��������� ������ ������
    {
        Player.XPGain(xp);
    }
    void goAway() //����� ������� ������� ������� ������ � ������� ����� �� 3 ���� ��� ������
    {
        Instantiate(newPig, new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), transform.rotation);
        Destroy(gameObject);
    }
    public void Aimed(int aim) //����� ����������� � ����� ����� � ������� ���� ����� aim 1 - Head, aim 2 - Body, 0 - no Aim
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

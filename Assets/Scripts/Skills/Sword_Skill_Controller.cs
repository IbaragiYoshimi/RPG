using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    [SerializeField] private float returnSpeed = 12;
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private bool canRotate = true;
    private bool isReturning;

    [Header("Bounce info")]
    [SerializeField] private float bounceSpeed;
    private bool isBouncing;
    private int bounceAmount;
    private List<Transform> enemyTarget;
    private int targetIndex = 0;

    [Header("Pierce info")]
    [SerializeField] private float pierceAmount;

    /* ����δ֪ԭ�򣬶���ʱ��SetupSword �� Start ����ִ�У����� rb �����á�
     * ��Ҫ�� rb �Ȼ�ȡ����������ǰ�� Awake �С�*/
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>(); 
    }

    private void Start()
    {
        
    }

    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player)
    {   
        player = _player;

        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;

        if(pierceAmount <= 0)
            anim.SetBool("Rotation", true);
    }

    public void SetupBounce(bool _isBouncing, int _amountOfBounces)
    {
        isBouncing = _isBouncing;
        bounceAmount = _amountOfBounces;

        // ��enemyTarget ����Ϊ private �Ļ���Unity �����Զ����������ֶ�����֮��public ����Զ�������
        enemyTarget = new List<Transform>();
    }

    public void SetupPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }

    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        // rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;
    }

    private void Update()
    {
        // ��Ȼ������ȥ��ʱ���õ�����ת�Ķ�������ʵ����Ҳ�ø�����ٶ���������ֵ���������������еĺ��ͷ��������������ܸ�����ת�ˣ�ǰ�������ȵ��� prefab���ý��ķ���Ҫ����ͷ����һ�£���
        if (canRotate)
            transform.right = rb.velocity;

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < 1)
                player.CatchTheSword();
        }

        BounceLogic();
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);

            // Update ��������Ҳ��ѭ�����õģ�ÿһ֡����һ�Σ�����ֻ��Ҫ����ÿһ֡̽��һ�ν���Χ��Ŀ�꣬�����Ƿ�С���ض��ľ��룬��һ֡�ý��ͻᵯ�䵽��Ŀ���ϡ�
            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < 0.1f)
            {
                targetIndex++;
                bounceAmount--;

                if (bounceAmount <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }

                if (targetIndex >= enemyTarget.Count)
                    targetIndex = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���ڷ���;����ʱ����Ե��˲����κη�Ӧ��
        if (isReturning)
            return;

        collision.GetComponent<Enemy>()?.DamageEffect();

        if (collision.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                        enemyTarget.Add(hit.transform);
                }
            }
        }

        StuckInto(collision);
    }

    private void StuckInto(Collider2D collision)
    {
        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }

        canRotate = false;
        cd.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTarget.Count > 0)
            return;

        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }
}

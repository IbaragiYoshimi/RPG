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

    /* 由于未知原因，丢剑时，SetupSword 比 Start 更早执行，导致 rb 无引用。
     * 需要将 rb 等获取组件的语句提前到 Awake 中。*/
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

        // 将enemyTarget 设置为 private 的话，Unity 不会自动创建，需手动；反之，public 则会自动创建。
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
        // 虽然丢剑出去的时候用的是旋转的动画，但实际上也用刚体的速度向量，赋值给剑在世界坐标中的红箭头，这样剑本体就能跟着旋转了（前提是事先调整 prefab，让剑的方向要与红箭头方向一致）。
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

            // Update 函数本身也是循环调用的，每一帧调用一次，所以只需要考虑每一帧探测一次剑周围的目标，看看是否小于特定的距离，下一帧该剑就会弹射到该目标上。
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
        // 剑在返回途中暂时不会对敌人产生任何反应。
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

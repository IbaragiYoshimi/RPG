using System.Collections;
using System.Collections.Generic;
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

        anim.SetBool("Rotation", true);
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

            if(Vector2.Distance(transform.position, player.transform.position) < 1)
                player.CatchTheSword();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 剑在返回途中暂时不会对敌人产生任何反应。
        if (isReturning)
            return;

        anim.SetBool("Rotation", false);

        canRotate = false;
        cd.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        transform.parent = collision.transform;
    }
}

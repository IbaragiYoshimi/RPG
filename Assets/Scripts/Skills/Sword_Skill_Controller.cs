using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

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

    public void SetupSword(Vector2 _dir, float _gravityScale)
    {
        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;
    }
}

using System;
using UnityEngine;

public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}
public class Sword_Skill : Skill
{
    public SwordType swordType = SwordType.Regular;

    [Header("Bounce info")]
    [SerializeField] private int bounceAmount;
    [SerializeField] private float bounceGravity;

    [Header("Pierce info")]
    [SerializeField] private int pierceAmount;
    [SerializeField] private float pierceGravity;

    [Header("Skill info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce;       // �����ĳ�ʼ x��y ���ٶ�
    [SerializeField] private float swordGravity;


    private Vector2 finalDir;

    [Header("Aim dots")]
    [SerializeField] private int numberOfDots;          // ��׼�������
    [SerializeField] private float spaceBetweenDots;    // ��ļ��
    [SerializeField] private GameObject dotPrefab;      // ���Ԥ�Ƽ�
    [SerializeField] private Transform dotsParent;      // ���λ��

    private GameObject[] dots;                          // �������

    protected override void Start()
    {
        base.Start();

        // һ������Ϸ�������� 20 �������ߵ㣬������ player �� dotParent �¡�
        GenerateDots();

        SetupGravity();
    }

    private void SetupGravity()
    {
        if (swordType == SwordType.Bounce)
            swordGravity = bounceGravity;
        else if(swordType == SwordType.Pierce)
            swordGravity = pierceGravity;
    }

    protected override void Update()
    {
        // ���ɿ�����Ҽ�ʱ���õ����յķ����������ɽ���
        if (Input.GetKeyUp(KeyCode.Mouse1))
            /* normalized ��һ�����������Ϊȡ������ģ��ȡ��λ������ */
            finalDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);


        // ����������Ҽ�ʱ���������ߵ������ֵ����Ϊʱ�� t��Ȼ���ٳ˵��� spaceBetweenDots����Ϊ������ʱ���� t������ DotsPosition()��
        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        }
    }

    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        Sword_Skill_Controller newSwordScript = newSword.GetComponent<Sword_Skill_Controller>();

        if(swordType == SwordType.Bounce)
            newSwordScript.SetupBounce(true, bounceAmount);
        else if(swordType == SwordType.Pierce)
            newSwordScript.SetupPierce(pierceAmount);

        // ������ʩ�Ӹ�������������������������ֵ�� ���� rb������Զ�����ÿһ֡��Ӧ�÷ɵ�ʲôλ�á��������ߵ㲻ͬ����Ҫ�����ֶ����㡣
        newSwordScript.SetupSword(finalDir, swordGravity, player);

        player.AssignNewSword(newSword);

        // �����ɺ󣬹ر����������ߵ㡣
        DotsActive(false);
    }

    #region Aim region
    public Vector2 AimDirection()
    {
        /* player �������ǻ����������ꡣ */
        Vector2 playerPosition = player.transform.position;
        /* ���������ǻ�����Ļ�ģ���Ļ���ǻ���������ģ�
         * Ҳ����˵����Ҫ���������꣬ͨ��������� ScreenToWorldPoint ת��Ϊ�������ꡣ */
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;

        return direction;
    }


    public void DotsActive(bool _isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }
    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);       // ������Ϸ�����������ߵ㣬�����������������أ�
            dots[i].SetActive(false);                                                                           // �ر����������ߵ����ʾ��
        }
    }

    // ���ݴ����ʱ�䣬����õ�Ӧ����ʲôλ�á�new Vector2 �� normalized ����λ���� v������λ�� = vt * 1/2 * g * t^2
    private Vector2 DotsPosition(float t)
    {
        // �����߹�ʽ����������y�������ʱ���λ�ù�ʽ�� vt + 1/2gt^2
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * launchForce.x,
            AimDirection().normalized.y * launchForce.y) * t + 0.5f * (Physics2D.gravity * swordGravity) * (t * t);

        return position;
    }
    #endregion
}

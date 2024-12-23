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
    [SerializeField] private Vector2 launchForce;       // 丢剑的初始 x、y 轴速度
    [SerializeField] private float swordGravity;


    private Vector2 finalDir;

    [Header("Aim dots")]
    [SerializeField] private int numberOfDots;          // 瞄准点的数量
    [SerializeField] private float spaceBetweenDots;    // 点的间距
    [SerializeField] private GameObject dotPrefab;      // 点的预制件
    [SerializeField] private Transform dotsParent;      // 点的位置

    private GameObject[] dots;                          // 点的数组

    protected override void Start()
    {
        base.Start();

        // 一进入游戏，就生成 20 个抛物线点，挂载在 player 的 dotParent 下。
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
        // 当松开鼠标右键时，得到最终的方向，用于生成剑。
        if (Input.GetKeyUp(KeyCode.Mouse1))
            /* normalized 归一化，可以理解为取向量的模，取单位向量。 */
            finalDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);


        // 当按下鼠标右键时，将抛物线点的索引值，作为时间 t，然后再乘点间距 spaceBetweenDots，作为真正的时间间隔 t，代入 DotsPosition()。
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

        // 将最终施加给剑的力（向量）、重力，赋值给 剑的 rb，其会自动计算每一帧剑应该飞到什么位置。但抛物线点不同，需要我们手动计算。
        newSwordScript.SetupSword(finalDir, swordGravity, player);

        player.AssignNewSword(newSword);

        // 剑生成后，关闭所有抛物线点。
        DotsActive(false);
    }

    #region Aim region
    public Vector2 AimDirection()
    {
        /* player 的坐标是基于世界坐标。 */
        Vector2 playerPosition = player.transform.position;
        /* 鼠标的坐标是基于屏幕的，屏幕又是基于摄像机的，
         * 也就是说，需要将鼠标的坐标，通过摄像机的 ScreenToWorldPoint 转换为世界坐标。 */
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
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);       // 进入游戏即生成抛物线点，并存放起来。（对象池）
            dots[i].SetActive(false);                                                                           // 关闭所有抛物线点的显示。
        }
    }

    // 根据代入的时间，计算该点应该在什么位置。new Vector2 用 normalized 作单位向量 v，最终位置 = vt * 1/2 * g * t^2
    private Vector2 DotsPosition(float t)
    {
        // 抛物线公式，高中物理，y方向根据时间的位置公式是 vt + 1/2gt^2
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * launchForce.x,
            AimDirection().normalized.y * launchForce.y) * t + 0.5f * (Physics2D.gravity * swordGravity) * (t * t);

        return position;
    }
    #endregion
}

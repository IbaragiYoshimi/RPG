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
    [SerializeField] private float bounceSpeed;

    [Header("Pierce info")]
    [SerializeField] private int pierceAmount;
    [SerializeField] private float pierceGravity;

    [Header("Spin info")]
    [SerializeField] private float hitCooldown = 0.35f;
    [SerializeField] private float maxTravelDistance = 7;
    [SerializeField] private float spinDuration = 2;
    [SerializeField] private float spinGravity = 1;

    [Header("Skill info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce;       // Throw sword's default speed.
    [SerializeField] private float swordGravity;
    [SerializeField] private float freezeTimeDuration;
    [SerializeField] private float returnSpeed;


    private Vector2 finalDir;

    [Header("Aim dots")]
    [SerializeField] private int numberOfDots;          // Number of dots.
    [SerializeField] private float spaceBetweenDots;    // Distance of dots.
    [SerializeField] private GameObject dotPrefab;      // Prefab of dots.
    [SerializeField] private Transform dotsParent;      // Position of dots when not in aiming state.

    private GameObject[] dots;                          // Array of dots.

    protected override void Start()
    {
        base.Start();

        // Whenever player the game, generate 20 dots. And storage them in the dotsParent.
        GenerateDots();

        SetupGravity();
    }

    private void SetupGravity()
    {
        if (swordType == SwordType.Bounce)
            swordGravity = bounceGravity;
        else if(swordType == SwordType.Pierce)
            swordGravity = pierceGravity;
        else if(swordType == SwordType.Spin)
            swordGravity = spinGravity;
    }

    protected override void Update()
    {
        // Get the direction when the moment of you release the right mouse button.
        if (Input.GetKeyUp(KeyCode.Mouse1))
            // Take the unit vector.
            finalDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);

        // When you press right mouse button, the index of dots as time value t, multiply dots' distance. Then as the real time value passing parameters to DotsPosition().
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
            newSwordScript.SetupBounce(true, bounceAmount, bounceSpeed);
        else if(swordType == SwordType.Pierce)
            newSwordScript.SetupPierce(pierceAmount);
        else if (swordType == SwordType.Spin)
            newSwordScript.SetupSpin(true, maxTravelDistance, spinDuration, hitCooldown);

        // Differ from rigidbody can calculate sword's position each frame automatically, we should calculate dots' position using Newton's Law manually.
        newSwordScript.SetupSword(finalDir, swordGravity, player, freezeTimeDuration, returnSpeed);

        player.AssignNewSword(newSword);

        // Unactive all the dots when sword generates.
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * 感觉这里的无限背景实现并不好，但暂时先这样。以后再改。
 * 
 * 
 */

public class ParallaxBackground : MonoBehaviour
{
    private GameObject cam;

    [SerializeField] private float parallaxEffect;

    private float xPosition;
    private float length;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera");

        // 获取背景图片的 x 轴长度。
        length = GetComponent<SpriteRenderer>().bounds.size.x;

        // 因为脚本挂载在 BG_Sky_Layer 和 BG_City_Layer 上，获取的是它们的位置的 x 轴。
        xPosition = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        #region Parallax background
        // 根据主摄像机 x 轴的移动距离，乘以视差效果距离，得到背景图应该移动的距离。
        float distanceToMove = cam.transform.position.x * parallaxEffect;//

        // 相当于让背景图跟随主摄像机移动，但不同背景层的位移距离不同。越远的位移距离越小。
        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);
        #endregion

        #region Endless background
        // 计算背景图将被移动的距离。
        float distanceMoved = cam.transform.position.x * (1 - parallaxEffect);

        if (distanceMoved > xPosition + length)
            xPosition = xPosition + length;
        else if (distanceMoved < xPosition - length)
            xPosition = xPosition - length;
        #endregion
    }
}

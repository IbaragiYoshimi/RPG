using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * �о���������ޱ���ʵ�ֲ����ã�����ʱ���������Ժ��ٸġ�
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

        // ��ȡ����ͼƬ�� x �᳤�ȡ�
        length = GetComponent<SpriteRenderer>().bounds.size.x;

        // ��Ϊ�ű������� BG_Sky_Layer �� BG_City_Layer �ϣ���ȡ�������ǵ�λ�õ� x �ᡣ
        xPosition = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        #region Parallax background
        // ����������� x ����ƶ����룬�����Ӳ�Ч�����룬�õ�����ͼӦ���ƶ��ľ��롣
        float distanceToMove = cam.transform.position.x * parallaxEffect;//

        // �൱���ñ���ͼ������������ƶ�������ͬ�������λ�ƾ��벻ͬ��ԽԶ��λ�ƾ���ԽС��
        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);
        #endregion

        #region Endless background
        // ���㱳��ͼ�����ƶ��ľ��롣
        float distanceMoved = cam.transform.position.x * (1 - parallaxEffect);

        if (distanceMoved > xPosition + length)
            xPosition = xPosition + length;
        else if (distanceMoved < xPosition - length)
            xPosition = xPosition - length;
        #endregion
    }
}

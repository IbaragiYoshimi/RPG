using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Endless background
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

        // Get background picture's X axis's length.
        length = GetComponent<SpriteRenderer>().bounds.size.x;

        // Because the script attempts on BG_Sky_Layer and BG_City_Layer, get their position's X axis.
        xPosition = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        #region Parallax background
        
        float distanceToMove = cam.transform.position.x * parallaxEffect;

        
        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);
        #endregion

        #region Endless background
        // Calculate the distance for moving of background picture.
        float distanceMoved = cam.transform.position.x * (1 - parallaxEffect);

        if (distanceMoved > xPosition + length)
            xPosition = xPosition + length;
        else if (distanceMoved < xPosition - length)
            xPosition = xPosition - length;
        #endregion
    }
}

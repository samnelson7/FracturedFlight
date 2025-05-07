using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BackgroundController : MonoBehaviour
{ // freeze vertical movement
    private float imageStartingPosition, imageLength;
    public GameObject cam;
    public float parallaxEffect;
    void Start()
    {
        imageStartingPosition = transform.position.x;
        imageLength = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float distancex = cam.transform.position.x * parallaxEffect; // 0 = move with camera, 1 = won't move
        //float distancey = cam.transform.position.y * 1; // 0 = move with camera, 1 = won't move
        float movement = cam.transform.position.x * (1-parallaxEffect);
                                                                    
        transform.position = new Vector3(imageStartingPosition +  distancex, cam.transform.position.y, transform.position.z);

        if (movement > imageStartingPosition + imageLength - 20) // Extra padding so player can't see render of new image
        {
            imageStartingPosition += imageLength;
        }
        else if (movement < imageStartingPosition - imageLength + 20) // Extra padding so player can't see render of new image
        {
            imageStartingPosition -= imageLength;
        }
    }
}

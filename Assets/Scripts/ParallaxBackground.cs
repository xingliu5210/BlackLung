using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] Transform cam;
    [SerializeField] int imageOffset;
    [SerializeField] private float xParallaxEffect;
    [SerializeField] private float yParallaxEffect;

    [Header("Testing")]
    [SerializeField] Transform player;
    [SerializeField] Vector3 playerPosition;
    [SerializeField] Vector3 cameraPosition;
    [SerializeField] Vector3 imagePosition;
    [SerializeField] float distance;

    private float width;
    private float height;

    private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()  
    {
        //Find size of image.
        width = GetComponent<SpriteRenderer>().bounds.size.x;
        height = GetComponent<SpriteRenderer>().bounds.size.y;
        //Set start position to the x position of camera and calculate offset if it is left and right image.
        float xStartPosition = cam.position.x + imageOffset * width;
        //float yStartPosition = transform.position.y + imageOffset * height;
        startPosition = new Vector3(xStartPosition, transform.position.y, transform.position.z);

        transform.position = new Vector3(xStartPosition, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void LateUpdate()
    {

        float xTmp = (cam.position.x * (1 - xParallaxEffect));
        float xDistance = (cam.position.x * xParallaxEffect);
        float yTmp = (cam.position.y * (1 - yParallaxEffect));
        float yDistance = (cam.position.y * yParallaxEffect);

        transform.position = new Vector3(startPosition.x + xDistance, startPosition.y - yDistance, transform.position.z);

        if (xTmp > startPosition.x + width)
        {
            startPosition.x += 2* width;
        }
        else if (xTmp < startPosition.x - width)
        {
            startPosition.x -= 2* width;
        }

        cameraPosition = cam.position;
        imagePosition = transform.position;
        distance = xDistance;
    }
}

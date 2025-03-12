using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [SerializeField] Transform cam;
    [SerializeField] private float xParallaxEffect;
    [SerializeField] private float yParallaxEffect;


    [SerializeField] private float width;
    [SerializeField] private float height;
    [SerializeField] Vector3 startPosition;

    private float xTmp;
    private float yTmp;
    private float xDistance;
    private float yDistance;

    [Header("Testing")]
    [SerializeField] Vector3 cameraPosition;
    [SerializeField] Vector3 imagePosition;
    [SerializeField] float distance;

    // Start is called before the first frame update
    void Start()
    {
        width = GetComponent<BoxCollider>().size.x;
        height = GetComponent<BoxCollider>().size.y;
        startPosition = new Vector3(cam.position.x - width/2, cam.position.y - height/4, transform.position.z);
        transform.position = startPosition;
    }

    // Update is called once per frame
    void Update()
    {
        //Calculating parallax effect
        xTmp = (cam.position.x * (1 - xParallaxEffect));
        yTmp = (cam.position.y * yParallaxEffect);
        xDistance = cam.position.x * xParallaxEffect;
        yDistance = cam.position.y * yParallaxEffect;

        transform.position = new Vector3(startPosition.x + xDistance, startPosition.y + yDistance, transform.position.z);

        //TESTING
        cameraPosition = cam.position;
        imagePosition = transform.position;
    }
}

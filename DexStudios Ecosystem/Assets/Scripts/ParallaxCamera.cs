using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxCamera : MonoBehaviour
{
    [Header("Parallax Settings")]
    public float maxHorizontalOffset = 5f;  
    public float maxVerticalOffset = 3f;  
    public float smoothSpeed = 0.1f;     

    private Vector3 initialPosition;  

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;

        float normalizedX = (mousePosition.x / Screen.width) * 2 - 1;
        float normalizedY = (mousePosition.y / Screen.height) * 2 - 1;

        float targetX = initialPosition.x + normalizedX * maxHorizontalOffset;
        float targetY = initialPosition.y + normalizedY * maxVerticalOffset;

        Vector3 targetPosition = new Vector3(targetX, targetY, initialPosition.z);

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
    }
}


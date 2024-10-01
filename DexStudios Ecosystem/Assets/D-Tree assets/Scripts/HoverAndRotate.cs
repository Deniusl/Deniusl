using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverAndRotate : MonoBehaviour
{
    [Header("Hover Settings")]
    public float hoverHeight = 2.0f;  
    public float hoverSpeed = 2.0f;     

    [Header("Rotation Settings")]
    public bool continuousRotation = true;  
    public bool limitRotation = false;     
    public Vector3 rotationAxis = Vector3.up;
    public float rotationSpeed = 100.0f;   

    [Header("Limited Rotation Settings")]
    public float minAngle = -70.0f;     
    public float maxAngle = 70.0f;     

    [Header("Original Settings")]
    private Vector3 originalPosition;     
    private Quaternion originalRotation;   

    private bool isHovered = false;        
    private bool returningToPosition = false;

    private float currentAngle = 0.0f;      

    void Start()
    {      
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    void Update()
    {
        if (isHovered)
        {          
            Vector3 targetPosition = originalPosition + new Vector3(0, hoverHeight, 0);
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * hoverSpeed);
           
            if (continuousRotation)
            {
                if (limitRotation)
                {
                    currentAngle = Mathf.PingPong(Time.time * rotationSpeed, maxAngle - minAngle) + minAngle;
                    transform.rotation = originalRotation * Quaternion.AngleAxis(currentAngle, rotationAxis);
                }
                else
                {
                    transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime, Space.Self);
                }
            }
            else
            {
                float angle = Mathf.PingPong(Time.time * rotationSpeed, 180);
                transform.rotation = Quaternion.AngleAxis(angle, rotationAxis);
            }
        }
        else if (returningToPosition)
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition, Time.deltaTime * hoverSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, originalRotation, Time.deltaTime * hoverSpeed);

            if (Vector3.Distance(transform.position, originalPosition) < 0.01f && Quaternion.Angle(transform.rotation, originalRotation) < 0.1f)
            {
                transform.position = originalPosition;
                transform.rotation = originalRotation;
                returningToPosition = false;
            }
        }
    }

    void OnMouseEnter()
    {
        isHovered = true;
        returningToPosition = false;
    }

    void OnMouseExit()
    {
        isHovered = false;
        returningToPosition = true;
    }
}


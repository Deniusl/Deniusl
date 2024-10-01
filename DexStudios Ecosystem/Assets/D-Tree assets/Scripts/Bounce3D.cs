using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce3D : MonoBehaviour
{
    [Header("Position Bounce Settings")]
    public float positionAmplitude = 1.0f;
    public float positionFrequency = 1.0f;
    public float positionOffset = 0.0f;

    [Header("Scale Bounce Settings")]
    public float scaleAmplitude = 0.1f;
    public float scaleFrequency = 1.0f;
    public float scaleOffset = 0.0f;

    private Vector3 startPos;
    private Vector3 startScale;

    void Start()
    {
        startPos = transform.position;
        startScale = transform.localScale;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * positionFrequency + positionOffset) * positionAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        float scaleModifier = 1 + Mathf.Sin(Time.time * scaleFrequency + scaleOffset) * scaleAmplitude;
        transform.localScale = startScale * scaleModifier;
    }
}


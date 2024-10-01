using System.Collections;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGrowthAnimationController : MonoBehaviour
{
    public Animation treeAnimation;
    public string growthAnimationName = "Grow";
    public string deathAnimationName = "Die";
    public int maxWateringDays = 21;
    public int currentWateringDays = 0;
    public bool isDying = false;

    public float initialTreeScale = 0.1f;
    public float maxScale = 1.0f;
    public float interpolationSpeed = 2.0f; 
    public float bounceIntensity = 0.05f;  
    public float bounceDuration = 0.5f;    

    private float lastScale = 0.0f;
    private int lastWateringDays = 0;    
    private float bounceTimer = 0.0f;   
    private bool wasIncreasing = true; 

    void Start()
    {
        UpdateAnimationBasedOnWateringDays();
    }

    void Update()
    {
        if (isDying)
        {
            PlayDeathAnimation();
        }
        else
        {
            if (currentWateringDays != lastWateringDays)
            {
                wasIncreasing = currentWateringDays > lastWateringDays;
                bounceTimer = bounceDuration; 
                lastWateringDays = currentWateringDays;
            }
            UpdateAnimationBasedOnWateringDays();
        }
    }

    void UpdateAnimationBasedOnWateringDays()
    {
        float normalizedTime = MapDayToAnimation(currentWateringDays, maxWateringDays);
        AnimationState state = treeAnimation[growthAnimationName];

        float targetScale = Mathf.Lerp(initialTreeScale, maxScale, normalizedTime);
        float smoothScale = Mathf.SmoothStep(lastScale, targetScale, Time.deltaTime * interpolationSpeed);

        if (bounceTimer > 0)
        {
            smoothScale += (wasIncreasing ? 1 : -1) * Mathf.Sin((bounceDuration - bounceTimer) * Mathf.PI / bounceDuration) * bounceIntensity;
            bounceTimer -= Time.deltaTime;
        }

        state.normalizedTime = normalizedTime;
        treeAnimation.Play(growthAnimationName);
        treeAnimation.Sample();
        treeAnimation.Stop();

        transform.localScale = new Vector3(smoothScale, smoothScale, smoothScale);
        lastScale = smoothScale;
    }


    void PlayDeathAnimation()
    {
        if (!treeAnimation.IsPlaying(deathAnimationName))
        {
            treeAnimation.Play(deathAnimationName);
        }
    }

    float MapDayToAnimation(int currentDay, int totalDays)
    {
        float startFrame = 4.0f / 60.0f;
        float endFrame = 1.0f;
        float normalizedTime = (currentDay - 1) / (float)(totalDays - 1);
        return Mathf.Lerp(startFrame, endFrame, normalizedTime);
    }
}

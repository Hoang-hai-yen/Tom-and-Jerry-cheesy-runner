using UnityEngine;
using System.Collections;

public class PlayerFlyController : MonoBehaviour
{
    public static PlayerFlyController Instance;

    [Header("Settings")]
    public float flyHeight = 5f;
    public float transitionSpeed = 5f;
    
    public bool IsFlying { get; private set; }
    
    public float CurrentTargetY { get; private set; }

    void Awake() => Instance = this;

    public void StartFly(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(FlyRoutine(duration));
    }

    private IEnumerator FlyRoutine(float duration)
    {
        IsFlying = true;
        float elapsed = 0;
        while (elapsed < duration)
        {
            CurrentTargetY = flyHeight;
            elapsed += Time.deltaTime;
            yield return null;
        }

        IsFlying = false;
        CurrentTargetY = 0;
    }
}
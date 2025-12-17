using UnityEngine;
using System.Collections;

public class PlayerFlyController : MonoBehaviour
{
    public static PlayerFlyController Instance;

    [Header("Model Settings")]
    public GameObject normalModel; 
    public GameObject flyingModel; 

    [Header("Settings")]
    public float flyHeight = 5f;
    public float transitionSpeed = 5f;
    
    public bool IsFlying { get; private set; }
    public float CurrentTargetY { get; private set; }

    void Awake() => Instance = this;

    public void StartFly(float duration)
    {
        PlayerMovement player = GetComponent<PlayerMovement>();
        if (player != null) player.DeactivateBoost();

        StopAllCoroutines();
        StartCoroutine(FlyRoutine(duration));
    }

    private IEnumerator FlyRoutine(float duration)
    {
        IsFlying = true;
        CurrentTargetY = flyHeight;

        if (normalModel != null) normalModel.SetActive(false);
        if (flyingModel != null) flyingModel.SetActive(true);

        yield return new WaitForSeconds(duration);

        IsFlying = false;
        CurrentTargetY = 0f;

        if (normalModel != null) normalModel.SetActive(true);
        if (flyingModel != null) flyingModel.SetActive(false);
    }
}
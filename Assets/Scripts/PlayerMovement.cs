using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Animator anim;

    private Vector3 direction;

    [Header("Movement Parameters")]
    public float baseForwardSpeed = 10f;
    public float laneSpeed = 10f;
    public float laneDistance = 3f;
    public float jumpForce = 5f;
    public float gravity = -20f;
    public float maxFallSpeed = -30f;

    private int desiredLane = 1;
    private float targetLaneX;

    [Header("Slide Parameters")]
    public float slideDuration = 1f;
    private float slideTimer;
    private bool isSliding = false;
    private float originalHeight;
    private Vector3 originalCenter;

    [Header("Magnet")]
    public float magnetRadius = 7f;
    private bool isMagnetActive = false;
    private float magnetTimer;

    [Header("Shield Settings")]
    public bool isShieldActive = false;
    public GameObject shieldVisual;
    private float shieldTimer;

    [Header("Boost Settings")]
    public float boostSpeedMultiplier = 2f;
    private bool isBoosting = false;
    private float boostTimer;

    private float currentForwardSpeed;

    [Header("VFX")]
    public ParticleSystem boostVfx;
    [Header("Tom Follower")]
    public TomFollower tomFollower;
    public bool isStopped = false;
    [Header("Speed Progression")]
    public float speedIncreaseRate = 0.05f;
    public float maxForwardSpeed = 30f;     

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();

        originalHeight = controller.height;
        originalCenter = controller.center;

        currentForwardSpeed = baseForwardSpeed;

        if (shieldVisual != null) shieldVisual.SetActive(false);
        if (boostVfx != null) boostVfx.Stop();
    }
    private void Update()
    {
        if (!isStopped)
        {
            HandleSpeedProgression();
            HandleInput();
            HandleBoostTimer(); 
            HandleLaneMovement();
        }

        if (PlayerFlyController.Instance != null && PlayerFlyController.Instance.IsFlying)
        {
            float targetY = PlayerFlyController.Instance.CurrentTargetY;
            float newY = Mathf.Lerp(transform.position.y, targetY, Time.deltaTime * PlayerFlyController.Instance.transitionSpeed);
            
            direction.y = (newY - transform.position.y) / Time.deltaTime;
        }
        else
        {
            ApplyGravity();
        }

        if (!isStopped)
        {
            direction.z = currentForwardSpeed;
            controller.Move(direction * Time.deltaTime);
        }
        else
        {
            controller.Move(new Vector3(0, direction.y, 0) * Time.deltaTime);
        }

        HandleSlideTimer();
        HandleMagnet();
        UpdateAnimations();
    }

    private void HandleSpeedProgression()
    {
        baseForwardSpeed = Mathf.Min(
            baseForwardSpeed + speedIncreaseRate * Time.deltaTime, 
            maxForwardSpeed
        );

        if (!isBoosting)
        {
            currentForwardSpeed = baseForwardSpeed;
        }
    }
    
    #region Movement Core
    private void ApplyGravity()
    {
        if (PlayerFlyController.Instance != null && PlayerFlyController.Instance.IsFlying)
        {
            direction.y = 0; 
            return;
        }

        if (controller.isGrounded)
        {
            if (direction.y < 0) direction.y = -2f;
        }
        else
        {
            direction.y += gravity * Time.deltaTime;
            direction.y = Mathf.Max(direction.y, maxFallSpeed);
        }
    }

    private void HandleInput()
    {
        bool isFlying = PlayerFlyController.Instance != null && PlayerFlyController.Instance.IsFlying;

        if (Input.GetKeyDown(KeyCode.UpArrow) && controller.isGrounded && !isFlying)
            Jump();

        if (Input.GetKeyDown(KeyCode.LeftArrow) && desiredLane > 0)
            MoveLeft();

        if (Input.GetKeyDown(KeyCode.RightArrow) && desiredLane < 2)
            MoveRight();

        if (Input.GetKeyDown(KeyCode.DownArrow) && controller.isGrounded && !isSliding && !isFlying)
            StartSlide();
    }

    private void HandleLaneMovement()
    {
        targetLaneX = (desiredLane - 1) * laneDistance;

        Vector3 currentPos = transform.position;
        Vector3 targetPos = new Vector3(targetLaneX, currentPos.y, currentPos.z);

        Vector3 newPos = Vector3.Lerp(currentPos, targetPos, laneSpeed * Time.deltaTime);

        direction.x = (newPos - currentPos).x / Time.deltaTime;
    }

    private void Jump()
    {
        direction.y = jumpForce;
        anim.SetBool("isJumping", true);
    }

    private void MoveLeft()
    {
        desiredLane--;
        anim.ResetTrigger("turnLeft");
        anim.SetTrigger("turnLeft");
    }

    private void MoveRight()
    {
        desiredLane++;
        anim.ResetTrigger("turnRight");
        anim.SetTrigger("turnRight");
    }
    #endregion


    #region Slide Logic
    private void StartSlide()
    {
        isSliding = true;
        slideTimer = slideDuration;

        controller.height = originalHeight / 2;
        controller.center = new Vector3(originalCenter.x, originalCenter.y / 2, originalCenter.z);

        anim.SetBool("isSliding", true);
    }

    private void EndSlide()
    {
        isSliding = false;
        controller.height = originalHeight;
        controller.center = originalCenter;

        anim.SetBool("isSliding", false);
    }

    private void HandleSlideTimer()
    {
        if (!isSliding) return;

        slideTimer -= Time.deltaTime;
        if (slideTimer <= 0) EndSlide();
    }
    #endregion


    #region Magnet Logic
    public void ActivateMagnet(float duration)
    {
        isMagnetActive = true;
        magnetTimer = duration;
        Debug.Log("Magnet Activated");
    }

    private void HandleMagnet()
    {
        if (!isMagnetActive) return;

        magnetTimer -= Time.deltaTime;

        if (magnetTimer <= 0)
        {
            isMagnetActive = false;
            Debug.Log("Magnet Deactivated");
            return;
        }

        Collider[] hits = Physics.OverlapSphere(transform.position, magnetRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Cheese"))
            {
                var cheese = hit.GetComponent<CheeseCollect>();
                if (cheese != null)
                    cheese.Attract(transform);
            }
        }
    }
    #endregion


    #region Boost Logic
    public void ActivateBroomBoost(float duration)
    {
        if (PlayerFlyController.Instance != null && PlayerFlyController.Instance.IsFlying) return;

        isBoosting = true;
        boostTimer = duration; 

        currentForwardSpeed = baseForwardSpeed * boostSpeedMultiplier;

        if (boostVfx != null) boostVfx.Play();
    }

    private void HandleBoostTimer()
    {
        if (!isBoosting) return;

        boostTimer -= Time.deltaTime;

        if (boostTimer <= 0)
            DeactivateBoost();
    }

    public void DeactivateBoost() 
    {
        isBoosting = false;
        boostTimer = 0; 
        currentForwardSpeed = baseForwardSpeed;

        if (boostVfx != null) boostVfx.Stop();
        
        Debug.Log("Boost Deactivated");
    }
    #endregion


    #region Shield Logic

    public Texture shieldIcon;
    private BuffUIItem shieldBuffUI;
    public void ActivateShield()
    {
        isShieldActive = true;
        if (shieldVisual != null) shieldVisual.SetActive(true);
        Debug.Log("Shield Activated");
        shieldBuffUI = BuffUIManager.Instance.AddBuff(shieldIcon, 8);

        StartCoroutine(AutoDisableShieldAfter(8f));
    }

    private IEnumerator AutoDisableShieldAfter(float time)
    {
        yield return new WaitForSeconds(time);

        if (isShieldActive)
            DeactivateShield();
    }


    public void DeactivateShield()
    {
        isShieldActive = false;
        if (shieldVisual != null) shieldVisual.SetActive(false);

        if (shieldBuffUI != null)
        {
            BuffUIManager.Instance.RemoveBuff(shieldBuffUI);
            shieldBuffUI = null;
        }
    }
    #endregion


    #region Collision Detection
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!hit.collider.CompareTag("Obstacle")) return;
        if (PlayerFlyController.Instance != null && PlayerFlyController.Instance.IsFlying) return;

        if (Vector3.Dot(hit.normal, Vector3.up) > 0.1f) 
        {
            Debug.Log("Player: Va chạm bị bỏ qua (va chạm từ trên xuống).");
            return; 
        }
        

        if (isBoosting)
        {
            Destroy(hit.gameObject);
            return;
        }

        if (isShieldActive)
        {
            DeactivateShield();

            Destroy(hit.gameObject);
            Debug.Log("Shield absorbed hit");
            return;
        }

        currentForwardSpeed = 0f; 
        direction.x = 0f; 
        isStopped = true; 
        Debug.Log("Player: Va chạm ngang thành công. Jerry dừng lại (isStopped=true)."); 

        if (tomFollower != null)
        {
            Debug.Log("Player: Lệnh kích hoạt Catch Up đã gửi tới Tom.");
            tomFollower.StartCatchUpSequence();
        }
        else
        {
            Debug.LogWarning("Player: TomFollower không được gán trong PlayerMovement!");
        }

        Destroy(hit.gameObject);
        
    }
    #endregion

    private void UpdateAnimations()
    {
        bool isFlying = PlayerFlyController.Instance != null && PlayerFlyController.Instance.IsFlying;

        anim.SetBool("isRunning", controller.isGrounded && !isSliding && direction.z > 0 && !isFlying);
        anim.SetBool("isSliding", isSliding);

        if (controller.isGrounded && anim.GetBool("isJumping"))
            anim.SetBool("isJumping", false);
    }
    private void OnDrawGizmos()
    {
        if (laneDistance == 0f) return;

        Gizmos.color = Color.yellow; 

        float gizmoLength = 100f; 
        
        float startZ = transform.position.z - gizmoLength / 2f; 
        
        float endZ = transform.position.z + gizmoLength / 2f + 10f; 

        for (int i = 0; i < 3; i++)
        {
            float laneX = (i - 1) * laneDistance; 

            Vector3 startPoint = new Vector3(laneX, transform.position.y, startZ);
            Vector3 endPoint = new Vector3(laneX, transform.position.y, endZ);

            Gizmos.DrawLine(startPoint, endPoint);
        }
    }
}

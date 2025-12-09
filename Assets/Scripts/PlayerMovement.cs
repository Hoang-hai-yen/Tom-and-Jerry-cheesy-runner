using UnityEngine;

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

    [Header("Boost Settings")]
    public float boostSpeedMultiplier = 2f;
    private bool isBoosting = false;
    private float boostTimer;

    private float currentForwardSpeed;

    [Header("VFX")]
    public ParticleSystem boostVfx;


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
        ApplyGravity();
        HandleInput();
        HandleLaneMovement();
        HandleSlideTimer();
        HandleMagnet();
        HandleBoostTimer();

        direction.z = currentForwardSpeed;
        controller.Move(direction * Time.deltaTime);

        UpdateAnimations();
    }

    #region Movement Core
    private void ApplyGravity()
    {
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
        if (Input.GetKeyDown(KeyCode.UpArrow) && controller.isGrounded)
            Jump();

        if (Input.GetKeyDown(KeyCode.LeftArrow) && desiredLane > 0)
            MoveLeft();

        if (Input.GetKeyDown(KeyCode.RightArrow) && desiredLane < 2)
            MoveRight();

        if (Input.GetKeyDown(KeyCode.DownArrow) && controller.isGrounded && !isSliding)
            StartSlide();
    }

    private void HandleLaneMovement()
    {
        // 0 = left, 1 = middle, 2 = right
        targetLaneX = (desiredLane - 1) * laneDistance;

        Vector3 currentPos = transform.position;
        Vector3 targetPos = new Vector3(targetLaneX, currentPos.y, currentPos.z);

        // smooth x movement
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
        if (isBoosting) return;

        isBoosting = true;
        boostTimer = duration;

        currentForwardSpeed = baseForwardSpeed * boostSpeedMultiplier;

        if (boostVfx != null) boostVfx.Play();

        Debug.Log("Boost Activated");
    }

    private void HandleBoostTimer()
    {
        if (!isBoosting) return;

        boostTimer -= Time.deltaTime;

        if (boostTimer <= 0)
            DeactivateBoost();
    }

    private void DeactivateBoost()
    {
        isBoosting = false;
        currentForwardSpeed = baseForwardSpeed;

        if (boostVfx != null) boostVfx.Stop();

        Debug.Log("Boost Deactivated");
    }
    #endregion


    #region Shield Logic
    public void ActivateShield()
    {
        isShieldActive = true;
        if (shieldVisual != null) shieldVisual.SetActive(true);

        Debug.Log("Shield Activated");
    }
    #endregion


    #region Collision Detection
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!hit.collider.CompareTag("Obstacle")) return;

        if (isBoosting)
        {
            Destroy(hit.gameObject);
            return;
        }

        if (isShieldActive)
        {
            isShieldActive = false;
            if (shieldVisual != null) shieldVisual.SetActive(false);
            Destroy(hit.gameObject);
            Debug.Log("Shield absorbed hit");
            return;
        }

        GameOverManager.instance.TriggerGameOver();
    }
    #endregion


    private void UpdateAnimations()
    {
        anim.SetBool("isRunning", controller.isGrounded && !isSliding && direction.z > 0);
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

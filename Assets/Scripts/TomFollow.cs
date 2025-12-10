//using UnityEngine;

//public class TomFollower : MonoBehaviour
//{
//    [Header("Target Settings")]
//    public Transform player;
//    public PlayerMovement playerMovement;

//    [Header("Follow Settings")]
//    public float followOffsetZ = 7f;
//    public float followLerpSpeed = 6f;

//    [Header("Catch Up Settings")]
//    public float catchDistance = 1.8f;

//    private bool isCatchingUp = false;
//    private float catchUpTimer = 0f;
//    private const float CATCH_UP_DURATION = 0.5f;
//    private Vector3 catchUpStartPosition; 

//    [Header("Gravity")]
//    public float gravity = -20f;
//    public float maxFallSpeed = -30f;
//    private float yVelocity = 0f; 

//    [Header("Ground Detection")]
//    public float groundRayDistance = 5f;
//    public LayerMask groundLayer;
//    public float terrainAdjustSpeed = 12f;
//    private float yAdjustment = 0f; 

//    private CharacterController controller;
//    private Animator anim;
//    private Vector3 movementVectorXZ; 

//    private void Start()
//    {
//        controller = GetComponent<CharacterController>();
//        anim = GetComponentInChildren<Animator>();

//        if (player == null && playerMovement != null)
//        {
//            player = playerMovement.transform;
//        }

//        if (controller == null)
//        {
//            Debug.LogError("TomFollower: Thiếu CharacterController!");
//        }
//    }

//    private void Update()
//    {
//        if (player == null || controller == null) return;

//        // Ngăn lỗi nếu controller đã tắt (sau Game Over)
//        if (!controller.enabled) return; 

//        ApplyGravity();
//        AdjustHeightToTerrain();

//        if (!playerMovement.isStopped)
//        {
//            movementVectorXZ = FollowPlayer();
//        }
//        else
//        {
//            movementVectorXZ = CatchUpTimed();
//        }

//        Vector3 finalVelocity = new Vector3(
//            movementVectorXZ.x,
//            (yVelocity + yAdjustment) * Time.deltaTime,
//            movementVectorXZ.z
//        );

//        controller.Move(finalVelocity);

//        UpdateRotation(); 
//        yAdjustment = 0f;

//        UpdateAnimation();
//    }

//    private void ApplyGravity()
//    {
//        if (!controller.isGrounded)
//        {
//            yVelocity += gravity * Time.deltaTime;
//            yVelocity = Mathf.Max(yVelocity, maxFallSpeed);
//        }
//        else
//        {
//            yVelocity = -1f;
//        }
//    }

//    private void AdjustHeightToTerrain()
//    {
//        RaycastHit hit;
//        Vector3 rayOrigin = transform.position + Vector3.up * 1f; 

//        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, groundRayDistance, groundLayer))
//        {
//            float desiredY = hit.point.y;
//            float currentY = transform.position.y;
//            float heightDifference = desiredY - currentY;

//            yAdjustment = heightDifference * terrainAdjustSpeed;
//        }
//        else
//        {
//            yAdjustment = 0f;
//        }
//    }

//    public void StartCatchUpSequence()
//    {
//        if (isCatchingUp) return;

//        isCatchingUp = true;
//        catchUpTimer = 0f; 

//        catchUpStartPosition = transform.position; 
//    }

//    private Vector3 CatchUpTimed()
//    {
//        if (!isCatchingUp) return Vector3.zero;

//        catchUpTimer += Time.deltaTime;

//        float t = catchUpTimer / CATCH_UP_DURATION;
//        t = Mathf.Clamp01(t); 

//        Vector3 finalCatchTarget = new Vector3(
//            player.position.x,
//            player.position.y + 0.1f, 
//            player.position.z - catchDistance / 2f 
//        );

//        Vector3 nextPos = Vector3.Lerp(
//            catchUpStartPosition,
//            finalCatchTarget,
//            t
//        );

//        Vector3 moveVectorXZ = new Vector3(nextPos.x - transform.position.x, 0, nextPos.z - transform.position.z);

//        if (t >= 1f)
//        {
//            transform.position = finalCatchTarget; 
//            controller.enabled = false; 

//            Debug.Log("Tom đã hoàn thành đuổi bắt. Game Over!");

//            isCatchingUp = false;

//            if (GameOverManager.instance != null)
//                GameOverManager.instance.TriggerGameOver();

//            return Vector3.zero;
//        }

//        return moveVectorXZ;
//    }

//    private Vector3 FollowPlayer()
//    {
//        Vector3 playerPos = player.position;
//        float targetZ = playerPos.z - followOffsetZ;
//        float targetX = playerPos.x;

//        Vector3 targetPos = new Vector3(targetX, transform.position.y, targetZ);

//        Vector3 lerped = Vector3.Lerp(
//            transform.position,
//            targetPos,
//            followLerpSpeed * Time.deltaTime
//        );

//        return new Vector3(lerped.x - transform.position.x, 0, lerped.z - transform.position.z);
//    }

//    private void UpdateRotation()
//    {
//        transform.rotation = Quaternion.LookRotation(player.forward);
//    }

//    void UpdateAnimation()
//    {
//        if (anim == null) return;
//        anim.SetBool("isRunning", true); 
//    }
//}

using UnityEngine;

public class TomFollower : MonoBehaviour
{
    public Transform player;
    public PlayerMovement playerMovement;

    public float followOffsetZ = 7f;
    public float followLerpSpeed = 6f;

    public float catchDistance = 1.8f;

    private bool isCatchingUp = false;
    private bool isGameFinished = false;

    private float catchUpTimer = 0f;
    private const float CATCH_UP_DURATION = 0.5f;
    private Vector3 catchUpStartPosition;

    public float gravity = -20f;
    public float maxFallSpeed = -30f;
    private float yVelocity = 0f;

    public float groundRayDistance = 5f;
    public LayerMask groundLayer;
    public float terrainAdjustSpeed = 12f;
    private float yAdjustment = 0f;

    private CharacterController controller;
    private Animator anim;
    private Vector3 movementVectorXZ;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();

        if (player == null && playerMovement != null)
        {
            player = playerMovement.transform;
        }
    }

    private void Update()
    {
        if (player == null || controller == null) return;

        if (isGameFinished) return;

        ApplyGravity();
        AdjustHeightToTerrain();

        if (!playerMovement.isStopped)
        {
            movementVectorXZ = FollowPlayer();
        }
        else
        {
            movementVectorXZ = CatchUpTimed();
        }

        if (isGameFinished) return;
        if (!controller.enabled) return;

        Vector3 finalVelocity = new Vector3(
            movementVectorXZ.x,
            (yVelocity + yAdjustment) * Time.deltaTime,
            movementVectorXZ.z
        );

        controller.Move(finalVelocity);

        UpdateRotation();
        yAdjustment = 0f;
        UpdateAnimation();
    }

    private void ApplyGravity()
    {
        if (!controller.enabled) return;

        if (!controller.isGrounded)
        {
            yVelocity += gravity * Time.deltaTime;
            yVelocity = Mathf.Max(yVelocity, maxFallSpeed);
        }
        else
        {
            yVelocity = -1f;
        }
    }

    private void AdjustHeightToTerrain()
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * 1f;

        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, groundRayDistance, groundLayer))
        {
            float desiredY = hit.point.y;
            float currentY = transform.position.y;
            float heightDifference = desiredY - currentY;
            yAdjustment = heightDifference * terrainAdjustSpeed;
        }
        else
        {
            yAdjustment = 0f;
        }
    }

    public void StartCatchUpSequence()
    {
        if (isCatchingUp) return;

        isCatchingUp = true;
        catchUpTimer = 0f;
        catchUpStartPosition = transform.position;
    }

    private Vector3 CatchUpTimed()
    {
        if (!isCatchingUp) return Vector3.zero;

        catchUpTimer += Time.deltaTime;
        float t = catchUpTimer / CATCH_UP_DURATION;
        t = Mathf.Clamp01(t);

        Vector3 finalCatchTarget = new Vector3(
            player.position.x,
            player.position.y + 0.1f,
            player.position.z - catchDistance / 2f
        );

        Vector3 nextPos = Vector3.Lerp(
            catchUpStartPosition,
            finalCatchTarget,
            t
        );

        Vector3 moveVectorXZ = new Vector3(
            nextPos.x - transform.position.x,
            0,
            nextPos.z - transform.position.z
        );

        if (t >= 1f)
        {
            transform.position = finalCatchTarget;

            controller.enabled = false;
            isGameFinished = true;

            isCatchingUp = false;

            if (GameOverManager.instance != null)
                GameOverManager.instance.TriggerGameOver();

            return Vector3.zero;
        }

        return moveVectorXZ;
    }

    private Vector3 FollowPlayer()
    {
        Vector3 playerPos = player.position;
        float targetZ = playerPos.z - followOffsetZ;
        float targetX = playerPos.x;

        Vector3 targetPos = new Vector3(
            targetX,
            transform.position.y,
            targetZ
        );

        Vector3 lerped = Vector3.Lerp(
            transform.position,
            targetPos,
            followLerpSpeed * Time.deltaTime
        );

        return new Vector3(
            lerped.x - transform.position.x,
            0,
            lerped.z - transform.position.z
        );
    }

    private void UpdateRotation()
    {
        if (!controller.enabled) return;
        transform.rotation = Quaternion.LookRotation(player.forward);
    }

    private void UpdateAnimation()
    {
        if (anim == null) return;
        anim.SetBool("isRunning", true);
    }
}
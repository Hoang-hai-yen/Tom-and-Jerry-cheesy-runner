using UnityEngine;

public class TomFollower : MonoBehaviour
{
    [Header("Target & Follow Settings")]
    public Transform target;
    public float followOffsetZ = 10f;     
    public float followLerpSpeed = 6f;    

    [Header("Catch Up Settings")]
    public float catchUpSpeed = 25f;     
    public float catchDistance = 2f;     

    [Header("Gravity")]
    public float gravity = -20f; 
    public float maxFallSpeed = -30f;

    [Header("Ground Detection")]
    public float groundRayDistance = 5f;
    public LayerMask groundLayer;
    public float terrainAdjustSpeed = 12f;

    private CharacterController controller;
    private float yVelocity = 0f;

    private bool isCatchUp = false;
    private float originalOffsetZ;
    private float catchUpDelayTimer = 0f;
    private const float CATCH_UP_START_DELAY = 0.2f;

    void Start()
    {
        originalOffsetZ = followOffsetZ;

        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                target = player.transform;
        }

        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            Debug.LogError("TomFollower: CharacterController component is missing!");
        }

        if (target != null)
        {
            transform.position = new Vector3(
                target.position.x,
                transform.position.y,
                target.position.z - followOffsetZ
            );
        }
    }

    void Update()
    {
        if (target == null || controller == null) return;

        ApplyGravity();

        if (isCatchUp && catchUpDelayTimer > 0)
        {
            catchUpDelayTimer -= Time.deltaTime;
            controller.Move(new Vector3(0, yVelocity * Time.deltaTime, 0));
            return;
        }

        FollowPlayerXZ();
        AdjustHeightToTerrain();
        CheckCatchSuccess();
    }

    void ApplyGravity()
    {
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

    void FollowPlayerXZ()
    {
        float currentSpeed = isCatchUp ? catchUpSpeed : followLerpSpeed;
        float detZ = isCatchUp ? catchDistance : followOffsetZ;

        Vector3 newPos = new Vector3(
            target.position.x,
            transform.position.y,
            target.position.z - detZ
        );

        Vector3 lerped = Vector3.Lerp(transform.position, newPos, currentSpeed * Time.deltaTime);

        Vector3 velocity = new Vector3(
            lerped.x - transform.position.x,
            yVelocity * Time.deltaTime,
            lerped.z - transform.position.z
        );

        controller.Move(velocity);
    }

    void AdjustHeightToTerrain()
    {
        RaycastHit hit;

        Vector3 rayOrigin = transform.position + Vector3.up * 1f;

        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, groundRayDistance, groundLayer))
        {
            float desiredY = hit.point.y;
            float smoothY = Mathf.Lerp(transform.position.y, desiredY, Time.deltaTime * terrainAdjustSpeed);

            transform.position = new Vector3(transform.position.x, smoothY, transform.position.z);
        }
    }

    void CheckCatchSuccess()
    {
        float actualDistance = Mathf.Abs(transform.position.z - (target.position.z - catchDistance));

        if (isCatchUp && actualDistance < 0.4f)
        {
            isCatchUp = false;
            if (GameOverManager.instance != null)
            {
                GameOverManager.instance.TriggerGameOver();
            }
        }
    }

    public void StartCatchUpSequence()
    {
        isCatchUp = true;
        catchUpDelayTimer = CATCH_UP_START_DELAY;
    }

    public void ResetFollowerDistance()
    {
        followOffsetZ = originalOffsetZ;
        isCatchUp = false;
    }
}

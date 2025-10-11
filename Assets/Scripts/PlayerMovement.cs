using NUnit.Framework;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    private Animator anim;

    [Header("Movement Parameters")]
    public float forwardSpeed = 10f;
    public float laneSpeed = 5f;
    public float laneDistance = 3f;
    public float jumpForce = 5f;
    public float gravity = -20f;
    public float slideDuration = 1.0f;
    public float MagnetRadius = 7f;

    private float slideTimer;
    private bool isSliding = false;
    private int desiredLane = 1;
    private float laneXPos = 0f;
    private bool isMagnetActive = false;
    private float magnetTimer;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            if (slideTimer <= 0) EndSlide();
        }

        if (controller.isGrounded)
        {
            if (direction.y < 0) direction.y = -2f;
        }
        else
        {
            direction.y += gravity * Time.deltaTime;
        }

        HandleInput();

        float targetLaneX = 0;
        if (desiredLane == 0) targetLaneX = -laneDistance;
        else if (desiredLane == 2) targetLaneX = laneDistance;

        laneXPos = Mathf.MoveTowards(laneXPos, targetLaneX, laneSpeed * Time.deltaTime);
        direction.x = (laneXPos - controller.transform.position.x) / Time.deltaTime;
        direction.z = forwardSpeed;

        controller.Move(direction * Time.deltaTime);

        UpdateAnimations();
        if (isMagnetActive)
        {
            magnetTimer -= Time.deltaTime;
            if (magnetTimer <= 0)
            {
                isMagnetActive = false;
                Debug.Log("Magnet Deactivated");
            }
            else
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, MagnetRadius);
                foreach (var hitCollider in hitColliders)
                {
                    if (hitCollider.CompareTag("Cheese"))
                    {
                        CheeseCollect cheese = hitCollider.GetComponent<CheeseCollect>();
                        if (cheese != null)
                        {
                            cheese.Attract(transform);
                        }
                    }
                }
            }
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && controller.isGrounded)
        {
            direction.y = jumpForce;
            anim.SetBool("isJumping", true);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && desiredLane > 0)
        {
            desiredLane--;
            anim.SetTrigger("turnLeft");
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && desiredLane < 2)
        {
            desiredLane++;
            anim.SetTrigger("turnRight");
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && controller.isGrounded && !isSliding)
        {
            StartSlide();
        }
    }

    private void UpdateAnimations()
    {
        anim.SetBool("isRunning", controller.isGrounded && !isSliding && direction.z > 0);
        anim.SetBool("isSliding", isSliding);

        if (controller.isGrounded && anim.GetBool("isJumping"))
        {
            anim.SetBool("isJumping", false);
        }
    }

    private void StartSlide()
    {
        isSliding = true;
        slideTimer = slideDuration;
        controller.height /= 2;
        controller.center = new Vector3(controller.center.x, controller.center.y / 2, controller.center.z);
        anim.SetBool("isSliding", true);
    }

    private void EndSlide()
    {
        isSliding = false;
        controller.height *= 2;
        controller.center = new Vector3(controller.center.x, controller.center.y * 2, controller.center.z);
        anim.SetBool("isSliding", false);
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Obstacle"))
        {
            GameOverManager.instance.TriggerGameOver();
        }
    }
    public void ActivateMagnet(float duration)
    {
        isMagnetActive = true;
        magnetTimer = duration;
        Debug.Log("Magnet Activated");
    }
}

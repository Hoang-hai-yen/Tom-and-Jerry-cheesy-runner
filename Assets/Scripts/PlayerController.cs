using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {
    [Header("Movement")]
    public float forwardSpeed = 8f;
    public float laneOffset = 2f;
    public int currentLane = 1;             // 0=trái, 1=giữa, 2=phải
    public float laneChangeSpeed = 10f;

    [Header("Jump & Gravity")]
    public float jumpForce = 7f;
    public float gravity = -20f;

    [Header("Slide")]
    public float slideDuration = 0.8f;
    public float slideHeight = 0.5f;

    private CharacterController cc;
    private Animator anim;

    private Vector3 moveDirection;
    private float verticalVelocity;
    private bool isSliding = false;
    private float originalHeight;

    void Start() {
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        originalHeight = cc.height;

        if (anim) anim.SetBool("IsRunning", true);
    }

    void Update() {
        float moveZ = forwardSpeed * Time.deltaTime;

        // --- Đổi lane ---
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            if (currentLane > 0) {
                currentLane--;
                if (anim) anim.SetTrigger("JumpLeft"); // animation đổi lane trái
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            if (currentLane < 2) {
                currentLane++;
                if (anim) anim.SetTrigger("JumpRight"); // animation đổi lane phải
            }
        }

        float targetX = (currentLane - 1) * laneOffset;
        float diffX = targetX - transform.position.x;
        float moveX = diffX * Time.deltaTime * laneChangeSpeed;

        // --- Jump / Slide ---
        if (cc.isGrounded) {
            if (verticalVelocity < 0) verticalVelocity = -1f;

            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                verticalVelocity = jumpForce;
                if (anim) anim.SetTrigger("Jump");
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) && !isSliding) {
                StartCoroutine(Slide());
                if (anim) anim.SetTrigger("Slide");
            }
        } else {
            verticalVelocity += gravity * Time.deltaTime;
        }

        moveDirection = new Vector3(moveX, verticalVelocity * Time.deltaTime, moveZ);
        cc.Move(moveDirection);
    }

    private IEnumerator Slide() {
        isSliding = true;
        cc.height = slideHeight;
        yield return new WaitForSeconds(slideDuration);
        cc.height = originalHeight;
        isSliding = false;
    }
}

// using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Exp 관련 PlayerBase 코드 참고하기

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1.0f;

    private Rigidbody rb;
    // private Animator anim;

    public float jumpForce = 5;
    public bool isGrounded;

    Vector2 moveInput; 
    Vector2 cameraInput;
    // public CinemachineVirtualCamera vCam;
    public float turnSpeed = 100f;
    Transform cameraTransform;
    bool isCameraInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // anim = GetComponent<Animator>();
        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Walk();
        Jump();
        Attack_Sword();
    }

    private void FixedUpdate()
    {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        //카메라 방향에서 y축값을 0으로 고정 (기본 값이 있기 때문에 초기화해야함)
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // RotateCamera();
        Rotate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            // anim.SetBool("isJumping", false);
        }
    }

    void Rotate()
    {
        if (!isCameraInput) return;
        Vector3 cameraForward = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.y);
        if (cameraForward.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(cameraForward, Vector3.up);
            transform.rotation = targetRotation;
        }
    }

    // void RotateCamera()
    // {
    //     if (vCam != null && cameraInput != Vector2.zero)
    //     {
    //         isCameraInput = true;
    //         var vCamTransposer = vCam.GetCinemachineComponent<CinemachineTransposer>();
    //         if (vCamTransposer != null)
    //         {
    //             Vector3 rotation = vCamTransposer.m_FollowOffset;
    //             rotation = Quaternion.Euler(-cameraInput.y * turnSpeed * Time.fixedDeltaTime,
    //                 cameraInput.x * turnSpeed * Time.fixedDeltaTime, 0) * rotation;
    //             vCamTransposer.m_FollowOffset = rotation;
    //         }
    //     }
    //     else
    //     {
    //         isCameraInput = false;
    //     }
    // }

    void Walk()
    {
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            // anim.SetBool("Walk", true);
            WalkMoving();
        }
        else
        {
            // anim.SetBool("Walk", false);
        }
    }

    void WalkMoving()
    {
        float xInput = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float zInput = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.Translate(xInput, 0, zInput);
    }

    void Attack_Sword()
    {
       if (Input.GetButtonDown("Fire1"))
        {
            // anim.SetBool("Attack", true);
        }
       else
        {
            // anim.SetBool("Attack", false);
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isGrounded = false;
            // anim.SetBool("isJumping", true);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}

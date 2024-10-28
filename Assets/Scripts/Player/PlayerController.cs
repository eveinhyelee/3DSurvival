using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumpPower;
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask;
    

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;
    private Vector2 mouseDelta;


    private Rigidbody rigidbody;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;        
    }

    void FixedUpdate()
    {
        Move();
    }
    private void LateUpdate()
    {
        CameraLook(); //Y값도 Min/Max 제어 필요해보임!
    }
    private void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = rigidbody.velocity.y; // Y값 초기화 = 점프를 할때만 위아래로 넣어줘야함.
        
        rigidbody.velocity = dir;
    }

    private void CameraLook() //플레이어대신 카메라값
    {
        //TODO : 플레이어값을 받아와서 플레이어 방향 조절필요
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot,minXLook,maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) // Started = 키가 눌린순간 1번만
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        { 
            curMovementInput = Vector2.zero;
        }
    }
    public void OnLook(InputAction.CallbackContext context)
    { 
        mouseDelta = context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded()) 
        {
            rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }
    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position+(transform.forward * 0.2f) + (transform.position* 0.01f), Vector3.down),
            new Ray(transform.position+(-transform.forward * 0.2f) + (transform.position* 0.01f), Vector3.down),
            new Ray(transform.position+(transform.right * 0.2f) + (transform.position* 0.01f), Vector3.down),
            new Ray(transform.position+(-transform.right * 0.2f) + (transform.position* 0.01f), Vector3.down)
        };
        for(int i =0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            { 
                return true;
            }
        }
        return false;
    }
}

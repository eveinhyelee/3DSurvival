using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float runSpeed;
    public float jumpPower;
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask;
    private Animator animator;
    private bool isRun;
    public bool IsRun
    {
        get { return isRun;  }
        set { isRun = value; }
    }

    private float jumpZoneJumpForce = 100f;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;
    private Vector2 mouseDelta;
    public bool canLook = true; //���콺Ŀ��

    public Action inventory;
    public Rigidbody rigidbody;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("JumpSpot"))
        {
            rigidbody.AddForce(Vector3.up * jumpZoneJumpForce, ForceMode.Impulse);
        }
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
        if (canLook)
        { 
            CameraLook(); //Y���� Min/Max ���� �ʿ��غ���!        
        }
    }
    private void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = rigidbody.velocity.y; // Y�� �ʱ�ȭ = ������ �Ҷ��� ���Ʒ��� �־������.
        
        rigidbody.velocity = dir;

        if (isRun == true)
        {
            dir *= runSpeed;
        }
        else if (isRun == false)
        {
            dir *= moveSpeed;
            animator.SetBool("isRun", false);
        }
    }

    private void CameraLook() //�÷��̾��� ī�޶�
    {        
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot,minXLook,maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) // Started = Ű�� �������� 1����
        {
            curMovementInput = context.ReadValue<Vector2>();
            animator.SetBool("isWalk",true);
        }
        else if (context.phase == InputActionPhase.Canceled)
        { 
            curMovementInput = Vector2.zero;
            animator.SetBool("isWalk", false);
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
            animator.SetTrigger("isJump");
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
    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {            
            isRun = true;
            animator.SetBool("isRun", true);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            isRun = false;
            animator.SetBool("isRun", false);
        }
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }
    void ToggleCursor()
    { 
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
}

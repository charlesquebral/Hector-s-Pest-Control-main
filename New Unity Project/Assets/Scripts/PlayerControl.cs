using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float movementSpeed = 5.0f;
    public float mouseSensitivity = 2.0f;
    public float jumpForce = 5.0f;
    public float gravity = -9.81f;

    private CharacterController characterController;
    private Camera playerCamera;
    private float verticalRotation = 0f;
    private Vector3 playerVelocity;

    public ScoreKeeper sk;

    public Vector3[] stance;
    public float[] collHeight;
    public Vector3[] collPos;

    public bool crouch = false;
    public bool prone = false;

    void Start()
    {
        sk = FindObjectOfType<ScoreKeeper>();
        characterController = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            crouch = !crouch;
            prone = false;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            crouch = false;
            prone = !prone;
        }

        if (crouch)
        {
            characterController.height = collHeight[1];
            characterController.center = collPos[1];
            playerCamera.transform.localPosition = Vector3.Slerp(playerCamera.transform.localPosition, stance[1], 5 * Time.deltaTime);
            movementSpeed = 3;
        }
        else if (prone)
        {
            characterController.height = collHeight[2];
            characterController.center = collPos[2];
            playerCamera.transform.localPosition = Vector3.Slerp(playerCamera.transform.localPosition, stance[2], 5 * Time.deltaTime);
            movementSpeed = 1;
        }
        else
        {
            characterController.height = collHeight[0];
            characterController.center = collPos[0];
            playerCamera.transform.localPosition = Vector3.Slerp(playerCamera.transform.localPosition, stance[0], 5 * Time.deltaTime);

            if (Input.GetKey(KeyCode.LeftShift))
            {
                movementSpeed = 8;
            }
            else
            {
                movementSpeed = 5;
            }
        }

        Move();
    }

    private void Move()
    {
        float horizontalMovement = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
        float verticalMovement = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;

        Vector3 movement = transform.right * horizontalMovement + transform.forward * verticalMovement;
        characterController.Move(movement);

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        playerVelocity.y += gravity * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);

        if (characterController.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            playerVelocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }
}
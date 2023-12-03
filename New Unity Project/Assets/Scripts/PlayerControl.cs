using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float movementSpeed = 5.0f;
    public float mouseSensitivity = 2.0f;
    public float jumpForce = 5.0f;
    public float gravity = -9.81f;

    private CharacterController characterController;
    public Camera playerCamera;
    private float verticalRotation = 0f;
    private Vector3 playerVelocity;

    public ScoreKeeper sk;

    public Vector3[] stance;
    public float[] collHeight;
    public Vector3[] collPos;

    public bool crouch = false;
    public bool prone = false;

    public Animator legs;
    public Animator shadow;

    public float camSpeed;

    void Start()
    {
        sk = FindObjectOfType<ScoreKeeper>();
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        shadow.transform.localPosition = new Vector3(0, 0, 0.2199995f);

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
            legs.SetBool("crouch", true);
            shadow.SetBool("crouch", true);
            characterController.height = collHeight[1];
            characterController.center = collPos[1];
            playerCamera.transform.localPosition = Vector3.Slerp(playerCamera.transform.localPosition, stance[1], 5 * Time.deltaTime);
            movementSpeed = 3;
            legs.gameObject.transform.localScale = Vector3.Slerp(legs.gameObject.transform.localScale, new Vector3(1.45f, 1.45f, 1.45f), 5 * Time.deltaTime);
        }
        else if (prone)
        {
            legs.SetBool("crouch", false);
            shadow.SetBool("crouch", false);
            characterController.height = collHeight[2];
            characterController.center = collPos[2];
            playerCamera.transform.localPosition = Vector3.Slerp(playerCamera.transform.localPosition, stance[2], 5 * Time.deltaTime);
            movementSpeed = 1;
            legs.gameObject.transform.localScale = Vector3.Slerp(legs.gameObject.transform.localScale, Vector3.zero, 5 * Time.deltaTime);
        }
        else
        {
            legs.SetBool("crouch", false);
            shadow.SetBool("crouch", false);
            characterController.height = collHeight[0];
            characterController.center = collPos[0];
            playerCamera.transform.localPosition = Vector3.Slerp(playerCamera.transform.localPosition, stance[0], 5 * Time.deltaTime);
            legs.gameObject.transform.localScale = Vector3.Slerp(legs.gameObject.transform.localScale, new Vector3(1.45f,1.45f,1.45f), 5 * Time.deltaTime);

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetAxis("Vertical") > 0 && !crouch && !prone)
            {
                movementSpeed = 8;
                legs.SetBool("run", true);
                shadow.SetBool("run", true);
            }
            else
            {
                movementSpeed = 5;
                legs.SetBool("run", false);
                shadow.SetBool("run", false);
            }
        }

        Move();

        legs.SetFloat("forward", Input.GetAxis("Vertical"));
        legs.SetFloat("strafe", Input.GetAxis("Horizontal"));
        shadow.SetFloat("forward", Input.GetAxis("Vertical"));
        shadow.SetFloat("strafe", Input.GetAxis("Horizontal"));
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

        playerCamera.transform.localRotation = Quaternion.Slerp(playerCamera.transform.localRotation, Quaternion.Euler(verticalRotation, 0f, 0f), camSpeed * Time.deltaTime);
        transform.Rotate(Vector3.up * mouseX);

        playerVelocity.y += gravity * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);

        if (characterController.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            playerVelocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }
}
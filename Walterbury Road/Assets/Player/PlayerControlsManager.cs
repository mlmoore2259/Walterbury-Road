using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerControlsManager : MonoBehaviour
{
    // Look specific variables
    [SerializeField] float mouseSensitivity = 100f;
    [SerializeField] Transform cameraTransform;
    private Vector2 lookInput;
    private float xAxisClamp = 0f;

    // PLayer movement specific variables
    [SerializeField] Transform playerBody;
    [SerializeField] CharacterController characterController;
    private float playerSpeed;
    private float gravityValue = -9.81f;
    [SerializeField] PlayerControls playerControls;
    [SerializeField] Vector3 movementInput;

    // Player object variables
    [SerializeField] GameObject flashlightLight;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
    }

    private void Awake()
    {
        playerSpeed = 5f;
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        // Handle camera rotation based on look input
        if (lookInput.sqrMagnitude > 0)
        {
            playerBody.Rotate(Vector3.up * lookInput.x);
            xAxisClamp += lookInput.y;
            xAxisClamp = Mathf.Clamp(xAxisClamp, -90f, 90f);
            cameraTransform.localRotation = Quaternion.Euler(-xAxisClamp, 0f, 0f);
        }

        // Apply player movement
        Vector3 move = new Vector3(movementInput.x, gravityValue, movementInput.y);
        move = playerBody.transform.TransformDirection(move);
        characterController.Move(move * playerSpeed * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>() * mouseSensitivity * Time.deltaTime;
        if (context.canceled)
        {
            lookInput = Vector2.zero;
        }
    }

    public void OnFlashlightToggle(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            flashlightLight.SetActive(!flashlightLight.activeSelf);
        }
    }

    private void LateUpdate()
    {

    }
}

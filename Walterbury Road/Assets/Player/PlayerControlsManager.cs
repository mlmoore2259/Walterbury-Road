using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlsManager : MonoBehaviour
{
    // Look specific varibales
    [SerializeField] float mouseSensitivity = 100f;
    [SerializeField] Transform cameraTransform;
    private Vector2 lookInput;
    private float xAxisClamp = 0f;

    // PLayer movement specific variables
    [SerializeField] Transform playerBody;
    [SerializeField] Rigidbody playerRigidbody;
    [SerializeField] PlayerControls playerControls;
    [SerializeField] Vector3 movementInput;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Awake()
    {
        playerControls = new PlayerControls();
        
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>() * mouseSensitivity * Time.deltaTime;
    }

    private void LateUpdate()
    {
        if (lookInput.sqrMagnitude > 0)
        {
            playerBody.Rotate(Vector3.up * lookInput.x);
            xAxisClamp += lookInput.y;
            xAxisClamp = Mathf.Clamp(xAxisClamp, -90f, 90f);
            cameraTransform.localRotation = Quaternion.Euler(-xAxisClamp, 0f, 0f);
        }
    }
}

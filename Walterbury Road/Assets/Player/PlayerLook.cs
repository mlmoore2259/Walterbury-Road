using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    public Transform cameraTransform;
    private Vector2 lookInput;
    private float xAxisClamp = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
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

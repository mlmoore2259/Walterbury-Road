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
    private float gravityValue = -1.81f;
    [SerializeField] PlayerControls playerControls;
    [SerializeField] Vector3 movementInput;

    // Player object variables
    [SerializeField] GameObject flashlightLight;
    [SerializeField] float interactRange = 2.5f;
    [SerializeField] PlayerGameInfo playerGameInfo;

    // UI Objects
    [SerializeField] GameObject notebook;
    [SerializeField] NotebookManager notebookManager;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
        playerGameInfo = GetComponent<PlayerGameInfo>();
        notebook.SetActive(false);
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
        CameraRotation();
        MovePlayer();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>() * mouseSensitivity * Time.deltaTime;
    }

    public void OnFlashlightToggle(InputAction.CallbackContext context)
    {
        // Turn flashlight on and off
        if (context.performed)
        {
            flashlightLight.SetActive(!flashlightLight.activeSelf);
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Shoot a raycast from the center of the screen to detect objects
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

            if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
            {
                // Evidence case
                if (hit.collider.gameObject.CompareTag("Evidence"))
                {
                    Debug.Log("Interacted with: " + hit.collider.gameObject.name);
                    hit.collider.gameObject.GetComponent<EvidenceItem>().found = true;
                }

                // Non-evidence, but still interactable
                else if (hit.collider.gameObject.CompareTag("Interactable"))
                {

                }
            }
        }
    }

    public void CameraRotation()
    {
        // Handle camera rotation based on look input
        if (lookInput.sqrMagnitude > 0)
        {
            playerBody.Rotate(Vector3.up * lookInput.x);
            xAxisClamp += lookInput.y;
            xAxisClamp = Mathf.Clamp(xAxisClamp, -90f, 90f);
            cameraTransform.localRotation = Quaternion.Euler(-xAxisClamp, 0f, 0f);
        }
    }

    public void MovePlayer()
    {
        // Apply player movement
        Vector3 move = new Vector3(movementInput.x, gravityValue, movementInput.y);
        move = playerBody.transform.TransformDirection(move);
        characterController.Move(move * playerSpeed * Time.deltaTime);
    }

    public void OnNotebookToggle(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            notebook.SetActive(!notebook.activeSelf);
            // If notebook is active pause the game
            //if (notebook.activeSelf)
            //{
            //    Time.timeScale = 0f;
            //    // Disable player controls
                
            //}
            //else
            //{
            //    Time.timeScale = 1f;
            //}
        }
    }

    public void NextPage()
    {
        if (notebookManager.currPage < 4)
        {
            notebookManager.currPage++;
            notebookManager.SetPage(notebookManager.currPage - 1);
        }
    }

    public void PrevPage()
    {
        if (notebookManager.currPage > 0)
        {
            notebookManager.currPage--;
            notebookManager.SetPage(notebookManager.currPage + 1);
        }
    }
}

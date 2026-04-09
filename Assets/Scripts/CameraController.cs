using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CameraMovement))]
public class CameraController : MonoBehaviour
{
    [Header("Movement")]
    public float movementSpeed = 10;
    public float movementAcc = 100;

    [Header("Rotation")]
    public float mouseSensitivity = 1;

    private CameraMovement _cameraMovement;

    private InputActionMap _playerInputActionMap;
    private InputAction _moveAction;
    private InputAction _lookAction;
    private InputAction _mouseMoveAction;

    //Movement
    private Vector3 _speed;
    private bool _rotating = false;

    //Mouse Position
    private Vector2 _cachedMousePosition;
    private int _mousePositionReadDelay;


    void Awake()
    {
        _cameraMovement = GetComponent<CameraMovement>();

        _playerInputActionMap = InputSystem.actions.FindActionMap("Movement");
        _moveAction = _playerInputActionMap.FindAction("Move");
        _mouseMoveAction = _playerInputActionMap.FindAction("MouseMove");
        _lookAction = _playerInputActionMap.FindAction("Look");
    }

    void Update()
    {
        //Camera Movement
        var moveDirection = _moveAction.ReadValue<Vector3>().normalized;
        var targetSpeed = moveDirection * movementSpeed;
        _speed = Vector3.MoveTowards(_speed, targetSpeed, movementAcc * Time.deltaTime);
        _cameraMovement.HandleMovement(_speed * Time.deltaTime);

        if (_rotating)
        {
            var mouseDelta = _mouseMoveAction.ReadValue<Vector2>();
            _cameraMovement.HandleRotation(mouseDelta * mouseSensitivity);
        }

        //Mouse Position Calculation
        _mousePositionReadDelay -= 1;
    }


    void OnEnable()
    {
        _lookAction.started += OnLookStarted;
        _lookAction.canceled += OnLookCanceled;
        
        _playerInputActionMap.Enable();
    }

    void OnDisable()
    {
        _lookAction.started -= OnLookStarted;
        _lookAction.canceled -= OnLookCanceled;

        _playerInputActionMap.Disable();
    }

    private void OnLookStarted(InputAction.CallbackContext ctx)
    {
        _cachedMousePosition = GetMousePosition();
        _rotating = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnLookCanceled(InputAction.CallbackContext ctx)
    {
        _rotating = false;
        Cursor.lockState = CursorLockMode.None;
        Mouse.current.WarpCursorPosition(_cachedMousePosition);
        _mousePositionReadDelay = 1;
    }

    private Vector2 GetMousePosition()
    {
        if (Cursor.lockState == CursorLockMode.Locked || _mousePositionReadDelay > 0)
        {
            return _cachedMousePosition;
        }
        else
        {
            return Mouse.current.position.value;
        }
    }
}
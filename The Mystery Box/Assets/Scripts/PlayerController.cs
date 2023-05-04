using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : NetworkBehaviour
{

    public CharacterController controller;
    public Transform cam;

    public float speed = 6f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    // cannot use value that can be null
    private NetworkVariable<int> randomNumber = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    
    private CinemachineFreeLook vcam;
    private Transform tFollowTarget;

    // jump variables
    private Vector3 _playerVelocity;
    private bool _grounded;
    [SerializeField] private float _jumpHeight = 5.0f;
    private bool _jumpPressed = false;
    private float _gravity = -9.81f;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera").transform;
        vcam = GameObject.FindGameObjectWithTag("ThirdPersonCamera").GetComponent<CinemachineFreeLook>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        if (tFollowTarget == null)
        {
            tFollowTarget = gameObject.transform;
            vcam.LookAt = tFollowTarget;
            vcam.Follow = tFollowTarget;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            randomNumber.Value = Random.Range(0, 100);
        }

        MovementJump();

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }

    public override void OnNetworkSpawn()
    {
        randomNumber.OnValueChanged += (int previousValue, int newValue) =>
        {
            Debug.Log(OwnerClientId + "; randomNumber: " + randomNumber.Value);
        };
    }

    private void MovementJump()
    {
        _grounded = controller.isGrounded;

        if (_grounded)
        {
            _playerVelocity.y = 0.0f;
        }

        if (_jumpPressed && _grounded)
        {
            _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -1.0f * _gravity);
            _jumpPressed = false;
        }

        _playerVelocity.y += _gravity * Time.deltaTime;
        controller.Move(_playerVelocity * Time.deltaTime);
    }

    private void OnJump()
    {
        if (controller.velocity.y == 0)
        {
            Debug.Log("can jump");
            _jumpPressed = true;
        } else
        {
            Debug.Log("Cant jump");
        }
    }
}

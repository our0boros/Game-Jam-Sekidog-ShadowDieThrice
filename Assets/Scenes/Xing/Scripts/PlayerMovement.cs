using UnityEngine;

public class PlayerMovementCC : MonoBehaviour {
    [System.Serializable]
    public class MovementKeys {
        public KeyCode up = KeyCode.W;
        public KeyCode down = KeyCode.S;
        public KeyCode left = KeyCode.A;
        public KeyCode right = KeyCode.D;
        public KeyCode sprint = KeyCode.LeftShift; // ������̼�
    }

    [Header("Player Settings")]
    public int playerNumber = 1;
    public MovementKeys movementKeys;

    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float gravity = -9.81f;
    public float sprintSpeed = 10f; // ��������ٶ�
    public float jumpHeight = 2f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("Rotation Settings")]
    public bool faceMovementDirection = true;
    public float rotationSpeed = 720f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    public Vector3 movement;
    private bool isMovementActivated = true;
    private bool isSprinting = false;
    private float sprintCooldown = 2.5f; // �����ȴʱ��
    public float sprintDuration = 0.1f; // ��̳���ʱ��
    private float lastSprintTime;
    private float sprintStartTime;

    private void Start() {
        controller = GetComponent<CharacterController>();
        if (controller == null) {
            controller = gameObject.AddComponent<CharacterController>();

        }

        SetupMovementKeys();
    }

    private void SetupMovementKeys() {
        if (playerNumber == 2) {
            movementKeys.up = KeyCode.UpArrow;
            movementKeys.down = KeyCode.DownArrow;
            movementKeys.left = KeyCode.LeftArrow;
            movementKeys.right = KeyCode.RightArrow;
            movementKeys.sprint = KeyCode.RightShift; // player 2 �ĳ�̼�
        }
    }

    private void Update() {
        if (!isMovementActivated) return;

        HandleMovementInput();
        ApplyGravity();
        MovePlayer();
        RotatePlayer();
    }

    private void HandleMovementInput() {
        movement = Vector3.zero;

        // ����̼��İ��º��ɿ�״̬
        if (Input.GetKeyDown(movementKeys.sprint) && Time.time >= lastSprintTime + sprintCooldown)
        {
            isSprinting = true;
            sprintStartTime = Time.time; // ��¼��̿�ʼʱ��
            lastSprintTime = Time.time; // �������һ�γ��ʱ��
        }

        // ������ʱ�䳬���趨��������ʱ�䣬ֹͣ���
        if (isSprinting && Time.time >= sprintStartTime + sprintDuration)
        {
            isSprinting = false;
        }

        if (Input.GetKeyUp(movementKeys.sprint))
        {
            isSprinting = false;
        }


        float currentSpeed = isSprinting ? sprintSpeed : moveSpeed;

        if (Input.GetKey(movementKeys.up))
            movement += Vector3.forward;
        if (Input.GetKey(movementKeys.down))
            movement += Vector3.back;
        if (Input.GetKey(movementKeys.left))
            movement += Vector3.left;
        if (Input.GetKey(movementKeys.right))
            movement += Vector3.right;

        movement = movement.normalized * currentSpeed;
    }

    private void ApplyGravity() {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
    }

    private void MovePlayer() {
        controller.Move(movement * moveSpeed * Time.deltaTime);
        controller.Move(velocity * Time.deltaTime);
    }

    private void RotatePlayer() {
        if (faceMovementDirection && movement != Vector3.zero) {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public bool IsMovementActivated() => isMovementActivated;
    public void ActivateMovement() => isMovementActivated = true;
    public void DeactivateMovement() => isMovementActivated = false;
}
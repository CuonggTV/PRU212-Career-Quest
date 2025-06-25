using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    public CharacterController controller;
    private Animator animator;

    public Transform camera;

    [Header("Jump Settings")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private float gravity = -9.81f;    
    public Vector3 velocity;

    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    public float turnSmoothVelocity = 0.1f;

    public float jumpHeight = 3f;

    public bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }

    public bool JumpPressed()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }
    void Update()
    {
        
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        var isGrounded = IsGrounded();
        if (!isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (JumpPressed() && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        // Store direction
        var direction = new Vector3(horizontal, 0f, vertical);
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            var moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(speed * Time.deltaTime * moveDir.normalized);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}

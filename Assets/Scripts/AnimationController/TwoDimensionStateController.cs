using UnityEngine;
using UnityEngine.InputSystem;

public class TwoDimensionStateController : MonoBehaviour
{
    public ThirdPersonController thirdPersonController;
    private Animator animator;

    private float velocityX = 0.0f;
    private float velocityZ = 0.0f;

    [Header("Movement Settings")]
    public float acceleration = 5.0f;
    public float deceleration = 5.0f;
    public float maximumWalkVelocity = 0.5f;
    public float maximumRunVelocity = 2.0f;

    [Header("Jump Settings")]
    public bool isGrounded;
    public bool jumpPressed;

    private int VelocityXHash;
    private int VelocityZHash;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (thirdPersonController == null)
        {
            thirdPersonController = GetComponent<ThirdPersonController>();
        }
        // Cache animator parameter hashes for performance
        VelocityXHash = Animator.StringToHash("Velocity X");
        VelocityZHash = Animator.StringToHash("Velocity Z");
    }

    /// <summary>
    /// Updates velocity for a given axis based on input.
    /// </summary>
    /// <param name="velocity">Reference to the velocity variable (X or Z)</param>
    /// <param name="negativePressed">Input for negative direction (e.g., left)</param>
    /// <param name="positivePressed">Input for positive direction (e.g., right)</param>
    /// <param name="maxVelocity">Maximum velocity depending on walk/run</param>
    void UpdateVelocity(ref float velocity, bool negativePressed, bool positivePressed, float maxVelocity)
    {
        if (negativePressed)
        {
            velocity = Mathf.MoveTowards(velocity, -maxVelocity, acceleration * Time.deltaTime);
        }
        else if (positivePressed)
        {
            velocity = Mathf.MoveTowards(velocity, maxVelocity, acceleration * Time.deltaTime);
        }
        else
        {
            velocity = Mathf.MoveTowards(velocity, 0, deceleration * Time.deltaTime);
        }
    }

    void Update()
    {
        // Input handling (polling keys)
        bool forwardPressed = Keyboard.current.wKey.isPressed;
        bool leftPressed = Keyboard.current.aKey.isPressed;
        bool rightPressed = Keyboard.current.dKey.isPressed;
        bool runPressed = Keyboard.current.leftShiftKey.isPressed;

        float currentMaxVelocity = runPressed ? maximumRunVelocity : maximumWalkVelocity;

        // Update movement velocities
        UpdateVelocity(ref velocityZ, false, forwardPressed, currentMaxVelocity);
        UpdateVelocity(ref velocityX, leftPressed, rightPressed, currentMaxVelocity);

        // Update jumping

        // Apply to Animator
        animator.SetFloat(VelocityXHash, velocityX);
        animator.SetFloat(VelocityZHash, velocityZ);
        animator.SetBool("IsGround", thirdPersonController.IsGrounded());
        animator.SetBool("JumpPress", thirdPersonController.JumpPressed());

    }
}

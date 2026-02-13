using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Player Input")]
    [SerializeField] private InputSystem_Actions input;
    private Vector2 moveNormalized;
    private Rigidbody rb;

    [Header("Player Scalars")]
    [SerializeField] private Vector2 maxSpeed = new Vector2(10, 10);
    [SerializeField] private Vector2 acceleration = new Vector2(5, 5);
    [SerializeField] private Vector2 jumpForce = new Vector2(0, 5);

    private void Awake()
    {
        input = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        input.Player.Enable();
        input.Player.Move.performed += ctx => moveNormalized = ctx.ReadValue<Vector2>().normalized;
        input.Player.Move.canceled += ctx => moveNormalized = Vector2.zero;
        input.Player.Jump.performed += _ => Jump();
    }

    private void OnDisable()
    {
        input.Player.Disable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        // Accelerate in input direction (no direct velocity set)
        rb.AddForce(moveNormalized * acceleration, ForceMode.Acceleration);

        // Optional speed cap (prevents endless acceleration)
        Vector3 finalVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, 0);
        if (rb.linearVelocity.x > maxSpeed.x) finalVelocity.x = maxSpeed.x;
        else if (rb.linearVelocity.x < -maxSpeed.x) finalVelocity.x = -maxSpeed.x;
        if (rb.linearVelocity.y > maxSpeed.y) finalVelocity.y = maxSpeed.y;
        else if (rb.linearVelocity.y < -maxSpeed.y) finalVelocity.y = -maxSpeed.y;

        rb.linearVelocity = finalVelocity;
    }

    private void Jump()
    {
        rb.AddForce(jumpForce, ForceMode.Impulse);
    }
}

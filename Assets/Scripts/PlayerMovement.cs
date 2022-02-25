using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Range(0f, 1f)] [SerializeField] private float _runningResponsiveness = .6f;
    [SerializeField] private float _runningSpeed = 5f;
    [SerializeField] private float _jumpStrength = 10f;
    [SerializeField] private int _maxJumps = 2;

    [SerializeField] private Transform feetTransform;
    [SerializeField] private Transform cameraTransform;

    private Rigidbody _rigidbody;

    private int _remainingJumps;

    private float _horizontalInput;
    private float _verticalInput;
    private bool _jumpInput;
    
    private Vector2 _runningInput;
    
    public void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _remainingJumps = _maxJumps;
    }

    public void Update()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
        _jumpInput = Input.GetButtonDown("Jump");

        _runningInput = new Vector2(_horizontalInput, _verticalInput);
    }

    public void FixedUpdate()
    {
        Jump();
        Run();
    }

    private void Run()
    {
        // Camera's coordinate system.
        Vector2 cameraRight = new Vector2(cameraTransform.right.x, cameraTransform.right.z).normalized;
        Vector2 cameraForward = new Vector2(cameraTransform.forward.x, cameraTransform.forward.z).normalized;

        // Movement calculation.
        Vector2 currentVelocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.z);
        Vector2 targetVelocity = Vector2.ClampMagnitude(_runningSpeed * (_runningInput.x * cameraRight + _runningInput.y * cameraForward), _runningSpeed);
        Vector2 difference = targetVelocity - currentVelocity;

        // Executes movement.
        _rigidbody.AddForce(new Vector3(difference.x, 0f, difference.y) * _runningResponsiveness, ForceMode.VelocityChange);
        
        // Rotates character in the direction of movement.
        if (targetVelocity != Vector2.zero)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(new Vector3(targetVelocity.x, .0f, targetVelocity.y)), .2f);
        }
    }
    private void Jump()
    {
        if (!_jumpInput) return;
        if (Physics.OverlapSphere(feetTransform.position, .01f).Length > 1)
        {
            _remainingJumps = _maxJumps;
        }
        if (_remainingJumps > 0)
        {
            --_remainingJumps;
            _rigidbody.AddForce(Vector3.up * _jumpStrength, ForceMode.VelocityChange);
        }
    }
}

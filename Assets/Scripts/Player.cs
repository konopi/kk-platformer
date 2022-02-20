using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private float _horizontalInput;
    private float _verticalInput;
    private bool _jumpInput;
    
    private Vector2 _runningInput;
    private Quaternion _rotation;

    [Range(0f, 1f)] [SerializeField] private float _runningResponsiveness = .6f;
    [SerializeField] private float _runningSpeed = 5f;
    [SerializeField] private float _jumpStrength = 10f;

    [SerializeField] private Transform feetTransform;
    [SerializeField] private Transform cameraTransform;
    
    public void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
        _jumpInput = Input.GetButton("Jump");

        _runningInput = new Vector2(_horizontalInput, _verticalInput);
    }

    public void FixedUpdate()
    {
        Run();
        Jump();
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
        if (_jumpInput && Physics.OverlapSphere(feetTransform.position, .001f).Length > 1)
        {
            _rigidbody.AddForce(Vector3.up * _jumpStrength, ForceMode.VelocityChange);
        }
    }
}

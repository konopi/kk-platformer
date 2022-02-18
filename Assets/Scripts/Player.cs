using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private float _horizontalInput;
    private float _verticalInput;
    private bool _jumpInput;

    private Vector2 _runningInput;
    [Range(0f, 1f)] [SerializeField] private float _runningResponsiveness = .6f;
    [SerializeField] private float _runningSpeed = 5f;
    [SerializeField] private float _jumpStrength = 3f;

    [SerializeField] private Transform feetTransform;
    
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
        Vector2 currentVelocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.z);
        Vector2 targetVelocity = Vector2.ClampMagnitude(_runningSpeed * _runningInput, _runningSpeed);
        Vector2 difference = targetVelocity - currentVelocity;
        
        _rigidbody.AddForce(new Vector3(difference.x, 0f, difference.y) * _runningResponsiveness, ForceMode.VelocityChange);
    }
    private void Jump()
    {
        if (_jumpInput && Physics.OverlapSphere(feetTransform.position, .1f).Length > 1)
        {
            _rigidbody.AddForce(Vector3.up * _jumpStrength, ForceMode.VelocityChange);
        }
    }
}

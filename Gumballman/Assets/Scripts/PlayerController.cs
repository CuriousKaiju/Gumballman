using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("COMPONENTS")]
    private Rigidbody2D _rb2D;
    private Transform _transform;

    [Header("LAYER MASKS")]
    [SerializeField] private LayerMask _groundLM;

    [Header("MOVOMENT VARIABLES")]
    [SerializeField] private float _moveAcceleration;
    [SerializeField] private float _maxMoveSpeed;
    [SerializeField] private float _linearGroundDrug;
    private float _horizontalDirection;
    private bool _facingRight;
    private bool _changingDirection => (_rb2D.velocity.x > 0 && _horizontalDirection < 0) || (_rb2D.velocity.x < 0 && _horizontalDirection > 0);

    [Header("JUMP VARIABLES")]
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _airLinearDrug;
    [SerializeField] private float _fallMultiplier;
    [SerializeField] private float _lowJumpFallMultiplier;
    [SerializeField] private float _coyoteTime;
    [SerializeField] private int _extraJumps;
    [SerializeField] private float _jumpBufferLength;
    private int _exteaJumpValue;
    private float _coyoteTimeCounter;
    private float _jumpBufferCounter;
    private bool _canJump => _jumpBufferCounter > 0 && (_coyoteTimeCounter > 0 || _exteaJumpValue > 0);

    [Header("COLLISION VARIABLES")]
    [SerializeField] private float _groundRaycastLength;
    [SerializeField] private bool _onGround;
    [SerializeField] private Vector3 _shifRaycastPosition;

    void Start()
    {
        _rb2D = gameObject.GetComponent<Rigidbody2D>();
        _transform = gameObject.GetComponent<Transform>();
    }
    void Update()
    {
        _horizontalDirection = GetInput().x;
        if (Input.GetButtonDown("Jump"))
        {
            _jumpBufferCounter = _jumpBufferLength;
        }
        else
        {
            _jumpBufferCounter -= Time.deltaTime;
        }
        if (_canJump)
        {
            Jump();
        }
    }
    private void FixedUpdate()
    {
        CheckCollisions();
        MoveCharacter();
        
        if(_onGround)
        {
            ApplyGroundLinearDrug();
            _exteaJumpValue = _extraJumps;
            _coyoteTimeCounter = _coyoteTime;
        }
        else
        {
            ApplyAirLinearDrug();
            FallMultiplier();
            _coyoteTimeCounter -= Time.deltaTime;
        }
    }
    private Vector2 GetInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }
    private void MoveCharacter()
    { 
        _rb2D.AddForce(new Vector2(_horizontalDirection, 0) * _moveAcceleration, ForceMode2D.Force);
        if((_horizontalDirection > 0 && !_facingRight) || (_horizontalDirection < 0 && _facingRight))
        {
            Flip();
        }
        if(Mathf.Abs(_rb2D.velocity.x) > _maxMoveSpeed)
        {
            _rb2D.velocity = new Vector2(Mathf.Sign(_rb2D.velocity.x) * _maxMoveSpeed, _rb2D.velocity.y);
        }
    }
    private void ApplyGroundLinearDrug()
    {
        if (Mathf.Abs(_horizontalDirection) < 0.4f || _changingDirection)
        {
            _rb2D.drag = _linearGroundDrug;
        }
        else
        {
            _rb2D.drag = 0;
        }
    }
    private void Jump()
    {
        if(!_onGround)
        {
            _exteaJumpValue -= 1;
        }
        _rb2D.velocity = new Vector2(_rb2D.velocity.x, 0f);
        _rb2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        _coyoteTimeCounter = 0;
        _jumpBufferCounter = 0;
    }
    private void CheckCollisions()
    {
        _onGround = Physics2D.Raycast(transform.position + _shifRaycastPosition, Vector3.down + _shifRaycastPosition, _groundRaycastLength, _groundLM) ||
                    Physics2D.Raycast(transform.position - _shifRaycastPosition, Vector3.down - _shifRaycastPosition, _groundRaycastLength, _groundLM);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + _shifRaycastPosition, transform.position + _shifRaycastPosition + Vector3.down * _groundRaycastLength);
        Gizmos.DrawLine(transform.position - _shifRaycastPosition, transform.position - _shifRaycastPosition + Vector3.down * _groundRaycastLength);
    }
    private void ApplyAirLinearDrug()
    {
        _rb2D.drag = _airLinearDrug;
    }
    private void FallMultiplier()
    {
        if (_rb2D.velocity.y < 0)
        {
            _rb2D.gravityScale = _fallMultiplier;
        }
        else if (_rb2D.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            _rb2D.gravityScale = _lowJumpFallMultiplier;
        }
        else
        {
            _rb2D.gravityScale = 1f;
        }
    }
    private void Flip()
    {
        _facingRight = !_facingRight;
        _transform.rotation = Quaternion.Euler(0, _facingRight ? 0 : 180, 0);
    }

}

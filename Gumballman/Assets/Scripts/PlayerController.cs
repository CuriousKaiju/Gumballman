using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("COMPONENTS")]
    private Rigidbody2D _rb2D;
    private Transform _transform;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    [Header("LAYER MASKS")]
    [SerializeField] private LayerMask _groundLM;

    [Header("MOVOMENT VARIABLES")]
    [SerializeField] private float _moveAcceleration;
    [SerializeField] private float _maxMoveSpeed;
    [SerializeField] private float _linearGroundDrug;
    private float _horizontalDirection;
    private bool _facingRight = true;
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
    [SerializeField] GameObject _LoseParticles;

    [Header("BUBBLE VARIABLES")]
    [SerializeField] private GameObject _bubbleObject;
    [SerializeField] private float _bubbleJumpForce;
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _attackSpeedCounter;

    [Header("GET DAMAGE VARIABLES")]
    [SerializeField] private float _getDamageForce;

    private bool _canGetDamage = true;
    private BubbleFunctions _presentBubble; 
    private bool _itIsBubbleJump;

    private void Awake()
    {
        GameEvents.GameLose += PlayerLose;
    }
    private void OnDestroy()
    {
        GameEvents.GameLose -= PlayerLose;
    }

    void Start()
    {
        _rb2D = gameObject.GetComponent<Rigidbody2D>();
        _transform = gameObject.GetComponent<Transform>();
        _animator = gameObject.GetComponent<Animator>();
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
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

        if (Input.GetButtonDown("BubbleSpawn") && _attackSpeedCounter >= _attackSpeed)
        {
            _attackSpeedCounter = 0;
            BubbleSpawn();

        }
        else if(_attackSpeedCounter < _attackSpeed)
        {
            _attackSpeedCounter += Time.deltaTime;
        }
        
        
    }
    private void FixedUpdate()
    {
        CheckCollisions();
        MoveCharacter();
        
        if(_onGround)
        {
            _itIsBubbleJump = false;
            if (GetInput().x != 0)
            {
                _animator.SetBool("Run", true);
            }
            else
            {
                _animator.SetBool("Run", false);
            }
            ApplyGroundLinearDrug();
            _exteaJumpValue = _extraJumps;
            _coyoteTimeCounter = _coyoteTime;
        }
        else
        {
            
            if (_rb2D.velocity.y > 0)
            {
                _animator.SetBool("Jump", true);
            }
            else if (_rb2D.velocity.y < 0)
            {
                _animator.SetBool("Jump", false);
                _animator.SetTrigger("Fall");
            }

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
    private void BubbleJump()
    {
        _itIsBubbleJump = true;
        _rb2D.velocity = new Vector2(_rb2D.velocity.x, 0f);
        _rb2D.AddForce(Vector2.up * _bubbleJumpForce, ForceMode2D.Impulse);
        //_coyoteTimeCounter = 0;
        //_jumpBufferCounter = 0;
       
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
        else if (_rb2D.velocity.y > 0 && !Input.GetButton("Jump") && !_itIsBubbleJump)
        {
            _rb2D.gravityScale = _lowJumpFallMultiplier;
        }
        else if(_itIsBubbleJump)
        {
            _rb2D.gravityScale = 1f;
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bubble") && !_onGround && _rb2D.velocity.y < 0)
        {
            collision.gameObject.GetComponent<BubbleFunctions>().CollisionEnabler();
            BubbleJump();
        }
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "SlimePlayerCollision")
        {
            if (_rb2D.velocity.y < 0 && !_onGround)
            {
                collision.GetComponentInParent<SlimeMover>().Destroyer();
                BubbleJump();
            }
            else if (_canGetDamage)
            {
                GetDamage();
            }
        }
        else if (collision.gameObject.name == "FlowerPlayerCollision")
        {
            if (_rb2D.velocity.y < 0 && !_onGround)
            {
                collision.GetComponentInParent<FlowerFunctions>().Destroyer();
                BubbleJump();
            }
            else if (_canGetDamage)
            {
                GetDamage();
            }
        }
        else if (collision.gameObject.name == "TurtlePlayerCollision")
        {
            if (_canGetDamage)
            {
                GetDamage();
            }
        }
        else if (collision.gameObject.CompareTag("Bullet"))
        {
            if (_canGetDamage)
            {
                GetDamage();
                collision.gameObject.GetComponent<BulletOfFlower>().Destroyer();
            }
        }
        else if (collision.gameObject.CompareTag("Health"))
        {
            collision.gameObject.GetComponent<Health>().Destroyer();
        }
    }

    private void BubbleSpawn()
    {
        if (_presentBubble != null)
        {
            _presentBubble.CollisionEnabler();
        }
        _animator.SetTrigger("Attack");
        _presentBubble = Instantiate(_bubbleObject, _transform.position, Quaternion.identity).GetComponent<BubbleFunctions>();
        _presentBubble._spawnPosition = new Vector2(_transform.position.x, _transform.position.y);

        if (_facingRight)
        {
            _presentBubble._moveDirection = 1;
        }
        else
        {
            _presentBubble._moveDirection = -1;
        }
        _attackSpeedCounter = 0;
    }
    public void GetDamage()
    {
        _canGetDamage = false;
        _rb2D.velocity = new Vector2(0, 0);
        if (_facingRight)
        {
            _rb2D.AddForce(new Vector2(-1, 0) * _getDamageForce, ForceMode2D.Impulse);
        }
        else
        {
            _rb2D.AddForce(new Vector2(1, 0) * _getDamageForce, ForceMode2D.Impulse);
        }
        GameEvents.CallHpChangeEvent(1);
        _animator.SetTrigger("GetDamage");
        StartCoroutine("Invisible");

    }
    IEnumerator Invisible()
    {
        for (float a = 1f; a >= 0.2f; a -= 0.05f)
        {

            Color color = _spriteRenderer.material.color;
            color.a = a;
            _spriteRenderer.material.color = color;
            yield return new WaitForSeconds(0.05f);
        }

        for (float a = 0.5f; a <= 0.8f; a += 0.05f)
        {
            Color color = _spriteRenderer.material.color;
            color.a = a;
            _spriteRenderer.material.color = color;
            yield return new WaitForSeconds(0.05f);
        }

        for (float a = 1f; a >= 0.2f; a -= 0.05f)
        {
            Color color = _spriteRenderer.material.color;
            color.a = a;
            _spriteRenderer.material.color = color;
            yield return new WaitForSeconds(0.05f);
        }

        for (float a = 0.5f; a <= 1f; a += 0.05f)
        {
            Color color = _spriteRenderer.material.color;
            color.a = a;
            _spriteRenderer.material.color = color;
            yield return new WaitForSeconds(0.05f);
        }
        _canGetDamage = true;
    }
    public void PlayerGetDamageFromFire()
    {
        _canGetDamage = true;
        Instantiate(_LoseParticles, _transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }
    public void PlayerLose()
    {
        Instantiate(_LoseParticles, _transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}

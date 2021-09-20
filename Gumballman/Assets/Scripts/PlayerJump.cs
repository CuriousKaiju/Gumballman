using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [Header("COMPONENTS")]
    Rigidbody2D _rb2D;

    [Header("INPUT BUTTONS")]
    [SerializeField] private KeyCode _jumpButton;

    [Header("LAYER MASKS")]
    [SerializeField] private LayerMask _groundLM;

    [Header("JUMP VARIABLES")]
    [SerializeField] private float _jumpForce;

    [Header("COLLISION VARIABLES")]
    [SerializeField] private float _raycastLength;
    private bool _onGround;

    void Start()
    {
        _rb2D = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        CollisionDetection();
        if(_onGround)
        {
            Jump();
        }
    }
    private void Jump()
    {
        _rb2D.AddForce(transform.up * _jumpForce, ForceMode2D.Impulse);

    }
    private void CollisionDetection()
    {
        _onGround = Physics2D.Raycast(transform.position, transform.position + -transform.up, _raycastLength, _groundLM);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + -transform.up * _raycastLength);
    }
}

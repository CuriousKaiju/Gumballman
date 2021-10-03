using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletOfFlower : MonoBehaviour
{
    [SerializeField] private float _bulletSpeed;
    [SerializeField] GameObject _bulletParticles;
    public Vector3 _moveDirection;
    private Transform _transform;
    void Start()
    {
        _transform = gameObject.GetComponent<Transform>();
    }
    void FixedUpdate()
    {
        _transform.Translate(_moveDirection * _bulletSpeed);
    }
    public void SetBulletDirection(Vector3 direction)
    {
        _moveDirection = direction;
    }
    public void Destroyer()
    {
        Instantiate(_bulletParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            Destroyer();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
         if(collision.gameObject.CompareTag("Ground"))
        {
            Destroyer();
        }
    }

}

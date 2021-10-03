using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerFunctions : MonoBehaviour
{
    [Header("COMPONENTS")]
    private Transform _transform;
    [SerializeField] private Animator _animator;

    [Header("COLLISION VARIABLES")]
    [SerializeField] private GameObject _parentObject;
    [SerializeField] private GameObject _flowerDestroyParticles;

    [Header("ATTACK VARIABLES")]
    [SerializeField] private float _flowerRaycastLength;
    [SerializeField] private float _AttackDiley;
    [SerializeField] private float _idleDiley;
    [SerializeField] private float _highPosBulletSpawn;
    [SerializeField] private Vector3 _attackDirection;
    [SerializeField] private GameObject[] _bullets;




    void Start()
    {
        _transform = gameObject.GetComponent<Transform>();
        StartCoroutine(FlowerFire());
    }
    private void SpawnBullet()
    {
        Instantiate(_bullets[Random.Range(0, _bullets.Length)], new Vector2(_transform.position.x, _transform.position.y + _highPosBulletSpawn), Quaternion.identity).GetComponent<BulletOfFlower>().SetBulletDirection(_attackDirection);
    }
    public void Destroyer()
    {
        Instantiate(_flowerDestroyParticles, transform.position, Quaternion.identity);
        Destroy(_parentObject);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + _attackDirection * _flowerRaycastLength);
    }
    IEnumerator FlowerFire()
    {
        while (true)
        {
            _animator.SetTrigger("AttackON");
            yield return new WaitForSeconds(_idleDiley);
            SpawnBullet();
            yield return new WaitForSeconds(_AttackDiley);
        }

    }
}

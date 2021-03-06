using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleFunctions : MonoBehaviour
{
    [Header("COMPONENTS")]
    private Collider2D _bubbleCollider;
    private Animator _animator;
    private Transform _transform;

    [Header("BUBBLE VARIABLES")]
    [SerializeField] float _bubbleMoveDistance;
    [SerializeField] float _time;
    private float _timeCounter;
    private Vector2 _finishPos;

    [SerializeField] float _bubbleMoveVerticalDistance;
    [SerializeField] float _timeForVertical;
    private float _timeCounterForVertical;

    private bool _phaseType;
    private bool _MobInside;

    public int _moveDirection = 1;
    public Vector2 _spawnPosition = new Vector2(0,0);

    private void Update()
    {
        if(!_phaseType)
        {
            BubbleSpawn(_spawnPosition);
        }
        else
        {
            BubbleUp(_finishPos);
        }
    }
    private void Start()
    {
        _transform = gameObject.GetComponent<Transform>();
        _bubbleCollider = gameObject.GetComponent<Collider2D>();
        _animator = gameObject.GetComponent<Animator>();
    }
    public void CollisionEnabler()
    {
        DestroyInsideEnemy();
        _animator.SetTrigger("TriggerWithPlayer");
        _bubbleCollider.enabled = false;
    }
    public void CanDestroy()
    {
        Destroy(gameObject);
    }
    public void BubbleSpawn(Vector2 posA)
    {
        
        if(_timeCounter >= _time)
        {
            _finishPos = _transform.position;
            _phaseType = true;
            return;
        }

        var normalizedTime = _timeCounter / _time;

        _transform.position = Vector2.Lerp(posA, new Vector2(posA.x + _bubbleMoveDistance * _moveDirection, posA.y), normalizedTime);
        _timeCounter += Time.deltaTime;
    }
    public void BubbleUp(Vector2 posA)
    {
        if (_timeCounterForVertical >= _timeForVertical)
        {
            DestroyInsideEnemy();
            CollisionEnabler();
        }

        var normalizedTime = _timeCounterForVertical/ _timeForVertical;
        _transform.position = Vector2.Lerp(posA, new Vector2(posA.x, posA.y + _bubbleMoveVerticalDistance), normalizedTime);
        _timeCounterForVertical += Time.deltaTime;

    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyForBubble"))
        {
            if (!_phaseType && !_MobInside)
            {
                _phaseType = true;
                _finishPos = collision.gameObject.transform.position;
            }
            if (collision.gameObject.name == "SlimeMob" && !_MobInside)
            {
                _MobInside = true;
                collision.gameObject.transform.SetParent(gameObject.transform);
                collision.transform.localPosition = new Vector3(0, 0.25f, 0);
                collision.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                collision.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                collision.gameObject.transform.localScale = new Vector3(1.792735f, 1.792735f, 1.792735f);
                collision.gameObject.GetComponent<SlimeMover>().enabled = false;
            }
            if (collision.gameObject.name == "FlowerMob" && !_MobInside)
            {
                _MobInside = true;
                collision.gameObject.transform.SetParent(gameObject.transform);
                collision.transform.localPosition = new Vector3(0.18f, 0.2f, 0);
                collision.gameObject.transform.localScale = new Vector3(-6.281425f, 6.281425f, -6.281425f);
                collision.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                collision.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
            if (collision.gameObject.name == "TurtleMob" && !_MobInside)
            {
                _MobInside = true;
                collision.gameObject.transform.SetParent(gameObject.transform);
                collision.transform.localPosition = new Vector3(0, 0.24f, 0);
                collision.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                collision.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                collision.gameObject.transform.localScale = new Vector3(6.024634f, 6.024634f, 6.024634f);
                collision.gameObject.GetComponent<SlimeMover>().enabled = false;
            }

        }
    }
    private void DestroyInsideEnemy()
    {
        if (gameObject.transform.childCount > 0)
        {
            Destroy(gameObject.transform.GetChild(0).gameObject);
        }
    }
    private void ChangeTagToBubble()
    {
        gameObject.tag = "Bubble";
    }

}

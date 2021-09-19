using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("COMPONENTS")]
    Rigidbody2D _rb2D;
    Transform _transform;

    [Header("INPUT BUTTONS")]
    [SerializeField] private KeyCode _rightButton;
    [SerializeField] private KeyCode _leftButton;
    [SerializeField] private KeyCode _upButton;
    [SerializeField] private KeyCode _downButton;

    [Header("MOVMENT VARIABLE")]
    [SerializeField] private float _movmentAcceleration;
    [SerializeField] private float _maxMoveSpeed;
    [SerializeField] private float _linearDrug;
    private float _horizontalDirection;

    void Start()
    {
        _rb2D = gameObject.GetComponent<Rigidbody2D>();
        _transform = gameObject.GetComponent<Transform>();
    }
    void Update()
    {
        _horizontalDirection = GetInputHorizontal().x; //определяем направление движения по "X"
    }
    void FixedUpdate()
    {
        PlayerHorizontalMovement();
        ApplyLinearDrug();
    }

    [ContextMenu("Input Axis")]
    private Vector3 GetInputHorizontal()
    {
        //Получаем -1 или 1 по "X" в зависимости от нажатых клавиш

        if (Input.GetKey(_leftButton) && Input.GetKey(_rightButton)) //Если нажаты 2 клавиши - стоим на месте
        {
            return new Vector3(0, _transform.position.y, 0);
        }
        if (Input.GetKey(_rightButton)) //Вектор движения вправо
        {
            //Debug.Log("Right" + _transform.right);
            return _transform.right;
        }
        if (Input.GetKey(_leftButton)) //Вектор движения влево
        {
            //Debug.Log("Left" + -_transform.right);
            return -_transform.right;
        }
        return new Vector3(0, 0, 0);
    } 
    private void PlayerHorizontalMovement()
    {
        //Двигает персонажа в FixedUpdatee

        _rb2D.AddForce(new Vector2(_horizontalDirection, 0) * _movmentAcceleration, ForceMode2D.Force);
        if(Mathf.Abs(_rb2D.velocity.x) > _maxMoveSpeed)
        {
            _rb2D.velocity = new Vector2(Mathf.Sign(_rb2D.velocity.x) * _maxMoveSpeed, _rb2D.velocity.y);
            //Debug.Log(Mathf.Sign(_rb2D.velocity.x));
        }
    }
    private void ApplyLinearDrug()
    {
        //Применяет линейное сопротивление к объекту после отпускания клавиши

        if (Mathf.Abs(_horizontalDirection) == 0)
        {
            _rb2D.drag = _linearDrug;
            //Debug.Log("Linear drug activated");
        }
        else
        {
            _rb2D.drag = 0f;
        }
    }
}

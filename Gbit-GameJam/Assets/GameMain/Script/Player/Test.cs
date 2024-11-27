using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private SpriteRenderer m_SpriteRenderer;
    private Rigidbody2D m_Rigidbody;
    private Vector3 m_MoveDirection;
    
    private float m_MoveSpeed = 10f;

    public void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        //m_SpriteRenderer = transform.Find("PlayerImage").GetComponent<SpriteRenderer>();
    }

    protected void Update()
    {

        GetMoveInput();


    }

    private void GetMoveInput()
    {
        float xDirection = Input.GetAxisRaw("Horizontal");
        float yDirection = Input.GetAxisRaw("Vertical");
        m_MoveDirection = new Vector2(xDirection, yDirection);
        m_MoveDirection = m_MoveDirection.normalized;
        m_Rigidbody.velocity = m_MoveDirection * m_MoveSpeed;
    }
}


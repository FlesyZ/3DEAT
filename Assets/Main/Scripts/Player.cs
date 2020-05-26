﻿using UnityEngine;

public class Player : MonoBehaviour
{
    #region preset
    [Header("移動速度"), Range(1, 1000)]
    public float PlayerSpeed = 10;
    [Header("跳躍高度"), Range(1, 5000)]
    public float JumpHeight;

    private float moveTime;

    /// <summary>
    /// 偵測是否站在地上
    /// </summary>
    private bool isGround
    {
        get
        {
            if (transform.position.y < 0.051f) return true;
            else return false;
        }
    }

    /// <summary>
    /// 移動時旋轉角度
    /// </summary>
    private Vector3 direction;

    private float rotation;

    private Animator A;
    private Rigidbody a;

    /// <summary>
    /// 偵測是否在跳
    /// </summary>
    private bool jump;
    #endregion

    #region method
    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        float V = Input.GetAxisRaw("Vertical");
        float H = Input.GetAxisRaw("Horizontal");

        #region move
        A.SetBool("Back", V == -1);
        A.SetBool("Left", H == -1);
        A.SetBool("Right", H == 1);
        A.SetBool("Walking", Mathf.Abs(V) > 0);

        if (A.GetBool("Walking")) moveTime += Time.deltaTime;
        else moveTime = 0;
        A.SetBool("Running", V == 1 && moveTime > 5f);

        if (A.GetBool("Back"))
        {
            if (Mathf.Abs(V) > 0)
                a.AddForce(transform.forward * PlayerSpeed * Mathf.Abs(V) * -0.8f);
            else if (V == -1 && Mathf.Abs(H) > 0)
                a.AddForce(transform.forward * PlayerSpeed * Mathf.Abs(H) * -0.8f);
        }
        else if (A.GetBool("Running"))
        {
            if (Mathf.Abs(V) > 0)
                a.AddForce(transform.forward * PlayerSpeed * Mathf.Abs(V) * 1.3f);
            else if (V == 1 && Mathf.Abs(H) > 0)
                a.AddForce(transform.forward * PlayerSpeed * Mathf.Abs(H) * 1.3f);
        }
        else
        {
            if (Mathf.Abs(V) > 0)
                a.AddForce(transform.forward * PlayerSpeed * Mathf.Abs(V));
            else if (V == 1 && Mathf.Abs(H) > 0)
                a.AddForce(transform.forward * PlayerSpeed * Mathf.Abs(H));
        }
        #endregion

        #region turn
        if (A.GetBool("Right")) rotation += 1;
        else if (A.GetBool("Left")) rotation -= 1;

        direction = new Vector3(0, rotation, 0);
        transform.eulerAngles = direction;
        #endregion
    }

    /// <summary>
    /// 跳躍
    /// </summary>
    private void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {

        }
        
        if (isGround && Input.GetButtonDown("Jump"))
        {
            A.SetTrigger("Jump");
            jump = true;
        }
        if (jump)
        { 
            Invoke("DoJump", 0.3f);
            jump = false;
        }
    }

    private void DoJump()
    {
        a.AddForce(0, JumpHeight, 0);
    }

    /// <summary>
    /// 吃道具
    /// </summary>
    private void Eat()
    {

    }
    #endregion

    #region events
    private void Start()
    {
        a = GetComponent<Rigidbody>();
        A = GetComponent<Animator>();
    }

    // FPS = 50, Must in this event for any physics
    private void FixedUpdate()
    {
        Move();
    }

    // FPS = 60
    private void Update()
    {
        Jump();
    }
    #endregion

}

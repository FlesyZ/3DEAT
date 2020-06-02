using UnityEngine;

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
    private AudioSource[] SE;

    public GameManager GM;

    /// <summary>
    /// 偵測是否在跳
    /// </summary>
    private bool jump;

    [Header("道具音效")]
    public AudioClip good;
    public AudioClip bad;

    [Header("人物音效")]
    public AudioClip run;
    public AudioClip win;
    public AudioClip lose;
    public AudioClip[] jumpSE;
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

        if (A.GetBool("Walking")) 
        {
            if (Input.GetButtonDown("Fire1"))
                moveTime = 7f;
            else
                moveTime += Time.deltaTime;
        }
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

        if (A.GetBool("Running"))
        {
            if (!SE[0].isPlaying && isGround) SE[0].Play();
            else if (!isGround && SE[0].isPlaying) SE[0].Pause();
        }
        else SE[0].Stop();
        #endregion

        #region turn
        if (A.GetBool("Running"))
        {
            if (A.GetBool("Right")) rotation += 1;
            else if (A.GetBool("Left")) rotation -= 1;
        }
        else if (A.GetBool("Walking"))
        {
            if (A.GetBool("Right")) rotation += 3;
            else if (A.GetBool("Left")) rotation -= 3;
        }
        else if (V == 0)
        {
            if (A.GetBool("Right")) rotation += 5;
            else if (A.GetBool("Left")) rotation -= 5;
        }

        direction = new Vector3(0, rotation, 0);
        transform.eulerAngles = direction;
        #endregion
    }

    /// <summary>
    /// 跳躍
    /// </summary>
    private void Jump()
    {
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
        SE[1].PlayOneShot(jumpSE[Random.Range(0, 2)], 1f);
    }

    /// <summary>
    /// 吃道具
    /// </summary>
    private void Eat(GameObject prop)
    {
        switch (prop.tag)
        {
            case "good":
                SE[1].PlayOneShot(good, 1);
                break;
            case "bad":
                SE[1].PlayOneShot(bad, 1);
                break;
        }
        GM.Eat(prop.tag);
        Destroy(prop);
    }

    public void Win()
    {
        SE[1].PlayOneShot(win, 1f);
        A.SetTrigger("Win");
    }
    public void Lose()
    {
        SE[1].PlayOneShot(lose, 1f);
        A.SetTrigger("TimeUp");
    }
    #endregion

    #region events
    private void Start()
    {
        a = GetComponent<Rigidbody>();
        A = GetComponent<Animator>();
        SE = GetComponents<AudioSource>();
        GM = FindObjectOfType<GameManager>();
    }

    // FPS = 50, Must in this event for any physics
    private void FixedUpdate()
    {
        if (!(A.GetBool("TimeUp") || A.GetBool("Win")))
            Move();
    }

    // FPS = 60
    private void Update()
    {
        if (!(A.GetBool("TimeUp") || A.GetBool("Win")))
            Jump();
    }

    private void OnTriggerEnter(Collider other)
    {
        Eat(other.gameObject);
    }
    #endregion

}

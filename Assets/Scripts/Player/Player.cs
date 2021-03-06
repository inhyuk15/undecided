using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// animation, input 처리
public class Player : MonoBehaviour
{
    static public Player instance;

    Animator m_animator;
    Rigidbody2D m_rigidBody2D;
    BoxCollider2D m_collider2D;

    [SerializeField]
    Player_Controller m_playerController; 

    [SerializeField]
    private float speedX, speedY;
    private float climbVelocity;

    [SerializeField]
    public AudioClip hurtSound;


    private bool crouch, move, jump, climb;


    private int m_curLife;
    public int CurLife
    {
        get
        {
            return m_curLife;
        }
        set
        {
            m_curLife = value;
        }
    }

    [SerializeField]
    private Vector2 velocity;

    [SerializeField] public AudioClip m_JumpSound;

    private void OnEnable()
    {
        m_animator = GetComponent<Animator>();
        m_rigidBody2D= GetComponent<Rigidbody2D>();
        m_collider2D = GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;

            m_curLife = ScoreManager.CurLife;
            
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        getInput();
    }

    void getInput()
    {
        speedX = SimpleInput.GetAxis("Horizontal");
        //move = SimpleInput.GetKey(KeyCode.LeftArrow) || SimpleInput.GetKey(KeyCode.RightArrow);
        move = (speedX != 0) ? true : false;
        if (SimpleInput.GetAxis("Vertical") > 0) {
            jump = true;
        }

        //crouch = SimpleInput.GetKey(KeyCode.DownArrow);
        crouch = (SimpleInput.GetAxis("Vertical") < 0);

        m_animator.SetBool("move", move);
        m_animator.SetFloat("speedX", speedX);

        m_animator.SetBool("jump", jump);


        // 사다리 위에 있을 때 키를 누른다면 사다리를 타는거
        if (m_playerController.m_OnLaddered)
        {
            climbVelocity = 0f;
            if (SimpleInput.GetAxis("Vertical") > 0)
            {
                climbVelocity = 1f;
            }
            else if(SimpleInput.GetAxis("Vertical") < 0)
            {
                climbVelocity = -1f;
            }
            
            
            if (climbVelocity != 0)
            {
                climb = true;
                m_animator.SetFloat("climbSpeed", 1f);
            }
            else
            {
                m_animator.SetFloat("climbSpeed", 0f);
            }
        }
        else
        {
            climb = false;
            climbVelocity = 0f;
            m_animator.SetFloat("climbSpeed", 1f);
        }
    }

  

    private void FixedUpdate()
    {
        m_playerController.Movement(speedX, jump, crouch, climb, climbVelocity);

        m_animator.SetFloat("speedY", m_rigidBody2D.velocity.y);
    }


    public void OnLand()
    {
        jump = false;
        jumping = false;
        m_animator.SetBool("onGround", true);
    }

    public void OnCrouch(bool _crouch)
    {
        m_animator.SetBool("crouch", _crouch);
    }

    public bool jumping = false;

    public void OnJump()
    {
        m_animator.SetBool("onGround", false);
        m_animator.SetBool("jump", true);

        if (Settings.canSound && !jumping)
        {
            jumping = true;
            AudioSource.PlayClipAtPoint(m_JumpSound, transform.position, Settings.volume);
            //StartCoroutine(WaitAndJump());
        }
    }
    IEnumerator WaitAndJump()
    {
        
        yield return new WaitForSeconds(0.5f);
    }

    public void OnClimb(bool _climb)
    {
        m_animator.SetBool("climb", _climb);
    }

    public void OnLadder(bool _OnLadder)
    {
        m_animator.SetBool("onLadder", _OnLadder);
    }

    bool hurted = false;
    public void SetDamage(int damage)
    {
        m_curLife -= damage;
        ScoreManager.CurLife = m_curLife;

        if (Settings.canSound && !hurted)
        {
            StartCoroutine(WaitAndHurt());
        }

        if (m_curLife <= 0)
        {
            GameManager.instance.GameOver(true);
        }


        m_animator.SetTrigger("hurt");

    }
    IEnumerator WaitAndHurt()
    {
        hurted = true;
        AudioSource.PlayClipAtPoint(hurtSound, transform.position, Settings.volume);
        yield return new WaitForSeconds(0.5f);
        hurted = false;
    }

}

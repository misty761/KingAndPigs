using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // speed
    public float speed = 1f;
    // looking direction
    public bool isLookingRight;
    // idle
    public bool isIdle;
    // anomator
    public Animator animator;
    // rigidbody
    public Rigidbody2D mRigidbody;
    // jump
    public int forceJump = 150;
    public int countJump;
    // ground
    public bool isGround;
    // falling
    bool isFalling;
    // particles
    public GameObject pfParticlesRun;
    public float delayParticles = 0.7f;
    public float offsetParticlesRunX = 0.1f;
    public float offsetParticlesRunY = -0.23f;
    float timeParticles;
    public GameObject pfParticlesJump;
    public float offsetParticlesJumpX = -0.1f;
    public float offsetParticlesJumpY = -0.1f;
    public GameObject pfParticlesFall;
    public float offsetParticlesFallX = -0.15f;
    public float offsetParticlesFallY = -0.15f;
    // life
    public int life;
    public int lifeMax = 3;
    PlayerLife playerLife;
    // attack
    public float power = 0.5f;
    PlayerAttack attack;
    // sound
    public AudioClip audioDamaged;
    public AudioClip audioDie;
    public AudioClip audioJump;
    // joystick
    float h;
    float v;
    public bool isJoystickdown;
    public bool isJoystickUp;
    Joystick joystick;
    // button
    ButtonJump buttonJump;
    // damage delay
    float timeDamage;
    public float delayDamage = 1f;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        attack = GetComponent<PlayerAttack>();
    }

    // Start is called before the first frame update
    void Start()
    {
        joystick = FindObjectOfType<Joystick>();
        mRigidbody = GetComponent<Rigidbody2D>();
        buttonJump = FindObjectOfType<ButtonJump>();
        playerLife = FindObjectOfType<PlayerLife>();
        
        Init();
    }

    public void Init()
    {
        life = lifeMax;
        playerLife.UpdatePlayerLife();
        isLookingRight = true;
        isIdle = true;
        isGround = false;
        isFalling = true;
        timeParticles = 0f;
        isJoystickdown = false;
        countJump = 0;
        timeDamage = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // delay damage
        timeDamage += Time.deltaTime;

        // sprite(change sprite depanding on looking direction)
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (isLookingRight)
            {
                transform.localScale = new Vector2(1, 1);
            }
            else
            {
                transform.localScale = new Vector2(-1, 1);
            }
        }

        // falling
        if (!isGround)
        {
            if (mRigidbody.velocity.y > 0)
            {
                isFalling = false;
            }
            else
            {
                isFalling = true;
            }
        }
        else
        {
            isFalling = false;
        }

        // animator
        animator.SetBool("Idle", isIdle);
        animator.SetBool("Ground", isGround);
        animator.SetBool("Fall", isFalling);

        // return
        if (life <= 0
            || animator.GetCurrentAnimatorStateInfo(0).IsName("DoorOut")
            || animator.GetCurrentAnimatorStateInfo(0).IsName("DoorIn")
            || GameManager.instance.state != GameManager.State.Play) return;

        // joystick
        if (joystick == null)
        {
            joystick = FindObjectOfType<Joystick>();
        }
        else
        {
            float factorJoystick = 2f;
            h = joystick.Horizontal * factorJoystick;
            v = joystick.Vertical * factorJoystick;
            if (h > speed) h = speed;
            else if (h < -speed) h = -speed;
            if (v > speed) v = speed;
            else if (v < -speed) v = -speed;
            //print("joystick h : " + h);
            //print("joystick v : " + v);
            if (v < -speed + 0.01f) isJoystickdown = true;
            else isJoystickdown = false;
            if (v > speed - 0.01f) isJoystickUp = true;
            else isJoystickUp = false;
        }

        // jump
        if (buttonJump == null)
        {
            buttonJump = FindObjectOfType<ButtonJump>();
        }
        else
        {
            if (countJump == 0)
            {
                if (Input.GetKeyDown(KeyCode.Space) || buttonJump.isTouchDown)
                {
                    countJump++;

                    // button jump
                    buttonJump.isTouchDown = false;
                    // sound
                    SoundManager.instance.PlaySound(audioJump, transform.position, 1f);
                    // particles
                    timeParticles = 0f;
                    MakeParticles(pfParticlesJump, offsetParticlesJumpX, offsetParticlesJumpY);
                    // velocity y sets to 0
                    mRigidbody.velocity = new Vector2(mRigidbody.velocity.x, 0);
                    // jump
                    mRigidbody.AddForce(new Vector2(0, forceJump));
                    isGround = false;
                }
            }
            else
            {
                // jump(점프키를 빨리 릴리즈하면 점프를 조금만 하도록 함)
                if (Input.GetKeyUp(KeyCode.Space) || buttonJump.isTouchUp)
                {
                    // button jump
                    buttonJump.isTouchUp = false;
                    // player is jumping up
                    if (mRigidbody.velocity.y > 0f)
                    {
                        // fall
                        mRigidbody.velocity = new Vector2(mRigidbody.velocity.x, 0);
                    }

                }

            }
        }

        // input(A & D 동시 입력시)
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            isIdle = true;
        }
        // move left
        else if (Input.GetKey(KeyCode.A) || h < -0.2f)
        {
            // keyboard
            if (Input.GetKey(KeyCode.A))
            {
                // move
                transform.Translate(Vector2.left * Time.deltaTime * speed);
            }
            // joystick
            else
            {
                // move
                transform.Translate(Vector2.right * Time.deltaTime * speed * h);
            }

            isLookingRight = false;
            isIdle = false;

            // particle
            MakeParticles(pfParticlesRun, offsetParticlesRunX, offsetParticlesRunY);

        }
        // move right
        else if (Input.GetKey(KeyCode.D) || h > 0.2f)
        {
            // keyboard
            if (Input.GetKey(KeyCode.D))
            {
                // move
                transform.Translate(Vector2.right * Time.deltaTime * speed);
            }
            // joystick
            else
            {
                // move
                transform.Translate(Vector2.right * Time.deltaTime * speed * h);
            }

            isLookingRight = true;
            isIdle = false;

            // particle
            MakeParticles(pfParticlesRun, -offsetParticlesRunX, offsetParticlesRunY);
        }
        else
        {
            isIdle = true;
        }    
    }

    /*
    public void ReturnMethod()
    {
        if (life <= 0
            || animator.GetCurrentAnimatorStateInfo(0).IsName("DoorOut")
            || animator.GetCurrentAnimatorStateInfo(0).IsName("DoorIn")
            || GameManager.instance.state != GameManager.State.Play) return;
    }
    */

    public Vector2 DistPlayer(Vector2 pos)
    {
        float distX = transform.position.x - pos.x;
        float distY = transform.position.y - pos.y;
        return new Vector2(distX, distY);
    }

    void MakeParticles(GameObject particles, float offsetX, float offsetY)
    {
        Vector2 pos = new Vector2(transform.transform.position.x + offsetX, transform.position.y + offsetY);

        // sprite(캐릭터 방향에따라 스프라이트 회전)
        if (isLookingRight)
        {
            particles.transform.localScale = new Vector2(1, 1);
        }
        else
        {
            particles.transform.localScale = new Vector2(-1, 1);
        }

        // particles
        if (timeParticles == 0f && isGround)
        {
            Instantiate(particles, pos, Quaternion.Euler(Vector2.zero));
        }
        // paticles delay time
        if (timeParticles < delayParticles)
        {
            timeParticles += Time.deltaTime;
        }
        else
        {
            timeParticles = 0f;
        }

    }

    public void GetDamage(float _damage)
    {
        // damage
        int damage;
        if (_damage < 0.5f)
        {
            damage = 1;
        }
        else
        {
            damage = 2;
        }

        // life --
        GetDamage(damage);
    }

    public void GetDamage(int damage)
    {
        // delay damage
        if (timeDamage > delayDamage)
        {
            timeDamage = 0f;
            // return
            if (GameManager.instance.state == GameManager.State.GameOver) return;
            // life --
            life -= damage;
            if (life < 0) life = 0;
            playerLife.UpdatePlayerLife();
            // animator
            animator.SetInteger("Life", life);
            animator.SetTrigger("Damaged");
            // disable attack
            attack.FinishAttack();

            if (life > 0)
            {
                // sound
                SoundManager.instance.PlaySound(audioDamaged, transform.position, 1f);
            }
            // die
            else
            {
                // sound
                SoundManager.instance.PlaySound(audioDie, transform.position, 1f);
                // game over
                GameManager.instance.GameOver();
            }
        }
    }

    public void AddLife()
    {
        if (life > 0)
        {
            if (life >= lifeMax)
            {
                life = lifeMax;
                GameManager.instance.Scored(10);
            }
            else
            {
                // sound
                SoundManager.instance.PlaySound(SoundManager.instance.audioLifeUp, transform.position, 0.1f);

                life++;
            }
            
            // life bar
            playerLife.UpdatePlayerLife();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Wall"))
        {
            // sound
            SoundManager.instance.PlaySound(SoundManager.instance.audioThud, transform.position, SoundManager.instance.volumeThud);

            isGround = true;
            countJump = 0;

            // player가 위에서 충돌
            if (collision.contacts[0].point.normalized.y < 0f)
            {
                // particles
                timeParticles = 0f;
                MakeParticles(pfParticlesFall, offsetParticlesFallX, offsetParticlesFallY);
            }
        }
    }

    
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Wall"))
        {
            isGround = true;
            countJump = 0;
        }
    }
    

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGround = false;
            if (mRigidbody.velocity.y > 0f) countJump++;
        }
    }
    public void DoorOut()
    {
        animator.SetTrigger("DoorOut");
        attack.FinishAttack();
    }

    public void DoorIn()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("DoorIn"))
        animator.SetTrigger("DoorIn");
        attack.FinishAttack();
    }


}

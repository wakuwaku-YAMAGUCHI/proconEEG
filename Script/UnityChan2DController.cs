using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun; 
using Photon.Realtime; 
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class UnityChan2DController : MonoBehaviourPunCallbacks, IPunObservable,IDdamage
{
    public float maxSpeed = 10f;
    public float jumpPower = 1000f;
    public Vector2 backwardForce = new Vector2(-4.5f, 5.4f);

    public LayerMask whatIsGround;

    private Animator m_animator;
    private BoxCollider2D m_boxcollier2D;
    private Rigidbody2D m_rigidbody2D;
    private bool m_isGround;
    private const float m_centerY = 1.5f;

    private State m_state = State.Normal;

  
    public GameObject gameoverText;
    public GameObject MahoPrefab;
    public AudioClip sound1;
    AudioSource audioSource;
     
     public AudioClip Hit;


     public PhotonView myView;


     public float CounterGage=90f;
     private float Countertime=2.0f;
     [SerializeField]
   
    private float Gagetime;
    private float cooltime;
      

            
   [SerializeField] private MagicDataBase magicDataBase;//データベース呼び出し
    Magic magic;        //保存用

    private int life=100;

    [SerializeField]
    private  Slider hpslider;
    private  float mp=0f;
    public static float LMP=0f;
    [SerializeField]
    private Slider mpslider;
    private float defense=1;

    //public PhotonTransformView myTransform;
    private Camera mainCam;
    
    
    
     private void Start() {
        life=100;
        if (myView.IsMine)    //自キャラであれば実行
        {
            magic =magicDataBase.magics[0];//魔法の種類
            MahoPrefab=magic.Hantei;

            //MainCameraのtargetにこのゲームオブジェクトを設定
            mainCam = Camera.main;  
            mainCam.GetComponent<Ca>().target = this.gameObject.transform;
            StartGage.flagc=true;
        }
    
    }

   

    void Reset()
    {
        Awake();

        // UnityChan2DController
        backwardForce = new Vector2(-4.5f, 5.4f);
        whatIsGround = 1 << LayerMask.NameToLayer("Ground");

        // Transform
        transform.localScale = new Vector3(1, 1, 1);

        // Rigidbody2D
        m_rigidbody2D.gravityScale = 3.5f;
        //m_rigidbody2D.fixedAngle = true;

        // BoxCollider2D
        m_boxcollier2D.size = new Vector2(1, 2.5f);
        m_boxcollier2D.offset = new Vector2(0, -0.25f);

        // Animator
        m_animator.applyRootMotion = false;
    }

    void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_boxcollier2D = GetComponent<BoxCollider2D>();
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    

   
    private void Update()
    {
        var gamepad = Gamepad.current;
        cooltime+=Time.deltaTime;
        //Debug.Log(gamepad);
               

         if (myView.IsMine)
        {
            if (m_state != State.Damaged)
            {
                
                float x = Input.GetAxis("Horizontal");
                bool jump = Input.GetButtonDown("Jump");
                hpslider.value=(float)life;

                Move(x, jump);

            }


            LMP=mp;
            if (Input.GetMouseButtonDown(1)&&magic.lostmp<=mp&&Shot.caun==false){ 
                mp=mp-magic.lostmp;
                m_animator.SetBool("Attack", true);
            }

            if (Input.GetKeyDown ("joystick button 1")&&magic.lostmp<=mp&&Gamepad.current == null&&Shot.caun==false){ 
                mp=mp-magic.lostmp;
                m_animator.SetBool("Attack", true);
            }
            
            if (Input.GetKeyDown ("joystick button 2")&&magic.lostmp<=mp&&Gamepad.current == null&&Shot.caun==false){ 
                mp=mp-magic.lostmp;
                m_animator.SetBool("Attack", true);
            }

            Shot.Mylife=life;

            if(cooltime>=0.5f){
                MPInc();
                
                if (myView.IsMine) {
                    if(life<=100f){
                        life+=testkansu.life;
                    }
                    cooltime=0;
                }
            }

            //Debug.Log(CounterGage);

            if (Input.GetKey(KeyCode.E)&&CounterGage>=90f){
                

                    Countertime=2.0f;
                    CounterGage=0.0f;
                    Shot.caun=true;
                    
                
            }

            if (Input.GetKeyDown ("joystick button 0")&&CounterGage>=90f&&Gamepad.current == null){
                

                    Countertime=2.0f;
                    CounterGage=0.0f;
                    Shot.caun=true;
                    
                
            }

           

            if(Shot.caun==true&&Countertime>=0f){
                Countertime-=Time.deltaTime;
            }
            if(Shot.caun==true&&0f>=Countertime){
               Shot.caun=false;
            }

            if(Shot.caun==false){
                if(90f>CounterGage&&CounterGage>=0.0f){
                    Gagetime+=Time.deltaTime;
                    if(1.0f<=Gagetime){
                        Gagetime=0.0f;
                        CounterGage+=3.0f;
                    }
                }
            }

        }
           
        hpslider.value=life;
        mpslider.value=mp;
        CASlider.Gage=CounterGage;
        if (Gamepad.current == null) 
            return;            

        if (Gamepad.current.aButton.wasPressedThisFrame&&magic.lostmp<=mp) 
                mp=mp-magic.lostmp;
        
         if (CounterGage>=90f&&Gamepad.current.xButton.wasPressedThisFrame){
                
                    Countertime=2.0f;
                    CounterGage=0.0f;
                    Shot.caun=true;   
                
        }
    }

    void Move(float move, bool jump)
    {
        
        if (Mathf.Abs(move) > 0)
        {
            Quaternion rot = transform.rotation;
            transform.rotation = Quaternion.Euler(rot.x, Mathf.Sign(move) == 1 ? 0 : 180, rot.z);
        }

        m_rigidbody2D.velocity = new Vector2(move * maxSpeed, m_rigidbody2D.velocity.y);
        

         float speed=move;
         if(speed<=-0.001)
            speed=speed*-1;
            

        m_animator.SetFloat("Horizontal", speed);
        m_animator.SetFloat("Vertical", m_rigidbody2D.velocity.y);
        m_animator.SetBool("isGround", m_isGround);

        if (jump && m_isGround)
        {
            m_animator.SetTrigger("Jump");
            SendMessage("Jump", SendMessageOptions.DontRequireReceiver);
            m_rigidbody2D.AddForce(Vector2.up * jumpPower);
            audioSource.PlayOneShot(sound1);
            
        }
    }

    void FixedUpdate()
    {
        Vector2 pos = transform.position;
        Vector2 groundCheck = new Vector2(pos.x, pos.y - (m_centerY * transform.localScale.y));
        Vector2 groundArea = new Vector2(m_boxcollier2D.size.x * 0.49f, 0.05f);

        m_isGround = Physics2D.OverlapArea(groundCheck + groundArea, groundCheck - groundArea, whatIsGround);
        m_animator.SetBool("isGround", m_isGround);
    }

    // void OnTriggerStay2D(Collider2D other)
    // {
    //     if (other.tag == "DamageObject" && m_state == State.Normal)
    //     {
    //         m_state = State.Damaged;
    //         StartCoroutine(INTERNAL_OnDamage());
    //     }
    // }

    IEnumerator INTERNAL_OnDamage()
    {
        m_animator.Play(m_isGround ? "Damage" : "AirDamage");
        m_animator.Play("Idle");

        SendMessage("OnDamage", SendMessageOptions.DontRequireReceiver);

        m_rigidbody2D.velocity = new Vector2(transform.right.x * backwardForce.x, transform.up.y * backwardForce.y);

        yield return new WaitForSeconds(.2f);

        while (m_isGround == false)
        {
            yield return new WaitForFixedUpdate();
        }
        m_animator.SetTrigger("Invincible Mode");
        m_state = State.Invincible;
    }

    void MPInc(){//MPを増加するプログラム
        if (myView.IsMine) {
            if(mp<=100f){
               mp+=testkansu.mp;
               
            }
        }
        
    }

     // ダメージ判定
    public void Damagea(float power){
        life=life-(int)(power*(1.0f-defense*0.01));
        Debug.Log(life);
    }

    enum State
    {
        Normal,
        Damaged,
        Invincible,
    }

    void FinA(){
         m_animator.SetBool("Attack", false);
    }

   


    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            // 自身のHP,MPを送信する
            stream.SendNext(mp);
            stream.SendNext(life);
        } else {
            // 他プレイヤーのHP,MPを受信する
            mp = (float)stream.ReceiveNext();
            life = (int)stream.ReceiveNext();
        }
    }
}



 public interface IDdamage
{
       public void Damagea(float power);
}




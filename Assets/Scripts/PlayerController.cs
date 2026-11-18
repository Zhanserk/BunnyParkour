using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;



public class PlayerController : MonoBehaviour

{
    [SerializeField] PlayerStats stats;   // Assignment 11: скорость, прыжок и высота падения теперь в ассете PlayerStats

    [SerializeField] LevelManager levelManager;   // Assignment 11: сообщаем ему, что уровень пройден

    bool _goalSet;   // Assignment 11: цель по морковкам уже задал LevelManager?

    public AudioClip jumpSound;

    public AudioClip coinSound;

    private AudioSource _audio;

    private Health _health;

    private Animator _anim;

    









      

    private bool _isGround;

    public TMP_Text carrottext;


    private int _carrots;

    private int _totalcarrot;





    private Rigidbody _rd;

    private Vector2 _move;


     void Awake()
    {
        _rd = GetComponent<Rigidbody>();

        _health = GetComponent<Health>();
        _audio = GetComponent<AudioSource>();
        _anim = GetComponentInChildren<Animator>();




        
    }

    void Start()
    {
       if(!_goalSet)   // Assignment 11: если цель не задал LevelManager, считаем морковки в сцене, как раньше
       {
           _totalcarrot =  GameObject.FindGameObjectsWithTag("CollectiveCoin").Length;
       }
        ShowCarrots();


    }

    // Assignment 11: LevelManager задаёт, сколько морковок нужно собрать на этом уровне
    public void SetCarrotGoal(int goal)
    {
        _totalcarrot = goal;
        _goalSet = true;
    }

    void FixedUpdate()
    {
        _rd.linearVelocity = (new Vector3 (_move.x  * stats.moveSpeed, _rd.linearVelocity.y , _move.y * stats.moveSpeed));   // Assignment 11: speed -> stats.moveSpeed
        if(_move.magnitude > 0.1f)
        {
           transform.rotation = Quaternion.LookRotation(new Vector3(_move.x ,  0 , _move.y));
        }
        
    }

    void Update()
    {
        if(transform.position.y <= stats.fallDeathY)   // Assignment 11: было -3
        {
            _health.Die();

        }

        _anim.SetFloat("Speed" , _move.magnitude);
        _anim.SetBool("Grounded" , _isGround);
    }

    public void OnMove(InputValue value)
    {
        _move = value.Get<Vector2>();

    }

    public void OnJump()
    {
        if(!_isGround)
        {
            return;

        }

        _rd.linearVelocity = new Vector3(_rd.linearVelocity.x , stats.jumpSpeed , _rd.linearVelocity.z  );   // Assignment 11: jupmspeed -> stats.jumpSpeed
        _audio.PlayOneShot(jumpSound);


    }

     void OnCollisionStay(Collision other) {
        if(other.gameObject.tag == "Ground")
        {
            _isGround = true;
        }   
    }





    void OnCollisionExit(Collision collision)
    {
       if(collision.gameObject.tag == "Ground")
        {
            _isGround = false;

        } 
    }


    void ShowCarrots()
    {
        carrottext.text = "Carrots: " + _carrots + " / " + _totalcarrot;

    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "CollectiveCoin")
        {
            _carrots++;
            Destroy(other.gameObject);
            _audio.PlayOneShot(coinSound);

            ShowCarrots();


        }
        if(other.gameObject.tag == "Hazard")
        {
            _health.Hurt();
        }

        if(other.gameObject.tag == "Heart")

        {      
            

            Destroy(other.gameObject);

            _audio.PlayOneShot(coinSound);


            _health.Heal();

        }

        if(other.gameObject.tag == "Exit" && _carrots >= _totalcarrot)
        {
            if(levelManager != null) levelManager.Complete();   // Assignment 11: сначала сохраняем рекорд и открываем следующий уровень
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);


        }
        
    }
}
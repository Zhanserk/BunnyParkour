using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;



public class PlayerController : MonoBehaviour

{
    public float speed = 6f;

    public float jupmspeed = 8f;

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
       _totalcarrot =  GameObject.FindGameObjectsWithTag("CollectiveCoin").Length;
        ShowCarrots();


    }

    void FixedUpdate()
    {
        _rd.linearVelocity = (new Vector3 (_move.x  * speed, _rd.linearVelocity.y , _move.y * speed));
        if(_move.magnitude > 0.1f)
        {
           transform.rotation = Quaternion.LookRotation(new Vector3(_move.x ,  0 , _move.y));
        }
        
    }

    void Update()
    {
        if(transform.position.y <= -3)
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

        _rd.linearVelocity = new Vector3(_rd.linearVelocity.x , jupmspeed , _rd.linearVelocity.z  );
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);


        }
        
    }
}
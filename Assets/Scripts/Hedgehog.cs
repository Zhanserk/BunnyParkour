using TMPro;
using UnityEngine;

public class Hedgehog : MonoBehaviour
{

    enum State {Idle , Aim , Fire , Cooldown};
    [SerializeField] Transform player;

    [SerializeField] Transform muzzle;

    [SerializeField] GameObject needlePrefab;

    [SerializeField] GameObject aimLazer;
    [SerializeField] TMP_Text stateLabel;
    
    [SerializeField] float veiwRange = 10f;
    [SerializeField] float fov = 120f;
    [SerializeField] float aimTime = 0.8f;
    [SerializeField] float cooldown = 1f;
    [SerializeField] float turnSpeed = 10f;

    State state;
    float timer;
    Vector3 startForward;

    void Start()
    {
      startForward = transform.forward;
      SetState(State.Idle);
      
    }
    void SetState(State next)
    {
        state = next;
        timer = 0f;
        stateLabel.text = next.ToString();
        aimLazer.SetActive(next == State.Aim);
        
        
    }

    bool CanSeePlayer()
    {

        Vector3 toPlayer = player.position - transform.position;
        if(toPlayer.magnitude > veiwRange)
        {
            return false;

        }

        if(Vector3.Angle(startForward , toPlayer) > fov / 2f)
        {
            return false;

        }


        return true;
    }
    void TurnToPlay()
    {
        Vector3 dir = player.position - transform.position;

        dir.y = 0f;
        if(dir == Vector3.zero)
        {
            return;
        }
        Quaternion look = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation , look , turnSpeed * Time.deltaTime);

    }

    void Update()
    {

        timer += Time.deltaTime;


        switch (state)
        {
            case State.Idle:
                if (CanSeePlayer())
                {
                    SetState(State.Aim);
                }
                
                
            break;

            case State.Aim:
                TurnToPlay();
                if(!CanSeePlayer())
                {
                 SetState(State.Idle);
                    
                }else if(timer >= aimTime){
                    SetState(State.Fire);
                }
            break;
            
            case State.Fire:
                Instantiate(needlePrefab , muzzle.position , muzzle.rotation);
                SetState(State.Cooldown);
                break;

            case State.Cooldown:
                if(timer >= cooldown)
                {
                    SetState(State.Idle);
                }    
                break;

        }
    }

    void LateUpdate()
    {
        stateLabel.transform.rotation = Camera.main.transform.rotation;
        
    }






}

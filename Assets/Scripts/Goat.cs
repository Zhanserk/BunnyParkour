using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Goat : MonoBehaviour
{

    enum State {Idle , WindUp , Dash , Stunned , Return };

    [SerializeField] Transform player;
    [SerializeField] GameObject dashLane;
    [SerializeField] TMP_Text stateLabel;


    [SerializeField] float triggerRange  = 6f;
    [SerializeField] float fov = 120f;

    [SerializeField] float warningup = 0.7f;

    [SerializeField] float dashSpeed = 10f;

    [SerializeField] float dashDistance = 8f;

    [SerializeField] float hitRange = 1.2f;

    [SerializeField] float stunTime = 1f;

    [SerializeField] float idleCooldown = 1.5f;

    [SerializeField] float turnSpeed = 6f;

    NavMeshAgent agent;
    State state;

    float timer;



    Vector3 startForward;

    Vector3 home;


    Vector3 dashDir;

    float dashed;
    
    bool hasHit;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        home = transform.position;
        startForward = transform.forward;
        SetState(State.Idle);
        
    }

    void Update()
    {
        timer += Time.deltaTime;

        switch (state)
        {
            case State.Idle:
                if(timer >= idleCooldown &&  CanSeePlayer())
                {
                    SetState(State.WindUp);
                }
                break;
            
            case State.WindUp:
                TurnToPlayer();

                if(timer > warningup)
                    {
                        dashDir = transform.forward;
                        SetState(State.Dash);

                    }

                    break;

            case State.Dash:
                float step = dashSpeed * Time.deltaTime;
                agent.Move(dashDir * step);
                dashed += step;
                if(!hasHit && Vector3.Distance(transform.position , player.position) < hitRange)
                {
                    player.GetComponent<Health>().Hurt();;
                    hasHit = true;


                }

                if(dashed >= dashDistance)
                {
                    SetState(State.Stunned);

                }
                break;

            case State.Stunned:
                if(timer >= stunTime)
                {
                    SetState(State.Return);

                }
                break;

            case State.Return:
                if(!agent.pathPending && agent.remainingDistance < 0.3f)
                {
                    SetState(State.Idle);
                }  

                break;




                
        }

    }

    void SetState(State next)
    {
        state = next;
        timer = 0f;
        stateLabel.text = next.ToString();
        dashLane.SetActive(next == State.WindUp);
        agent.isStopped = (next != State.Return);
        if(next == State.Dash)
        {
            dashed = 0f;
            hasHit = false;



        }
        if(next == State.Return)
        {
            agent.SetDestination(home);

        }





        
        
    }


    bool CanSeePlayer()
    {
        Vector3 toPlayer = player.position - transform.position;
        if(toPlayer.magnitude > triggerRange)
        {
            return false;

        }
        if(Vector3.Angle(startForward , toPlayer) > fov / 2f)
        {
            return false;
        }
        
        return true;


    }

    void TurnToPlayer()


    {

        
        Vector3 dir = player.position - transform.position;
        dir.y = 0f;
        Quaternion look = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation , look , turnSpeed * Time.deltaTime);
        Debug.Log("козёл: " + transform.position + "   заяц: " + player.position + "   стрелка: " + dir);


    }
    
    void LateUpdate()
    {
        stateLabel.transform.rotation  =  Camera.main.transform.rotation;
    }






}

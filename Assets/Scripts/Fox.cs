using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Fox : MonoBehaviour
{
    enum State {Patrol , Chase , Attack , Rest};

    [SerializeField] Transform player;
    [SerializeField] Transform[] points;

    [SerializeField] TMP_Text stateLabel;

    [SerializeField] GameObject warnRing;
    [SerializeField] LayerMask walls;

    [SerializeField] float viewRange =  7f;

    [SerializeField] float fov  = 90f;

    [SerializeField] float loseTime =   2f;

    [SerializeField] float attackRange = 1.2f;
     
    [SerializeField] float windUp  = 0.5f;
    [SerializeField] float restTime  = 0.8f;

    State state;

    float timer;
    float lostTimer;

    

    int i ;

    NavMeshAgent agent;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SetState(State.Patrol);

        

        
    }
    void Update()
    {
        timer += Time.deltaTime;
        switch (state)
        {
            case State.Patrol:
                Patrol();
                if (CanSeePlayer())
                {
                    SetState(State.Chase);
                
                }
                break;
            case State.Chase:
                Chase();

            
                break;




        }
    }

    void SetState(State next)
    {
        state = next;

        timer = 0f;
        lostTimer = 0f;
        stateLabel.text = next.ToString();
        agent.isStopped = (next == State.Attack || next == State.Rest);
        warnRing.SetActive(next == State.Attack);
        if(next == State.Patrol) 
        agent.SetDestination(points[i].position);


      








    }

        void Patrol()
        {
            
            if(!agent.pathPending && agent.remainingDistance < 0.3f)
            {
                i = (i + 1) % points.Length;
                agent.SetDestination(points[i].position);
            }
        }


        bool CanSeePlayer()
    {
        Vector3 toPlayer = player.position - transform.position;
        if(toPlayer.magnitude > viewRange)
        {
            return false;
        }

        if(Vector3.Angle(transform.forward,toPlayer) > fov / 2f)
        {
            return false;

        }

        Vector3 eye = transform.position + Vector3.up * 0.5f;

        Vector3 target = player.position + Vector3.up * 0.5f;

        Vector3 dir = target - eye;

        if(Physics.Raycast(eye , dir.normalized, dir.magnitude , walls))
        {
            return false;

        }

        return true;


    }

    void Chase()
    {
        agent.SetDestination(player.position);
        if (CanSeePlayer())
        {
            lostTimer = 0f;

        }
        else
        {
           lostTimer += Time.deltaTime;
        }

        if(lostTimer > loseTime)
        {
            SetState(State.Patrol);
            return;
        }


        if (Vector3.Distance(transform.position , player.position) <= attackRange)
        {
            SetState(State.Attack);
        }
    }



        











}

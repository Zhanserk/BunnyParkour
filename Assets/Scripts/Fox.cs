using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class Fox : MonoBehaviour
{
    enum State { Patrol, Chase, Attack, Rest }

    [SerializeField] Transform player;
    [SerializeField] Transform[] points;
    [SerializeField] TMP_Text stateLabel;
    [SerializeField] GameObject warnRing;
    [SerializeField] LayerMask walls;
    [SerializeField] FoxStats stats;

    NavMeshAgent agent;
    State state;
    float timer;
    int i;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SetState(State.Patrol);
    }

    void Update()
    {
        timer += Time.deltaTime;
        float dist = Vector3.Distance(transform.position, player.position);

        switch (state)
        {
            case State.Patrol:
                Patrol();
                if (CanSeePlayer()) SetState(State.Chase);
                break;

            case State.Chase:
                agent.SetDestination(player.position);
                if (CanSeePlayer()) timer = 0f;
                if (dist < stats.attackRange) SetState(State.Attack);
                else if (timer > stats.loseTime) SetState(State.Patrol);
                break;

            case State.Attack:
                if (timer > stats.windUp)
                {
                    if (dist < stats.attackRange + stats.biteExtraRange)
                        player.GetComponent<Health>().Hurt();
                    SetState(State.Rest);
                }
                break;

            case State.Rest:
                if (timer > stats.restTime) SetState(State.Chase);
                break;
        }
    }

    void LateUpdate()
    {
        stateLabel.transform.rotation = Camera.main.transform.rotation;
    }

    void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < stats.arriveDistance)
        {
            i = (i + 1) % points.Length;
            agent.SetDestination(points[i].position);
        }
    }

    bool CanSeePlayer()
    {
        Vector3 eye = transform.position + Vector3.up * stats.eyeHeight;
        Vector3 toPlayer = player.position + Vector3.up * stats.eyeHeight - eye;

        if (toPlayer.magnitude > stats.viewRange) return false;
        if (Vector3.Angle(transform.forward, toPlayer) > stats.fov / 2f) return false;
        if (Physics.Raycast(eye, toPlayer.normalized, toPlayer.magnitude, walls))
            return false;
        return true;
    }

    void SetState(State next)
    {
        state = next;
        timer = 0f;
        stateLabel.text = next.ToString();
        agent.isStopped = (next == State.Attack || next == State.Rest);
        warnRing.SetActive(next == State.Attack);
        if (next == State.Patrol) agent.SetDestination(points[i].position);
    }
}

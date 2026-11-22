using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class Goat : MonoBehaviour
{
    enum State { Idle, WindUp, Dash, Stunned }

    [SerializeField] Transform player;
    [SerializeField] GameObject dashLane;
    [SerializeField] TMP_Text stateLabel;
    [SerializeField] GoatStats stats;

    NavMeshAgent agent;
    State state;
    float timer;
    bool hit;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SetState(State.Idle);
    }

    void Update()
    {
        timer += Time.deltaTime;
        float dist = Vector3.Distance(transform.position, player.position);

        switch (state)
        {
            case State.Idle:
                if (dist < stats.range && player.position.z < transform.position.z)
                    SetState(State.WindUp);
                break;

            case State.WindUp:
                if (timer > stats.windUp) SetState(State.Dash);
                break;

            case State.Dash:
                agent.Move(transform.forward * stats.dashSpeed * Time.deltaTime);
                if (!hit && dist < stats.hitDistance)
                {
                    player.GetComponent<Health>().Hurt();
                    hit = true;
                }
                if (timer > stats.dashTime) SetState(State.Stunned);
                break;

            case State.Stunned:
                if (timer > stats.stunTime) SetState(State.Idle);
                break;
        }
    }

    void LateUpdate()
    {
        stateLabel.transform.rotation = Camera.main.transform.rotation;
    }

    void SetState(State next)
    {
        state = next;
        timer = 0f;
        stateLabel.text = next.ToString();
        dashLane.SetActive(next == State.WindUp);

        if (next == State.WindUp)
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        if (next == State.Dash) hit = false;
    }
}

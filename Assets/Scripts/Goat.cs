using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class Goat : MonoBehaviour
{
    enum State { Idle, WindUp, Dash, Stunned }

    [SerializeField] Transform player;      // заяц
    [SerializeField] GameObject dashLane;   // красная полоса = предупреждение
    [SerializeField] TMP_Text stateLabel;   // табличка
    [SerializeField] GoatStats stats;       // Assignment 11: дальность, время, скорость рывка теперь в ассете GoatStats

    NavMeshAgent agent;
    State state;
    float timer;
    bool hit;   // уже ударил в этом рывке?

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
                    SetState(State.WindUp);                                  // заметил
                break;

            case State.WindUp:
                if (timer > stats.windUp) SetState(State.Dash);             // предупредил
                break;

            case State.Dash:
                agent.Move(transform.forward * stats.dashSpeed * Time.deltaTime); // бежит вперёд, NavMesh не даст упасть
                if (!hit && dist < stats.hitDistance)                       // задел зайца
                {
                    player.GetComponent<Health>().Hurt();
                    hit = true;                                             // только один удар за рывок
                }
                if (timer > stats.dashTime) SetState(State.Stunned);
                break;

            case State.Stunned:
                if (timer > stats.stunTime) SetState(State.Idle);           // очухался
                break;
        }
    }

    void LateUpdate()
    {
        stateLabel.transform.rotation = Camera.main.transform.rotation;     // табличка к камере
    }

    void SetState(State next)
    {
        state = next;
        timer = 0f;
        stateLabel.text = next.ToString();
        dashLane.SetActive(next == State.WindUp);                           // полоса только при предупреждении

        if (next == State.WindUp)   // повернуться к зайцу: полоса покажет, куда побежит
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        if (next == State.Dash) hit = false;
    }
}

using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class Goat : MonoBehaviour
{
    enum State { Idle, WindUp, Dash, Stunned }

    [SerializeField] Transform player;      // заяц
    [SerializeField] GameObject dashLane;   // красная полоса = предупреждение
    [SerializeField] TMP_Text stateLabel;   // табличка
    [SerializeField] float range = 6f;      // м, с какого расстояния разгоняется
    [SerializeField] float windUp = 0.7f;   // с, предупреждение
    [SerializeField] float dashSpeed = 10f; // м/с
    [SerializeField] float dashTime = 0.8f; // с (10 м/с × 0.8 с = 8 м)
    [SerializeField] float stunTime = 1.5f; // с, стоит оглушённый

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
                if (dist < range && player.position.z < transform.position.z)
                    SetState(State.WindUp);                                  // заметил
                break;

            case State.WindUp:
                if (timer > windUp) SetState(State.Dash);                   // предупредил
                break;

            case State.Dash:
                agent.Move(transform.forward * dashSpeed * Time.deltaTime); // бежит вперёд, NavMesh не даст упасть
                if (!hit && dist < 1.2f)                                    // задел зайца
                {
                    player.GetComponent<Health>().Hurt();
                    hit = true;                                             // только один удар за рывок
                }
                if (timer > dashTime) SetState(State.Stunned);
                break;

            case State.Stunned:
                if (timer > stunTime) SetState(State.Idle);                 // очухался
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
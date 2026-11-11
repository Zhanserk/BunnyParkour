using UnityEngine;
using UnityEngine.AI;
using TMPro;

// Козёл-таран: Idle -> WindUp -> Dash -> Stunned -> Return -> Idle
[RequireComponent(typeof(NavMeshAgent))]
public class Goat : MonoBehaviour
{
    enum State { Idle, WindUp, Dash, Stunned, Return }

    [Header("Ссылки")]
    [SerializeField] Transform player;          // заяц (Hero)
    [SerializeField] GameObject dashLane;       // красная полоса-предупреждение
    [SerializeField] TMP_Text stateLabel;       // табличка над головой

    [Header("Числа")]
    [SerializeField] float triggerRange = 6f;   // м, с какого расстояния разгоняется
    [SerializeField] float fov = 120f;          // градусы, смотрит только вперёд
    [SerializeField] float windUp = 0.7f;       // с, предупреждение перед рывком
    [SerializeField] float dashSpeed = 10f;     // м/с
    [SerializeField] float dashDistance = 8f;   // м, длина рывка
    [SerializeField] float hitRange = 1.2f;     // м, на каком расстоянии задевает
    [SerializeField] float stunTime = 1f;       // с, стоит оглушённый
    [SerializeField] float idleCooldown = 1.5f; // с, пауза перед следующим рывком
    [SerializeField] float turnSpeed = 6f;      // скорость поворота

    NavMeshAgent agent;
    Health playerHealth;
    State state;
    float timer;
    Vector3 home;          // стартовая точка
    Vector3 startForward;  // куда смотрел в начале
    Vector3 dashDir;       // направление рывка
    float dashed;          // сколько уже пробежал
    bool hasHit;           // уже ударил в этом рывке?

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        playerHealth = player.GetComponent<Health>();
        home = transform.position;
        startForward = transform.forward;
    }

    void Start()
    {
        SetState(State.Idle);
    }

    void Update()
    {
        timer += Time.deltaTime;

        switch (state)
        {
            case State.Idle:
                if (timer >= idleCooldown && CanSeePlayer()) SetState(State.WindUp);
                break;

            case State.WindUp:
                TurnToPlayer();                        // целится, полоса показывает куда побежит
                if (timer >= windUp)
                {
                    dashDir = transform.forward;       // запомнил направление
                    SetState(State.Dash);
                }
                break;

            case State.Dash:
                float step = dashSpeed * Time.deltaTime;
                agent.Move(dashDir * step);            // рывок, NavMesh не даст упасть с острова
                dashed += step;
                if (!hasHit && Vector3.Distance(transform.position, player.position) < hitRange)
                {
                    playerHealth.Hurt();               // ударил один раз
                    hasHit = true;
                }
                if (dashed >= dashDistance) SetState(State.Stunned);
                break;

            case State.Stunned:
                if (timer >= stunTime) SetState(State.Return);       // очухался
                break;

            case State.Return:
                if (!agent.pathPending && agent.remainingDistance < 0.3f)
                    SetState(State.Idle);                            // дошёл домой
                break;
        }
    }

    void LateUpdate()
    {
        stateLabel.transform.rotation = Camera.main.transform.rotation;
    }

    bool CanSeePlayer()
    {
        Vector3 toPlayer = player.position - transform.position;
        if (toPlayer.magnitude > triggerRange) return false;                     // далеко
        if (Vector3.Angle(startForward, toPlayer) > fov / 2f) return false;      // за спиной
        return true;
    }

    void TurnToPlayer()
    {
        Vector3 dir = player.position - transform.position;
        dir.y = 0f;
        if (dir == Vector3.zero) return;
        Quaternion look = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, look, turnSpeed * Time.deltaTime);
    }

    void SetState(State next)
    {
        state = next;
        timer = 0f;
        stateLabel.text = next.ToString();

        dashLane.SetActive(next == State.WindUp);      // полоса только во время предупреждения
        agent.isStopped = (next != State.Return);      // по навигатору ходит только домой

        if (next == State.Dash) { dashed = 0f; hasHit = false; }
        if (next == State.Return) agent.SetDestination(home);
    }
}
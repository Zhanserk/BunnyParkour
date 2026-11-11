using UnityEngine;
using UnityEngine.AI;
using TMPro;

// Лиса-патрульный: Patrol -> Chase -> Attack -> Rest -> Chase
[RequireComponent(typeof(NavMeshAgent))]
public class Fox : MonoBehaviour
{
    enum State { Patrol, Chase, Attack, Rest }

    [Header("Ссылки")]
    [SerializeField] Transform player;          // заяц (Hero)
    [SerializeField] Transform[] points;        // FoxPoint1..4
    [SerializeField] TMP_Text stateLabel;       // табличка над головой
    [SerializeField] GameObject warnRing;       // красный круг-предупреждение
    [SerializeField] LayerMask obstacleMask;    // только слой Walls

    [Header("Зрение")]
    [SerializeField] float viewRange = 6f;      // м, как далеко видит
    [SerializeField] float fov = 90f;           // градусы, угол обзора
    [SerializeField] float loseTime = 2f;       // с, не видит зайца -> Patrol

    [Header("Движение")]
    [SerializeField] float patrolSpeed = 2f;    // м/с
    [SerializeField] float chaseSpeed = 4f;     // м/с (заяц бегает 6)

    [Header("Атака")]
    [SerializeField] float attackRange = 1.2f;  // м, дистанция укуса
    [SerializeField] float windUp = 0.5f;       // с, рычит перед укусом
    [SerializeField] float restTime = 0.8f;     // с, стоит после укуса

    NavMeshAgent agent;     // навигатор
    Health playerHealth;    // здоровье зайца
    State state;            // карточка в руке
    float timer;            // секундомер
    int i;                  // номер флажка

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        playerHealth = player.GetComponent<Health>();
    }

    void Start()
    {
        SetState(State.Patrol);
    }

    void Update()
    {
        timer += Time.deltaTime;

        switch (state)
        {
            case State.Patrol:
                Patrol();
                if (CanSeePlayer()) SetState(State.Chase);          // увидела -> гонюсь
                break;

            case State.Chase:
                if (CanSeePlayer()) timer = 0f;                     // timer = сколько НЕ вижу
                agent.SetDestination(player.position);              // бегу к зайцу
                if (DistanceToPlayer() < attackRange) SetState(State.Attack);   // близко -> кусаю
                else if (timer > loseTime) SetState(State.Patrol);  // потеряла -> гуляю
                break;

            case State.Attack:
                if (timer >= windUp)                                // порычала
                {
                    Strike();
                    SetState(State.Rest);
                }
                break;

            case State.Rest:
                if (timer >= restTime) SetState(State.Chase);       // отдохнула
                break;
        }
    }

    void LateUpdate()
    {
        // табличка всегда смотрит в камеру, чтобы текст читался
        stateLabel.transform.rotation = Camera.main.transform.rotation;
    }

    void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.3f)  // дошла до флажка
        {
            i = (i + 1) % points.Length;                           // следующий по кругу
            agent.SetDestination(points[i].position);
        }
    }

    void SetState(State next)
    {
        state = next;
        timer = 0f;
        stateLabel.text = next.ToString();

        if (next == State.Chase) agent.speed = chaseSpeed;
        else agent.speed = patrolSpeed;

        agent.isStopped = (next == State.Attack || next == State.Rest);  // стоит, когда кусает и отдыхает
        warnRing.SetActive(next == State.Attack);                         // круг только во время рыка

        if (next == State.Patrol) agent.SetDestination(points[i].position);  // вернуться на маршрут
    }

    bool CanSeePlayer()
    {
        Vector3 eye = transform.position + Vector3.up;            // глаза: 1 м над лапами
        Vector3 toPlayer = player.position + Vector3.up - eye;    // стрелка от глаз к зайцу

        if (toPlayer.magnitude > viewRange) return false;                         // 1. далеко
        if (Vector3.Angle(transform.forward, toPlayer) > fov / 2f) return false;  // 2. за спиной
        if (Physics.Raycast(eye, toPlayer.normalized, toPlayer.magnitude, obstacleMask))
            return false;                                                          // 3. камень мешает
        return true;
    }

    float DistanceToPlayer()
    {
        return Vector3.Distance(transform.position, player.position);
    }

    void Strike()
    {
        // кусаем, только если заяц всё ещё рядом: успел отпрыгнуть — промах
        if (DistanceToPlayer() < attackRange + 0.3f) playerHealth.Hurt();
    }

    void OnDrawGizmosSelected()
    {
        // в окне Scene: жёлтый круг = дальность зрения, голубые линии = угол обзора
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRange);
        Vector3 left = Quaternion.Euler(0f, -fov / 2f, 0f) * transform.forward;
        Vector3 right = Quaternion.Euler(0f, fov / 2f, 0f) * transform.forward;
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, left * viewRange);
        Gizmos.DrawRay(transform.position, right * viewRange);
    }
}
using UnityEngine;
using UnityEngine.AI;
using TMPro;

// Лиса-патрульный: Patrol -> Chase -> Attack -> Rest -> Chase
public class Fox : MonoBehaviour
{
    enum State { Patrol, Chase, Attack, Rest }

    [SerializeField] Transform player;          // заяц
    [SerializeField] Transform[] points;        // флажки маршрута FoxPoint1..4
    [SerializeField] TMP_Text stateLabel;       // табличка над головой
    [SerializeField] GameObject warnRing;       // красный круг = предупреждение
    [SerializeField] LayerMask walls;           // слой Walls (камень)
    [SerializeField] float viewRange = 6f;      // м, как далеко видит
    [SerializeField] float fov = 90f;           // градусы, угол обзора
    [SerializeField] float loseTime = 2f;       // с, не видит зайца -> Patrol
    [SerializeField] float attackRange = 1.2f;  // м, дистанция укуса
    [SerializeField] float windUp = 0.5f;       // с, рычит перед укусом
    [SerializeField] float restTime = 0.8f;     // с, отдых после укуса

    NavMeshAgent agent;   // навигатор
    State state;          // карточка в руке
    float timer;          // секундомер
    int i;                // номер флажка

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SetState(State.Patrol);
    }

    void Update()
    {
        timer += Time.deltaTime;
        float dist = Vector3.Distance(transform.position, player.position);   // расстояние до зайца

        switch (state)
        {
            case State.Patrol:
                Patrol();                                          // хожу по флажкам
                if (CanSeePlayer()) SetState(State.Chase);         // увидела -> гонюсь
                break;

            case State.Chase:
                agent.SetDestination(player.position);             // бегу к зайцу
                if (CanSeePlayer()) timer = 0f;                    // вижу -> секундомер с нуля
                if (dist < attackRange) SetState(State.Attack);    // близко -> кусаю
                else if (timer > loseTime) SetState(State.Patrol); // 2 с не видно -> гуляю
                break;

            case State.Attack:
                if (timer > windUp)                                // порычала 0.5 с
                {
                    if (dist < attackRange + 0.3f)                 // заяц ещё рядом?
                        player.GetComponent<Health>().Hurt();      // кусь!
                    SetState(State.Rest);
                }
                break;

            case State.Rest:
                if (timer > restTime) SetState(State.Chase);       // отдохнула -> снова гонюсь
                break;
        }
    }

    void LateUpdate()
    {
        stateLabel.transform.rotation = Camera.main.transform.rotation;   // табличка к камере
    }

    void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.3f)  // дошла до флажка
        {
            i = (i + 1) % points.Length;                           // следующий по кругу
            agent.SetDestination(points[i].position);
        }
    }

    bool CanSeePlayer()
    {
        Vector3 eye = transform.position + Vector3.up;             // глаза: 1 м над лапами
        Vector3 toPlayer = player.position + Vector3.up - eye;     // стрелка от глаз к зайцу

        if (toPlayer.magnitude > viewRange) return false;                          // 1. далеко
        if (Vector3.Angle(transform.forward, toPlayer) > fov / 2f) return false;   // 2. за спиной
        if (Physics.Raycast(eye, toPlayer.normalized, toPlayer.magnitude, walls))
            return false;                                                           // 3. камень мешает
        return true;                                                                // вижу!
    }

    void SetState(State next)
    {
        state = next;
        timer = 0f;
        stateLabel.text = next.ToString();
        agent.isStopped = (next == State.Attack || next == State.Rest);   // стоит, когда кусает и отдыхает
        warnRing.SetActive(next == State.Attack);                         // круг только во время рыка
        if (next == State.Patrol) agent.SetDestination(points[i].position);  // вернуться на маршрут
    }
}
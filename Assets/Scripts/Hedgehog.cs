using UnityEngine;
using TMPro;

// Ёж-стрелок: Idle -> Aim -> Fire -> Cooldown -> Idle
public class Hedgehog : MonoBehaviour
{
    enum State { Idle, Aim, Fire, Cooldown }

    [Header("Ссылки")]
    [SerializeField] Transform player;          // заяц (Hero)
    [SerializeField] Transform muzzle;          // нос, откуда вылетает иголка
    [SerializeField] GameObject needlePrefab;   // префаб Needle
    [SerializeField] GameObject aimLaser;       // красный лазер-предупреждение
    [SerializeField] TMP_Text stateLabel;       // табличка над головой

    [Header("Числа")]
    [SerializeField] float viewRange = 10f;     // м, с какого расстояния замечает
    [SerializeField] float fov = 120f;          // градусы, смотрит только вперёд
    [SerializeField] float aimTime = 0.8f;      // с, целится перед выстрелом
    [SerializeField] float cooldown = 1.5f;     // с, отдых после выстрела
    [SerializeField] float turnSpeed = 5f;      // скорость поворота

    State state;
    float timer;
    Vector3 startForward;   // куда ёж смотрел в начале

    void Start()
    {
        startForward = transform.forward;
        SetState(State.Idle);
    }

    void Update()
    {
        timer += Time.deltaTime;

        switch (state)
        {
            case State.Idle:
                if (CanSeePlayer()) SetState(State.Aim);                 // заметил
                break;

            case State.Aim:
                TurnToPlayer();                                          // поворачивается
                if (!CanSeePlayer()) SetState(State.Idle);               // заяц ушёл
                else if (timer >= aimTime) SetState(State.Fire);         // прицелился
                break;

            case State.Fire:
                Instantiate(needlePrefab, muzzle.position, muzzle.rotation);  // выстрел
                SetState(State.Cooldown);
                break;

            case State.Cooldown:
                if (timer >= cooldown) SetState(State.Idle);             // отдохнул
                break;
        }
    }

    void LateUpdate()
    {
        stateLabel.transform.rotation = Camera.main.transform.rotation;  // табличка к камере
    }

    bool CanSeePlayer()
    {
        Vector3 toPlayer = player.position - transform.position;
        if (toPlayer.magnitude > viewRange) return false;                        // далеко
        if (Vector3.Angle(startForward, toPlayer) > fov / 2f) return false;      // за спиной: не стреляет из-за кадра
        return true;
    }

    void TurnToPlayer()
    {
        Vector3 dir = player.position - transform.position;
        dir.y = 0f;                                    // поворот только влево-вправо
        if (dir == Vector3.zero) return;
        Quaternion look = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, look, turnSpeed * Time.deltaTime);
    }

    void SetState(State next)
    {
        state = next;
        timer = 0f;
        stateLabel.text = next.ToString();
        aimLaser.SetActive(next == State.Aim);         // лазер только когда целится
    }
}
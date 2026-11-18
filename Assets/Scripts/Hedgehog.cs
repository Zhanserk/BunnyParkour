using UnityEngine;
using TMPro;

public class Hedgehog : MonoBehaviour
{
    enum State { Idle, Aim, Cooldown }

    [SerializeField] Transform player;      // заяц
    [SerializeField] Transform muzzle;      // нос, откуда вылетает иголка
    [SerializeField] GameObject needle;     // префаб Needle
    [SerializeField] GameObject aimLaser;   // красный лазер = предупреждение
    [SerializeField] TMP_Text stateLabel;   // табличка
    [SerializeField] HedgehogStats stats;   // Assignment 11: дальность, время прицеливания и отдыха теперь в ассете HedgehogStats

    State state;
    float timer;

    void Start() { SetState(State.Idle); }

    void Update()
    {
        timer += Time.deltaTime;

        switch (state)
        {
            case State.Idle:
                if (PlayerInFront()) SetState(State.Aim);             // заметил
                break;

            case State.Aim:
                if (!PlayerInFront()) SetState(State.Idle);           // ушёл
                else if (timer > stats.aimTime)                       // прицелился
                {
                    Instantiate(needle, muzzle.position, muzzle.rotation);   // выстрел
                    SetState(State.Cooldown);
                }
                break;

            case State.Cooldown:
                if (timer > stats.cooldown) SetState(State.Idle);     // отдохнул
                break;
        }
    }

    bool PlayerInFront()
    {
        // близко И заяц ещё не прошёл ежа (ёж виден на экране)
        return Vector3.Distance(transform.position, player.position) < stats.range
            && player.position.z < transform.position.z;
    }

    void SetState(State next)
    {
        state = next;
        timer = 0f;
        stateLabel.text = next.ToString();
        aimLaser.SetActive(next == State.Aim);   // лазер только когда целится
    }
}

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
    [SerializeField] float range = 10f;     // м, с какого расстояния стреляет
    [SerializeField] float aimTime = 0.8f;  // с, целится перед выстрелом
    [SerializeField] float cooldown = 1.5f; // с, отдых после выстрела

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
                else if (timer > aimTime)                             // прицелился
                {
                    Instantiate(needle, muzzle.position, muzzle.rotation);   // выстрел
                    SetState(State.Cooldown);
                }
                break;

            case State.Cooldown:
                if (timer > cooldown) SetState(State.Idle);           // отдохнул
                break;
        }
    }

    bool PlayerInFront()
    {
        // близко И заяц ещё не прошёл ежа (ёж виден на экране)
        return Vector3.Distance(transform.position, player.position) < range
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
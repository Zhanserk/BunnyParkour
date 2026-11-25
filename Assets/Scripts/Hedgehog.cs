using UnityEngine;
using TMPro;

public class Hedgehog : MonoBehaviour
{
    enum State { Idle, Aim, Cooldown }

    [SerializeField] Transform player;
    [SerializeField] Transform muzzle;
    [SerializeField] GameObject needle;
    [SerializeField] GameObject aimLaser;
    [SerializeField] TMP_Text stateLabel;
    [SerializeField] HedgehogStats stats;
    [SerializeField] RandomSound aimSound;    // Assignment 12: писк прицела в начале предупреждения (вход в Aim)
    [SerializeField] RandomSound shotSound;   // Assignment 12: выстрел иголкой

    State state;
    float timer;
    AudioSource voice;   // Assignment 12: динамик ежа (3D, группа SFX)

    void Start()
    {
        voice = GetComponent<AudioSource>();   // Assignment 12
        SetState(State.Idle);
    }

    void Update()
    {
        timer += Time.deltaTime;

        switch (state)
        {
            case State.Idle:
                if (PlayerInFront()) SetState(State.Aim);
                break;

            case State.Aim:
                if (!PlayerInFront()) SetState(State.Idle);
                else if (timer > stats.aimTime)
                {
                    Instantiate(needle, muzzle.position, muzzle.rotation);
                    shotSound.Play(voice);   // Assignment 12: выстрел
                    SetState(State.Cooldown);
                }
                break;

            case State.Cooldown:
                if (timer > stats.cooldown) SetState(State.Idle);
                break;
        }
    }

    bool PlayerInFront()
    {
        return Vector3.Distance(transform.position, player.position) < stats.range
            && player.position.z < transform.position.z;
    }

    void SetState(State next)
    {
        state = next;
        timer = 0f;
        stateLabel.text = next.ToString();
        aimLaser.SetActive(next == State.Aim);
        if (next == State.Aim) aimSound.Play(voice);   // Assignment 12: писк в начале прицеливания
    }
}

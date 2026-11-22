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

    State state;
    float timer;

    void Start() { SetState(State.Idle); }

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
    }
}

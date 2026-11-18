using TMPro;
using UnityEngine;

public class Hedgehog : MonoBehaviour
{

    enum State {Idle , Aim , Fire , Cooldown};
    [SerializeField] Transform player;

    [SerializeField] Transform muzzle;

    [SerializeField] GameObject needlePrefab;

    [SerializeField] GameObject aimLazer;
    [SerializeField] TMP_Text stateLabel;

    [SerializeField] float veiwRange = 10f;
    [SerializeField] float fov = 120f;
    [SerializeField] float extraAngle = 15f;   // правка: запас угла в градусах. Начал целиться при 60 градусах, бросит только после 75 (чтобы не дёргался на границе)
    [SerializeField] float aimTime = 0.8f;
    [SerializeField] float cooldown = 1f;
    [SerializeField] float turnSpeed = 180f;   // правка: теперь это градусов в секунду (раньше было 10 для Slerp)

    State state;
    float timer;
    Vector3 startForward;

    void Start()
    {
      startForward = transform.forward;
      startForward.y = 0f;                     // правка: смотрим только по земле
      startForward.Normalize();
      SetState(State.Idle);

    }
    void SetState(State next)
    {
        state = next;
        timer = 0f;
        stateLabel.text = next.ToString();
        aimLazer.SetActive(next == State.Aim);


    }

    // правка: halfAngle - половина угла обзора (в Idle строже, в Aim мягче)
    bool CanSeePlayer(float halfAngle)
    {

        Vector3 toPlayer = player.position - transform.position;
        toPlayer.y = 0f;                       // правка: высоту (прыжки зайца) не считаем, иначе на границе обзора ёж дёргается Idle <-> Aim
        if(toPlayer.magnitude > veiwRange)
        {
            return false;

        }

        if(Vector3.Angle(startForward , toPlayer) > halfAngle)
        {
            return false;

        }


        return true;
    }
    void TurnToPlay()
    {
        Vector3 dir = player.position - transform.position;
        dir.y = 0f;

        if(dir.sqrMagnitude < 0.01f) return;   // правка: заяц прямо над ежом, не вертимся
        TurnTo(dir);

    }

    // правка: поворот с постоянной скоростью (градусов в секунду) вместо Slerp: без рывков и без "туда-сюда"
    void TurnTo(Vector3 dir)
    {
        Quaternion look = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation , look , turnSpeed * Time.deltaTime);
    }

    void Update()
    {

        timer += Time.deltaTime;


        switch (state)
        {
            case State.Idle:
                if (CanSeePlayer(fov / 2f))
                {
                    SetState(State.Aim);
                }
                else
                {
                    TurnTo(startForward);       // правка: не видит зайца -> плавно вернуться в исходное положение
                }


            break;

            case State.Aim:
                TurnToPlay();
                if(!CanSeePlayer(fov / 2f + extraAngle))   // правка: с запасом угла
                {
                 SetState(State.Idle);

                }else if(timer >= aimTime){
                    SetState(State.Fire);
                }
            break;

            case State.Fire:
                Instantiate(needlePrefab , muzzle.position , muzzle.rotation);
                SetState(State.Cooldown);
                break;

            case State.Cooldown:
                if(timer >= cooldown)
                {
                    SetState(State.Idle);
                }
                break;

        }
    }

    void LateUpdate()
    {
        stateLabel.transform.rotation = Camera.main.transform.rotation;

    }






}

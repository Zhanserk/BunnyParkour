using TMPro;                 // библиотека для таблички (TMP_Text)
using UnityEngine;           // основа Unity

public class Hedgehog : MonoBehaviour   // скрипт «Ёж», вешается на объект
{

    enum State {Idle , Aim , Fire , Cooldown};   // 4 карточки: жду, целюсь, выстрел, отдых
    [SerializeField] Transform player;           // заяц: нужна его позиция

    [SerializeField] Transform muzzle;           // нос ежа: откуда вылетает иголка

    [SerializeField] GameObject needlePrefab;    // префаб иголки (штамп для копий)

    [SerializeField] GameObject aimLazer;        // красный лазер-предупреждение
    [SerializeField] TMP_Text stateLabel;        // табличка с именем карточки
    
    [SerializeField] float veiwRange = 10f;      // видит на 10 м
    [SerializeField] float fov = 120f;           // угол обзора: по 60° в каждую сторону
    [SerializeField] float aimTime = 0.8f;       // целится 0.8 с перед выстрелом
    [SerializeField] float cooldown = 1f;        // отдыхает 1 с после выстрела
    [SerializeField] float turnSpeed = 10f;      // как быстро поворачивается к зайцу

    State state;            // карточка в руке
    float timer;            // секундомер
    Vector3 startForward;   // «перёд»: куда смотрел в начале игры

    void Start()            // Unity вызывает 1 раз в начале
    {
      startForward = transform.forward;   // запоминаю, куда смотрю сейчас
      SetState(State.Idle);               // первая карточка: жду
      
    }
    void SetState(State next)                    // поменять карточку
    {
        state = next;                            // новая карточка в руку
        timer = 0f;                              // секундомер с нуля
        stateLabel.text = next.ToString();       // имя карточки на табличке
        aimLazer.SetActive(next == State.Aim);   // лазер горит только в Aim
        
        
    }

    bool CanSeePlayer()                          // «вижу зайца?» → да/нет
    {

        Vector3 toPlayer = player.position - transform.position;   // стрелка от меня к зайцу
        if(toPlayer.magnitude > veiwRange)                          // дальше 10 м?
        {
            return false;                                           // → не вижу, выхожу

        }

        if(Vector3.Angle(startForward , toPlayer) > fov / 2f)       // больше 60° от «перёда»?
        {
            return false;                                           // → сбоку/сзади, не вижу

        }


        return true;                                                // близко и впереди → вижу
    }
    void TurnToPlay()                                          // повернуться к зайцу
    {
        Vector3 dir = player.position - transform.position;   // стрелка к зайцу
        dir.y = 0f;                                           // без высоты: крутимся только влево-вправо
        Quaternion look = Quaternion.LookRotation(dir);       // какой нужен поворот «нос на зайца»
        transform.rotation = Quaternion.Slerp(transform.rotation , look , turnSpeed * Time.deltaTime);   // поворачиваюсь плавно, по кусочку за кадр

    }

    void Update()                  // Unity вызывает каждый кадр
    {

        timer += Time.deltaTime;   // секундомер тикает


        switch (state)             // какая карточка в руке?
        {
            case State.Idle:                   // ЖДУ
                if (CanSeePlayer())            //   вижу зайца?
                {
                    SetState(State.Aim);       //   → целюсь
                }
                
                
            break;

            case State.Aim:                    // ЦЕЛЮСЬ
                TurnToPlay();                  //   поворачиваюсь к зайцу
                if(!CanSeePlayer())            //   потерял его?
                {
                 SetState(State.Idle);         //   → жду
                    
                }else if(timer >= aimTime){    //   целился 0.8 с?
                    SetState(State.Fire);      //   → выстрел
                }
            break;
            
            case State.Fire:                   // ВЫСТРЕЛ (длится 1 кадр)
                Instantiate(needlePrefab , muzzle.position , muzzle.rotation);   // копия иголки из носа
                SetState(State.Cooldown);      //   → отдых
                break;

            case State.Cooldown:               // ОТДЫХ
                if(timer >= cooldown)          //   отдохнул 1 с?
                {
                    SetState(State.Idle);      //   → жду
                }    
                break;

        }
    }

    void LateUpdate()   // Unity вызывает после всех Update
    {
        stateLabel.transform.rotation = Camera.main.transform.rotation;   // табличка смотрит в камеру
        
    }

}
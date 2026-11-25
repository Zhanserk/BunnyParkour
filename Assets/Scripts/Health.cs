using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class Health : MonoBehaviour
{

    [SerializeField] PlayerStats stats;

    public int hp;

    public TMP_Text hptext;

    [SerializeField] RandomSound hurtSounds;   // Assignment 12: урон (2-3 варианта)

    private bool _dead;   // Assignment 12: смерть засчитывается только один раз
    

    private AudioSource _audio;

    private bool _safe;

    private Renderer[] _parts;





    void Awake()
    {
        _parts = GetComponentsInChildren<Renderer>();
        _audio = GetComponent<AudioSource>();
        hp = stats.maxHp;
    }



    void Start()
    {
        Showhp();

 
    }

    private void Showhp()
    {
        hptext.text = "HP: " + hp;

    }

    public void Hurt()
    {

        if (_safe)
        {
            return;

        }

        hp -= stats.hurtDamage;
        if (hp > 0) hurtSounds.Play(_audio);   // Assignment 12: урон; при смерти звучит death
        Showhp();
        if(hp <= 0)
        {
            Die();

        }
        else
        {
            StartCoroutine(Blink());
        }



        
    
    }

    public void Die()
    {
        if (_dead) return;                       // Assignment 12
        _dead = true;
        SoundManager.Instance.PlayDeath();       // Assignment 12: смерть и проигрыш, звук не обрывается при перезагрузке
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    private IEnumerator Blink()
    {
        _safe = true;
        for(int i  = 0 ; i < stats.blinkCount ; i++)
        {
            for(int j = 0 ; j < _parts.Length ; j++)
            {
                _parts[j].enabled = !_parts[j].enabled;

            }
            yield return new WaitForSeconds(stats.blinkInterval);
        }
        for(int i = 0 ; i < _parts.Length ; i++)
        {
            _parts[i].enabled =true;
            

        }
        _safe = false;

    }

    public void Heal()
    {   
        
        if(hp < stats.maxHp)
        {
            hp = Mathf.Min(hp + stats.healAmount, stats.maxHp);

            Showhp();

        }
    }
    

               





}

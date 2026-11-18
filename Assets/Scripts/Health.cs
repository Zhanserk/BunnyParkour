using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class Health : MonoBehaviour
{

    [SerializeField] PlayerStats stats;   // Assignment 11: максимум HP, лечение, мигание теперь в ассете PlayerStats

    public int hp;   // Assignment 11: стартовое значение берётся из stats.maxHp (в Awake)

    public TMP_Text hptext;

    public AudioClip hurtSound;
    

    private AudioSource _audio;

    private bool _safe;

    private Renderer[] _parts;





    void Awake()
    {
        _parts = GetComponentsInChildren<Renderer>();
        _audio = GetComponent<AudioSource>();
        hp = stats.maxHp;   // Assignment 11: стартовые жизни из ассета
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

        hp -= stats.hurtDamage;   // Assignment 11: было hp--
        _audio.PlayOneShot(hurtSound);
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    private IEnumerator Blink()
    {
        _safe = true;
        for(int i  = 0 ; i < stats.blinkCount ; i++)   // Assignment 11: было 10
        {
            for(int j = 0 ; j < _parts.Length ; j++)
            {
                _parts[j].enabled = !_parts[j].enabled;

            }
            yield return new WaitForSeconds(stats.blinkInterval);   // Assignment 11: было 0.2f
        }
        for(int i = 0 ; i < _parts.Length ; i++)
        {
            _parts[i].enabled =true;
            

        }
        _safe = false;

    }

    public void Heal()
    {   
        
        if(hp < stats.maxHp)   // Assignment 11: было hp < 3
        {
            hp = Mathf.Min(hp + stats.healAmount, stats.maxHp);   // Assignment 11: было hp++

            Showhp();

        }
    }
    

               





}

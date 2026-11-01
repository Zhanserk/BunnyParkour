using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class Health : MonoBehaviour
{

    public int hp= 3;

    public TMP_Text hptext;

    public AudioClip hurtSound;
    

    private AudioSource _audio;

    private bool _safe;

    private Renderer[] _parts;





    void Awake()
    {
        _parts = GetComponentsInChildren<Renderer>();
        _audio = GetComponent<AudioSource>();
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

        hp--;
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
        for(int i  = 0 ; i < 10 ; i++)
        {
            for(int j = 0 ; j < _parts.Length ; j++)
            {
                _parts[j].enabled = !_parts[j].enabled;

            }
            yield return new WaitForSeconds(0.2f);
        }
        for(int i = 0 ; i < _parts.Length ; i++)
        {
            _parts[i].enabled =true;
            

        }
        _safe = false;

    }

    public void Heal()
    {   
        
        if(hp < 3)
        {
            hp++;

            Showhp();

        }
    }
    

               





}

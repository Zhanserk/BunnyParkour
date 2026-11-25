using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    [SerializeField] AudioMixerSnapshot normalSnapshot;   // Assignment 12: обычный звук
    [SerializeField] AudioMixerSnapshot pausedSnapshot;   // Assignment 12: музыка приглушённая на паузе
    [SerializeField] float snapshotTime = 0.3f;           // Assignment 12: с, плавный переход (идёт и на паузе)

    public void SetPaused(bool paused)
    {
        pausePanel.SetActive(paused);
        (paused ? pausedSnapshot : normalSnapshot).TransitionTo(snapshotTime);   // Assignment 12

        if(paused == true)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
            
        }
        
    }


    public void Restart()
    {
        Time.timeScale = 1;
        normalSnapshot.TransitionTo(snapshotTime);   // Assignment 12
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }  
      public void ToMenu()
    {
        Time.timeScale = 1;
        normalSnapshot.TransitionTo(snapshotTime);   // Assignment 12
        SceneManager.LoadScene(0);
        

    }


}

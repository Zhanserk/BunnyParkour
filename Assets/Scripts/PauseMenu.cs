using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;

    public void SetPaused(bool paused)
    {
        pausePanel.SetActive(paused);

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }  
      public void ToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
        

    }


}

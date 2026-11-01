using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

        public void ToMenu()
    {
        SceneManager.LoadScene(0);
    }
            public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit");
        
    }

}

using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] LevelData data;
    [SerializeField] Transform player;
    [SerializeField] PlayerController playerController;
    [SerializeField] TMP_Text levelText;

    float timer;
    bool finished;

    void Awake()
    {
        player.position = data.spawnPosition;
        levelText.text = data.displayName;
        playerController.SetCarrotGoal(data.carrotsGoal);
    }

    void Update()
    {
        if (!finished) timer += Time.deltaTime;
    }

    public void Complete()
    {
        if (finished) return;
        finished = true;

        SaveData save = SaveSystem.Data;
        int index = data.levelNumber - 1;

        if (index >= 0 && index < save.bestTimes.Length)
        {
            float best = save.bestTimes[index];
            if (best <= 0f || timer < best) save.bestTimes[index] = timer;
        }

        int next = Mathf.Min(data.levelNumber + 1, save.bestTimes.Length);
        if (next > save.unlockedLevel) save.unlockedLevel = next;

        SaveSystem.Save();
        Debug.Log(data.displayName + ": time " + timer.ToString("0.0") + " s (par " + data.parTime.ToString("0") + " s)");
    }
}

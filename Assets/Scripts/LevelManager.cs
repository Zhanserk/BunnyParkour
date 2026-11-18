using TMPro;
using UnityEngine;

// Один общий скрипт для всех уровней. Берёт свой ассет LevelData и настраивает сцену:
// ставит зайца на старт, пишет название уровня, задаёт цель по морковкам и считает время.
public class LevelManager : MonoBehaviour
{
    [SerializeField] LevelData data;                    // ассет этого уровня (Level_01, Level_02 или Level_03)
    [SerializeField] Transform player;                  // заяц
    [SerializeField] PlayerController playerController; // скрипт зайца (ему скажем цель по морковкам)
    [SerializeField] TMP_Text levelText;                // надпись с названием уровня

    float timer;        // с, сколько идёт уровень
    bool finished;      // уровень уже пройден?

    void Awake()
    {
        player.position = data.spawnPosition;                   // заяц на старт
        levelText.text = data.displayName;                      // название уровня
        playerController.SetCarrotGoal(data.carrotsGoal);       // сколько морковок нужно
    }

    void Update()
    {
        // Time.deltaTime на паузе равен 0 (Time.timeScale = 0), значит на паузе время не идёт
        if (!finished) timer += Time.deltaTime;
    }

    // Вызывается из PlayerController, когда заяц дошёл до портала со всеми морковками
    public void Complete()
    {
        if (finished) return;       // второй раз не считаем
        finished = true;

        SaveData save = SaveSystem.Data;
        int index = data.levelNumber - 1;                       // уровень 1 лежит в ячейке 0

        // лучшее время: пишем, если рекорда не было (0) или новое время меньше
        if (index >= 0 && index < save.bestTimes.Length)
        {
            float best = save.bestTimes[index];
            if (best <= 0f || timer < best) save.bestTimes[index] = timer;
        }

        // открываем следующий уровень (но не дальше последнего)
        int next = Mathf.Min(data.levelNumber + 1, save.bestTimes.Length);
        if (next > save.unlockedLevel) save.unlockedLevel = next;

        SaveSystem.Save();
        Debug.Log(data.displayName + ": time " + timer.ToString("0.0") + " s (par " + data.parTime.ToString("0") + " s)");
    }
}

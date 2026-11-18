using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Главное меню: выбор уровня, громкость музыки, сброс прогресса.
// Висит на объекте MenuController в сцене MainMenu (рядом со старым MenuButtons).
public class MainMenuUI : MonoBehaviour
{
    [SerializeField] LevelData[] levels;        // три ассета Level_01..Level_03
    [SerializeField] Button[] levelButtons;     // три кнопки выбора уровня
    [SerializeField] TMP_Text[] levelLabels;    // надписи на этих кнопках
    [SerializeField] Slider volumeSlider;       // слайдер громкости музыки
    [SerializeField] MusicVolume music;         // музыка в этой сцене
    [SerializeField] GameObject confirmPanel;   // панель "Are you sure?"
    [SerializeField] Color lockedColor = new Color(0.45f, 0.45f, 0.45f, 1f);   // серый цвет закрытой кнопки

    Color[] _unlockedColors;    // обычные цвета кнопок (запоминаем, чтобы вернуть после серого)

    void Awake()
    {
        _unlockedColors = new Color[levelButtons.Length];
        for (int i = 0; i < levelButtons.Length; i++)
            _unlockedColors[i] = levelButtons[i].image.color;   // запоминаем исходный цвет
    }

    void Start()
    {
        volumeSlider.SetValueWithoutNotify(SaveSystem.Data.musicVolume);   // ставим слайдер без вызова OnVolumeChanged
        confirmPanel.SetActive(false);                                     // подтверждение скрыто
        Refresh();
    }

    // Обновляет три кнопки: закрытые серые и "Locked", открытые с лучшим временем
    void Refresh()
    {
        SaveData save = SaveSystem.Data;
        for (int i = 0; i < levels.Length; i++)
        {
            int number = levels[i].levelNumber;
            bool unlocked = number <= save.unlockedLevel;

            levelButtons[i].interactable = unlocked;                                    // закрытую нельзя нажать
            levelButtons[i].image.color = unlocked ? _unlockedColors[i] : lockedColor;  // серая, если закрыта

            if (!unlocked)
            {
                levelLabels[i].text = "Locked";
            }
            else
            {
                float best = save.bestTimes[number - 1];    // уровень 1 лежит в ячейке 0
                string text = "LEVEL " + number;
                if (best > 0f)                              // рекорд есть -> вторая строка поменьше
                    text += "\n<size=70%>Best " + best.ToString("0.0", CultureInfo.InvariantCulture) + " s</size>";
                levelLabels[i].text = text;
            }
        }
    }

    // Кнопка уровня: в Inspector у каждой кнопки стоит номер (0, 1, 2)
    public void LoadLevel(int index)
    {
        SceneManager.LoadScene(levels[index].sceneName);
    }

    // Слайдер громкости: вызывается при каждом движении
    public void OnVolumeChanged(float value)
    {
        SaveSystem.Data.musicVolume = value;
        music.Apply();              // сразу слышно
        SaveSystem.Save();          // и сохраняем
    }

    // Кнопка Reset Progress и кнопка No: показать или скрыть вопрос
    public void ShowConfirm(bool show)
    {
        confirmPanel.SetActive(show);
    }

    // Кнопка Yes: сбросить прогресс и обновить кнопки
    public void ConfirmReset()
    {
        SaveSystem.ResetProgress();
        Refresh();
        confirmPanel.SetActive(false);
    }
}

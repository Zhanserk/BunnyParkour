using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] LevelData[] levels;
    [SerializeField] Button[] levelButtons;
    [SerializeField] TMP_Text[] levelLabels;
    [SerializeField] Slider volumeSlider;
    [SerializeField] MusicVolume music;
    [SerializeField] GameObject confirmPanel;
    [SerializeField] Color lockedColor = new Color(0.45f, 0.45f, 0.45f, 1f);

    Color[] _unlockedColors;

    void Awake()
    {
        _unlockedColors = new Color[levelButtons.Length];
        for (int i = 0; i < levelButtons.Length; i++)
            _unlockedColors[i] = levelButtons[i].image.color;
    }

    void Start()
    {
        volumeSlider.SetValueWithoutNotify(SaveSystem.Data.musicVolume);
        confirmPanel.SetActive(false);
        Refresh();
    }

    void Refresh()
    {
        SaveData save = SaveSystem.Data;
        for (int i = 0; i < levels.Length; i++)
        {
            int number = levels[i].levelNumber;
            bool unlocked = number <= save.unlockedLevel;

            levelButtons[i].interactable = unlocked;
            levelButtons[i].image.color = unlocked ? _unlockedColors[i] : lockedColor;

            if (!unlocked)
            {
                levelLabels[i].text = "Locked";
            }
            else
            {
                float best = save.bestTimes[number - 1];
                string text = "LEVEL " + number;
                if (best > 0f)
                    text += "\n<size=70%>Best " + best.ToString("0.0", CultureInfo.InvariantCulture) + " s</size>";
                levelLabels[i].text = text;
            }
        }
    }

    public void LoadLevel(int index)
    {
        SceneManager.LoadScene(levels[index].sceneName);
    }

    public void OnVolumeChanged(float value)
    {
        SaveSystem.Data.musicVolume = value;
        music.Apply();
        SaveSystem.Save();
    }

    public void ShowConfirm(bool show)
    {
        confirmPanel.SetActive(show);
    }

    public void ConfirmReset()
    {
        SaveSystem.ResetProgress();
        Refresh();
        confirmPanel.SetActive(false);
    }
}

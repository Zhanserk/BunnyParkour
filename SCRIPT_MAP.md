# SCRIPT_MAP - how scripts and data are connected (Assignment 11)

A simple map. Arrow `A --> B` means "A reads from B" or "A calls B".

## 1. Balance data (ScriptableObject assets in `Assets/Data`)

```
PlayerStats.asset     <-- PlayerController (speed, jump, fall height)
                      <-- Health (max HP, damage, heal, blinking)

FoxStats.asset        <-- Fox (view range, angle, windUp, rest ...)
HedgehogStats.asset   <-- Hedgehog (range, aim time, cooldown)
                      <-- Needle (speed, lifetime)
GoatStats.asset       <-- Goat (range, windUp, dash speed/time, stun time)
```

Each script has `[SerializeField] XStats stats;` and reads `stats.field`.
The numbers are not in the code any more. To change the balance: select the
asset in the Project window and edit it in the Inspector.

## 2. Levels (Level1 / Level2 / Level3 scenes)

```
Level_01.asset ┐
Level_02.asset ├──> LevelManager (one script, one object per level scene)
Level_03.asset ┘        │
                        ├─ puts Hero on spawnPosition
                        ├─ writes displayName into LevelText
                        ├─ tells PlayerController the carrotsGoal
                        ├─ counts the time
                        └─ Complete(): saves best time, unlocks next level
                                 ▲
PlayerController (Hero touches the portal "Exit" with enough carrots)
```

## 3. Saves

```
SaveManager (created once, DontDestroyOnLoad)
   ├─ Awake:            SaveSystem.Load()
   ├─ OnApplicationPause(true): SaveSystem.Save()
   └─ OnApplicationQuit:        SaveSystem.Save()

SaveSystem (static) ── Data : SaveData { version, unlockedLevel, bestTimes[3], musicVolume }
   ├─ Load()          reads save.json; missing / empty / broken file -> defaults + warning
   ├─ Save()          writes save.json (JsonUtility, pretty print)
   └─ ResetProgress() unlockedLevel = 1, bestTimes = 0

save.json = Application.persistentDataPath + "/save.json"
```

Who uses `SaveSystem.Data`:

```
LevelManager.Complete()  --> writes unlockedLevel, bestTimes, then Save()
MainMenuUI               --> reads unlockedLevel, bestTimes (level buttons)
                         --> writes musicVolume (slider), ResetProgress()
MusicVolume (on Music)   --> reads musicVolume, sets AudioSource.volume
```

## 4. Main menu

```
MainMenu scene
  Canvas / SafeArea
    PlayButton, QuitButton        --> MenuButtons (old script)
    LevelButton1..3               --> MainMenuUI.LoadLevel(0..2) --> SceneManager.LoadScene(sceneName)
    MusicSlider                   --> MainMenuUI.OnVolumeChanged --> SaveSystem + MusicVolume.Apply()
    ResetButton                   --> MainMenuUI.ShowConfirm(true)
    ConfirmPanel: Yes / No        --> MainMenuUI.ConfirmReset() / ShowConfirm(false)
```

## 5. Old scripts (not changed in logic)

`CameraFollow`, `MovingPlatform`, `SafeArea`, `PauseMenu`, `MenuButtons` are the same as before.
Changes in `PlayerController`, `Health`, `Fox`, `Hedgehog`, `Goat`, `Needle` are marked
with `// Assignment 11` (or the comment line says "Assignment 11").

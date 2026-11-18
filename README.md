   # Bunny Parkour

   3D mobile parkour game made in Unity 6 (URP).
   Play as a bunny, jump across floating islands, collect carrots and reach the portal.

## Assignment 11 - Data and Saves

**Where the data is**

| What | Where |
|---|---|
| Balance numbers (speed, jump, HP, enemy times and ranges) | `Assets/Data/PlayerStats`, `FoxStats`, `HedgehogStats`, `GoatStats` |
| Levels (scene, spawn position, carrots goal, par time) | `Assets/Data/Level_01`, `Level_02`, `Level_03` |
| Data scripts (ScriptableObjects, SaveData, SaveSystem, SaveManager) | `Assets/Scripts/Data` |
| Level and menu scripts | `Assets/Scripts/LevelManager.cs`, `MainMenuUI.cs`, `MusicVolume.cs` |

To change the balance, select an asset in the Project window and edit it in the Inspector. No code change is needed.
How the scripts are connected: see `SCRIPT_MAP.md`. Decisions and AI use: `DECISIONS.md`, `AI_LOG.txt`. Video script: `VIDEO_SCRIPT.md`.

**Where `save.json` is** (`Application.persistentDataPath/save.json`; the Console prints the full path at start):

- Windows (also the Unity Editor): `C:\Users\<user>\AppData\LocalLow\<Company>\<Product>\save.json`
  (now `DefaultCompany\Midka3d`, taken from Player Settings)
- iPhone: `Documents/save.json` inside the app sandbox (`/var/mobile/Containers/Data/Application/<id>/Documents`)
- Android: `/storage/emulated/0/Android/data/<package name>/files/save.json`

What is saved: `version`, `unlockedLevel`, `bestTimes[3]`, `musicVolume`.

**Test "broken save file"**

1. Press Play, then Stop (this creates `save.json`). Find the path in the Console line `Save file path: ...`.
2. Open `save.json` in Notepad, replace everything with `{ not json`, save.
3. Press Play. The game does not crash. The Console shows a warning: `Save file is broken, using default values.`
   Only Level 1 is open. (The same happens if the file is empty or deleted.)
4. When you press Stop, the game writes a fresh valid `save.json` with default values.

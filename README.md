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

## Sounds and music (Assignment 12)

All files below were **generated for this project with a Python synthesis script** (sine, square, saw,
triangle waves and filtered noise). No samples from other people are used. Source: own work (made with
the help of Claude Code, see `AI_LOG.txt`). License: own work, free to use in this project.
The old sound files of unknown origin were deleted.

| File | Event | Folder |
|---|---|---|
| `jump1.wav`, `jump2.wav`, `jump3.wav` | jump (random variant) | `Assets/Audio/SFX` |
| `land.wav` | landing | `Assets/Audio/SFX` |
| `pickup1.wav`, `pickup2.wav`, `pickup3.wav` | carrot (random variant) | `Assets/Audio/SFX` |
| `heal.wav` | heart | `Assets/Audio/SFX` |
| `hurt1.wav`, `hurt2.wav`, `hurt3.wav` | damage (random variant) | `Assets/Audio/SFX` |
| `death.wav`, `lose.wav` | bunny dies, then lose jingle | `Assets/Audio/SFX` |
| `win.wav` | portal reached | `Assets/Audio/SFX` |
| `ui_click.wav` | every UI button | `Assets/Audio/SFX` |
| `fox_windup.wav` | fox warning before the bite | `Assets/Audio/SFX` |
| `hedgehog_aim.wav`, `needle_shot.wav` | hedgehog aiming, shot | `Assets/Audio/SFX` |
| `goat_windup.wav`, `goat_dash.wav` | goat warning, dash | `Assets/Audio/SFX` |
| `music_menu.wav` | calm loop: MainMenu and Win | `Assets/Audio/Music` |
| `music_level.wav` | faster loop: Level1-3 | `Assets/Audio/Music` |

**Mixer** `Assets/Audio/GameMix.mixer`: `Master > Music > Tracks`, `Master > SFX`, `Master > UI`.
Exposed: `MusicVol`, `SFXVol`, `MasterVol`. Snapshots `Normal` and `Paused` (muffled music, -6 dB).
Volumes are saved in `save.json` (`musicVolume`, `sfxVolume`). An old save without `sfxVolume` uses 1.
Repeated sounds use a random clip (not the same one twice) and a random pitch 0.92-1.08.

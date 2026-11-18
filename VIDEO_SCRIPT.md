# VIDEO SCRIPT - Bunny Parkour, Assignment 11 "Data and Saves"

Length: about 2:30 - 3:00. Language: simple English (B1). Read it slowly, in your own words.
Lines in **[SHOW]** are what to put on the screen. Lines in quotes are what to say.

> Important and honest: I have an iPhone and a Windows PC, but I do not have a build of the game.
> So I test "restart" with **Stop -> Play** in the Unity Editor. Say this in the video.

---

## 0:00 - 0:15  Intro

**[SHOW]** The main menu in the Game view.

"Hi, I am Zhanserik. This is my game Bunny Parkour. In this assignment I added data and saves.
The numbers of the game are now in data assets, the levels are made from data,
and the game saves your progress in a JSON file."

## 0:15 - 0:55  Part 1: balance in ScriptableObject assets

**[SHOW]** Project window, folder `Assets/Data`. Click `GoatStats`. Show the Inspector.

"Before, the numbers were inside the scripts. Now they are in assets.
Here is GoatStats: range, wind up, dash speed, dash time, stun time.
Every field has a comment with the unit, for example seconds or meters per second."

**[SHOW]** Open `Goat.cs` for a few seconds. Point to `[SerializeField] GoatStats stats;` and `stats.windUp`.

"The script only reads `stats.windUp`. There is no number in the code."

**[SHOW]** Live change: set `windUp` from 0.7 to 2, press Play in Level3, walk to the goat, show the long red warning.
Then set it back to 0.7.

"I change the value in the asset and the game changes. I do not touch the code.
Now I set it back."

## 0:55 - 1:25  Part 2: three levels from data

**[SHOW]** `Level_03` asset, then the `LevelManager` object in the Level3 scene.

"A level is also data: number, name, scene, spawn position, carrots goal and par time.
One script, LevelManager, is used in all three levels. It puts the bunny on the start,
writes the level name, gives the carrot goal to the player and counts the time.
When I reach the portal, it saves my best time and unlocks the next level."

## 1:25 - 2:00  Part 3: save file and restart test

**[SHOW]** Play from the main menu. Only Level 1 is open, Level 2 and 3 say "Locked".
Finish Level 1 (or use the portal quickly). Press **Stop**, then **Play**.

"At first only Level 1 is open. I finish Level 1. Now I press Stop and Play,
like closing and opening the game. Level 2 is still open and the button shows my best time.
So the progress is saved."

**[SHOW]** Console: the line `Save file path: ...`. Open that folder in Explorer and open `save.json` in Notepad.

"The Console shows where the file is. It is a normal JSON file:
version, unlocked level, best times and music volume.
I use JsonUtility. I do not use PlayerPrefs for progress."

## 2:00 - 2:30  Part 4: broken file test

**[SHOW]** Close Unity Play mode. In Notepad write `{ not json` into `save.json`, save the file. Press Play.

"Now I break the file on purpose. Press Play. The game does not crash.
The Console shows a warning: the save file is broken, using default values.
Only Level 1 is open again. If the file is empty or missing, it is the same."

**[SHOW]** In the menu: move the Music Volume slider, then press Reset Progress, show "Are you sure?" and press Yes.

"The music volume is saved too. Reset Progress asks me first, and then it clears the levels and the times."

## 2:30 - 3:00  Part 5: script map and AI

**[SHOW]** `SCRIPT_MAP.md` on GitHub, then `DECISIONS.md`.

"This is the script map: stats assets feed the scripts, LevelManager uses the level assets
and SaveSystem, and the menu reads the save data.
I used Claude Code, an AI agent, for this assignment. It wrote the code, the assets and the commits
from my prompt. It is written in AI_LOG and DECISIONS. I reviewed the work and I can explain it.
Thank you."

---

### Checklist before recording
- [ ] Delete `save.json` first (so Level 2 and 3 are locked at the start).
- [ ] `GoatStats.windUp` is back to 0.7 after the live change.
- [ ] Say that there is no build: the restart test is Stop -> Play.
- [ ] Do not read this text like a robot. Use your own simple words. Practice twice.

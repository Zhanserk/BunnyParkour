# DECISIONS - Bunny Parkour, Assignment 10 (Three Enemy Types)

2026-11-09  Asked Claude Code to build enemy models (fox, hedgehog, goat) from
primitives, new islands and the NavMesh, without gameplay scripts. It moved Hero
above FoxIsland for testing - I moved it back to Island1.

2026-11-11  Wrote the Fox patrol myself with hints (enum, switch, SetState,
waypoints). Then asked Claude chat for the full Fox FSM (Patrol, Chase, Attack,
Rest). It was too complex for me to explain, so I asked for a simpler version
and moved the speed to the NavMeshAgent in the Inspector.

2026-11-12  Asked Claude chat for Hedgehog, Goat and Needle in the same FSM
pattern as my Fox. Added a rule: enemies attack only if the bunny has not
passed them yet, so there are no attacks from off-screen.

2026-11-12  Playtest: 1 tester (my sister) on the iPhone, [4] tries. She said
the phone controls are hard. Changed Goat windUp 0.7 -> 1.0 s and Hedgehog
aimTime 0.8 -> 1.0 s to give more time to react. Only one tester because of
time.


# DECISIONS - Bunny Parkour, Assignment 11 (Data and Saves)

2026-11-19  Asked Claude Code (an AI agent) to do the whole assignment from one prompt:
balance data in ScriptableObjects, three LevelData assets with a LevelManager, a JSON
save system, a menu with level select, music volume and reset, the documents, the git
commits and the merge to main. Claude Code wrote the code, created the assets, edited
the scenes and made the commits. I did not write these scripts myself.

2026-11-19  My unfinished enemy rewrite was committed first as "WIP: rewrite enemies" and
pushed to the branch rewrite-enemies. That branch is NOT merged into main. The work for this
assignment is on feature/data-saves, which starts from main (the Fox, Hedgehog, Goat and Needle
from pull request #3), not from my rewrite. So my own rewrite is not in this assignment.

2026-11-19  Balance numbers moved into four assets in Assets/Data: PlayerStats, FoxStats,
HedgehogStats, GoatStats. Values were taken from the Inspector in the scenes and prefabs;
they are the same as the old defaults in the code, so the game plays as before.

2026-11-19  Numbers that were hidden in the code were also moved: Fox arriveDistance (0.3 m),
biteExtraRange (0.3 m), eyeHeight (1 m); Goat hitDistance (1.2 m); Health damage and heal
amount (1 and 1) and the blink count and interval. NavMeshAgent speed is not in the code,
it stays on the NavMeshAgent component in the Inspector.

2026-11-19  Left in the code on purpose: the 0.1 stick threshold in PlayerController
(input, not balance) and the SaveSystem constants (3 levels, default volume 0.8).

2026-11-19  OPEN QUESTION FOR ME: the Assignment 10 playtest note above says Goat windUp
became 1.0 s and Hedgehog aimTime 1.0 s. In Level3 on main the Inspector still has 0.7 and 0.8.
Claude Code kept the Inspector values (0.7 and 0.8), because the prompt said the Inspector wins.
I must check which numbers are right and change them in GoatStats and HedgehogStats if needed.

2026-11-19  Level3 spawn is (0, 0.05, 0) and not (0, 0, 0). Level1 and Level2 already
use 0.05 so the collider does not start inside the island. Same idea here.

2026-11-19  carrotsGoal is the real number of carrots in each scene (3, 3, 3), read by the
editor script when the assets were made. parTime (60, 90, 150 s) is a guess by Claude Code,
not measured. parTime is only printed in the Console after a level; gameplay does not use it yet.

2026-11-19  SaveManager is created by [RuntimeInitializeOnLoadMethod] before the first scene,
not placed in MainMenu. Reason: it also works when I press Play directly in Level3.

2026-11-19  Music volume = the volume set in each scene (0.3 / 0.25 / 0.7) x the slider value.
The default slider value is 0.8, so the music is about 20% quieter than before. Simple formula
chosen on purpose. The volume is saved on every slider move (the file is tiny).

2026-11-19  Reset Progress keeps the music volume (it is a setting, not progress).
The Play button still starts Level 1; Level Select is the way to jump to another level.

2026-11-19  MainMenu got a SafeArea panel and its Canvas Scaler Match changed from 0.5 to 1
(same as the level scenes), because the longer menu would be cut on wide phones.

2026-11-19  Tests done by Claude Code in Unity batch mode: 9 SaveSystem checks (no file,
empty file, broken file, wrong bestTimes size, values out of range, missing fields, save and
load again, reset): all passed. Compile: no errors. All new fields in the scenes are filled
(no None). NOT tested: the game on iPhone, playing the levels by hand, and how the menu looks
(preview screenshots in batch mode were empty). I must check these myself.

STUDENT REVIEW: ...


# DECISIONS - Bunny Parkour, Assignment 12 (Sound Pass)

2026-11-26  Asked Claude Code (AI agent) to do the whole Sound Pass from one prompt: sounds,
mixer outputs, music manager, volume sliders, pause snapshot, event sounds, documents, commits
and the merge to main. I made the mixer GameMix.mixer myself before that. Claude Code wrote the
code, generated the sounds, edited the scenes and made the commits. I asked for commits without
a Claude co-author line; the use of AI is recorded here and in AI_LOG.txt.

2026-11-26  My earlier commit "Delete Comment" had also removed DECISIONS.md, SCRIPT_MAP.md and
VIDEO_SCRIPT.md. Claude Code restored DECISIONS.md and SCRIPT_MAP.md from git because this
assignment needs them. VIDEO_SCRIPT.md (Assignment 11) was not restored.

2026-11-26  Sounds are synthesized with a Python script instead of downloading Kenney packs.
Claude Code did not try the download: synthesis gives clear ownership and license. They sound
simple (retro style). I can replace any of them later.

2026-11-26  A new prefab Assets/Resources/AudioRoot is created before the first scene and is not
destroyed: MusicManager (2 sources, 1 s crossfade on unscaled time) and SoundManager (death,
lose, win, click, volumes). So death, lose and win sounds are not cut by a scene reload, and no
script creates AudioSources for single sounds. The old Music objects are disabled, not deleted.

2026-11-26  On death both sounds play: death, then lose 0.6 s later. Win plays at the portal on
every level. The Win scene uses the calm menu music. The landing sound plays only if the bunny
falls faster than 2 m/s.

2026-11-26  Enemy audio is 3D with linear rolloff, full volume up to 5 m, silent after 40 m (the
camera is the listener and is far from the enemy). Warning sounds start when the warning
starts: Fox enters Attack, Hedgehog enters Aim, Goat enters WindUp.

2026-11-26  The sliders control Music and SFX only; button clicks (UI group) are not changed by
them. Music default 0.8 is about -2 dB. The old per-scene music volumes (0.25-0.7) are not used
now, loudness is set inside the files and by the mixer. The main menu was rearranged (title a
bit smaller) to fit two sliders, each 120 px high.

2026-11-26  Checked in Unity batch mode: compile 0 errors; every AudioSource has a mixer group;
one AudioListener per scene; all 21 buttons have the click sound; no empty sound fields; menu
layout has no overlaps; old save.json without sfxVolume gives sfx = 1. NOT checked: how it
sounds, the crossfade, the pause muffling, and anything on the iPhone. I must listen myself.

STUDENT REVIEW: ...
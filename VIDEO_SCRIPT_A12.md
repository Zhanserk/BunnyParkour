# VIDEO SCRIPT - Bunny Parkour, Assignment 12 "Sound Pass"

Length: about 2:30. Simple English (B1). Say it in your own words.
**[SHOW]** = what is on the screen. Record with game sound ON.
I have an iPhone and a Windows PC and no build, so the "restart" test is **Stop -> Play**.

## 0:00 - 0:20  Intro and mixer
**[SHOW]** `GameMix` mixer window: groups and the `Paused` snapshot.
"Hi, I am Zhanserik. This is Bunny Parkour. I added sound and music. I made this audio mixer:
Master, Music with Tracks, SFX and UI. It has two snapshots, Normal and Paused."

## 0:20 - 0:35  Crossfade
**[SHOW]** Main menu, then press Play.
"The menu has calm music. When the level starts, the music changes smoothly in one second.
One MusicManager does this and it is not destroyed between scenes."

## 0:35 - 1:25  Events (8 or more)
**[SHOW]** Play a level and say each sound when you hear it.
"Jump. Landing. Carrot. Heart. Damage. If I fall: death sound and then the lose sound.
The portal: win sound. Buttons: click sound.
Jump, carrot and damage have several variants and a small random pitch,
so they do not repeat the same way."

## 1:25 - 1:50  I hear the attack before it hits
**[SHOW]** Level3: fox, hedgehog, goat.
"The fox growls before it bites. The hedgehog beeps while it aims, then shoots.
The goat bleats before the dash. So I can hear the danger before the hit."
Do not talk over the sounds. Stop speaking and let the audience hear them.

## 1:50 - 2:05  Pause
**[SHOW]** Press the pause button, wait, press Resume.
"On pause the music becomes muffled and quieter. This is the Paused snapshot.
It works although the time is stopped."

## 2:05 - 2:25  Volume sliders and restart test
**[SHOW]** Main menu: move MUSIC and SOUNDS. Press Stop, then Play.
"I change the music and sound volume. I have no build, so I press Stop and Play,
like restarting the game. The sliders are the same and the volume is applied.
It is saved in save.json."

## 2:25 - 2:40  Script map and AI
**[SHOW]** `SCRIPT_MAP.md` section 5, then `AI_LOG.txt`.
"This is how the scripts are connected. I used Claude Code, an AI agent, for the code and the
sounds. It is written in AI_LOG and DECISIONS. I tested it and I can explain it. Thank you."

### Checklist before recording
- [ ] Listen to every sound once; check that nothing is too loud.
- [ ] Console has no red errors.
- [ ] Say "I tested it" only after you really tested it.
- [ ] Practice the English twice.

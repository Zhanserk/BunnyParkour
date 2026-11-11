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

2026-11-12  Playtest: 1 tester (my sister) on the iPhone. She beat the level
on the first try: enemies were too easy, only the parkour jumps were hard on
the phone. Made enemies harder: Fox viewRange 6 -> 7 m, Hedgehog cooldown
1.5 -> 1.0 s. Kept all wind-ups inside 0.3-0.8 s (Goat 0.7 s, Hedgehog
aim 0.8 s). Only one tester because of time. After: [2] tries / not retested.
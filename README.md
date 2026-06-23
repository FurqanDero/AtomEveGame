# AtomEveGame
# Atom Eve: Reconstruction

A top-down arena shooter built in Unity 6, set in the Invincible universe.

## Play it live

[Play on itch.io](https://invincidero26.itch.io/atom-eve-reconstruction)

## Story

The city is overrun. Reanimen drones, Flaxan gunners, and shielded enemy units have taken control — and only Atom Eve's matter manipulation can stop them.

Fight through 4 rooms of escalating enemy encounters before facing **The Mauler Twins** in a brutal two-on-one final battle.

## Controls

| Input | Action |
|---|---|
| `W A S D` | Move |
| Automatic | Auto-aim and fire at nearest enemy |
| `Left Shift` | Dash |

## Features

- Top-down twin-stick-style combat with auto-aim targeting
- Object pooling for smooth, high-volume bullet performance
- 4 distinct enemy types:
  - **Drone** — rushes the player in melee
  - **Gunner** — keeps distance, fires from range
  - **Shield Drone** — blocks frontal damage, must be flanked
  - **Mauler Twins** — dual final boss, one melee + one ranged, with a dynamic enrage mechanic when one twin falls
- Room-based progression system with door-gated advancement
- Full HUD: HP bar, room counter, dash cooldown indicator
- Game Over and Victory screens with restart/main menu flow

## Built with

- Unity 6 (Universal 2D / URP)
- C#
- Unity UI (TextMeshPro)
- Git / GitHub for version control

## Project structure

```
Assets/
├── _Scenes/        MainMenu, Room_01–04, Room_Boss
├── Scripts/
│   ├── Player/     Movement, shooting, dash, health
│   ├── Enemies/    Drone, Gunner, ShieldDrone, MaulerTwin AI
│   ├── Room/       RoomManager, DoorTrigger
│   ├── UI/         HUDManager, GameOverUI, VictoryUI
│   └── Core/       GameManager, BulletPool, CameraShakeBoss
├── Prefabs/
├── Sprites/
└── Audio/
```

## What I learned

This is my second Unity project, following [Invincible: Earth's Last Stand](https://github.com/FurqanDero/InvincibleGame). Building it taught me:

- Top-down character control and 8-directional movement
- Object pooling for performance-conscious projectile systems
- Auto-aim targeting logic (nearest-enemy detection via `OverlapCircleAll`)
- Finite State Machines across multiple distinct enemy behaviour types
- Multi-boss encounter design with dynamic difficulty scaling (enrage mechanic)
- Room/level progression systems and scene management
- Iterating on game balance based on actual playtesting

## About

Built by a software engineering student as a second Unity project, exploring a different genre (top-down arena shooter) after completing a 2D side-scrolling brawler. Set in the Invincible universe (Robert Kirkman).

Source code for the first game: [Invincible: Earth's Last Stand](https://github.com/FurqanDero/InvincibleGame)

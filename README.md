# Doofus Adventure Game

Doofus is a grid-based platform survival game built with Unity. The player continuously moves across a sequence of temporary platforms while trying to survive as long as possible.

The project is split into small, focused systems for gameplay, input, config data, managers, and UI. Runtime config comes from a JSON "Doofus Diary", which populates Unity `ScriptableObject` assets consumed by gameplay components.

---

## Gameplay Loop

1. `GameManager.StartGame()` starts the game.
2. `LevelManager` receives `GameStarted`, starts `PlatformGenerator`, and spawns the player.
3. `PlatformGenerator` places the first platform, then periodically spawns the next one in a random cardinal direction (Forward/Back/Left/Right), each with a random lifetime.
4. The player moves one grid cell at a time via the Unity Input System.
5. Entering a platform fires `PlatformGenerator.PlatformReached` → `ScoreManager` increments score → `ScoreUI` updates.
6. `PlatformTimerUI` shows each platform's remaining lifetime; expired platforms disable themselves.
7. `GameEndTrigger` follows the player's X/Z position; when the player enters it, `GameManager.EndGame()` fires `GameOver`.
8. `LevelManager` stops generation, `GameOverUI` shows the panel, and `GameManager.Restart()` reloads the scene.

---

## Project Architecture

| Namespace | Responsibility |
|---|---|
| `Doofus.Data` | Configuration data and data loading |
| `Doofus.Input` | Player input handling |
| `Doofus.Gameplay` | Core gameplay mechanics, including player movement |
| `Doofus.Manager` | High-level managers and game lifecycle |
| `Doofus.UI` | Runtime UI |
| `Doofus.Test` | Development/testing utilities |

Systems communicate through events rather than direct references:

```text
Platform.PlayerEntered → PlatformGenerator.PlatformReached → ScoreManager.ScoreChanged → ScoreUI
```

This lets, e.g., `ScoreUI` react to score changes without knowing how the score was produced.

### Folder Structure

```text
Assets/Doofus/
├── Data/     PlayerData, PulpitData, DoofusDiaryData, PlayerConfig, PulpitConfig, JsonExtractor
├── Gameplay/ Platform, PlatformGenerator, GameEndTrigger, PlayerController
├── Input/    InputManager, PlayerControls.inputactions
├── Manager/  GameManager, LevelManager, ScoreManager
├── UI/       ScoreUI, PlatformTimerUI, GameOverUI
└── Test/     JsonTester
```

---

## Configuration System

Two layers of config, bridged by `PopulateFrom(...)`:

1. **JSON data** — `DoofusDiaryData` (`PlayerData`, `PulpitData`), loaded from `Resources/doofus_diary.json` via `Resources.Load<TextAsset>()` (path configured without the `.json` extension).
2. **ScriptableObjects** — `PlayerConfig` (`moveSpeed`) and `PulpitConfig` (`minPulpitLifetime`, `maxPulpitLifetime`, `pulpitSpawnTime`), created via `Assets → Create → Doofus`.

`JsonExtractor.TryLoad()` validates: `speed > 0`; `min <= max` lifetime; `0 < spawnTime < minLifetime`. If the resource is missing, malformed, or invalid, defaults are used (`speed 5.0`, `min 3.0`, `max 6.0`, `spawn 1.5`).

> **Note:** JSON loading and ScriptableObject population are currently separate — `GameManager` only loads/logs `DoofusDiaryData`. A full pipeline (`JSON → JsonExtractor → DoofusDiaryData → Player/PulpitConfig → gameplay`) needs an explicit population step, or config edits won't reach `PlayerController`/`PlatformGenerator`.

---

## Scripts

### Gameplay

#### `PlatformGenerator` — `Doofus.Gameplay`
Generates the platform sequence from a lightweight 2-object pool (`Pool[0]`/`Pool[1]`, alternating), since only the active and next platform need to exist at once. Picks a random cardinal direction and lifetime per platform; new position = `previousPosition + direction * platformSize`.
API: `StartGeneration()`, `StopGeneration()` · Property: `IsGenerating` · Event: `PlatformReached`

#### `Platform` — `Doofus.Gameplay`
One temporary surface. Tracks `Lifetime`/`ElapsedTime`; disables itself and fires `Expired` once elapsed time exceeds lifetime. Fires `PlayerEntered` once per `Initialize()` when a `PlayerController` collides with it.
API: `Initialize(float lifetime)` · Properties: `Lifetime`, `ElapsedTime` · Events: `Expired`, `PlayerEntered`

#### `GameEndTrigger` — `Doofus.Gameplay`
A trigger collider that follows the player's X/Z (fixed Y). On entering it, if the collider has a `PlayerController`, calls `GameManager.EndGame()`. Acts as the survival loop's boundary condition.

#### `PlayerController` — `Doofus.Gameplay`
Grid-based movement (not free continuous movement), driven by `PlayerConfig.moveSpeed` (fallback `1`) and a `cellSize` (default `1`). Each move is a coroutine lerping (`Vector3.Lerp`) over `duration = cellSize / moveSpeed`; `IsMoving` blocks overlapping moves.
Property: `IsMoving`

### Input

#### `InputManager` — `Doofus.Input`
Singleton wrapper around the Unity Input System's generated `PlayerControls` (`Player.Move` action). Exposes `MoveInput`, reset to `Vector2.zero` on `Move.canceled`. `PlayerController` reads input through `InputManager.Instance`.
Property: `MoveInput`

### Manager

#### `GameManager` — `Doofus.Manager`
Central singleton game-state authority. Loads the Diary, tracks `IsGameRunning`, starts/ends the game, reloads the scene.
API: `StartGame()`, `EndGame()`, `Restart()` · Events: `GameStarted`, `GameOver`

#### `LevelManager` — `Doofus.Manager`
Reacts to `GameStarted`/`GameOver` to start/stop `PlatformGenerator` and spawn the player. Does not own game state itself.

#### `ScoreManager` — `Doofus.Manager`
Increments `Score` on `PlatformGenerator.PlatformReached` and fires `ScoreChanged`.
API: `ResetScore()` · Property: `Score` · Event: `ScoreChanged`

### UI

#### `ScoreUI` — `Doofus.UI`
Shows/hides on `GameStarted`, updates on `ScoreChanged` (starts at `0`), unsubscribes on destroy.

#### `PlatformTimerUI` — `Doofus.UI`
Displays `Lifetime - ElapsedTime` (clamped ≥ 0, 1 decimal) for its parent `Platform`, found via `GetComponentInParent<Platform>()` if not assigned. Typical hierarchy: `Platform → Canvas → Timer → TextMeshPro`.

#### `GameOverUI` — `Doofus.UI`
Hidden in `Awake()`; shows the panel when `GameManager.GameOver` fires.

---

## Design Principles

- **Single responsibility** — each script owns one job (see table above).
- **Event-driven** — `GameStarted`, `GameOver`, `PlayerEntered`, `PlatformReached`, `ScoreChanged` decouple producers from consumers; subscribers unsubscribe in `OnDestroy()`.
- **Config separation** — `JSON → data classes → config assets → gameplay`, leaving room for external tooling to drive strongly-typed runtime config.
- **Defensive null checks** — several systems no-op on missing references (`GameManager.Instance`, `platformGenerator`, `scoreManager`, `player`, `timerText`), though required scene refs should still be assigned correctly; null guards can mask setup mistakes.
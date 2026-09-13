# 🧠 Systems Architecture

## 🎮 Player System
- **Controller (Godot):** Thin `CharacterBody2D` proxy — input, `_PhysicsProcess`, `MoveAndSlide`.
- **Physics Logic (C#):** Domain math in `GameLogic` (pure .NET, no `Godot` types).

---

## 🧪 Core Logic Layer (`src/GameLogic`)
- **Authority:** Contains all math and decision-making logic (velocity, gravity, jump impulse, rules).
- **Constraint:** ⚠️ Zero dependency on `Godot` namespace for business rules.
- **DI:** Services registered via `ServiceLocator` in `GameGodot` `Main._Ready()`.
- **Unit Testing:** Validated via `xUnit` in `src/GameLogic.Tests` (`bash AI/script/xunit.sh`).

---

## 🎮 Engine Layer (`src/GameGodot`)
- **Proxy Nodes:** `CharacterBody2D` (player), `StaticBody2D` (ground) hold visuals and collision shapes only.
- **Bridge:** Scene scripts resolve services from `ServiceLocator` and apply results to nodes.
- **Integration tests:** GDUnit4 under `src/GameGodot/tests/` (`res://tests/`).

---

## 🔗 Communication Pattern
- **Downwards:** Godot callbacks (`_PhysicsProcess`) → C# logic methods.
- **Upwards:** Return values / signals → Godot motion and visual updates.

---

## 📂 Solution layout

| Project | Role |
| :--- | :--- |
| `src/GameLogic` | Pure C# domain |
| `src/GameGodot` | Godot 4 Mono engine layer |
| `src/GameLogic.Tests` | xUnit |
| `src/Game.sln` | Solution entrypoint |

Seed source: `examples/godot/godot-csharp-decoupled/`. Architecture is kept; classes/nodes are rewritten to match `design/gdd.md`.

---

## 🧩 Princípios

* Clear separation between engine and logic
* Tests focused on the C# layer
* Godot nodes stay thin proxies

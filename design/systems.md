# 🧠 Systems Architecture

## 🎮 Player System
- **Controller:** Input handling and state management.
- **Physics Logic:** C# class within `Game.Core` (Pure Logic).

---

## 🧪 Core Logic Layer (`Game.Core`)
- **Authority:** Contains all math and decision-making logic.
- **Constraint:** ⚠️ Zero dependency on `Godot` namespace for business rules.
- **Unit Testing:** Validated via `xUnit` in `tests/Game.Core.Tests`.

---

## 🎮 Engine Layer (Godot)
- **Proxy Nodes:** `CharacterBody2D` and `StaticBody2D` receive visuals and physical shapes.
- **Bridge:** Script attached to nodes delegates calls to `Game.Core`.

---

## 🔗 Communication Pattern
- **Downwards:** Godot callbacks (`_PhysicsProcess`) -> C# Logic methods.
- **Upwards:** C# Logic Events/Signals -> Godot visual updates.

---

## 🧩 Princípios

* Clear separation between engine and logic
* Tests focused on the C# layer
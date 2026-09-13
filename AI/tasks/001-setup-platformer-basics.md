# 📌 Task ID: 001-setup-platformer-basics

**status**: TODO  
**priority**: MEDIUM  
**type**: FEATURE  
**created_at**: 2026-04-09T00:55:05  
**updated_at**: 2026-04-09T00:55:05  

---

# 🎯 Goal

Setup the basic platformer physics foundation in Godot 4, implementing gravity, ground collision, and jump impulse based on defined contracts.

---

# 📖 Context

This is the project's bootstrap task. It implements the behaviors defined in `design/contracts/basic_movement.feature`. 
Success is defined by passing physics integration tests in a headless environment.

---

# 🧪 Expected Behavior

*   **Environment:** A `StaticBody2D` with a `Sprite2D` and a `CollisionShape2D` (RectangleShape2D) properly resized to serve as the ground.
*   **Player:** A `CharacterBody2D` containing its own `Sprite2D` and `CollisionShape2D`.
*   **Movement:** A script associated with the player using the standard Godot template, with a specific adjustment of -600 (Jump Velocity) to allow movement testing.
*   **Interaction:** Upon starting the test, the character must physically interact with the level.

---

# 🧪 Test Requirements

### Required Tests:

*   [ ] **Contract Validation:** `Player` node must exist and have a `CharacterBody2D` component.
*   [ ] **Contract Validation:** `Ground` node must exist and have a `StaticBody2D` component.
*   [ ] **Physics:** Gravity must apply downward force when the player is in mid-air.
*   [ ] **Physics:** Jump velocity must be exactly `-600.0` upon trigger.

---

### Test Type:

*   Use Godot tests ONLY if:
    *   Engine interaction is required (Node, Scene, Physics, Signals) — **Necessary to validate collisions and node hierarchy.**

---

# 📂 Scope

### ✅ Allowed:

*   Creation of nodes `Node2D`, `StaticBody2D`, `CharacterBody2D`, `Sprite2D`, and `CollisionShape2D`.
*   Creation and editing of the main character script.
*   Importing and assigning image assets to sprites.

---

### ❌ Restricted:

*   Complex input configurations outside the standard template.
*   Particle systems or advanced UI.

---

# 🚫 Out of Scope

*   Implementação de animações via `AnimationPlayer`.
*   Criação de múltiplos níveis ou transições de cena.
*   Integração de áudio.

---

# 🔁 Execution Plan (TDD)

1.  Write failing test (verify player existence in scene).
2.  Run tests → MUST fail. (Expected: Scene not found or nodes not found)
3.  Implement minimal code (assemble the node hierarchy).
4.  Run tests → MUST pass.
5.  Refactor safely (adjust collision shapes and script values).

---

# 📊 Progress (Updated)

**current_step**: 2
**last_action**: Design aligned with SDD. Contracts created.
**next_action**: Initialize Tester to write failing integration tests in `tests/`.

---

# 🧠 Decisions

*   **Use of RectangleShape2D:** Chosen to simplify initial collision for both the ground and the player.
*   **Script Adjustment:** Change default value to -600 to ensure jump/movement is noticeable during initial testing.

# ⚠️ Issues

*   None reported so far.

---

# 📚 References

*   Video transcript: "Create A Platformer Game in 20 SECONDS! (Godot 4)".

---

# 🧠 Design Alignment

Linked to:
- **GDD:** `design/gdd.md`
- **Contract:** `design/contracts/basic_movement.feature`

---

# ✅ Definition of Done

*   [ ] `StaticBody2D` and `CharacterBody2D` nodes configured correctly.
*   [ ] Collision shapes adjusted to sprite size. (Covered by test requirement)
*   [ ] Movement script created and "-600" value applied. (Covered by test requirement)
*   [ ] Execution test successfully performed, confirming the player does not fall through the floor. (Covered by test requirement)

---

# 🧾 Final Notes (AFTER DONE)

*(A preencher após a conclusão da tarefa)*
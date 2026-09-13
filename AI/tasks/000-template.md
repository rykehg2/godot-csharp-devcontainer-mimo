# 📌 Task ID: 000-template

status: TODO
priority: MEDIUM
type: FEATURE
created_at: <timestamp>
updated_at: <timestamp>

---

# 🎯 Goal

One clear sentence describing the objective.

> Example:
> Implement player movement logic based on directional input.

---

# 📖 Context

Why this task exists and where it fits in the system.

> Example:
> The player currently does not move.
> This feature enables basic gameplay interaction and is required before implementing combat.

---

# 🧪 Expected Behavior

Describe observable outcomes (inputs → outputs).

> Example:
>
> * Player moves when directional input is applied
> * Player stops when no input is given
> * Movement speed is consistent
> * Movement is frame-rate independent

---

# 🧪 Test Requirements

### Required Tests:

* [ ] Core behavior
* [ ] Edge cases (if applicable)
* [ ] Failure scenarios (if applicable)

> Example:
>
> * Player moves right when input = (1, 0)
> * Player does not move when input = (0, 0)
> * Movement speed remains constant over time

---

### Test Type:

* Default: `.NET tests`
* Use Godot tests ONLY if:

  * Engine interaction is required (Node, Scene, Physics, Signals)

---

# 📂 Scope

### ✅ Allowed:

List exactly what can be modified.

> Example:
>
> * game/Game.Core/Player/**
> * tests/Game.Core.Tests/**

---

### ❌ Restricted:

List what MUST NOT be modified.

> Example:
>
> * design/**
> * docs/**
> * unrelated systems

---

# 🚫 Out of Scope

Explicitly define what should NOT be implemented.

> Example:
>
> * No animation system
> * No physics integration
> * No input system (handled in another task)

---

# 🔁 Execution Plan (TDD)

1. Write failing test
2. Run tests → MUST fail
3. Implement minimal code
4. Run tests → MUST pass
5. Refactor safely

---

# 📊 Progress

current_step: 0
last_action: none

> Example progression:
>
> * step 1 → test created
> * step 2 → test failing confirmed
> * step 3 → minimal implementation added
> * step 4 → tests passing

---

# 🧠 Decisions

Append-only log of important decisions.

> Example:
>
> * Used Vector2 struct for movement
> * Decided to keep logic independent from Godot Node

---

# ⚠️ Issues

Append-only list of problems encountered.

> Example:
>
> * Test failing due to floating point precision
> * Missing reference between test and core project

---

# 📚 References

List useful references.

> Example:
>
> * /examples/csharp/movement
> * /examples/patterns/state-machine
> * /design/mechanics.md

---

# 🧠 Design Alignment

Implementation MUST follow `/design`.

If conflict occurs:

* Update code OR
* Propose design change

---

# ✅ Definition of Done

* [ ] Tests created before implementation
* [ ] Tests fail initially
* [ ] Tests pass after implementation
* [ ] Code follows rules.md
* [ ] No regression introduced
* [ ] state.md updated

---

# 🧾 Final Notes (AFTER DONE)

Summarize what was implemented and why.

> Example:
> Implemented player movement using a pure C# service.
> Logic is decoupled from Godot and fully testable via .NET tests.
> Ready for integration with input system.

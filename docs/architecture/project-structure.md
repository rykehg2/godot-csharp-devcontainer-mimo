# 📂 Project Structure

This document describes the purpose of each directory in the project.

---

# 🧱 Root Structure

```text
.design/          → (see /design)
/design           → Game design and behavior definitions (source of truth)
/docs             → Technical documentation
/examples         → Reference / seed implementations
/AI               → AI execution system (roles, tasks, scripts, logs)
/.devcontainer    → Reproducible development environment
/src              → Live game solution (seeded at postCreate; gitignored)
```

---

# 🎮 /design

Defines how the game should behave.

## Structure

```text
/design/
├── contracts/     → Behavior definitions (BDD .feature files)
├── gdd.md         → Game vision and Phase 1 mechanics
├── mechanics.md   → Gameplay rules
├── systems.md     → System architecture
├── roadmap.md     → Development plan
├── decisions.md   → Architectural decisions
├── review.md      → Reviewer handoff notes
```

---

## 📜 contracts/

Defines expected behavior using BDD-style rules.

Example:

* Given X → expect Y

👉 This is the **source of truth for behavior**

---

# 📚 /docs

Technical documentation.

## Structure

```text
/docs/
├── api/           → External API contracts
├── architecture/  → System design documentation
├── integrations/  → Integration details
├── godot-start/   → Godot start notes/snippets
```

---

# 🧪 /examples

Reusable patterns and reference code.

## Structure

```text
/examples/
├── godot/         → Godot seed (godot-csharp-decoupled → copied into src/)
├── csharp/        → C# examples
├── patterns/      → Design patterns
```

## Purpose

* Avoid reinventing solutions
* Provide reference implementations
* Seed `src/` on first container create
* Guide AI and developers

---

# 🎮 /src

Live working tree for the game (not committed; created by `.devcontainer/postCreate.sh`).

## Structure (current seed)

```text
/src/
├── Game.sln                 → Central .NET solution
├── GameLogic/               → Pure C# (no Godot)
│   ├── Interfaces/
│   ├── Services/
│   ├── Rules/
│   ├── Models/
│   └── ServiceLocator.cs
├── GameGodot/               → Godot engine layer
│   ├── project.godot
│   ├── scenes/              → Nodes + thin C# proxies
│   ├── tests/               → GDUnit4 (res://tests/)
│   └── addons/gdUnit4/      → Installed by postCreate
└── GameLogic.Tests/         → xUnit for GameLogic
```

## Architecture rule (keep when rewriting for the platformer GDD)

* Gameplay math and rules live in **GameLogic** (xUnit-tested).
* **GameGodot** nodes only read input, call services, and apply motion/collision.
* DI via **ServiceLocator** registered in `Main._Ready()`.
* Seed classes/nodes may be rewritten; this layering must not be broken.

---

# 🤖 /AI

AI execution system.

## Structure

```text
/AI/
├── context.md      → Operational rules for agents
├── rules.md        → Global constraints
├── rules-lite.md   → Short rules
├── agent_mode.md   → Role & mode router
├── agents/         → Role bootstraps (planner, architect, tester, developer, reviewer)
├── states/         → Per-role memory / handoff
├── script/         → xunit.sh, gdunit.sh, validate.sh, task-*.sh, handoff.sh
├── prompts/        → Reusable prompts
├── task.md         → Active task pointer
├── state.md        → Execution memory
├── tasks/          → Task files (000-template, 001-…)
└── logs/           → Test/validate logs (gitignored)
```

---

## Responsibilities

* Control AI execution
* Define workflow (assembly line roles)
* Track progress via tasks + state
* Anchor validation in CLI scripts

---

# 🐳 /.devcontainer

Development environment configuration.

## Structure

```text
/.devcontainer/
├── Dockerfile
├── devcontainer.json
├── postCreate.sh
├── run_post_create.sh
├── gdunit.sh
├── tests/
└── tmp/            → postCreate.log (runtime)
```

---

## Purpose

* Reproducible environment (Fedora 41, .NET 10, Godot 4.6.2 Mono, GDUnit4)
* CLI-based execution
* Headless development
* Seeds `src/` and installs GDUnit4 on first create

---

# 🎯 Key Principles

* Clear separation of concerns
* Behavior defined before implementation (`design/contracts`)
* Code must follow design
* AI must follow context rules
* `src/` layout is the operational truth for builds/tests

---

# 🚀 Summary

| Folder           | Responsibility           |
| ---------------- | ------------------------ |
| /design          | Behavior definition      |
| /docs            | Technical reference      |
| /examples        | Seed + reusable patterns |
| /src             | Live game code           |
| /AI              | AI execution & tasks     |
| /.devcontainer   | Environment              |

👉 This structure ensures scalability and clarity.

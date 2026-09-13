# 🤖 AI Context – Godot C# TDD

This file defines how any AI agent should interact with this project.

# 🛠 Technology Stack
- **Engine:** Godot 4.6.2 (C# Mono)
- **Runtime:** .NET 8.0
- **Test Frameworks:** xUnit (.NET logic), GDUnit4 (Godot scenes/nodes)
- **Environment:** Fedora 41 Dev Container (Headless)

# 🚀 Critical Commands
- **Build:** `dotnet build game/GameSolution.sln`
- **Test (.NET):** `dotnet test game/GameSolution.sln`
- **Test (Godot):** `bash AI/gdunit.sh -a res://test/`
- **Run ALL validation:** `dotnet build && dotnet test && bash AI/gdunit.sh -a res://test/`

# 📝 Learned Lessons (Troubleshooting)
* *C# Sync:* After adding new nodes with scripts, `dotnet build` is mandatory before running Godot tests.
* *CLI Testing:* Always use the `--headless` flag when running Godot directly if not using the helper scripts.
* *Solution Path:* Always run `dotnet` commands pointing to `game/GameSolution.sln` when at the project root to avoid MSB1003.

---

# 🧠 Pre-Implementation Checklist

Before writing code:

1. Check `/design/contracts` → expected behavior
2. Check `/examples` → reusable patterns
3. Check `/docs` → external constraints

Rules:

* Prefer reuse over new code
* Do not reinvent existing solutions
* Do not guess behavior

---

# 🎯 Objective

* Maintain a clean, testable, and modular codebase
* Follow **Test-Driven Development (TDD)**
* Ensure all changes are verifiable via CLI
* Keep the project runnable in a headless environment

---

# 📜 Contract Authority Rule

/design/contracts.md defines the expected system behavior.

Tasks must:

* Implement contracts
* Or extend contracts (when adding new behavior)

Tasks must NOT:

* Contradict existing contracts
* Introduce behavior not defined in contracts without updating them

---

# 📌 Task System (CRITICAL)

## Active Task

The current task is defined in:

```text
AI/task.md
```

Which points to:

```text
AI/tasks/<task-file>.md
```

---

## Task Rules

* ONLY work on the active task

* DO NOT create new tasks unless instructed

* DO NOT modify:

  * Goal
  * Scope
  * Expected Behavior

* You MAY update:

  * Progress
  * Decisions
  * Issues
  * status

---

## 🚨 Failure Protocol
If a step fails, the agent is responsible for the fix. 
1. Analyze the CLI error log.
2. Fix the code.
3. Update the "Learned Lessons" section in this file if the error is environment-related.

---

# 🔌 Interaction Strategy

The developer can switch between:
1. **Manual Chat:** Follow the prompt suggestions at the bottom of responses.
2. **CLI Agent:** Run `bash AI/scripts/aider-task.sh` for automated TDD iterations.

Regardless of the mode, the **Task System** remains the source of truth for progress.

## Execution Model

* Execute **ONE STEP per iteration**
* NEVER complete the entire task at once
* ALWAYS update task.md and state.md after each step

---

# 🔄 Contract-Driven TDD Flow


---

# 🧠 Pre-Implementation Rules

Before writing code:

1. Check `/examples` for similar implementations
2. Reuse patterns whenever possible
3. Avoid reinventing solutions
4. Check `/design` for expected behavior
5. Check `/docs` for external contracts

---

# ⚙️ Environment

## Run project

```bash
godot --headless --path game
```

---

## Run tests (.NET - FAST)

```bash
dotnet test
```

Used for:

* Business logic
* Pure C# code
* Algorithms
* Game rules

---

## Run tests (Godot - INTEGRATION)

```bash
bash AI/gdunit.sh -a res://test/
```

Used for:

* Scenes
* Nodes
* Signals
* Physics
* Engine behavior

---

## Run ALL tests

```bash
dotnet test && godot --headless --path game -s addons/gdUnit4/bin/GdUnitCmdTool.gd -a run
```

---

## Build

```bash
dotnet build
```

---

## Run validation

```bash
dotnet build && dotnet test && godot --headless --path game -s addons/gdUnit4/bin/GdUnitCmdTool.gd -a run
```

---

# 🧪 Testing Strategy

## 🧠 .NET Tests (PRIMARY)

* Default choice
* Fast feedback loop
* Used for most features

---

## 🎮 Godot Tests (SECONDARY)

* Only when engine interaction is required
* Slower but validates real behavior

---

# 🧪 Testing Rules (TDD)

* ALWAYS write tests before implementation (after contract is defined)
* ALWAYS start with .NET tests
* ONLY use Godot tests when necessary
* NEVER implement without a failing test

Tests must:

* Fail before implementation
* Pass after implementation

---

## Test Quality Rules

* Avoid trivial tests
* Focus on behavior, not implementation
* Tests must provide real value

---

# 🧠 Test Selection Rule

* Default → .NET tests
* Use Godot tests ONLY for engine behavior

---

# 🧱 Code Guidelines

* Prefer small, modular classes and functions
* Avoid tight coupling
* Use signals instead of direct dependencies
* Keep gameplay logic in C#
* Separate:

  * Scene (Godot)
  * Logic (C#)

---

## Simplicity Rule

* Avoid overengineering
* Do not introduce abstractions without need
* Prefer simplest working solution

---

# 🔁 XP Loop

1. Write .NET test (FAIL)
2. Run `dotnet test`
3. Implement minimal code (PASS)
4. Run tests again
5. Refactor
6. If needed → add Godot test

---

# 🔁 Execution Flow (TASK-DRIVEN)

For each iteration:

### 1. Read

* context.md
* rules.md
* task.md
* state.md
* active task file

---

### 2. Analyze

* Understand goal
* Check scope
* Identify current step

---

### 3. Execute ONE step

Examples:

* Create test
* Run tests
* Implement minimal logic
* Refactor

---

### 4. Validate

* Run tests
* Confirm expected result

---

### 5. Update

Update:

* task (Progress, Decisions, Issues, status)
* state.md

---

### 6. Stop

Do NOT continue to next step automatically

---

# 🚨 Constraints

* Do NOT break existing functionality
* Do NOT remove tests
* Do NOT introduce unused code
* Do NOT hardcode values unnecessarily
* Do NOT assume behavior — verify via CLI
* Do NOT create files or folders outside defined project structure
* Do NOT invent paths that do not exist
* Always verify structure before writing files

---

# 🎯 Scope Control

# 🎯 Authority Order (CRITICAL)

1. Contracts (/design/contracts) → define system behavior
2. Task → defines what to implement now
3. Tests → validate behavior

---

## Rules

* Tasks must follow contracts
* If task conflicts with contracts:

  - Update contract FIRST
  - Then proceed with task

* Never implement behavior not defined in contracts

Do NOT:

* Add extra features
* Anticipate future requirements
* Expand scope

---

# 📂 Project Knowledge

## Design

`/design` defines game behavior.

If implementation conflicts:

* Update code
* OR propose design change

---

## Documentation

`/docs` defines:

* APIs
* Architecture
* Integrations

Never guess external behavior.

---

## Examples

`/examples` contains reusable patterns.

* Always check before implementing
* Prefer reuse over new code

---

# 🔄 Design Sync Rule

If code diverges from `/design/contracts`:

* Update code to match contracts
* OR explicitly update contracts first

Never:

* Modify code without updating contracts
* Diverge silently

---

# 🧠 Agent Behavior

* Think step-by-step
* Prefer execution over assumptions
* Use CLI to validate
* Keep changes minimal

---

# ⚠️ Important

* Never skip failing test step
* Never assume correctness
* Always update state.md

---

# 🧪 Validation Checklist

Before finishing:

* [ ] Code compiles
* [ ] Tests pass (.NET)
* [ ] Tests pass (Godot if needed)
* [ ] No runtime errors
* [ ] No unused imports
* [ ] No regressions

---

# 📌 Notes

* This project is designed for **AI-assisted execution**
* All actions must be reproducible via CLI

# 🤖 AI Context – Godot C# TDD

This file defines how any AI agent should interact with this project.

# 🛠 Technology Stack
- **Engine:** Godot 4.3+ (C# Mono)
- **Runtime:** .NET 10.0
- **Test Frameworks:** xUnit (.NET logic), GDUnit4 (Godot scenes/nodes)
- **Environment:** Fedora 41 Dev Container (Headless)

# 🚀 Critical Commands
- **Build:** `dotnet build src/GameSolution.slnx`
- **Test (.NET):** `bash AI/script/xunit.sh`
- **Test (Godot):** `bash AI/script/gdunit.sh -a res://tests/`
- **Run ALL validation:** `bash AI/script/validate.sh`

# 📂 Project Structure
**src/** (Main Source & Solution)
- `GameSolution.slnx`: Arquivo de solução central do .NET 10.
- **Game.Core/**: Lógica de domínio (sem dependência de Godot).
  - `Domain/`: Entidades e regras de negócio.
  - `Services/`: Serviços de domínio e algoritmos.
  - `ValueObjects/`: Estruturas de dados imutáveis.
- **Game.Godot/**: Camada da Engine.
  - `Scenes/`: Cenas e hierarquia de nodes.
  - `Scripts/`: Lógica de Nodes (`Nodes/`), UI (`UI/`) e Autoloads (`Managers/`).
- **xunitTests/**: Projetos de testes unitários técnicos para o Core.

**AI/**: Prompts, tarefas e logs operacionais.
**design/**: Contratos de comportamento e GDD (Fonte da Verdade).

# � Learned Lessons (Troubleshooting)
* *C# Sync:* After adding new nodes with scripts, `dotnet build` is mandatory before running Godot tests.
* *CLI Testing:* Always use the `--headless` flag when running Godot directly if not using helper scripts.
* *Solution Path:* Always run `dotnet` commands pointing to `src/GameSolution.slnx` to avoid MSB1003.

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

# 🔌 Interaction Strategy

### 🤖 Role-Based Execution
This project uses specialized agents defined in `AI/agent_mode.md`. 
Before starting work, the agent must be initialized in a specific role (Architect, Tester, or Developer) and mode (Full or Fast).

### 🛠️ Tools
1. **Manual Chat:** Role selection via `AI/agent_mode.md`.
2. **CLI Agent:** Automated iterations via **OpenCode** (`opencode`).
3. **Context Management:** **context-mode** utility to gather project state and rules for the LLM.
4. **Automation Scripts:** Helper scripts in `AI/script/` (xunit.sh, validate.sh, task-new.sh).
5. **Source of Truth:** The **Task System** (`AI/tasks/`) remains the master record of progress regardless of the agent role.

### 🤖 Chat vs CLI Mode
* **CLI Mode (OpenCode):** The agent executes scripts and manages files autonomously.
* **Chat Mode (Gemini Code Assist):** The agent acts as a **Technical Director**. 
    * The Agent provides the **Diffs** and **Commands**.
    * The User applies changes and reports back with terminal output.
    * The Agent must never say "I updated the file"; it must say "Please apply this diff to the file".

## Execution Model

* Execute **ONE STEP per iteration**
* NEVER complete the entire task at once
* ALWAYS update task.md and state.md after each step

---
# 🚨 Atomic Planning Rule (CRITICAL)
An Architect MUST NOT create or update a Contract (`design/contracts/*.feature`) without:
1. Creating/Updating the corresponding Task file (`AI/tasks/*.md`).
2. Updating the Active Task pointer in `AI/task.md`.
3. Linking the Task ID within the Contract's metadata.
Failure to do all three simultaneously is considered an execution error.

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
godot --headless --path src/Game.Godot
```

---

## Run tests (.NET - FAST)

**Script:** `bash AI/script/xunit.sh`

Used for:
* Business logic
* Pure C# code (Logs saved to `AI/logs/last_xunit_test.log`)

---

## Run tests (Godot - INTEGRATION)
**Script:** `bash AI/gdunit.sh -a res://tests/`
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

**Script:** `bash AI/script/validate.sh`
Performs full build, runs xUnit, and runs GDUnit. Logs saved to `AI/logs/full_validation.log`.

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

# 🔁 Assembly Line Flow (Role-Based)

1. **Planner:** Discovery with PO → Updates `design/gdd.md` and `roadmap.md`.
2. **Architect:** Contract formalization (ensuring tasks are linked in contracts) → Decomposes into `AI/tasks/XXX.md` (via `AI/script/task-init.sh`).
3. **Tester (Red):** Reads Task → Writes failing tests → Logs failure in `AI/logs/`.
4. **Developer (Green):** Reads Task + Logs → Implements minimal code → Verifies local pass.
5. **Tester (Done):** Runs full suite (`validate.sh`) → Final validation against Contracts → Marks Task as DONE.
6. **Reviewer:** Audits handoff → Compresses context into `design/review.md` → Cleans agent states.

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
* state.md (Include timestamp using `date +"%Y-%m-%d %H:%M:%S"`)
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

# 🤖 Godot C# AI Dev Environment (XP + TDD)

> Execution-driven development environment powered by AI + TDD

## 📄 License

This project is licensed under the **GNU General Public License v3.0** (GPL-3.0).  
See the [LICENSE](LICENSE) file for details.

---

# 🚀 Quick Start (TL;DR)

### 🛠️ Prerequisites
Before starting, ensure you have the following installed on your host machine:
- **Docker Desktop** (or Docker Engine on Linux).
- **VS Code**.
- **Dev Containers Extension** (ms-vscode-remote.remote-containers).
- **Godot Engine (4.x Mono/C#)**: Required locally for editor access and UI/Editor testing.

### ⚙️ How to Start
1. Open this project folder in **VS Code**.
2. Press `F1` (or `Ctrl+Shift+P`) and select: **Dev Containers: Reopen in Container**.
3. Ensure your `GEMINI_API_KEY` or `ANTHROPIC_API_KEY` is set in your host environment or a `.env` file for AI features. (Not mandatory).
4. Use your preferred AI chat code assist (Copilot, Gemini, Qwen, Cline, Continue...) or the OpenCode CLI agent.
5. **CLI Shortcuts:** Use `opencode` for the AI agent or `context-mode .` to refresh LLM context.

### 🧪 Initial Validation
Inside the container terminal, run:
```bash
dotnet test src/Game.sln && bash AI/script/validate.sh
```

* IDEs errors like "GDScript server not connected" are expected.

---

# 🧯 Troubleshooting (known issues when starting/running)

## 1. `dotnet test` at the workspace root fails (MSB1003)
The solution lives in `src/` — there is no `.sln` at the root. Run `dotnet test src/Game.sln` (see Initial Validation) or `cd src && dotnet test`.

## 2. `$'\r': command not found` in `.devcontainer/tmp/postCreate.log` (Windows CRLF)
The shell scripts must stay LF; a Windows checkout can silently convert them to CRLF and `bash` breaks (`$'\r': command not found`, `syntax error: unexpected end of file`), aborting the postCreate step. Fix once, then reopen the container:

```bash
git config core.autocrlf input
printf '* text=auto\n*.sh text eol=lf\n' > .gitattributes
git add --renormalize .
```

The current working tree already ships LF scripts.

## 3. GDUnit4 C# tests fail to compile: `CS0246 'TestSuiteAttribute' / 'TestCaseAttribute' could not be found`
`GameGodot.csproj` must reference the addon C# project — it brings the `gdUnit4.api` NuGet package that defines the C# test API (and builds `GdUnit4.dll`):

```xml
<ProjectReference Include="addons\gdUnit4\gdUnit4.csproj" />
```

The seed (`examples/godot/godot-csharp-decoupled`) already includes this reference; re-add it only if `src/` is re-seeded manually.

## 4. `GdUnit0501` analyzer error, or `IPlayerMovementService` not found
Godot-using tests require `[RequireGodotRuntime]`, and the game interfaces namespace must be imported:

```csharp
using GameLogic.Interfaces;

[RequireGodotRuntime]
[TestCase]
public void Should_Instantiate_Main_Scene_And_Nodes() { ... }
```

The seeded test already ships both fixes.

## 5. postCreate prints "Environment ready" even when Godot tests failed
By design `postCreate.sh` runs the Godot suite with `|| true` so the container startup never blocks on test failures. Confirm the real state with:

```bash
tail -n 30 .devcontainer/tmp/postCreate.log
tail -n 30 AI/logs/last_gdunit_test.log
bash AI/script/validate.sh   # build + xUnit + GDUnit4 — fails loudly on error
```

## 6. OpenCode CLI works but there is no reachable model
Use free OpenCode Zen models (no credentials required at the moment — `opencode providers list` reports 0 credentials and `opencode run` still works):

```bash
opencode run --model opencode/nemotron-3.5-lightning-free "your prompt"
opencode models   # catalog — the *-free / big-pickle entries cost $0 (limited time; may use prompts for training)
```

Or point `opencode.json` at a reachable OpenAI-compatible server, or login with your own key (`opencode providers login`).

---

# 🧠 How to work on this project

1. **Initialize the Agent:** Choose a role and mode from `AI/agent_mode.md` (e.g., "Initialize as Architect in FULL mode").
2. **Follow the Assembly Line:**
   * **Planner:** Discovery with PO → Updates `design/gdd.md` and `roadmap.md`.
   * **Architect:** Refines `design/contracts` (linking tasks) and decomposes features into `AI/tasks/XXX.md`.
   * **Tester (Red):** Writes failing tests and logs results in `AI/logs/`.
   * **Developer:** Implements minimal logic to turn the tests green locally.
   * **Tester (Done):** Final validation against contracts → Marks Task as DONE.
   * **Reviewer:** Final Audit → Compresses context and updates `design/review.md`.
3. **Source of Truth:** 
   * `AI/task.md` points to the active task.
   * `AI/context.md` defines the technical rules.
   * `design/contracts/` defines the behavioral requirements.

👉 Everything is guided by **Specialized Agents + TDD + Tasks**

---

## 🛠 Validation Loop & Feedback Cycle (SDD)

This project strictly adheres to **Spec-Driven Development (SDD)** principles. In this workflow, the specification is the single source of truth, and code is treated as a secondary artifact derived from it. To ensure system integrity when working with AI agents, we implement a rigorous feedback loop:

### 1. Validation Scripts as Reality Anchors
Consistent with the "Clean Code for AI Agents" philosophy, AI output must be validated by deterministic tools. Our automated scripts close the gap between generation and correctness:
* `AI/script/validate.sh`: Performs static analysis and contract integrity checks.
* `AI/script/gdunit.sh` & `xunit.sh`: Ensure that C#/Godot implementations satisfy technical requirements without hallucinations.

### 2. Spec-First & TDD Workflow
No agent is authorized to write implementation code unless:
1.  The contract in `design/contracts/` has been validated.
2.  Unit tests (TDD) are defined and failing.
3.  The validation scripts provide the final "green light" for task completion.

> **Golden Rule:** Task success is not defined by how "correct" the code looks, but by passing the validation scripts. This eliminates semantic noise and focuses on technical precision.

---

# 📂 Main structure

```
.devcontainer/   → Reproducible environment (Docker)
src/             → Game + .NET solution (seeded at postCreate from examples/godot/godot-csharp-decoupled)
                  — src/GameGodot, src/GameLogic, src/GameLogic.Tests + src/Game.sln
AI/              → AI system (agents, context, tasks, specialized states, scripts)

design/          → Game rules (source of truth, Dev work here)
docs/            → APIs and architecture
examples/        → Reusable references for IA or Dev (seed source for src/)
```

---

# 🔄 Development flow

1. **Planner:** Discovery with PO → Updates `design/gdd.md` and `roadmap.md`.
2. **Architect:** Contract formalization (linking tasks) → Decomposes into `AI/tasks/XXX.md` (via `AI/script/task-init.sh`).
3. **Tester (Red):** Reads Task → Writes failing tests → Logs results in `AI/logs/`.
4. **Developer:** Reads Task + Logs → Implements minimal code → Verifies Green state locally.
5. **Tester (Done):** Final validation against contracts → Marks Task as DONE.
6. **Reviewer:** Audits the handoff → Compresses context into `design/review.md` → Cleans agent states.

---

# 🎯 Philosophy

✔ Behavior
✔ Tests
✔ CLI execution
✔ Small iterations
✔ AI as executor, not decision maker

---

# 📚 Full documentation

---

# 🔄 Update – Dev Container + IA (XP + TDD)

This project implements a complete environment for:

* 🤖 AI-assisted development
* 🧪 Test-Driven Development (TDD)
* 🔄 Extreme Programming (XP) based flow

---

# 🐳 Container (Source of Truth)

## 🎯 Objective

Environment:

* Reproducible
* Headless
* Automated
* IDE independent

> 🔥 Container = source of truth
> IDE = auxiliary tool

---

## ⚙️ Stack

* Fedora 41
* .NET 10
* Godot 4 (mono, headless)
* GDUnit4
* SCons (Godot toolchain)
* OpenCode (AI CLI Agent)
* Native toolchain (gcc, clang, cmake, ninja)

---

## 🧱 Structure

```
.devcontainer/
├── Dockerfile
├── devcontainer.json
├── postCreate.sh
├── run_post_create.sh
└── tests/
```

---

## 🚀 Automatic initialization

When starting the container, the `postCreateCommand` runs `.devcontainer/run_post_create.sh` (→ `postCreate.sh`, log in `.devcontainer/tmp/postCreate.log`) which:

* Seeds the project into `src/` from `examples/godot/godot-csharp-decoupled/` (only if `src/GameGodot/project.godot` is missing)
* Builds the C# Solution `src/Game.sln`: `GameLogic` (core), `GameGodot` (Godot), `GameLogic.Tests` (xUnit)
* Installs GDUnit4 (addon v6.1.3) and wires it as a `ProjectReference` of `GameGodot.csproj`
* Imports Godot resources, restores NuGet, then runs the test suites (xUnit + GDUnit4)

---

## 🔍 Logs

```bash
.devcontainer/tmp/postCreate.log
```

---

# 🤖 AI System

```
AI/
├── context.md      → Operational rules
├── rules.md        → Global constraints
├── agent_mode.md   → Agent Router (Role & Mode definitions)
├── agents/         → Specialized Agent Bootstraps
├── states/         → Specialized agent memory & Handoffs
├── script/         → Automation scripts (handoff, task-init, etc)
├── task.md         → Active task
├── state.md        → Memory
└── tasks/          → Task history
```

---

## 🧠 context.md

Defines:

* How to run the project
* How to test
* TDD rules
* XP flow

---

## 📋 rules.md

Defines:

* Global constraints
* Code quality
* Execution rules

---

## 📌 task.md

Defines:

* What must be done NOW

---

## 🧠 state.md

Stores:

* Progress
* Decisions
* Issues

---

# 🧩 Task System

```
AI/tasks/
├── 000-template.md
├── 001-feature-x.md
├── 002-bugfix-y.md
```

---

## 🔄 Model

Each task follows:

```text
Goal → Expected Behavior → Tests → Implementation → Done
```

---

## ⚠️ Rules

* Work on ONE task at a time
* Do not expand scope
* Update progress continuously

---

# 🎮 Game Design

```
design/
├── gdd.md
├── mechanics.md
├── systems.md
├── roadmap.md
├── decisions.md
└── contracts/
```

---

## 🧠 Source of truth

👉 `/design` defines game behavior

---

## 📜 Contracts (BDD-like)

```
design/contracts/
```

Defines behaviors as:

```text
Given / When / Then
```

---

## ⚠️ Rule

If code ≠ design:

* Fix code
  **OR**
* Update design

Never silently diverge

---

# 📚 Technical documentation

```
docs/
├── api/
├── architecture/
└── integrations/
```

---

## Usage

* External APIs → `/docs/api`
* Architecture → `/docs/architecture`

---

# 🧪 Reusable examples

```
examples/
├── godot/
├── csharp/
└── patterns/
```

---

## Rule

Before implementing:

👉 Check `/examples`

---

# 🧪 Testing Strategy

## 🧠 .NET (PRIORITY)

* Logic
* Rules
* Algorithms

```bash
bash AI/script/xunit.sh
```

---

## 🎮 Godot (SECONDARY)

* Nodes
* Scenes
* Engine

```bash
bash AI/script/gdunit.sh
```

---

# 🔄 XP + TDD Loop

```text
1. Create test (FAIL)
2. Run tests
3. Implement minimum (PASS)
4. Run again
5. Refactor
6. Update state.md
```

---

# 🧠 Benefits

✔ Total reproducibility
✔ Test-driven development
✔ AI with controlled context
✔ Low regression
✔ High predictability

---

# 🚀 Future

* Continuous AI execution
* Distributed tests
* External service integration

---

# 📌 Conclusion

This project is not just a development setup.

> 🧠 It's a complete execution system oriented by AI + TDD + XP

---

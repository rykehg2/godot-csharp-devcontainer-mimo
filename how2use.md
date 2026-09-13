# 🧠 How to use your system in practice (truly simple)

Think of it this way:

> You WON'T use everything all the time.
> You'll use **layers as needed**.

---
# ⚡ Real workflow (day-to-day)

# ⚙️ Initial Setup (Prerequisites & Setup
1.  **Environment:** Open the project in VS Code with the **Dev Containers** extension.
2.  **Initialization:** The `postCreate.sh` script runs automatically to configure C# dependencies and environment variables.
3.  **Verification:** Ensure the `AI/` directory is present, as it contains the logic for the development agents.

---

## 💻 CLI Mode OpenCode - Recommended

For autonomous execution directly in the terminal, use the convenience scripts:

```bash
# Start interactive session with OpenCode:
opencode
/model <-- choose model

# Generate context for external LLM (paste bin):
context-mode .
```

> **Note:** `context-mode` is installed in the container via npm from `/opt/context-mode`.

---

# 🥇 Best way to start

Always start the conversation with:

```
Analyze the project environment, starting with README.md and AI/agent_mode.md.

- Specialized roles: Planner, Architect, Tester, Developer, Reviewer. 
- TDD (Red/Green/Refactor) & Assembly Line Flow. 
- Specialized states in AI/states/ and handoff protocols. 
- Use scripts in AI/script/ for task management and handoffs. 
```
---

## 🥇 Standard Loop (90% of the time)

To start working, choose a role from `AI/agent_mode.md` and send:

```
Initialize as [ROLE] in [MODE] mode. 
Execute next step from AI/task.md.
```

👉 And that's it.

---

## 💬 Real Example

You:

```
Initialize as Developer in FAST mode. 
Task: Fix the jumping gravity in player_movement.md. 
Execute next step.
```

AI will:

* read task
* create test
* run mental logic
* propose code

---

# 🧠 When to use FULL MODE

When planning or audit or when something breaks or becomes complex:

```
Initialize as Planner in FULL mode to discover the "Inventory System".
```

---

# 🔥 Golden Rule

| Situation | Recommended Role | Mode | Useful Script |
| :--- | :--- | :--- | :--- |
| Plan features / Discovery / GDD | **Planner** | 🧠 FULL | `handoff.sh` |
| Create Contracts and Decompose Tasks | **Architect** | 🧠 FULL | `task-init.sh` |
| Adjust details in existing tasks | **Architect** | ⚡ FAST | `handoff.sh` |
| Create new tests (RED Phase) | **Tester** | 🧠 FULL | `xunit.sh` / `gdunit.sh` |
| Validate final solution (DONE Phase) | **Tester** | ⚡ FAST | `validate.sh` |
| Implement logic (GREEN Phase) | **Developer** | ⚡ FAST | `xunit.sh` |
| Context Compression/Auditing | **Reviewer** | 🧠 FULL | `review-audit.sh` |

---

# 🧩 How to use with your `task.md`

You've already made the best choice: **active task + history**

So your workflow becomes:

---

## 1. Define task

The **Architect** creates using

```
AI/task.md
```

Pointing to:

```
AI/tasks/001-player-movement.md
```

---

## 2. Run AI

In ChatGPT:

```
Initialize as [Role] in FAST mode. Execute next step
```

---

## 3. AI responds with something like:

* create test (Tester)
* suggest code (Developer)
* handoff (`AI/script/handoff.sh`)

---

## 4. You execute in the container

```bash
bash AI/script/validate.sh
```

or

```bash
bash AI/script/gdunit.sh -a res://test/
```

## 5. Update state

AI or you update:

```
AI/states/state_[role].md
```

With progress, decisions, and issues.

---

## 6. Repeat

---

# 🚀 Even more practical way (mental shortcut)

You can literally use it like this:

### 🟢 Minimum Prompt

```
Next step (FAST MODE)
```

👉 because everything else is already in the project

---

# 🧠 VERY important tip

You don't need to paste all the files.

👉 The secret is:

* The thinking is structured in Design, Contracts, and States. 
* Use the Scripts to maintain integrity.

---


# 💡 Advanced trick (worth gold)

Create a fixed snippet:

### ⚡ FAST SNIPPET

```
Use FAST MODE
Execute next step from task.md
```

### 🧠 FULL SNIPPET

```
Use FULL MODE
Analyze state and execute next step
```

# ⚠️ Where it can go wrong

Without discipline, this becomes a mess.

Avoid:

❌ Asking one agent to do another's work (e.g., Developer changing GDD)
❌ Skipping tests 
❌ Not updating state.md 
❌ Changing tasks in the middle without recording

---

# 🧠 How you should think now

You are no longer using ChatGPT as a:

> “code generator”

You are using it as:

> **an engineering system executor**

---

# 🔥 Brutally simple summary

Your entire system reduces to this:

## Real Loop:

```
1. Define task (Architect)
2. Prompt: "Initialize as [Role] in [MODE]. Execute next step"
3. Write failing test (Tester - Red)
4. Implement minimal code (Developer - Green)
5. Run tests (validate.sh)
6. Update state (AI/states/state_[role].md)
7. Repeat until DONE
```

---

# Prompts Suggestion
> Initialize as Reviewer in FULL mode. Audit the como_usar.md and how2use.md files to ensure they are 100% consistent with the current project scripts.

>Initialize as Planner in FULL mode. Let's start the discovery for the "Save/Load System" using the roadmap log.
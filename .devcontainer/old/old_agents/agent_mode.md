# 🤖 AI Agent Mode Controller

This file defines how the AI chooses between:

- ⚡ FAST mode (low cost, quick iterations)
- 🧠 FULL mode (deep reasoning, full validation)

---

# 🎯 Objective

Optimize:

- Performance (cost & speed)
- Accuracy (correctness & safety)

---

# 🔌 Interaction Modes

## 💬 CHAT MODE (Human-in-the-loop)
- **Usage:** Copy/Paste context into Browser/IDE Chat.
- **Pros:** High control, user validates every step manually.
- **Cons:** Slower, manual context synchronization.

## 💻 CLI MODE (Autonomous)
- **Usage:** Run `bash AI/script/aider-task.sh` in terminal.
- **Pros:** Repository map awareness, automated TDD loops, direct file system access.
- **Cons:** Requires API credits, requires careful monitoring of "Auto-Apply" logic.

---

# ⚙️ Available Modes

## ⚡ FAST MODE

Use:
- agent_bootstrap_fast.md

Characteristics:
- Minimal context
- Fast execution
- Lower cost
- Higher risk if misused

---

## 🧠 FULL MODE

Use:
- agent_bootstrap_full.md

Characteristics:
- Full context awareness
- Slower execution
- Higher accuracy
- Safer for complex changes

---

## 🔄 SYNC MODES

### TASK ⮕ CONTRACT
Use: `agent_bootstrap_task_to_contract.md`
When: You have an implementation task but need to formalize the design/behavior first.

### CONTRACT ⮕ TASK
Use: `agent_bootstrap_contract_to_task.md`
When: New design/contract is created and needs to be turned into an actionable execution plan.

---

# 🧠 Mode Selection Heuristics

## Default

Start in:

→ ⚡ FAST MODE

---

## Switch to FULL MODE if ANY condition is true:

### 🔧 Complexity

- **Contract/Task Mismatch:** If the Task doesn't have a corresponding Contract.
  → Use 🔄 TASK ⮕ CONTRACT

- **Design First:** If starting from a new requirement in `/design`.
  → Use 🔄 CONTRACT ⮕ TASK

- Task involves multiple files
- Task involves architecture changes
- Task modifies core systems

---

### 🧪 Testing

- Tests are failing unexpectedly
- Flaky or inconsistent tests
- Debugging required

---

### 🔄 Refactoring

- Refactoring existing code
- Improving structure/design
- Removing duplication

---

### 🔗 Integration

- External API integration
- Godot engine interaction
- Signals / scenes / nodes

---

### 📜 Contracts / Design

- Modifies `/design/contracts`
- Introduces new behavior
- Risk of divergence from design

---

### 🚨 Risk

- Possible regression
- Touching shared components
- Unclear requirements

---

## Stay in FAST MODE if ALL conditions are true:

- Single file change
- Clear and isolated task
- No architecture impact
- No external dependencies
- Predictable behavior

---

# 🔁 Dynamic Switching

During execution:

## FAST → FULL

Switch if:

- Tests fail unexpectedly
- Behavior unclear
- Need deeper validation

---

## FULL → FAST

Switch if:

- Problem becomes simple
- Task is now well understood
- Only small steps remain

---

# 🔄 Execution Strategy

## FAST MODE LOOP

1. Load minimal context
2. Execute ONE step
3. Validate quickly
4. Update state
5. Repeat

---

## FULL MODE LOOP

1. Load full context
2. Analyze deeply
3. Execute ONE step
4. Validate thoroughly
5. Ensure consistency (design/docs/code)
6. Update state
7. Repeat

---

# 🧠 Safety Override

If uncertain:

→ ALWAYS choose FULL MODE

---

# 🚨 The Self-Correction Rule

1. **Agent Error?** Do NOT fix the file manually.
2. **Feedback Loop:** Provide the error output to the terminal.
3. **Update Memory:** Force the agent to fix the code AND update `AI/context.md` with a "Learned Lesson" if it's a recurring issue.

---


# 🎯 Priority Rules

1. Correctness > Speed
2. Safety > Cost
3. Simplicity > Cleverness

---

# 📌 Integration with Task System

Before each step:

1. Read `AI/task.md`
2. Evaluate complexity
3. Choose mode
4. Load appropriate bootstrap

---

# 🧠 Example Decisions

## Example 1

Task:
"Add simple damage calculation"

→ ⚡ FAST MODE

---

## Example 2

Task:
"Refactor player movement system"

→ 🧠 FULL MODE

---

## Example 3

Task:
"Tests failing after change"

→ 🧠 FULL MODE

---

## Example 4

Task:
"Create basic class"

→ ⚡ FAST MODE

---

# 🚀 Final Rule

Start fast.
Escalate when needed.
Never stay fast when the system demands depth.
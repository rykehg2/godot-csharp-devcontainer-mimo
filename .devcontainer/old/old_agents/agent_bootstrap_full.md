# 🧠 AI Agent Bootstrap (FULL MODE)

This mode provides full system awareness and maximum correctness.

Use this mode for:
- Complex tasks
- Refactoring
- Architecture decisions
- Debugging

---

# 📦 Load Hierarchy (STRICT ORDER)

1. AI/rules-lite.md   → fast decisions
2. AI/rules.md        → full validation
3. AI/context.md      → execution model
4. AI/task.md         → current objective
5. AI/state.md        → current state
6. AI/logs/**         → history of recent executions and failures
7. design/contracts/** → logic validation source

---

# 🎯 Objective

Execute tasks with:

- Full consistency
- Architectural alignment
- Zero regression

---

# 🧠 Role of Each File

## rules-lite.md
- Quick decisions
- Fast constraints

## rules.md
- Full validation
- Safety rules
- Code quality

## context.md
- Execution model
- TDD + XP flow
- Environment usage

## task.md
- What to do NOW

## state.md
- What has been done

---

# 🔁 Execution Model

Follow strictly:

1. Read all context
2. Understand task
3. Identify current step
4. Execute ONE step
5. Validate via CLI
6. Update task + state
7. STOP

---

# 🧪 TDD Enforcement

- ALWAYS start with failing test
- NEVER implement before failure
- NEVER skip test execution

---

# 🔄 Consistency Enforcement

Ensure alignment between:

- Code
- Tests
- /design (contracts)
- /docs

If mismatch:

- Fix code OR
- Update design

---

# 🚨 Critical Constraints

- Do NOT expand scope
- Do NOT skip steps
- Do NOT batch actions
- Do NOT assume results

---

# 🧠 Decision Model

When unsure:

1. Check /examples
2. Check /design/contracts
3. Check existing code
4. Choose simplest solution

---

# ⚙️ Execution Priority

Correctness > Safety > Speed

---

# ✅ Success Criteria

- Step completed
- Tests executed and validated
- No regression
- State updated

---

# 🧠 Philosophy

Slow is smooth.
Smooth is fast.

Correct iterations → stable system → scalable AI workflow
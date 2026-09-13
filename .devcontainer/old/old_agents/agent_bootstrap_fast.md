# ⚡ AI Agent Bootstrap (FAST MODE)

This is the minimal bootstrap for fast AI execution.

Use this mode for:
- Quick iterations
- Low-cost inference
- Small steps (XP loop)

---

# 📦 Load Order (STRICT)

1. AI/rules-lite.md
2. AI/context.md (PARTIAL)
3. AI/task.md
4. AI/logs/last_gdunit_test.log (If relevant to task)
5. AI/logs/last_dotnet_test.log (If relevant to task)

---

# 🎯 Objective

Execute the current task with:

- High precision
- Minimal cost
- Small iterations

---

# 📜 Context (PARTIAL LOAD)

From `context.md`, focus ONLY on:

- Task System
- Execution Flow (TASK-DRIVEN)
- Testing Strategy
- Constraints
- Validation Checklist

Ignore:

- Long explanations
- Background philosophy
- Non-critical sections

---

# 📌 Execution Rules

- Execute ONLY ONE step
- Do NOT complete the full task
- Always follow TDD:
  - Write failing test
  - Implement minimal code
  - Validate

---

# 🚨 Hard Constraints

- Do NOT expand scope
- Do NOT invent behavior
- Do NOT skip test execution

---

# 🔁 Loop

1. Read task.md
2. Identify current step
3. Execute ONE action
4. Run tests
5. Update task.md + state.md
6. STOP

---

# ✅ Success Criteria

- Step completed
- Tests executed
- State updated

---

# 🧠 Philosophy

Speed > Completeness

Small steps → validated → safe evolution
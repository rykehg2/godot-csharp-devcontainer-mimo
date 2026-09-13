# 🔄 AI Agent Bootstrap (TASK ⮕ CONTRACT)

Objective: Formalize behavior by generating or updating a Contract based on a Task.

---

# 📦 Load Order

1. AI/rules-lite.md
2. AI/context.md
3. AI/task.md (The source)
4. design/contracts/** (The target)

---

# 🎯 Execution Logic

1. **Analyze Task:** Extract technical goals and expected behavior from the active task.
2. **Check Existing Contracts:** Look for a matching file in `design/contracts/`.
3. **Identify Gaps:** Find behaviors described in the Task that are NOT yet formalized in Gherkin (Given/When/Then).
4. **Update/Create Contract:** 
   - Use Gherkin syntax.
   - Focus on "WHAT" (behavior), not "HOW" (implementation).
5. **Verify Alignment:** Ensure the Task's "Expected Behavior" section matches the Contract.

---

# 📜 Rules

- Do NOT write code.
- Do NOT update state.md yet.
- ONLY modify/create files in `design/contracts/`.
- Ensure the contract remains implementation-agnostic.

---

# ✅ Success Criteria

- Contract exists in `design/contracts/`.
- Contract contains Scenarios matching the Task's goals.
- Task and Contract are 100% aligned.
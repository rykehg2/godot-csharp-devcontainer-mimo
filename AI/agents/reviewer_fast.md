# 🧐 Reviewer Agent (FAST MODE)

Objective: Quick context summarization and handoff verification.

---

# 📦 Load Hierarchy
1. AI/rules-lite.md
2. AI/states/state_[active_role].md
3. design/review.md

---

# 🎯 Execution
1. Summarize the last completed task into `design/review.md`.
2. Validate the `READY` status of a handoff.
3. Suggest context removal for the next phase.
4. Update `AI/states/state_reviewer.md`.

---

# 🛠️ Skills (Scripts)
- `bash AI/script/review-audit.sh`: Quick overview of the current pulse.
- `bash AI/script/handoff.sh [role] "[message]"`: Fast status updates.
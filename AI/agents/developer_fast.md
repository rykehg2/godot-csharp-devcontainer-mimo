# 💻 Developer Agent (FAST MODE)

Objective: Quick implementation iteration and bug-fixing.

---

# 📦 Load Hierarchy
1. AI/rules-lite.md
2. AI/task.md
3. Relevant C# file in `game/`
4. AI/states/state_developer.md
5. AI/states/state_tester.md

---

# 🎯 Execution
1. Apply logic change.
2. Run `dotnet test`.
3. Update `AI/states/state_developer.md`.
4. If green, signal `READY` in the handoff of `AI/states/state_tester.md`.

---

# 🛠️ Skills (Scripts)
- `bash AI/script/xunit.sh`: Fast logic check.
- `bash AI/script/validate.sh`: Quick full project validation.
- `bash AI/script/handoff.sh tester "[message]"`: Signal ready for final validation.

---

# � Rules
- Limit responses to direct updates in `game/`, `AI/states/state_developer.md` and `AI/states/state_tester.md`.
# 🧠 Knowledge Map

AI/          → process authority, specialized agent roles, and tasks
design/      → source of truth for gameplay mechanics and contracts
docs/        → technical specs and architecture
examples/    → implementation reference and patterns
src/        → production code (implementation)
tests/       → behavioral validation (xUnit and GDUnit)

## Source of Truth Priority (IMPORTANT)

When there is conflicting information, follow this priority:

1. AI/rules.md & AI/agents/ (The "How")
2. design/contracts/ (The behavioral "What")
3. design/ (General GDD)
4. docs/
5. examples/
6. existing code

## 🤖 Agent Role Mapping
- **Architect:** Owns `design/` and `AI/tasks/`.
- **Tester:** Owns `tests/`, `AI/logs/`, and Task Work Logs.
- **Developer:** Owns `game/`.

Never override design decisions or process rules without explicit justification in `state.md`.

Never override design decisions without explicit justification.
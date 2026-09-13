#  AI Agent Router

This file defines the specialized agent roles and the corresponding bootstrap files to be used. Use this as a reference to switch between operational modes.

---

### 📅 Planner (Project Management)
*Focus: Project discovery, GDD expansion, phase planning, and PO interviews.*
- **FULL MODE:** `AI/agents/planner_full.md`
- **FAST MODE:** `AI/agents/planner_fast.md`

### 🧐 Reviewer (Quality & Context Audit)
*Focus: Handoff auditing, integrity check (GDD -> Contract -> Task), and context compression.*
- **FULL MODE:** `AI/agents/reviewer_full.md`
- **FAST MODE:** `AI/agents/reviewer_fast.md`


## 👥 Specialized Roles

### 🏛️ Architect (Planner)
*Focus: System design, Contract formalization, and MANDATORY Task linking/decomposition.*
- **FULL MODE:** `AI/agents/architect_full.md`
- **FAST MODE:** `AI/agents/architect_fast.md`

### 🧪 Tester (TDD Guardian)
*Focus: Writing failing tests, executing test suites, and logging results.*
- **FULL MODE:** `AI/agents/tester_full.md`
- **FAST MODE:** `AI/agents/tester_fast.md`

### 💻 Developer (Executor)
*Focus: Implementing minimal code to pass tests and refactoring.*
- **FULL MODE:** `AI/agents/developer_full.md`
- **FAST MODE:** `AI/agents/developer_fast.md`

---

## 🚀 Selection Logic

To initialize an agent, provide the following command to the AI Chat:

> "Initialize as **[ROLE]** in **[MODE]** mode."

**Example:**
*"Initialize as Architect in FULL mode to plan the inventory system."*

---

## 🚨 Workflow Rule
1. **Planner:** Discovery with PO → Updates `design/gdd.md` and `roadmap.md`.
2. **Architect:** Contract formalization (linking tasks) → Decomposes into `AI/tasks/XXX.md`.
3. **Tester (Red):** TDD Red Phase → Writes failing tests and updates `AI/logs/`.
4. **Developer (Green):** TDD Green Phase → Implements code until tests pass locally.
5. **Tester (Done):** Final validation against contracts → Marks Task as DONE.
6. **Reviewer:** Final Audit → Compresses context and updates `design/review.md`.
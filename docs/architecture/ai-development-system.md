# 🧠 AI Development System

This project uses a structured AI-assisted development model combining:

- **BDD (Behavior-Driven Development)** → via contracts
- **XP (Extreme Programming)** → via iterative workflow
- **TDD (Test-Driven Development)** → via automated tests
- **AI Execution** → controlled and deterministic

---

# 🎯 Core Idea

Instead of letting AI decide what to build:

- Behavior is defined explicitly
- Scope is controlled
- Execution is deterministic

---

# 🧩 System Components

## 📜 Contracts (BDD)

Location:

/design/contracts/

Defines **expected system behavior** using a BDD-like format.

Example:

- Given no input → player does not move
- Given right input → player moves right

👉 This is the **source of truth for behavior**

---

## 📌 Tasks (Execution Scope)

Location:

/AI/tasks/

Defines:

- What to implement now
- Scope restrictions
- Expected behavior (subset of contracts)

👉 Tasks do NOT define global behavior  
👉 Tasks implement or extend contracts

---

## 🧪 Tests (TDD)

Executed via:

```bash
dotnet test
````

Responsible for:

* Validating behavior
* Enforcing correctness

Rules:

* Must fail before implementation
* Must pass after implementation

---

## 💻 Code

* Implements behavior defined in contracts
* Must be minimal and test-driven

---

## 🧠 State

Location:

/AI/state.md

Tracks:

* Progress
* Decisions
* Issues

---

# 🔗 Relationship Between Components

```text
Contracts → define behavior
Task → defines scope
Tests → validate behavior
Code → implements behavior
State → tracks execution
```

---

# 🔁 Development Flow

```text
1. Define behavior (contracts)
2. Create task (scope)
3. Execute TDD loop:

   - Write test (FAIL)
   - Run tests
   - Implement minimal code (PASS)
   - Run tests again
   - Refactor

4. Update state
5. Repeat
```

---

# 👨‍💻 Responsibilities

## Developer

* Defines contracts
* Defines tasks
* Reviews results
* Maintains design consistency

---

## AI Agent

* Executes TDD loop
* Writes tests
* Implements code
* Updates state

---

# 📜 Core Rules

* Contracts are the **source of truth**
* Tasks define **scope only**
* AI must NOT invent behavior
* All validation must be done via CLI

---

# ⚠️ Important

If behavior is missing:

1. Update contracts
2. Then create task
3. Then implement

Never skip this order.

---

# 🚀 Goal

Create a:

> Deterministic, testable, and AI-executable development system
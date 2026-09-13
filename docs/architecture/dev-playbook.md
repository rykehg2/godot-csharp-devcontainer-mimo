# 🧠 Dev Playbook

This playbook defines how to work within this project.

---

# 🚀 Core Workflow

```text
Contracts → Task → Tests → Code → Refactor → State Update
````

---

# 🆕 Adding a New Feature

## Step-by-step

1. Define behavior in:

/design/contracts/

Example:

* Given input → expected output

---

2. Create a task:

/AI/tasks/

Define:

* Goal
* Scope
* Expected behavior

---

3. Activate task:

/AI/task.md

---

4. Execute TDD loop:

* Write test (FAIL)
* Run tests
* Implement minimal code (PASS)
* Run tests again
* Refactor

---

5. Update:

* task progress
* state.md

---

# 🐛 Fixing Bugs

## Steps

1. Reproduce the issue
2. Write a failing test
3. Run tests → confirm failure
4. Implement minimal fix
5. Run tests → confirm pass
6. Refactor if needed

---

# ♻️ Refactoring

## Rules

* Only refactor if tests exist
* Do not change behavior
* Run tests after changes

---

# 🔍 Debugging

## Process

1. Read error logs carefully
2. Identify root cause
3. Apply minimal fix
4. Re-run tests

Avoid guess-based fixes.

---

# 🧠 Working with AI

## DO

* Provide clear tasks
* Define behavior in contracts
* Validate results via CLI

---

## DO NOT

* Let AI define behavior
* Skip test execution
* Accept unvalidated results

---

# 🎯 Best Practices

* Small iterations
* One step at a time
* Always validate
* Keep code simple

---

# ⚠️ Important Rules

* Contracts are the source of truth
* Tasks define scope
* Tests validate behavior
* AI executes, not decides

---

# 🏁 Definition of Done

A task is complete when:

* Tests exist
* Tests fail before implementation
* Tests pass after implementation
* Code follows design
* state.md is updated

---

# 🚀 Goal

Ensure:

* Predictable development
* High-quality code
* Safe AI collaboration

```

---
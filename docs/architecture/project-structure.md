# 📂 Project Structure

This document describes the purpose of each directory in the project.

---

# 🧱 Root Structure

```text
/design        → Game design and behavior definitions
/docs          → Technical documentation
/examples      → Reference implementations
/IA            → AI execution system
/devcontainer  → Development environment
````

---

# 🎮 /design

Defines how the game should behave.

## Structure

```text
/design/
├── contracts/     → Behavior definitions (BDD)
├── gdd.md         → Game vision
├── mechanics.md   → Gameplay rules
├── systems.md     → System architecture
├── roadmap.md     → Development plan
├── decisions.md   → Architectural decisions
```

---

## 📜 contracts/

Defines expected behavior using BDD-style rules.

Example:

* Given X → expect Y

👉 This is the **source of truth for behavior**

---

# 📚 /docs

Technical documentation.

## Structure

```text
/docs/
├── api/           → External API contracts
├── architecture/  → System design documentation
├── integrations/  → Integration details
```

---

# 🧪 /examples

Reusable patterns and reference code.

## Structure

```text
/examples/
├── godot/         → Godot-specific examples
├── csharp/        → C# examples
├── patterns/      → Design patterns
```

---

## Purpose

* Avoid reinventing solutions
* Provide reference implementations
* Guide AI and developers

---

# 🤖 /IA

AI execution system.

## Structure

```text
/AI/
├── context.md     → AI behavior rules
├── rules.md       → Global constraints
├── task.md        → Active task pointer
├── state.md       → Execution memory
├── tasks/         → Task definitions
```

---

## Responsibilities

* Control AI execution
* Define workflow
* Track progress

---

# 🐳 /devcontainer

Development environment configuration.

## Structure

```text
/devcontainer/
├── Dockerfile
├── devcontainer.json
├── postCreate.sh
├── run_post_create.sh
├── tests/
├── tmp/
```

---

## Purpose

* Reproducible environment
* CLI-based execution
* Headless development

---

# 🎯 Key Principles

* Clear separation of concerns
* Behavior defined before implementation
* Code must follow design
* AI must follow context rules

---

# 🚀 Summary

Each folder has a clear responsibility:

| Folder        | Responsibility      |
| ------------- | ------------------- |
| /design       | Behavior definition |
| /docs         | Technical reference |
| /examples     | Reusable patterns   |
| /IA           | AI execution        |
| /devcontainer | Environment         |

👉 This structure ensures scalability and clarity.
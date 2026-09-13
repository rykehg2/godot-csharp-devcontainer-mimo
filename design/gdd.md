# 📝 Game Design Document: Platformer Prototype

## 1. Project Vision
A high-fidelity 2D platformer template using Godot 4 and C#, built with a focus on modularity, physics precision, and 100% testability through Spec-Driven Development (SDD).

### 1.1. Visual Style & Resolution
- **Internal Resolution:** 640x360 (16:9 Pixel Art ratio).
- **Scale:** Assets and physics should be calibrated for low-resolution crispness.

## 2. Core Mechanics (Phase 1)
### 2.1. Kinematic Movement
- **Player Controller:** Uses `CharacterBody2D` for precise control over movement and collision logic.
- **Jumping:** Vertical impulse logic with a target jump velocity of `-600.0`.
- **Physics Constraint:** Strict physics implementation. No "Coyote Time" or "Jump Buffering" in the initial bootstrap.
- **Navigation:** Horizontal movement based on standard platformer physics (Acceleration/Friction).
    - **Target Speed:** 400.0 units/sec.
    - **Acceleration:** 2000.0 (Fast response).
    - **Friction:** 1000.0 (Controlled slide).

### 2.2. Environmental Interaction
- **Static Geometry:** Levels are composed of `StaticBody2D` nodes to ensure stable collision surfaces for the player.

### 2.3. Input Mapping
- **Move Left:** `ui_left` / `A` key.
- **Move Right:** `ui_right` / `D` key.
- **Jump:** `ui_select` / `Space` / `W` key.

### 2.4. Camera System
- **Behavior:** 2D Camera that follows the player smoothly (Position Smoothing enabled).

### 2.5. Environmental Hazards
- **Death Plane:** A Y-axis threshold (or trigger area) that resets the player/level if the player falls off the screen.

## 3. Systems Architecture
- **Logic Separation:** All movement and physics calculations reside in C# classes.
- **Engine Layer:** Godot Nodes (`CharacterBody2D`) act only as visual and physics proxies, calling C# logic.
- **Communication:** Use of signals for decoupling interactive elements.

## 4. Technical Pillars
- **Engine:** Godot 4.3+ (Mono/C#).
- **Validation:** All mechanics must pass CLI-based validation (`xunit.sh` and `gdunit.sh`).
- **Architecture:** Strict separation between Godot Node hierarchy and C# business logic.

## 5. Traceability & Roadmap
| Task ID | Description | Status |
| :--- | :--- | :--- |
| **001** | Setup Platformer Basics (Player/Ground) | 🏗️ IN_PROGRESS |

## 6. Design Decisions
- **Collision Shapes:** Use `RectangleShape2D` for both Player and Ground to simplify initial physics calculations and debugging.
- **Headless First:** The project must be able to run and pass tests without a GPU/Display (Headless mode).
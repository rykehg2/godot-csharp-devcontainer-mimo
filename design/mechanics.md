# 🕹️ Game Mechanics: Physics & Movement

Detailed specifications for gameplay behaviors.

## 1. Player Character
Entity: `CharacterBody2D` (Godot) + `PlayerController` (C#).

### 1.1. Basic Movement
- **Horizontal:** Acceleration-based movement. 
- **Vertical (Jump):** Discrete impulse application.
- **Target Jump Velocity:** `-600.0` units.

### 1.2. Gravity
Constant downward force applied to `Velocity.Y` when `IsOnFloor()` is false. 

### 1.3. Collision
- **Detection:** Uses `CollisionShape2D` (RectangleShape2D).
- **Response:** Standard Kinematic slide logic.
- **Contracts:** Refer to `design/contracts/basic_movement.feature`.

## 2. Environment
### 2.1. Ground
Surface: `StaticBody2D` with `RectangleShape2D`. Must be tagged for collision layer 1 (Environment).
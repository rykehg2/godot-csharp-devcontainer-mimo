# 🗺️ Roadmap

## 🟢 Phase 1: Physics Foundation (Active)
- [x] 000: Project Bootstrap (DevContainer & .NET Setup)
- [ ] 001: Setup Platformer Basics (Player/Ground Physics) 🏗️
- [ ] 002: Hazards & Boundary (Death Plane Implementation)

---

## 🟡 Phase 2: Core Mechanics
- [ ] 003: Simple Enemy AI (Patrol)
- [ ] 004: Collectibles & Scoring System

---
## 🔵 Phase 3: Polish & Expansion

---

## 🔴 Future Phase

* Enemy AI
* Progression system
* Persistence

---

# 🎙️ Discovery & Audit Log

## [2024-05-22] Feature: Movement & World Constraints
**Status:** ✅ DEFINED

### ❓ Discovery Questions:
1. Should we use "Coyote Time"? -> **No, strict physics first.**
2. Internal Resolution? -> **640x360 for pixel art.**
3. Out-of-bounds handling? -> **Implement a Death Plane.**

### 💡 Decisions & Answers:
 - Physics will focus on predictability before adding "juice".
 - 640x360 resolution requires Architect to verify camera and sprite scaling.
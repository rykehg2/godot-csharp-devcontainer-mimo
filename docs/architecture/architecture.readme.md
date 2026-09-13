# Architecture docs

Reference material for system design. Live layout of code:

```text
src/
├── Game.sln
├── GameLogic/           → pure C# domain (xUnit)
├── GameGodot/           → Godot 4 Mono layer (GDUnit4)
└── GameLogic.Tests/     → xUnit project
```

See:

- `docs/architecture/project-structure.md` — folder map
- `design/systems.md` — runtime layering (logic vs engine)
- `design/gdd.md` — game behavior / Phase 1

docs/
├── api/
├── architecture/
└── integrations/

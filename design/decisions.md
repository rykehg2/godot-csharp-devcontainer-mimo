# 🧾 Decisions

## 📌 Architecture

✔ Gameplay logic in C#  
✔ Godot only as engine layer  

Reason:
- Easier testing
- Better separation of responsibilities

---

## 🧪 Testing

✔ Prioritize .NET tests  
✔ Use Godot only when necessary  

Reason:
- Performance
- Fast feedback

---

## 🧱 Structure

✔ Separation:

* Scene (Godot)
* Logic (C#)

Reason:
- Maintenance
- Clarity

---

## ⚙️ Execution

✔ Everything must run via CLI  

Reason:
- AI compatible
- Automation

---

## 🔄 Updates

Whenever a decision changes:

1. Update this file
2. Ensure consistency with code
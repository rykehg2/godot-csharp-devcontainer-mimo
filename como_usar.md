# 🧠 Como usar seu sistema na prática (simples mesmo)

Pense assim:

> Você NÃO vai usar tudo sempre.
> Você vai usar **camadas conforme necessário**.

---

# ⚡ Fluxo real (dia a dia)

# ⚙️ Configuração Inicial (API Keys)

1. Certifique-se de ter o **Docker** e a extensão **Dev Containers** instalada no VS Code.
2. Abra a pasta do projeto e selecione "Reopen in Container".
3. O script `postCreate.sh` configurará automaticamente o ambiente C# e as ferramentas de teste.

---

# 🥇 Melhor forma de começar

Sempre começar conversa com:
```
You are working in a structured, specialized AI assembly line. Analyze the project, starting with README.md and AI/agent_mode.md.

Follow:
- Role-based specialization (Planner, Architect, Tester, Developer, Reviewer)
- TDD & XP loops
- Task-driven execution 
- I will initialize you in a specific ROLE and MODE.
```

---

## 💻 Modo CLI com OpenCode & Context-Mode (Recomendado) 
Para execução autônoma e direta no terminal: 

```bash
# Para iniciar uma sessão interativa com o OpenCode:
opencode
/model <-- escolha o modelo

# Para gerar o contexto atualizado para uma LLM externa:
context-mode .
```

> **Nota:** O `context-mode` é instalado no container via npm a partir de `/opt/context-mode`.

---

## 🥇 Loop padrão (90% do tempo)

Escolha um papel em `AI/agent_mode.md` e envie:

```
Initialize as [ROLE] in [MODE] mode. 
Execute next step from AI/task.md.
```

👉 E pronto.

---

## 💬 Exemplo real

Você:

```
Initialize as Developer in FAST mode. 
Task: Corrigir a gravidade do pulo em player_movement.md. 
Execute next step.
```

IA vai:

* ler task
* criar teste
* rodar lógica mental
* propor código

---

# 🧠 Quando usar FULL MODE

Só quando algo quebra ou fica complexo:

```
Initialize as Architect in FULL mode. 
Analyze the current project state and refine the contracts for the inventory system.
```

---

# 🔥 Regra de ouro

| Situação | Papel Recomendado | Modo | Script Útil |
| :--- | :--- | :--- | :--- |
| Planejar features / Descoberta / GDD | **Planner** | 🧠 FULL | `handoff.sh` |
| Criar Contratos e Decompor Tarefas | **Architect** | 🧠 FULL | `task-init.sh` |
| Ajustar detalhes em tarefas existentes | **Architect** | ⚡ FAST | `handoff.sh` |
| Criar novos testes (Fase RED) | **Tester** | 🧠 FULL | `xunit.sh` / `gdunit.sh` |
| Validar solução final (Fase DONE) | **Tester** | ⚡ FAST | `validate.sh` |
| Implementar lógica (Fase GREEN) | **Developer** | ⚡ FAST | `xunit.sh` |
| Compressão de Contexto/Auditoria | **Reviewer** | 🧠 FULL | `review-audit.sh` |

---

# 🧩 Como usar com seu `task.md`

Você já fez a melhor escolha: **task ativa + histórico**

Então seu fluxo vira:

---

## 1. Definir task

Você edita:

```
AI/task.md
```

Apontando para:

```
AI/tasks/001-player-movement.md
```

---

## 2. Rodar IA

No ChatGPT:

```
Initialize as [Role] in FAST mode. Execute next step
```

---

## 3. IA responde algo tipo:

* criar teste
* sugerir código
* explicar passo

---

## 4. Você executa no container

Pergunte ao Tester ou Developer qual script rodar, ou use os padrões:
* `bash AI/script/xunit.sh` (Lógica C#)
* `bash AI/script/gdunit.sh` (Integração Godot)
* `bash AI/script/validate.sh` (Validação completa)

---

## 5. Atualiza estado

A IA deve atualizar seu estado em:
`AI/states/state_[role].md`

Com progresso, decisões e problemas registrados.

E o estado global se necessário:
`AI/state.md`

---

## 6. Repete

---

# 🚀 Forma ainda mais prática (atalho mental)

Você pode literalmente usar assim:

### 🟢 Prompt mínimo

```
Next step (FAST MODE)
```

👉 porque todo o resto já está no projeto

---

# 🧠 Dica MUITO importante

Você não precisa colar todos os arquivos.

👉 O segredo é:

* Você **já estruturou o pensamento no repo**
* Agora só dá comandos curtos

---

# 💡 Truque avançado (vale ouro)

Cria um snippet fixo:

### ⚡ FAST SNIPPET

```
Use FAST MODE
Execute next step from task.md
```

### 🧠 FULL SNIPPET

```
Use FULL MODE
Analyze state and execute next step
```

---

# ⚠️ Onde pode dar errado

Sem disciplina, isso vira bagunça.

Evite:

❌ Pedir para um agente fazer o trabalho de outro (ex: Developer alterando GDD)
❌ Pular etapa de testes 
❌ Não atualizar state.md 
❌ Mudar task no meio sem registrar

---

# 🧠 Como você deve pensar agora

Você não está mais usando ChatGPT como:

> “gerador de código”

Você está usando como:

> **executor de um sistema de engenharia**

---

# 🔥 Resumo brutalmente simples

Seu sistema inteiro reduz para isso:

### Loop real:

```
1. Definir task (Architect)
2. Prompt: "Initialize as [Role] in [MODE]. Execute next step"
3. Escrever teste falhando (Tester - Red)
4. Implementar código mínimo (Developer - Green)
5. Rodar testes (validate.sh)
6. Atualizar estado (AI/states/state_[role].md)
7. Repetir até DONE
```

---

# Prompts Suggestion
> Initialize as Reviewer in FULL mode. Audit the como_usar.md and how2use.md files to ensure they are 100% consistent with the current project scripts.

>Initialize as Planner in FULL mode. Let's start the discovery for the "Save/Load System" using the roadmap log.
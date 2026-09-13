# 🧠 AI-Driven Godot C# Dev Environment

> 🚀 **Execution-driven development environment**
> Onde:
>
> * IDE ≠ fonte da verdade
> * Container = fonte da verdade

---

# 🎯 Visão Geral

Este projeto define um ambiente completo para desenvolvimento de jogos com **Godot + C#**, orientado por:

* 🐳 Containers reprodutíveis
* 🤖 Execução assistida por IA
* 🔁 XP (Extreme Programming)
* 🧪 TDD (Test-Driven Development)

O objetivo é criar um sistema onde:

> ✅ Código, testes e decisões são executáveis, rastreáveis e automatizáveis

---

# 🐳 Dev Container

## 🎯 Objetivo

Criar um ambiente:

* Reprodutível
* Headless (sem UI)
* Compatível com C# (.NET 8)
* Automatizado para testes
* Preparado para execução por IA

---

## ⚙️ Stack

* Linux (Fedora 43)
* .NET SDK 8.0
* Godot Engine (headless + mono)
* Python (scripts auxiliares)
* SCons (toolchain Godot)
* Toolchain nativa (gcc, clang, cmake, ninja)

---

## 🧱 Estrutura

```bash
.devcontainer/
├── Dockerfile
├── devcontainer.json
├── postCreate.sh
├── run_post_create.sh
├── tests/
└── tmp/              # logs e debug
```

---

## 🔧 Build

Durante o build:

```bash
godot --version
dotnet --version
/test_csharp.sh
```

👉 Garante que o ambiente está válido antes do uso

---

## 🚀 Inicialização

Ao subir o container:

```bash
postCreate.sh
```

Responsável por:

* Criar projeto Godot
* Criar solução .NET
* Criar projeto base C#
* Criar projeto de testes
* Vincular dependências
* Instalar GDUnit4
* Restaurar pacotes
* Executar testes

---

## 🔍 Observabilidade

```bash
run_post_create.sh
```

* Mostra logs no terminal
* Salva em `/tmp/postCreate.log`
* Facilita debugging

---

# 🤖 Sistema de IA

## 🧠 Estrutura

```bash
AI/
├── context.md        # manual operacional
├── rules.md          # regras globais
├── state.md          # memória do sistema
├── task.md           # task ativa
├── knowledge_map.md  # mapa de conhecimento
└── tasks/            # backlog + histórico
```

---

## 📄 context.md

Define:

* Como executar o projeto
* Como rodar testes
* Estratégia TDD híbrida (.NET + Godot)
* Fluxo XP
* Checklist de validação

---

## 📄 rules.md

Define regras obrigatórias:

* Sempre usar TDD
* Nunca pular validação
* Não quebrar código existente
* Preferir soluções simples
* Executar apenas um passo por vez

---

## 📄 state.md

Memória global:

* Progresso atual
* Decisões tomadas
* Problemas encontrados
* Próximas sugestões

---

## 📄 task.md

Define a tarefa ativa:

```md
# Current Task

See: AI/tasks/001-some-feature.md
```

---

## 🧠 tasks/

Sistema de tarefas versionadas:

```bash
AI/tasks/
├── 000-template.md
├── 001-feature.md
├── 002-feature.md
```

Cada task contém:

* Goal
* Context
* Test requirements
* Scope
* Progress
* Decisions
* Issues
* Definition of Done

👉 Funciona como backlog + histórico + documentação viva

---

# 🧪 Estratégia de Testes

## 🔁 Abordagem híbrida

### 🧠 .NET Tests (PRIMARY)

* Rápidos
* Usados para lógica pura
* Base do TDD

```bash
dotnet test
```

---

### 🎮 Godot Tests (GDUnit4)

* Testes de integração
* Usados quando há interação com engine

```bash
godot --headless --path game -s addons/gdUnit4/bin/GdUnitCmdTool.gd -a run
```

---

### 🚀 Executar tudo

```bash
dotnet test && godot --headless --path game -s addons/gdUnit4/bin/GdUnitCmdTool.gd -a run
```

---

# 🔁 Fluxo XP + TDD

```text
1. Ler contexto
2. Escrever teste (FAIL)
3. Executar testes
4. Implementar código mínimo (PASS)
5. Executar testes
6. Refatorar
7. Atualizar state.md
```

---

## 🎯 Princípios

* Testes antes do código
* Fail fast
* Iterações pequenas
* Tudo validado via CLI

---

# 📂 Estrutura do Projeto

```bash
game/        # projeto Godot
tests/       # testes .NET

AI/          # sistema de IA
design/      # regras e mecânicas do jogo
docs/        # documentação técnica
examples/    # exemplos reutilizáveis

.devcontainer/  # ambiente de execução
```

---

# 📚 Camadas de Conhecimento

## 🎮 design/

Define o comportamento do jogo:

```bash
design/
├── gdd.md
├── mechanics.md
├── systems.md
├── roadmap.md
└── decisions.md
```

---

## 📖 docs/

Documentação técnica:

```bash
docs/
├── api/
├── architecture/
└── integrations/
```

---

## 🧪 examples/

Base de reutilização:

```bash
examples/
├── godot/
├── csharp/
└── patterns/
```

👉 A IA deve **consultar exemplos antes de implementar**

---

# 🧠 Filosofia

Este projeto segue:

> 🧠 **Code as execution system**
> 📜 **Docs as source of truth**
> 🤖 **AI as deterministic executor**

---

# ✅ Benefícios

* Reprodutibilidade total
* Ambiente isolado
* Execução determinística
* Histórico completo de decisões
* Redução de regressões
* Base para automação avançada

---

# 🚀 Próximos Passos

* [ ] Loop automático de execução da IA
* [ ] Seleção automática de tasks
* [ ] Priorização de backlog
* [ ] Integração com APIs externas
* [ ] Expansão de examples

---

# 📌 Conclusão

Este projeto evoluiu para:

> 🧠 Um ambiente inteligente de desenvolvimento orientado por tarefas, testes e contexto

Com:

* Infraestrutura (Docker)
* Automação (scripts)
* Regras (rules.md)
* Contexto (context.md)
* Memória (state.md)
* Tarefas versionadas (tasks/)
* Conhecimento estruturado (design/docs/examples)

---

# ♻️ Reutilização

Este padrão pode ser aplicado em:

* Node.js
* Python
* Rust
* Backend APIs
* Sistemas distribuídos

---

> 💡 Este não é apenas um projeto.
> É um **modelo de engenharia assistida por IA**.

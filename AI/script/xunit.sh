#!/bin/bash

# Script to run xUnit tests and log output for AI analysis
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_ROOT="$(cd "$SCRIPT_DIR/../.." && pwd)"

LOG_DIR="$PROJECT_ROOT/AI/logs"
LOG_FILE="$LOG_DIR/last_xunit_test.log"
if [ ! -d "$LOG_DIR" ]; then
    mkdir -p "$LOG_DIR"
fi

# O adaptador de testes do GDUnit4 exige a variável GODOT_BIN definida.
# Caso não esteja no ambiente, tentamos localizar o binário automaticamente.
if [ -z "$GODOT_BIN" ]; then
    export GODOT_BIN=$(which godot 2>/dev/null)
fi

# --- CRITICAL: .NET 8 -> 10 RUNTIME COMPATIBILITY ---
# Os projetos compilam para net8.0, mas apenas o runtime .NET 10 está instalado.
# O testhost do VSTest só roda se o RollForward permitir major upgrade.
export DOTNET_ROLL_FORWARD=Major

echo "🧪 Running xUnit tests..." | tee "$LOG_FILE"
echo "📄 Log File: $LOG_FILE" | tee -a "$LOG_FILE"

# Localiza dinamicamente a solução (.sln ou .slnx)
SLN_PATH=$(find "$PROJECT_ROOT/src" -maxdepth 1 \( -name "*.slnx" -o -name "*.sln" \) | head -n 1)

if [ -z "$SLN_PATH" ]; then
    echo "❌ Error: No solution (.slnx or .sln) file found in src/" | tee -a "$LOG_FILE"
    exit 1
fi

# Focamos no projeto específico de testes para evitar falhas de build de outros 
# projetos irrelevantes (como os testes internos do GDUnit4)
TEST_PROJECT="$PROJECT_ROOT/src/GameLogic.Tests/GameLogic.Tests.csproj"

# Run dotnet test pointing to the solution
# Usamos um filtro para rodar apenas os testes do projeto (Game.Core.Tests) 
# e evitar carregar os testes internos do addon gdUnit4 durante a validação de lógica.
dotnet test "$TEST_PROJECT" \
    --filter "FullyQualifiedName!~gdUnit4" \
    --logger "console;verbosity=normal" 2>&1 | tee -a "$LOG_FILE"

EXIT_CODE=${PIPESTATUS[0]}

if [ $EXIT_CODE -eq 0 ]; then
    echo "✅ xUnit tests passed." | tee -a "$LOG_FILE"
else
    echo "❌ xUnit tests failed. Check $LOG_FILE for details." | tee -a "$LOG_FILE"
fi

exit $EXIT_CODE
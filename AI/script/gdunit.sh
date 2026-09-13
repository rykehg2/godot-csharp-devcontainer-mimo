#!/bin/bash

# Custom script to run GDUnit4 in Headless mode in the DevContainer

# Directory setup
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_ROOT="$(cd "$SCRIPT_DIR/../.." && pwd)"

LOG_DIR="$PROJECT_ROOT/AI/logs"
LOG_FILE="$LOG_DIR/last_gdunit_test.log"
GODOT_BIN=$(which godot)
FILTERED_ARGS=""

# Silence Godot root warning in logs
export GODOT_SILENCE_ROOT_WARNING=1

# --- CRITICAL: .NET 10 COMPATIBILITY ---
# Força o RollForward para Major em todo o processo para aceitar o host do Godot
export DOTNET_ROLL_FORWARD=Major

# Make sure log directory exists and is writable
if ! mkdir -p "$LOG_DIR"; then
    echo "❌ Critical Error: Could not create log directory $LOG_DIR"
    exit 1
fi

# Process arguments
while [ $# -gt 0 ]; do
    if [[ "$1" == "--godot_binary" ]] && [[ $# -gt 1 ]]; then
        GODOT_BIN="$2"
        shift 2
    else
        FILTERED_ARGS="$FILTERED_ARGS $1"
        shift
    fi
done

echo "🚀 Starting Godot Tests (Headless)..." | tee "$LOG_FILE"
echo "📝 Log File: $LOG_FILE" | tee -a "$LOG_FILE"

if [[ -z "$GODOT_BIN" ]]; then
    echo "❌ Error: Godot executable not found." | tee -a "$LOG_FILE"
    exit 1
fi

# --- DIAGNOSTIC: .NET HOST TRACE ---
# Habilita o rastreamento detalhado para entender por que a assembly falha ao carregar
export COREHOST_TRACE=1
export COREHOST_TRACEFILE="$LOG_DIR/dotnet_host_trace.log"

# --- DIAGNOSTIC START ---
echo "🔍 [DEBUG] Checking project.godot for GdUnit4 activation..." | tee -a "$LOG_FILE"
grep -H "gdUnit4" "$PROJECT_ROOT/src/GameGodot/project.godot" >> "$LOG_FILE" 2>&1 || echo "⚠️ GdUnit4 not mentioned in project.godot" | tee -a "$LOG_FILE"

echo "📂 [DEBUG] Listing test files in src/GameGodot/tests/:" | tee -a "$LOG_FILE"
ls -la "$PROJECT_ROOT/src/GameGodot/tests/" >> "$LOG_FILE" 2>&1
# --- DIAGNOSTIC END ---

# Garante que o Godot importe os recursos e sincronize os metadados do C# antes da compilação
echo "🔍 Initializing Godot project metadata..." | tee -a "$LOG_FILE"
"$GODOT_BIN" --headless --path src/GameGodot --import --quit >> "$LOG_FILE" 2>&1 || true

# 1. Compile C#
if [ -f "$PROJECT_ROOT/src/GameGodot/GameGodot.csproj" ]; then
    echo "📦 Cleaning and Compiling .NET Solution..." | tee -a "$LOG_FILE"
    SLN_PATH=$(find "$PROJECT_ROOT/src" -maxdepth 1 \( -name "*.sln" -o -name "*.slnx" \) | head -n 1)
     
    if [ -n "$SLN_PATH" ]; then
        dotnet clean "$SLN_PATH" >> "$LOG_FILE" 2>&1
        dotnet build "$SLN_PATH" --debug -c Debug >> "$LOG_FILE" 2>&1
    else
        # Fallback para o projeto se a solução não for encontrada
        dotnet build "$PROJECT_ROOT/src/GameGodot/GameGodot.csproj" --debug -c Debug >> "$LOG_FILE" 2>&1
    fi
fi

if [ $? -ne 0 ]; then
    echo "❌ C# Compilation failed. Check $LOG_FILE" | tee -a "$LOG_FILE"
    exit 1
fi

# Debugging: Check if Game.dll exists
GAME_DLL_PATH="$PROJECT_ROOT/src/GameGodot/.godot/mono/temp/bin/Debug/GameGodot.dll"
if [ -f "$GAME_DLL_PATH" ]; then
    echo "✅ GameGodot.dll found at: $GAME_DLL_PATH" | tee -a "$LOG_FILE"
else
    echo "❌ GameGodot.dll NOT found at: $GAME_DLL_PATH" | tee -a "$LOG_FILE"
    echo "This indicates a compilation issue or incorrect output path." | tee -a "$LOG_FILE"
    exit 1 # Exit if GameGodot.dll is not found, as tests cannot run without it.
fi

# Debugging: Sincronização de DLLs de dependências (Addons e Subprojetos)
# Garante que todas as dependências estejam na mesma pasta da Game.dll para evitar falha de carregamento
echo "🔄 Syncing all dependency assemblies..." >> "$LOG_FILE"
TEMP_BIN_DIR="$PROJECT_ROOT/src/GameGodot/.godot/mono/temp/bin/Debug"

# CRITICAL: Godot 4.6 loads the project assembly from .godot/mono/temp/bin/<config>/
# The AssemblyDependencyResolver uses the deps.json to resolve dependencies.
# GodotSharp.dll MUST remain in the output dir for the resolver to find it.
# Removing it causes ".NET: Failed to load project assembly".
echo "✅ Dependencies in output folder:" >> "$LOG_FILE"
ls "$TEMP_BIN_DIR"/*.dll >> "$LOG_FILE" 2>&1

# 2. Re-import para sincronizar classes C# compiladas
echo "🔍 Syncing C# classes to Godot..." | tee -a "$LOG_FILE"

# Primeiro importamos, depois usamos o editor para gerar metadados de scripts
export GODOT_SILENCE_ROOT_WARNING=1
"$GODOT_BIN" --headless --path src/GameGodot --import --quit >> "$LOG_FILE" 2>&1 || true
# O passo --editor foi removido pois o dotnet build + --import já são suficientes e mais estáveis aqui

# 3. Run Tests
# Adicionamos explicitamente a verbosidade e garantimos que o Godot veja as variáveis de ambiente
"$GODOT_BIN" --headless --path src/GameGodot -v -d -s res://addons/gdUnit4/bin/GdUnitCmdTool.gd --ignoreHeadlessMode $FILTERED_ARGS 2>&1 | tee -a "$LOG_FILE"
EXIT_CODE=$?

# Limpa as variáveis de trace para não afetar outros comandos
if [ $EXIT_CODE -ne 0 ] && [ -f "$COREHOST_TRACEFILE" ]; then
    echo "❌ Execution failed. Extracting critical .NET load errors:" | tee -a "$LOG_FILE"
    grep -iE "resolve|failed|not found" "$COREHOST_TRACEFILE" | tail -n 20 >> "$LOG_FILE"
fi

unset COREHOST_TRACE
unset COREHOST_TRACEFILE
echo "🔍 .NET Trace saved to: $LOG_DIR/dotnet_host_trace.log" | tee -a "$LOG_FILE"

echo "🏁 Test execution finished with code: $EXIT_CODE" | tee -a "$LOG_FILE"
ls -l "$LOG_FILE" # Verificação visual no terminal

# 3. Cleanup/Logs (Optional)
#"$GODOT_BIN" --headless --path game --quiet -s res://addons/gdUnit4/bin/GdUnitCopyLog.gd $FILTERED_ARGS > /dev/null 2>&1

exit $EXIT_CODE
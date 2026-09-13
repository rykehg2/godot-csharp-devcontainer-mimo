#!/bin/bash

# Custom script to run GDUnit4 in Headless mode in the DevContainer

LOG_DIR="/workspaces/godot-csharp-devcontainer/AI/logs"
LOG_FILE="$LOG_DIR/last_gdunit_test.log"
GODOT_BIN=$(which godot)
FILTERED_ARGS=""
if [ ! -d "$LOG_DIR" ]; then
    mkdir -p "$LOG_DIR"
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

if [[ -z "$GODOT_BIN" ]]; then
    echo "❌ Error: Godot executable not found."
    exit 1
fi

echo "🚀 Starting Godot Tests (Headless)..." | tee "$LOG_FILE"

# 1. Compile C# (Required for GDUnit4 Mono)
echo "📦 Compiling .NET Solution..." | tee -a "$LOG_FILE"
SLN_PATH=$(find game -maxdepth 1 \( -name "*.slnx" -o -name "*.sln" \) | head -n 1)
dotnet build "$SLN_PATH" --debug >> "$LOG_FILE" 2>&1
if [[ $? -ne 0 ]]; then
    echo "❌ C# Compilation failed. Check $LOG_FILE"
    exit 1
fi

# 2. Run Tests
# Removed --remote-debug port 0 which caused errors in Godot 4.x
"$GODOT_BIN" --headless --path game -d -s res://addons/gdUnit4/bin/GdUnitCmdTool.gd --ignoreHeadlessMode $FILTERED_ARGS 2>&1 | tee -a "$LOG_FILE"
EXIT_CODE=$?

echo "🏁 Test execution finished with code: $EXIT_CODE" | tee -a "$LOG_FILE"

# 3. Cleanup/Logs (Optional)
"$GODOT_BIN" --headless --path game --quiet -s res://addons/gdUnit4/bin/GdUnitCopyLog.gd $FILTERED_ARGS > /dev/null 2>&1

exit $EXIT_CODE
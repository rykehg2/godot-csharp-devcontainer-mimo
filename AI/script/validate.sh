#!/bin/bash

# Full project validation: Build + xUnit + GDUnit
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_ROOT="$(cd "$SCRIPT_DIR/../.." && pwd)"

LOG_DIR="$PROJECT_ROOT/AI/logs"
LOG_FILE="$LOG_DIR/full_validation.log"

if [ ! -d "$LOG_DIR" ]; then
    mkdir -p "$LOG_DIR"
fi

echo "🚀 Starting Full Validation..." | tee "$LOG_FILE"
echo "📄 Log File: $LOG_FILE" | tee -a "$LOG_FILE"

SLN_PATH=$(find "$PROJECT_ROOT/src" -maxdepth 1 \( -name "*.slnx" -o -name "*.sln" \) | head -n 1)

if [ -z "$SLN_PATH" ]; then
    echo "❌ Error: No solution (.slnx or .sln) file found in src/" | tee -a "$LOG_FILE"
    exit 1
fi

echo "🔨 Building Solution..." | tee -a "$LOG_FILE"
dotnet build "$SLN_PATH" >> "$LOG_FILE" 2>&1
if [ $? -ne 0 ]; then echo "❌ Build Failed" | tee -a "$LOG_FILE"; exit 1; fi

echo "🧪 Running C# Logic Tests (xUnit)..." | tee -a "$LOG_FILE"
bash "$SCRIPT_DIR/xunit.sh" >> "$LOG_FILE" 2>&1
if [ $? -ne 0 ]; then echo "❌ xUnit Tests Failed" | tee -a "$LOG_FILE"; exit 1; fi

echo "🎮 Running Godot Integration Tests (GDUnit4)..." | tee -a "$LOG_FILE"
bash "$SCRIPT_DIR/gdunit.sh" -a run >> "$LOG_FILE" 2>&1
if [ $? -ne 0 ]; then echo "❌ GDUnit Tests Failed" | tee -a "$LOG_FILE"; exit 1; fi

echo "✨ All systems green!" | tee -a "$LOG_FILE"
exit 0
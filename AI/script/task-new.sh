#!/bin/bash

# Usage: ./task-new.sh 002-my-feature
TASK_ID=$1
TEMPLATE="/workspaces/godot-csharp-devcontainer/AI/tasks/000-template.md"
TARGET_DIR="/workspaces/godot-csharp-devcontainer/AI/tasks"
MAIN_TASK_FILE="/workspaces/godot-csharp-devcontainer/AI/task.md"

if [ -z "$TASK_ID" ]; then
    echo "❌ Error: Provide a task name (e.g., 002-feature-name)"
    exit 1
fi

NEW_FILE="$TARGET_DIR/$TASK_ID.md"

if [ -f "$NEW_FILE" ]; then
    echo "⚠️ Warning: Task file $TASK_ID.md already exists."
else
    cp "$TEMPLATE" "$NEW_FILE"
    # Replace placeholder ID in the new file
    sed -i "s/000-template/$TASK_ID/g" "$NEW_FILE"
    echo "✅ Created new task: $NEW_FILE"
fi

# Update the main task pointer
echo "# Current Task" > "$MAIN_TASK_FILE"
echo "" >> "$MAIN_TASK_FILE"
echo "See: AI/tasks/$TASK_ID.md" >> "$MAIN_TASK_FILE"
echo "📌 Updated $MAIN_TASK_FILE to point to $TASK_ID.md"
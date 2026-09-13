#!/bin/bash
# Usage: bash AI/script/task-init.sh "name-of-task"

TASK_NAME=$1
if [ -z "$TASK_NAME" ]; then
    echo "❌ Error: Task name required."
    exit 1
fi

TASK_DIR="AI/tasks"
TEMPLATE="AI/tasks/000-template.md"

# Get last task number
LAST_ID=$(ls $TASK_DIR | grep -E '^[0-9]{3}' | sort | tail -n 1 | cut -c1-3)
NEXT_ID=$(printf "%03d" $((10#$LAST_ID + 1)))

NEW_FILENAME="$TASK_DIR/$NEXT_ID-$TASK_NAME.md"

cp "$TEMPLATE" "$NEW_FILENAME"
sed -i "s/000-template/$NEXT_ID-$TASK_NAME/g" "$NEW_FILENAME"
sed -i "s/<timestamp>/$(date +'%Y-%m-%d %H:%M')/g" "$NEW_FILENAME"

# Update active task
echo "# Current Task" > AI/task.md
echo "" >> AI/task.md
echo "See: $NEW_FILENAME" >> AI/task.md

echo "✅ Task created: $NEW_FILENAME and set as active in AI/task.md"
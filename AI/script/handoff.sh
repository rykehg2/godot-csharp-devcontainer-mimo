#!/bin/bash
# Usage: bash AI/script/handoff.sh [target_role] [message]
# Example: bash AI/script/handoff.sh tester "Contracts for movement are ready"

TARGET_ROLE=$1
MESSAGE=$2
STATE_FILE="AI/states/state_$TARGET_ROLE.md"

if [ ! -f "$STATE_FILE" ]; then
    echo "❌ Error: State file $STATE_FILE not found."
    exit 1
fi

# Update Status to READY
sed -i 's/Status: 🟥 WAITING/Status: 🟩 READY/g' "$STATE_FILE"

# Update Message (replaces the line starting with **Message:**)
if [ ! -z "$MESSAGE" ]; then
    sed -i "s|^\*\*Message:\*\*.*|**Message:** $MESSAGE|g" "$STATE_FILE"
fi

sed -i "s/\*Last updated:.*\*/\*Last updated: $(date +'%Y-%m-%d')\*/g" "$STATE_FILE"
echo "✅ Handoff to $TARGET_ROLE set to READY."
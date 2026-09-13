#!/bin/bash
# Summarizes the state of the assembly line for the Reviewer

echo "🔍 --- Assembly Line Audit ---"
for state in AI/states/state_*.md; do
    ROLE=$(basename "$state" | sed 's/state_//;s/.md//')
    STATUS=$(grep "Status:" "$state" | cut -d' ' -f2-)
    GOAL=$(grep -A 1 "## 🎯 Current Goal" "$state" | tail -n 1)
    
    if [[ $STATUS == *"READY"* ]]; then
        ICON="✅"
    else
        ICON="⏳"
    fi
    
    echo -e "$ICON $ROLE: $STATUS"
    echo -e "   Goal: $GOAL\n"
done
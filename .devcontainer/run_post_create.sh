#!/bin/bash

LOG_FILE=".devcontainer/tmp/postCreate.log"

echo "===================================="
echo "🚀 STARTING POST CREATE SCRIPT"
echo "===================================="

bash -x .devcontainer/postCreate.sh 2>&1 | tee $LOG_FILE

echo "===================================="
echo "✅ POST CREATE FINISHED"
echo "📄 Log disponível em: $LOG_FILE"
echo "===================================="
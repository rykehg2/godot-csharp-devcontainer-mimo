#!/bin/bash

set -e

echo "🚀 Configuring environment..."

# Define Godot binary for GDUnit4 .NET test adapters
export GODOT_BIN=$(which godot)
mkdir -p /workspaces/godot-csharp-devcontainer/AI/logs

# Limpeza preventiva para evitar conflitos de cache/objetos de builds anteriores
echo "🧹 Cleaning up old build artifacts..."
find . -type d -name "obj" -exec rm -rf {} + 2>/dev/null || true
find . -type d -name "bin" -exec rm -rf {} + 2>/dev/null || true

# =========================
# 🎮 GODOT PROJECT
# =========================
GODOT_VERSION_RAW=$($GODOT_BIN --version | grep -oE '^[0-9]+\.[0-9]+\.[0-9]+' || echo "4.6.2")
GODOT_SDK_VERSION=${GODOT_VERSION_RAW}

echo "🎯 Detected Godot Version: $GODOT_VERSION_RAW (Using SDK: $GODOT_SDK_VERSION)"

# Seed project from examples/godot/godot-csharp-decoupled if src/GameGodot is missing
if [[ ! -f "src/GameGodot/project.godot" ]]; then
    echo "🎮 Seeding project from examples/godot/godot-csharp-decoupled..."
    mkdir -p src
    if [[ -d "examples/godot/godot-csharp-decoupled" ]]; then
        cp -r examples/godot/godot-csharp-decoupled/. src/
        find src -type d -name "obj" -exec rm -rf {} + 2>/dev/null || true
        find src -type d -name "bin" -exec rm -rf {} + 2>/dev/null || true
    else
        echo "⚠️ Warning: examples/godot/godot-csharp-decoupled not found!"
    fi
fi

# Detectar caminho da solução em src/
SLN_PATH=$(find src -maxdepth 1 \( -name "*.sln" -o -name "*.slnx" \) | head -n 1)
echo "🔍 Using solution file: $SLN_PATH"

# Ensure all projects are registered in the solution
if [[ -n "$SLN_PATH" ]]; then
    echo "🔗 Configuring Solution..."
    [[ -f "src/GameLogic/GameLogic.csproj" ]] && dotnet sln "$SLN_PATH" add src/GameLogic/GameLogic.csproj 2>/dev/null || true
    [[ -f "src/GameGodot/GameGodot.csproj" ]] && dotnet sln "$SLN_PATH" add src/GameGodot/GameGodot.csproj 2>/dev/null || true
    [[ -f "src/GameLogic.Tests/GameLogic.Tests.csproj" ]] && dotnet sln "$SLN_PATH" add src/GameLogic.Tests/GameLogic.Tests.csproj 2>/dev/null || true
fi

# =========================
# 🎮 GDUNIT4
# =========================

# Instalação do GDUnit4 resiliente
if [[ ! -f "src/GameGodot/addons/gdUnit4/bin/GdUnitCmdTool.gd" ]]; then
    echo "🎮 Installing GDUnit4..."
    GDUNIT_VERSION="v6.1.3"
    TMP_GDUNIT_DIR=$(mktemp -d -t gdunit4-XXXXXXXX)

    git clone --branch "$GDUNIT_VERSION" --depth 1 https://github.com/MikeSchulze/gdUnit4.git "$TMP_GDUNIT_DIR" || { echo "Failed to clone GDUnit4 repository."; exit 1; }

    rm -rf src/GameGodot/addons/gdUnit4
    mkdir -p src/GameGodot/addons/gdUnit4

    if [[ -d "$TMP_GDUNIT_DIR/addons/gdUnit4" ]]; then
        cp -r "$TMP_GDUNIT_DIR/addons/gdUnit4/." src/GameGodot/addons/gdUnit4/
        cp "$TMP_GDUNIT_DIR/gdUnit4.csproj" src/GameGodot/addons/gdUnit4/ 2>/dev/null || true

        sed -i "s|Sdk=\"Godot.NET.Sdk/[^\"]*\"|Sdk=\"Godot.NET.Sdk/$GODOT_SDK_VERSION\"|" src/GameGodot/addons/gdUnit4/gdUnit4.csproj
        # Alinhar o TargetFramework do gdUnit4.csproj ao do projeto Godot (ex.: net8.0)
        # para que ele possa ser referenciado sem erro de incompatibilidade de TFM.
        GAME_TFM=$(grep -oP '(?<=<TargetFramework>)[^<]+' src/GameGodot/GameGodot.csproj | head -n 1 || echo "net8.0")
        sed -i "s|<TargetFramework>[^<]*</TargetFramework>|<TargetFramework>$GAME_TFM</TargetFramework>|" src/GameGodot/addons/gdUnit4/gdUnit4.csproj
        grep -q "<ImplicitUsings>" src/GameGodot/addons/gdUnit4/gdUnit4.csproj || sed -i '/<LangVersion>13.0<\/LangVersion>/a \    <ImplicitUsings>enable</ImplicitUsings>' src/GameGodot/addons/gdUnit4/gdUnit4.csproj
        sed -i '/<\/PropertyGroup>/i \    <NoWarn>$(NoWarn);CS9057;CS0436;CS0579;NU1605</NoWarn>' src/GameGodot/addons/gdUnit4/gdUnit4.csproj | head -n 1

        if [ -f "src/GameGodot/addons/gdUnit4/test/dotnet/GdUnit4CSharpApiTest.cs" ]; then
            grep -q "using System.Linq;" src/GameGodot/addons/gdUnit4/test/dotnet/GdUnit4CSharpApiTest.cs || sed -i '/using Godot.Collections;/a using System.Linq;' src/GameGodot/addons/gdUnit4/test/dotnet/GdUnit4CSharpApiTest.cs
        fi
        echo "Installed GDUnit4 addon files to src/GameGodot/addons/gdUnit4"
    else
        echo "Error: 'addons/gdUnit4' not found in cloned repository."
        exit 1
    fi

    # Clean up the temporary directory
    [ -d "$TMP_GDUNIT_DIR" ] && rm -rf "$TMP_GDUNIT_DIR"
    echo "Cleaned up temporary directory."
fi

# Importação crucial para o Godot indexar os novos arquivos .cs e o plugin
echo "📦 Importing project resources..."
godot --headless --path src/GameGodot --import --quit || true

# Garantir que o plugin GdUnit4 esteja ativado no project.godot
# (sem [editor_plugins] o addon não é carregado pelo Godot)
if [ -f "src/GameGodot/project.godot" ] && ! grep -q 'addons/gdUnit4/plugin.cfg' src/GameGodot/project.godot; then
    echo "🔌 Enabling GdUnit4 plugin in project.godot..."
    if grep -q '^\[editor_plugins\]' src/GameGodot/project.godot; then
        # Já existe a seção: ajusta somente a 1ª linha "enabled=" dela
        awk 'BEGIN{once=0} /^\[editor_plugins\]/{ins=1} ins&&/^enabled=/{if(!once){$0="enabled=PackedStringArray(\"res://addons/gdUnit4/plugin.cfg\")";once=1}} {print}' src/GameGodot/project.godot > src/GameGodot/project.godot.tmp && mv src/GameGodot/project.godot.tmp src/GameGodot/project.godot
    else
        printf '\n[editor_plugins]\n\nenabled=PackedStringArray("res://addons/gdUnit4/plugin.cfg")\n' >> src/GameGodot/project.godot
    fi
fi

# O gdUnit4.csproj NÃO é adicionado à solução: ele é referenciado pelo GameGodot.csproj
# via <ProjectReference>. Adicioná-lo recorrendo a `dotnet sln add` com caminho aninhado
# criava solution folders duplicados chamados "GameGodot", quebrando o build (MSB5004).

# =========================
# 📦 RESTORE
# =========================

echo "📦 Restoring dependencies..."
dotnet restore "$SLN_PATH"

# =========================
# 🧪 VALIDATION
# =========================

echo "🧪 Running .NET tests..."
bash AI/script/xunit.sh || true

echo "🎮 Running Godot tests..."
bash AI/script/gdunit.sh -a res://tests/ || true

echo "✅ Environment ready"
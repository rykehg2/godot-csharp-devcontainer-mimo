#!/bin/bash

set -e  # automatic failure

echo "🧪 Testing C# support in Godot..."

# Create temporary project
mkdir -p /tmp/godot-test
cd /tmp/godot-test

# Initialize Godot project
/usr/local/bin/godot --headless --path . --quit
godot --headless --help
echo "✔ Project created"

# Create C# script
cat <<EOF > Test.cs
using Godot;

public partial class Test : Node
{
    public override void _Ready()
    {
        GD.Print("C# working!");
    }
}
EOF

# Create manual .csproj file (in case Godot doesn't generate it)
cat <<EOF > godot-test.csproj
<Project Sdk="Godot.NET.Sdk/4.0.0">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
  </PropertyGroup>
</Project>
EOF

echo "✔ C# files created"

# Restore dependencies
dotnet restore

echo "✔ dotnet restore OK"

# Build project
dotnet build

echo "✔ C# Build OK"

echo "🎉 SUCCESS: C# is working in Godot!"

# Cleanup of residuals
cd /
rm -rf /tmp/godot-test
echo "🧹 Residuals cleaned"
#!/usr/bin/env python3
"""Fast checks that do not expose protected narrative data in their output."""
import json, pathlib, re, sys
ROOT=pathlib.Path(__file__).resolve().parents[1]; errors=[]
required=["Assets/Scenes/Bootstrap.unity","Assets/Scripts/GameDirector.cs","Assets/Editor/AutoBuilder.cs","Packages/manifest.json","PLAYER_README.md","BUILD_REPORT_SAFE.md","GameMaster/README.md"]
for item in required:
    if not (ROOT/item).is_file(): errors.append(f"fichier requis absent: {item}")
try:
    manifest=json.loads((ROOT/"Packages/manifest.json").read_text())
    if "com.unity.test-framework" not in manifest.get("dependencies",{}): errors.append("Unity Test Framework absent")
except Exception as error: errors.append(f"manifest invalide: {error}")
scene=(ROOT/"Assets/Scenes/Bootstrap.unity")
if scene.exists() and "%YAML 1.1" not in scene.read_text(errors="replace"): errors.append("scène bootstrap invalide")
for source in (ROOT/"Assets").rglob("*.cs"):
    text=source.read_text(errors="replace")
    if text.count("{") != text.count("}"): errors.append(f"accolades déséquilibrées: {source.relative_to(ROOT)}")
    if re.search(r"try\s*{\s*using ",text): errors.append(f"import protégé par try/catch: {source.relative_to(ROOT)}")
if errors:
    print("VALIDATION FAILED"); [print("-",e) for e in errors]; sys.exit(1)
print(f"VALIDATION OK: {len(required)} fichiers requis, {sum(1 for _ in (ROOT/'Assets').rglob('*.cs'))} sources C# contrôlées")


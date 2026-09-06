#!/usr/bin/env python3
"""Non-spoiler local build orchestrator for Windows and CI."""
from datetime import datetime
from pathlib import Path
import argparse, os, shutil, subprocess, sys

ROOT=Path(__file__).resolve().parents[1]; LOGS=ROOT/"HxHGameBuilder"/"logs"; REPORT=ROOT/"BUILD_REPORT_SAFE.md"
def run(label, command, log):
    with log.open("a",encoding="utf-8") as out:
        out.write(f"\n--- {label} ---\n"); result=subprocess.run(command,cwd=ROOT,stdout=out,stderr=subprocess.STDOUT)
    if result.returncode: raise RuntimeError(f"{label} a échoué (code {result.returncode}).")
def locate(explicit, names):
    if explicit: return explicit
    for name in names:
        found=shutil.which(name)
        if found:return found
    return None
def main():
    ap=argparse.ArgumentParser(); ap.add_argument("--unity"); ap.add_argument("--blender"); ap.add_argument("--skip-update",action="store_true"); args=ap.parse_args()
    LOGS.mkdir(parents=True,exist_ok=True); log=LOGS/(datetime.utcnow().strftime("build-%Y%m%d-%H%M%S.log"))
    steps=[]
    try:
        if not args.skip_update and (ROOT/".git").exists(): run("Mise à jour Git",["git","pull","--ff-only"],log); steps.append("Git: OK")
        run("Validations",[sys.executable,"tools/validate_project.py"],log); steps.append("Validations: OK")
        blender=locate(args.blender,["blender","blender.exe"])
        if blender: run("Blender",[blender,"--background","--python","BlenderTools/build_assets.py"],log); steps.append("Blender: OK")
        else: steps.append("Blender: ignoré (exécutable introuvable)")
        unity=locate(args.unity,["Unity","Unity.exe","unity-editor"])
        if not unity: raise RuntimeError("Unity 2022.3 est introuvable. Utilisez --unity CHEMIN.")
        run("Tests Unity",[unity,"-batchmode","-projectPath",str(ROOT),"-runTests","-testPlatform","EditMode","-testResults",str(LOGS/"tests.xml"),"-quit"],log)
        run("Build Unity",[unity,"-batchmode","-projectPath",str(ROOT),"-executeMethod","HxHGame.Editor.AutoBuilder.Build","-quit"],log); steps += ["Tests Unity: OK","Build Windows: OK"]
        REPORT.write_text("# Build\n\nOK\n\n## Étapes\n"+"\n".join(f"- {x}" for x in steps)+f"\n\nJournal : `{log.relative_to(ROOT)}`\n",encoding="utf-8")
        return 0
    except Exception as error:
        REPORT.write_text("# Build\n\nERREUR TECHNIQUE\n\n"+"\n".join(f"- {x}" for x in steps)+f"\n- {error}\n\nJournal : `{log.relative_to(ROOT)}`\n",encoding="utf-8"); print(error); return 1
if __name__=="__main__": raise SystemExit(main())


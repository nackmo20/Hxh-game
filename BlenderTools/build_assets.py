"""CLI entry: blender --background --python BlenderTools/build_assets.py."""
import os, sys, bpy
HERE=os.path.dirname(os.path.abspath(__file__)); sys.path.insert(0,HERE)
from buildings import generate_district
from export import export_collection

bpy.ops.object.select_all(action="SELECT"); bpy.ops.object.delete(use_global=False)
district=generate_district(seed=7319)
target=os.path.abspath(os.path.join(HERE,"..","Assets","Art","Generated","district.fbx"))
export_collection(district,target)
print("SAFE: generated environment asset validated and exported")


"""Validation and Unity-compatible FBX export."""
import os
import bpy

def validate_collection(collection):
    if not collection.objects: raise RuntimeError("Generated collection is empty")
    for obj in collection.objects:
        if obj.type == "MESH" and not obj.data.polygons: raise RuntimeError(f"Empty mesh: {obj.name}")

def export_collection(collection, target):
    validate_collection(collection); os.makedirs(os.path.dirname(target), exist_ok=True)
    bpy.ops.object.select_all(action="DESELECT")
    for obj in collection.objects: obj.select_set(True)
    bpy.ops.export_scene.fbx(filepath=target, use_selection=True, apply_scale_options="FBX_SCALE_ALL", axis_forward="-Z", axis_up="Y")


"""Reusable low-resolution anime material helpers for generated original assets."""
import bpy

def flat_material(name, rgba):
    material = bpy.data.materials.get(name) or bpy.data.materials.new(name)
    material.diffuse_color = rgba
    material.use_nodes = True
    material.node_tree.nodes["Principled BSDF"].inputs["Base Color"].default_value = rgba
    material.node_tree.nodes["Principled BSDF"].inputs["Roughness"].default_value = 0.85
    return material


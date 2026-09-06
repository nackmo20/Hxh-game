"""Seeded stylised building generation; safe for Blender background mode."""
import random
import bpy
from materials import flat_material

def generate_district(kind="transit", wealth=.5, density=.6, danger=.2, verticality=.4, seed=12345):
    rng = random.Random(seed)
    collection = bpy.data.collections.new("GeneratedDistrict")
    bpy.context.scene.collection.children.link(collection)
    palette = [(0.25,0.32,0.42,1),(0.48,0.36,0.42,1),(0.22,0.5,0.52,1)]
    count = max(3, int(8 * density))
    for index in range(count):
        height = 2.5 + rng.random() * (3 + verticality * 8)
        bpy.ops.mesh.primitive_cube_add(location=((index % 3)*5, (index // 3)*6, height/2))
        obj = bpy.context.object; obj.name = f"{kind}_building_{index:02d}"
        obj.scale = (1.8+rng.random(), 2+rng.random(), height/2)
        obj.data.materials.append(flat_material(f"district_{index%3}", palette[index%3]))
        for old in list(obj.users_collection): old.objects.unlink(obj)
        collection.objects.link(obj)
    return collection


"""Vegetation generation extension point."""
def vegetation_parameters(density=.5): return {"density": min(1,max(0,density))}

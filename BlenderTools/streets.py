"""Street generation extension point."""
def street_parameters(width=6.0, lanes=2): return {"width": max(2.0,width), "lanes": max(1,lanes)}

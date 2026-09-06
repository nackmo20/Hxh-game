"""Prop generation extension point."""
def prop_parameters(condition=.8): return {"condition": min(1,max(0,condition))}

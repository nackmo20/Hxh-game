using UnityEngine;

namespace HxHGame
{
    public static class WorldFactory
    {
        public static Material Material(Color c)
        {
            var shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard");
            var m = new Material(shader); m.color = c; return m;
        }
        public static GameObject Primitive(string name, PrimitiveType type, Vector3 pos, Vector3 scale, Color color, Transform parent = null)
        {
            var o = GameObject.CreatePrimitive(type); o.name = name; o.transform.SetParent(parent);
            o.transform.position = pos; o.transform.localScale = scale; o.GetComponent<Renderer>().material = Material(color); return o;
        }
        public static void Build(GameDirector game)
        {
            var world = new GameObject("PlayableDistrict").transform;
            Primitive("Ground", PrimitiveType.Cube, new Vector3(0,-.5f,0), new Vector3(34,1,28), new Color(.25f,.34f,.29f), world);
            for (int i=0;i<8;i++) {
                float side = i % 2 == 0 ? -1 : 1, z = -10 + (i/2)*7;
                Primitive("Building", PrimitiveType.Cube, new Vector3(side*12,2,z), new Vector3(7,5+(i%3),5), new Color(.42f+.03f*i,.39f,.47f), world);
            }
            Primitive("Road", PrimitiveType.Cube, new Vector3(0,.03f,0), new Vector3(9,.08f,27), new Color(.18f,.2f,.23f), world);
            Primitive("Kiosk", PrimitiveType.Cube, new Vector3(4,1,-3), new Vector3(3,2,2), new Color(.18f,.62f,.67f), world);
            var npc = Primitive("Exam Steward", PrimitiveType.Capsule, new Vector3(2,1,5), Vector3.one, new Color(.9f,.55f,.24f), world);
            npc.AddComponent<WorldInteraction>().Configure("Parler", game.BeginConversation);
            var clue = Primitive("Observation Point", PrimitiveType.Sphere, new Vector3(-4,.5f,7), Vector3.one*.65f, new Color(.95f,.82f,.25f), world);
            clue.AddComponent<WorldInteraction>().Configure("Observer", game.Inspect);
            for (int i=0;i<14;i++) Primitive("Tree", PrimitiveType.Cylinder, new Vector3(-8+(i%3)*8,1,-11+(i/3)*5), new Vector3(.35f,2,.35f), new Color(.25f,.17f,.1f), world);
        }
    }
}


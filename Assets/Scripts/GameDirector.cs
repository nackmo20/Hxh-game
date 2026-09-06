using UnityEngine;

namespace HxHGame
{
    public sealed class GameDirector : MonoBehaviour
    {
        public GameMode Mode { get; private set; } = GameMode.Exploration;
        public SaveData State { get; private set; } = new SaveData();
        public PlayerExplorer Player { get; private set; }
        readonly SaveSystem saves = new SaveSystem();
        StrategicCombat combat;
        string message = "Explorez les environs.  E : interagir  •  F5 : sauvegarder";
        int dialogueStep;

        void Start()
        {
            WorldFactory.Build(this); CreatePlayer(); CreateLighting();
            var loaded=saves.Load(1); if(loaded!=null) { State=loaded; Player.transform.position=new Vector3(loaded.position[0],loaded.position[1],loaded.position[2]); message="Sauvegarde chargée."; }
        }
        void CreatePlayer()
        {
            var p=WorldFactory.Primitive("Player",PrimitiveType.Capsule,new Vector3(0,1,-8),Vector3.one,new Color(.2f,.36f,.82f));
            Destroy(p.GetComponent<CapsuleCollider>()); var cc=p.AddComponent<CharacterController>(); cc.height=2; cc.radius=.48f;
            Player=p.AddComponent<PlayerExplorer>(); Player.game=this;
            var cam=new GameObject("Main Camera").AddComponent<Camera>(); cam.tag="MainCamera"; cam.transform.SetParent(p.transform); cam.transform.localPosition=new Vector3(0,8,-8); cam.transform.localRotation=Quaternion.Euler(38,0,0);
        }
        void CreateLighting() { var l=new GameObject("Sun").AddComponent<Light>(); l.type=LightType.Directional;l.intensity=1.25f;l.transform.rotation=Quaternion.Euler(45,-30,0); }
        public void BeginConversation() { Mode=GameMode.Dialogue; dialogueStep=0; message="Un responsable vous jauge sans interrompre les préparatifs."; }
        public void Inspect() { State.traits.Observe("observe",2); if(!State.inventory.Contains("note_observation")) State.inventory.Add("note_observation"); message="Vous remarquez un détail utile. Il est mémorisé."; }
        void ChooseDialogue(int choice)
        {
            State.traits.Observe(choice==0?"empathy":"risk"); dialogueStep++;
            if(dialogueStep>1) { Mode=GameMode.Exploration; message="La conversation se termine. Une nouvelle épreuve est accessible."; }
            else message=choice==0?"Votre calme obtient une réponse mesurée.":"Votre franchise provoque une réaction difficile à lire.";
        }
        void StartCombat(){ combat=new StrategicCombat(State.traits); Mode=GameMode.Combat; }
        public void Save(){ var p=Player.transform.position; State.position=new[]{p.x,p.y,p.z}; saves.Save(1,State); message="Partie sauvegardée (slot 1)."; }
        void OnGUI()
        {
            var box=new GUIStyle(GUI.skin.box){fontSize=16,alignment=TextAnchor.MiddleLeft,wordWrap=true};
            GUI.Box(new Rect(18,18,Mathf.Min(700,Screen.width-36),70),message,box);
            if(Mode==GameMode.Exploration && GUI.Button(new Rect(18,100,180,38),"Commencer l'épreuve")) StartCombat();
            if(Mode==GameMode.Dialogue) { GUI.Box(new Rect(18,100,600,150),message,box); if(GUI.Button(new Rect(35,190,260,42),"Répondre avec attention"))ChooseDialogue(0); if(GUI.Button(new Rect(310,190,260,42),"Prendre position"))ChooseDialogue(1); }
            if(Mode==GameMode.Combat) DrawCombat(box);
        }
        void DrawCombat(GUIStyle box)
        {
            GUI.Box(new Rect(18,100,700,270),$"VOUS  Vitalité {combat.playerHealth}/30  Aura {combat.aura}/20\nADVERSAIRE  Résistance {combat.enemyHealth}/24\nDistance : {combat.range}  •  Lecture : {combat.Intent}\n\n{string.Join("\n",combat.log.GetRange(Mathf.Max(0,combat.log.Count-4),Mathf.Min(4,combat.log.Count)))}",box);
            if(combat.finished){if(GUI.Button(new Rect(30,385,220,42),"Reprendre l'exploration")){Mode=GameMode.Exploration;message="L'épreuve est consignée.";}return;}
            string[] labels={"Attaquer","Se protéger","Observer","Changer distance","Résoudre autrement"};
            for(int i=0;i<labels.Length;i++){GUI.enabled=i!=4||combat.investigated;if(GUI.Button(new Rect(30+i*138,385,130,42),labels[i]))combat.Act(i);}GUI.enabled=true;
        }
    }
}

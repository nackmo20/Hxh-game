using System.Collections.Generic;
using UnityEngine;

namespace HxHGame
{
    public sealed class StrategicCombat
    {
        public int playerHealth=30, enemyHealth=24, aura=20, enemyIntent=0, turn;
        public RangeBand range=RangeBand.Near;
        public bool guarding, investigated, finished;
        readonly TraitProfile profile;
        public readonly List<string> log = new List<string>();
        public StrategicCombat(TraitProfile traits) { profile=traits; RollIntent(); log.Add("L'affrontement commence. Lisez la situation."); }
        void RollIntent() => enemyIntent = (turn * 7 + 3) % 3;
        public string Intent => investigated ? new[]{"approche prudente","attaque engagée","prépare quelque chose"}[enemyIntent] : "intention incertaine";
        public void Act(int action)
        {
            if(finished) return; guarding=false;
            if(action==0) { int damage=range==RangeBand.Close?7:range==RangeBand.Near?4:1; enemyHealth-=damage; aura=Mathf.Max(0,aura-2); log.Add($"Frappe mesurée : {damage}."); profile.Observe("resolve"); }
            if(action==1) { guarding=true; aura=Mathf.Min(20,aura+2); log.Add("Défense et récupération d'aura."); profile.Observe("caution"); }
            if(action==2) { investigated=true; log.Add("Vous étudiez les appuis et le rythme adverses."); profile.Observe("observe",2); }
            if(action==3) { range=(RangeBand)(((int)range+1)%3); log.Add("Vous changez la distance."); profile.Observe("ingenuity"); }
            if(action==4 && investigated) { finished=true; log.Add("Vous imposez une issue sans poursuivre l'affrontement."); profile.Observe("empathy"); return; }
            if(enemyHealth<=0) { finished=true; log.Add("L'adversaire ne peut plus continuer."); return; }
            int incoming = enemyIntent==1 ? (range==RangeBand.Close?8:4) : enemyIntent==2?5:3;
            if(guarding) incoming/=2; playerHealth-=incoming; log.Add($"Vous encaissez {incoming}.");
            if(playerHealth<=0) { playerHealth=8; finished=true; log.Add("L'épreuve s'arrête avant une blessure irréversible."); }
            turn++; RollIntent();
        }
    }
}


using System;
using System.Collections.Generic;
using UnityEngine;

namespace HxHGame
{
    public enum GameMode { Exploration, Dialogue, Combat, Paused }
    public enum RangeBand { Close, Near, Far }

    [Serializable] public sealed class TraitProfile
    {
        // Never displayed: these values turn observed behaviour into later progression opportunities.
        public int observation, resolve, empathy, caution, ingenuity, risk;
        public void Observe(string signal, int amount = 1)
        {
            switch (signal) { case "observe": observation += amount; break; case "resolve": resolve += amount; break;
                case "empathy": empathy += amount; break; case "caution": caution += amount; break;
                case "ingenuity": ingenuity += amount; break; case "risk": risk += amount; break; }
        }
    }

    [Serializable] public sealed class QuestState { public string id; public int stage; public bool complete; }
    [Serializable] public sealed class RelationState { public string id; public int trust; }
    [Serializable] public sealed class WorldFlag { public string id; public int value; }

    [Serializable] public sealed class SaveData
    {
        public int version = 1;
        public string scene = "Bootstrap";
        public float[] position = { 0, 1, 0 };
        public int health = 30, aura = 20;
        public TraitProfile traits = new TraitProfile();
        public List<string> inventory = new List<string>();
        public List<QuestState> quests = new List<QuestState>();
        public List<RelationState> relations = new List<RelationState>();
        public List<WorldFlag> flags = new List<WorldFlag>();
    }
}


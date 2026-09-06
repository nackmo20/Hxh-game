using UnityEngine;
using UnityEngine.SceneManagement;

namespace HxHGame
{
    /// <summary>Creates the dependency graph even when the deliberately tiny bootstrap scene is empty.</summary>
    public static class RuntimeBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void StartGame()
        {
            if (Object.FindObjectOfType<GameDirector>() != null) return;
            var root = new GameObject("GameRuntime");
            Object.DontDestroyOnLoad(root);
            root.AddComponent<GameDirector>();
        }
    }
}


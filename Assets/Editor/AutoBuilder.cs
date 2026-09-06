using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace HxHGame.Editor
{
    public static class AutoBuilder
    {
        public static void Build()
        {
            Directory.CreateDirectory("Builds/Windows"); Directory.CreateDirectory("Builds/Reports");
            var options=new BuildPlayerOptions { scenes=new[]{"Assets/Scenes/Bootstrap.unity"}, locationPathName="Builds/Windows/HxHGame.exe", target=BuildTarget.StandaloneWindows64, options=BuildOptions.None };
            BuildReport r=BuildPipeline.BuildPlayer(options);
            string safe=$"Build: {(r.summary.result==BuildResult.Succeeded?"OK":"ERREUR")}\nDurée: {r.summary.totalTime}\nTaille: {r.summary.totalSize}\nErreurs: {r.summary.totalErrors}\n";
            File.WriteAllText("Builds/Reports/BUILD_REPORT_SAFE.txt",safe);
            if(r.summary.result!=BuildResult.Succeeded) throw new Exception("Windows build failed; consult technical Editor log.");
        }
    }
}

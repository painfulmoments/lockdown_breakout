using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
internal static class LockdownBreakoutStartup
{
    private const string MainScene = "Assets/QA_HoldingCells/Demo/Demo_2.unity";
    private const string SessionKey = "LockdownBreakout.OpenedMainScene";

    static LockdownBreakoutStartup()
    {
        if (!Application.isBatchMode && !SessionState.GetBool(SessionKey, false))
            EditorApplication.update += OpenMainSceneWhenReady;
    }

    private static void OpenMainSceneWhenReady()
    {
        if (EditorApplication.isCompiling || EditorApplication.isUpdating)
            return;

        EditorApplication.update -= OpenMainSceneWhenReady;
        SessionState.SetBool(SessionKey, true);

        Scene currentScene = SceneManager.GetActiveScene();
        if (!EditorApplication.isPlayingOrWillChangePlaymode &&
            string.IsNullOrEmpty(currentScene.path) && !currentScene.isDirty &&
            File.Exists(MainScene))
            EditorSceneManager.OpenScene(MainScene);
    }
}

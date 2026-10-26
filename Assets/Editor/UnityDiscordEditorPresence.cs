#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using DiscordRPC;
using System.IO;
using UnityEditorInternal;

[InitializeOnLoad]
public static class UnityDiscordEditorPresence {
    private const string CLIENT_ID = "1542396836415938581";

    private static DiscordRpcClient client;
    private static Timestamps sessionTime;
    private static double lastUpdateTime = 0;

    static UnityDiscordEditorPresence() {
        EditorApplication.update += OnEditorUpdate;
        AssemblyReloadEvents.beforeAssemblyReload += Shutdown;
        EditorApplication.quitting += Shutdown;

        sessionTime = Timestamps.Now;
        Init();
    }

    private static void Init() {
        if (client != null) return;

        client = new DiscordRpcClient(CLIENT_ID);
        client.Initialize();
    }

    private static void OnEditorUpdate() {
        if (client == null || !client.IsInitialized) return;

        // Update berkala hanya jika Unity sedang dibuka/fokus
        if (EditorApplication.timeSinceStartup - lastUpdateTime > 3.0f) {
            lastUpdateTime = EditorApplication.timeSinceStartup;
            UpdatePresenceData();
        }
    }

    private static void UpdatePresenceData() {
        if (client == null) return;

        string projectName = Application.productName;
        if (string.IsNullOrEmpty(projectName)) {
            projectName = Path.GetFileName(Path.GetDirectoryName(Application.dataPath));
        }

        string sceneName = SceneManager.GetActiveScene().name;
        if (string.IsNullOrEmpty(sceneName)) sceneName = "Untitled Scene";

        string statusText;
        if (EditorApplication.isCompiling) {
            statusText = "Compiling Scripts...";
        } else if (EditorApplication.isPlaying) {
            statusText = "Testing (Play Mode)";
        } else if (Selection.activeObject != null) {
            statusText = $"Inspecting: {Selection.activeObject.name}";
        } else {
            statusText = $"Editing: {sceneName}";
        }

        client.SetPresence(new RichPresence() {
            Details = $"Project: {projectName}",
            State = statusText,
            Timestamps = sessionTime,
            Assets = new Assets() {
                LargeImageKey = "unity_logo",
                LargeImageText = $"Unity {Application.unityVersion}"
            }
        });
    }

    private static void Shutdown() {
        if (client != null) {
            client.Deinitialize();
            client.Dispose();
            client = null;
        }
    }
}
#endif
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class ToolsWindow : EditorWindow
{
    [MenuItem("Tools/Tools Window")]
    public static void ShowWindow()
    {
        GetWindow<ToolsWindow>("Tools Window");
    }

    private void OnGUI()
    {
        GUILayout.Label("Play Mode Controls", EditorStyles.boldLabel);

        if (GUILayout.Button("Restart Play Mode"))
        {
            RestartPlayMode();
        }
    }

    async Task RestartPlayMode()
    {
        await ExitPlayMode();
        EnterPlayMode();
    }

    async Task ExitPlayMode()
    {
        if (EditorApplication.isPlaying)
        {
            EditorApplication.ExitPlaymode();

            while (EditorApplication.isPlaying || EditorApplication.isPaused)
            {
                await Task.Delay(10);
            }
        }
    }

    void EnterPlayMode()
    {
        EditorApplication.EnterPlaymode();
    }
}
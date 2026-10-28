using UnityEngine;
using UnityEngine.SceneManagement;

namespace FrontierDispatch.Unity
{
    public sealed class LauncherController : MonoBehaviour
    {
        private void OnGUI()
        {
            const float width = 360f;
            GUILayout.BeginArea(new Rect(24f, 24f, width, Screen.height - 48f), GUI.skin.box);
            GUILayout.Label("FRONTIER DISPATCH - DSA LABORATORY");
            GUILayout.Space(12f);
            GUILayout.Label("Week 2");
            if (GUILayout.Button("Stack & Queue")) SceneManager.LoadScene("Week02_StackQueue");
            if (GUILayout.Button("Hash Lookup")) SceneManager.LoadScene("Week02_HashLookup");
            GUILayout.Space(12f);
            GUILayout.Label("Assignment / world-map baseline");
            if (GUILayout.Button("World Map Baseline")) SceneManager.LoadScene("Assignment_WorldMap");
            GUILayout.Space(18f);
            GUILayout.Label("Later weeks add new scenes to this same project.");
            GUILayout.EndArea();
        }
    }
}

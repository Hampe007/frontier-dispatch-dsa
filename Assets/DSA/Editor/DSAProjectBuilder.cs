using System.Collections.Generic;
using FrontierDispatch.Unity;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FrontierDispatch.Editor
{
    public static class DSAProjectBuilder
    {
        private const string SceneFolder = "Assets/DSA/Scenes";

        [MenuItem("Tools/DSA/Build Course Scenes")]
        public static void BuildCourseScenes()
        {
            EnsureFolder("Assets/DSA", "Scenes");
            var paths = new List<string>
            {
                BuildScene<LauncherController>("Launcher"),
                BuildScene<StackQueueDemoController>("Week02_StackQueue"),
                BuildScene<HashLookupDemoController>("Week02_HashLookup"),
                BuildScene<WorldMapDemoController>("Assignment_WorldMap")
            };

            EditorBuildSettings.scenes = paths.ConvertAll(path => new EditorBuildSettingsScene(path, true)).ToArray();
            EditorSceneManager.OpenScene(paths[0]);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("DSA course scenes created. Open Launcher and press Play.");
        }

        private static string BuildScene<T>(string name) where T : MonoBehaviour
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            var root = new GameObject(name + "_Controller");
            root.AddComponent<T>();
            if (typeof(T) != typeof(LauncherController)) root.AddComponent<ReturnToLauncher>();

            Camera camera = Camera.main!;
            camera.transform.position = new Vector3(0f, 9f, -10f);
            camera.transform.rotation = Quaternion.Euler(38f, 0f, 0f);

            string path = $"{SceneFolder}/{name}.unity";
            EditorSceneManager.SaveScene(scene, path);
            return path;
        }

        private static void EnsureFolder(string parent, string child)
        {
            string path = parent + "/" + child;
            if (!AssetDatabase.IsValidFolder(path))
            {
                AssetDatabase.CreateFolder(parent, child);
            }
        }
    }
}

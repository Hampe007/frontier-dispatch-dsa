using UnityEngine;
using UnityEngine.SceneManagement;

namespace FrontierDispatch.Unity
{
    public sealed class ReturnToLauncher : MonoBehaviour
    {
        private void OnGUI()
        {
            if (GUI.Button(new Rect(Screen.width - 150f, 20f, 130f, 32f), "Back to Launcher"))
            {
                SceneManager.LoadScene("Launcher");
            }
        }
    }
}

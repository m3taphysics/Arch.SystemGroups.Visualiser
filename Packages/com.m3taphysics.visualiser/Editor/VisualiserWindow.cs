using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class VisualiserWindow : EditorWindow
    {
        [MenuItem("Arch/Systems/Visualiser")]
        public static void ShowWindow()
        {
            GetWindow<VisualiserWindow>("VisualiserWindow");
        }

        void OnGUI()
        {
            GUILayout.Label("VisualiserWindow", EditorStyles.boldLabel);
            if (GUILayout.Button("Visualise"))
            {
                Visualise();
            }
        }

        void Visualise()
        {
            // Visualise
        }
    }
}

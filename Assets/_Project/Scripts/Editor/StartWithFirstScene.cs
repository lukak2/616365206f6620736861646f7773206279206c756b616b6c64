using UnityEditor;
using UnityEditor.SceneManagement;

namespace Scripts.Editor
{
    [InitializeOnLoad]
    public class StartWithFirstScene
    {
        private static bool IsEnabled
        {
            get => EditorPrefs.GetBool("StartWithFirstScene", false);
            set => EditorPrefs.SetBool("StartWithFirstScene", value);
        }
        
        static StartWithFirstScene()
        {
            Refresh();
        }
            
        private static void Refresh()
        {
            var firstScenePath = EditorBuildSettings.scenes[0].path;
            var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(firstScenePath);
            
            EditorSceneManager.playModeStartScene = IsEnabled ? sceneAsset : null;
        }

        [MenuItem("Tools/Start With First Scene/Enable", priority = 1)]
        public static void EnableStartWithFirstScene()
        {
            IsEnabled = true;
            
            Refresh();
        }

        [MenuItem("Tools/Start With First Scene/Enable", validate = true)]
        public static bool EnableStartWithFirstSceneValidate()
        {
            return !IsEnabled;
        }

        [MenuItem("Tools/Start With First Scene/Disable", priority = 2)]
        public static void DisableStartWithFirstScene()
        {
            IsEnabled = false;
            
            Refresh();
        }
        
        [MenuItem("Tools/Start With First Scene/Disable", validate = true)]
        public static bool DisableStartWithFirstSceneValidate()
        {
            return IsEnabled;
        }
    }
}

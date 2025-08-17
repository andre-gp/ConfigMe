using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace ConfigMe.EditorCM
{
    public class HomeTab : ConfigMeEditorTab
    {
        public override void InitTab(VisualElement root)
        {
            ObjectField objField = new ObjectField();
            objField.objectType = typeof(ConfigManager);
            objField.allowSceneObjects = false;
            objField.value = ConfigManagerTracker.ConfigManager;

            root.Add(objField);

            objField.RegisterValueChangedCallback(evt =>
            {
                ConfigManagerTracker.ConfigManager = evt.newValue as ConfigManager;
            });

            ConfigManagerTracker.OnChangeConfigManager += (manager) => { objField.SetValueWithoutNotify(manager); };
        }

        public override void Dispose()
        {
            
        }
    }
}

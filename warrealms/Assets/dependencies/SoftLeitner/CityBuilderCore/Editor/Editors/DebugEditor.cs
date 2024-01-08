using UnityEditor;

namespace CityBuilderCore.Editor
{
    public abstract class DebugEditor : UnityEditor.Editor
    {
        private bool _foldout;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (EditorApplication.isPlaying)
            {
                _foldout = EditorGUILayout.Foldout(_foldout, "DEBUG");
                if (_foldout)
                    drawDebugGUI();
            }
        }

        protected abstract void drawDebugGUI();
    }
}
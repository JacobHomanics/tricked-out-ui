using UnityEngine;
using UnityEditor;
using JacobHomanics.TrickedOutUI;

namespace JacobHomanics.TrickedOutUI.Editor
{
    [CustomEditor(typeof(BaseCurrentMaxTextComponent), true)]
    public class BaseCurrentMaxTextComponentEditor : UnityEditor.Editor
    {
        public enum TextComponentType
        {
            Current,
            Max,
            Difference,
            CurrentPercentage
        }

        private TextComponentType currentType;
        private bool initialized = false;

        private void OnEnable()
        {
            DetermineCurrentType();
        }

        private void DetermineCurrentType()
        {
            var target = (BaseCurrentMaxTextComponent)this.target;
            
            if (target is CurrentTextComponent)
                currentType = TextComponentType.Current;
            else if (target is MaxTextComponent)
                currentType = TextComponentType.Max;
            else if (target is DifferenceTextComponent)
                currentType = TextComponentType.Difference;
            else if (target is CurrentPercentageTextComponent)
                currentType = TextComponentType.CurrentPercentage;
            
            initialized = true;
        }

        public override void OnInspectorGUI()
        {
            if (!initialized)
                DetermineCurrentType();

            serializedObject.Update();

            // Store the current properties before switching
            TextProperties currentProperties = GetCurrentProperties();

            // Show the type selector
            EditorGUI.BeginChangeCheck();
            TextComponentType newType = (TextComponentType)EditorGUILayout.EnumPopup("Component Type", currentType);
            
            if (EditorGUI.EndChangeCheck() && newType != currentType)
            {
                SwitchComponentType(newType, currentProperties);
                return; // Exit early since we're destroying the current object
            }

            // Draw default inspector
            DrawDefaultInspector();
            
            serializedObject.ApplyModifiedProperties();
        }

        private TextProperties GetCurrentProperties()
        {
            var currentComponent = (BaseCurrentMaxTextComponent)target;
            
            if (currentComponent is CurrentTextComponent current)
                return current.properties;
            else if (currentComponent is MaxTextComponent max)
                return max.properties;
            else if (currentComponent is DifferenceTextComponent diff)
                return diff.properties;
            else if (currentComponent is CurrentPercentageTextComponent perc)
                return perc.properties;
            
            return new TextProperties(); // Return default if unknown type
        }

        private void SwitchComponentType(TextComponentType newType, TextProperties oldProperties)
        {
            var gameObject = ((BaseCurrentMaxTextComponent)target).gameObject;
            var oldComponent = (BaseCurrentMaxTextComponent)target;

            // Remove the old component
            Undo.DestroyObjectImmediate(oldComponent);

            // Add the new component based on the selected type
            BaseCurrentMaxTextComponent newComponent = null;
            
            switch (newType)
            {
                case TextComponentType.Current:
                    newComponent = Undo.AddComponent<CurrentTextComponent>(gameObject);
                    break;
                case TextComponentType.Max:
                    newComponent = Undo.AddComponent<MaxTextComponent>(gameObject);
                    break;
                case TextComponentType.Difference:
                    newComponent = Undo.AddComponent<DifferenceTextComponent>(gameObject);
                    break;
                case TextComponentType.CurrentPercentage:
                    newComponent = Undo.AddComponent<CurrentPercentageTextComponent>(gameObject);
                    break;
            }

            // Copy properties if they existed
            if (newComponent != null && oldProperties != null)
            {
                if (newComponent is CurrentTextComponent current)
                {
                    current.properties = CopyTextProperties(oldProperties);
                }
                else if (newComponent is MaxTextComponent max)
                {
                    max.properties = CopyTextProperties(oldProperties);
                }
                else if (newComponent is DifferenceTextComponent diff)
                {
                    diff.properties = CopyTextProperties(oldProperties);
                }
                else if (newComponent is CurrentPercentageTextComponent perc)
                {
                    perc.properties = CopyTextProperties(oldProperties);
                }
            }

            // Select the new component
            Selection.activeGameObject = gameObject;
            EditorUtility.SetDirty(gameObject);
        }

        private TextProperties CopyTextProperties(TextProperties source)
        {
            return new TextProperties
            {
                text = source.text,
                format = source.format,
                clampAtMax = source.clampAtMax,
                clampAtZero = source.clampAtZero,
                ceil = source.ceil,
                floor = source.floor
            };
        }
    }
}


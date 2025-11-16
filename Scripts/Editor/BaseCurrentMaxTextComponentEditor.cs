using UnityEngine;
using UnityEditor;
using JacobHomanics.TrickedOutUI;

namespace JacobHomanics.TrickedOutUI.Editor
{
    [CustomEditor(typeof(BaseCurrentMaxTextComponent))]
    public class BaseCurrentMaxTextComponentEditor : UnityEditor.Editor
    {
        public enum ValueComponentType
        {
            Current,
            Max,
            Difference,
            CurrentPercentage
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var targetComponent = (BaseCurrentMaxTextComponent)target;

            // Value Component Section
            DrawValueComponentSection(targetComponent);

            EditorGUILayout.Space();

            // Feature Components Section
            DrawFeatureComponentsSection(targetComponent);

            EditorGUILayout.Space();

            // Text Field Section
            DrawTextFieldSection();

            EditorGUILayout.Space();

            // Draw only the script reference
            EditorGUI.BeginDisabledGroup(true);
            SerializedProperty scriptProp = serializedObject.FindProperty("m_Script");
            if (scriptProp != null)
            {
                EditorGUILayout.PropertyField(scriptProp);
            }
            EditorGUI.EndDisabledGroup();
            
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawValueComponentSection(BaseCurrentMaxTextComponent targetComponent)
        {
            EditorGUILayout.LabelField("Value Component", EditorStyles.boldLabel);

            if (targetComponent.valueComponent != null)
            {
                string componentName = GetValueComponentName(targetComponent.valueComponent);
                EditorGUILayout.LabelField("Current:", componentName);
            }
            else
            {
                EditorGUILayout.LabelField("Current:", "None");
            }

            string buttonText = targetComponent.valueComponent != null ? "Change Value Component" : "Add Value Component";
            if (GUILayout.Button(buttonText))
            {
                ShowValueComponentMenu();
            }
        }

        private void AddValueComponent<T>() where T : BaseValueComponent
        {
            var targetComponent = (BaseCurrentMaxTextComponent)target;
            var gameObject = targetComponent.gameObject;

            // Store the old component to remove it later
            BaseValueComponent oldComponent = targetComponent.valueComponent;

            // Check if component already exists on the GameObject
            T existingComponent = gameObject.GetComponent<T>();
            if (existingComponent != null)
            {
                // Use existing component
                Undo.RecordObject(targetComponent, "Change Value Component");
                targetComponent.valueComponent = existingComponent;
            }
            else
            {
                // Create new component and set it
                T newComponent = Undo.AddComponent<T>(gameObject);
                Undo.RecordObject(targetComponent, "Change Value Component");
                targetComponent.valueComponent = newComponent;
            }

            // Remove the old component if it's different and not used elsewhere
            if (oldComponent != null && oldComponent != targetComponent.valueComponent)
            {
                // Check if the old component is used by other BaseCurrentMaxTextComponent instances
                bool isUsedElsewhere = false;
                var allTextComponents = gameObject.GetComponents<BaseCurrentMaxTextComponent>();
                foreach (var textComp in allTextComponents)
                {
                    if (textComp != targetComponent && textComp.valueComponent == oldComponent)
                    {
                        isUsedElsewhere = true;
                        break;
                    }
                }

                if (!isUsedElsewhere)
                {
                    Undo.DestroyObjectImmediate(oldComponent);
                }
            }

            EditorUtility.SetDirty(targetComponent);
            serializedObject.Update();
        }

        private void DrawFeatureComponentsSection(BaseCurrentMaxTextComponent targetComponent)
        {
            SerializedProperty featureComponentsProp = serializedObject.FindProperty("_featureComponents");

            EditorGUILayout.LabelField("Feature Components", EditorStyles.boldLabel);

            if (featureComponentsProp == null)
            {
                EditorGUILayout.HelpBox("Unable to find feature components property.", MessageType.Warning);
                return;
            }

            // Draw the array
            EditorGUI.indentLevel++;
            for (int i = 0; i < featureComponentsProp.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();
                
                SerializedProperty elementProp = featureComponentsProp.GetArrayElementAtIndex(i);
                BaseTextFeatureComponent feature = elementProp.objectReferenceValue as BaseTextFeatureComponent;
                
                if (feature != null)
                {
                    string featureName = GetFeatureComponentName(feature);
                    EditorGUILayout.LabelField(featureName);
                }
                else
                {
                    EditorGUILayout.LabelField("(Missing Component)");
                }
                
                if (GUILayout.Button("Remove", GUILayout.Width(60)))
                {
                    Undo.RecordObject(target, "Remove Feature Component");
                    RemoveFeatureComponentAt(i);
                    serializedObject.Update();
                    EditorUtility.SetDirty(target);
                    break;
                }
                
                EditorGUILayout.EndHorizontal();
            }
            EditorGUI.indentLevel--;

            // Add Component Button
            if (GUILayout.Button("Add Feature Component"))
            {
                ShowFeatureComponentMenu(targetComponent);
            }
        }

        private void ShowValueComponentMenu()
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Current"), false, () => AddValueComponent<CurrentValueComponent>());
            menu.AddItem(new GUIContent("Max"), false, () => AddValueComponent<MaxValueComponent>());
            menu.AddItem(new GUIContent("Difference"), false, () => AddValueComponent<DifferenceValueComponent>());
            menu.AddItem(new GUIContent("Percentage"), false, () => AddValueComponent<CurrentPercentageValueComponent>());
            menu.ShowAsContext();
        }

        private void ShowFeatureComponentMenu(BaseCurrentMaxTextComponent targetComponent)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Clamp At Zero"), false, () => AddFeatureComponent<ClampAtZeroComponent>());
            menu.AddItem(new GUIContent("Clamp At Max"), false, () => AddFeatureComponent<ClampAtMaxComponent>());
            menu.AddItem(new GUIContent("Ceil"), false, () => AddFeatureComponent<CeilComponent>());
            menu.AddItem(new GUIContent("Floor"), false, () => AddFeatureComponent<FloorComponent>());
            menu.AddItem(new GUIContent("Enclose In Braces"), false, () => AddFeatureComponent<EncloseInBracesComponent>());
            menu.AddItem(new GUIContent("Format"), false, () => AddFeatureComponent<FormatFeatureComponent>());
            
            menu.ShowAsContext();
        }

        private void DrawTextFieldSection()
        {
            EditorGUILayout.LabelField("Text", EditorStyles.boldLabel);
            
            SerializedProperty textProp = serializedObject.FindProperty("text");
            if (textProp != null)
            {
                EditorGUILayout.PropertyField(textProp);
            }
            else
            {
                EditorGUILayout.HelpBox("Unable to find text field.", MessageType.Warning);
            }
        }

        private string GetValueComponentName(BaseValueComponent component)
        {
            if (component is CurrentValueComponent)
                return "Current";
            else if (component is MaxValueComponent)
                return "Max";
            else if (component is DifferenceValueComponent)
                return "Difference";
            else if (component is CurrentPercentageValueComponent)
                return "Percentage";
            else
                return component.GetType().Name;
        }

        private string GetFeatureComponentName(BaseTextFeatureComponent component)
        {
            if (component is ClampAtZeroComponent)
                return "Clamp At Zero";
            else if (component is ClampAtMaxComponent)
                return "Clamp At Max";
            else if (component is CeilComponent)
                return "Ceil";
            else if (component is FloorComponent)
                return "Floor";
            else if (component is EncloseInBracesComponent)
                return "Enclose In Braces";
            else if (component is FormatFeatureComponent formatComponent)
                return $"Format ({formatComponent.format})";
            else
                return component.GetType().Name;
        }

        private void AddFeatureComponent<T>() where T : BaseTextFeatureComponent
        {
            var targetComponent = (BaseCurrentMaxTextComponent)target;
            var gameObject = targetComponent.gameObject;

            // Check if component already exists on the GameObject
            T existingComponent = gameObject.GetComponent<T>();
            if (existingComponent != null)
            {
                // Check if it's already in the array
                if (System.Array.Exists(targetComponent.featureComponents, f => f == existingComponent))
                {
                    EditorUtility.DisplayDialog("Component Already Added", 
                        $"A {typeof(T).Name} is already in the feature components list.", "OK");
                    return;
                }
                
                // Add existing component to array
                Undo.RecordObject(targetComponent, "Add Feature Component");
                var currentArray = targetComponent.featureComponents;
                System.Array.Resize(ref currentArray, currentArray.Length + 1);
                currentArray[currentArray.Length - 1] = existingComponent;
                targetComponent.featureComponents = currentArray;
            }
            else
            {
                // Create new component and add to array
                T newComponent = Undo.AddComponent<T>(gameObject);
                Undo.RecordObject(targetComponent, "Add Feature Component");
                var currentArray = targetComponent.featureComponents;
                System.Array.Resize(ref currentArray, currentArray.Length + 1);
                currentArray[currentArray.Length - 1] = newComponent;
                targetComponent.featureComponents = currentArray;
            }

            EditorUtility.SetDirty(targetComponent);
            serializedObject.Update();
        }

        private void RemoveFeatureComponentAt(int index)
        {
            var targetComponent = (BaseCurrentMaxTextComponent)target;
            
            if (index >= 0 && index < targetComponent.featureComponents.Length)
            {
                var componentToRemove = targetComponent.featureComponents[index];
                
                // Remove from array
                var newArray = new BaseTextFeatureComponent[targetComponent.featureComponents.Length - 1];
                for (int i = 0, j = 0; i < targetComponent.featureComponents.Length; i++)
                {
                    if (i != index)
                    {
                        newArray[j++] = targetComponent.featureComponents[i];
                    }
                }
                targetComponent.featureComponents = newArray;

                // Remove the component from the GameObject if it's not used by other text components
                if (componentToRemove != null)
                {
                    // Check if this component is used by other BaseCurrentMaxTextComponent instances
                    bool isUsedElsewhere = false;
                    var allTextComponents = targetComponent.gameObject.GetComponents<BaseCurrentMaxTextComponent>();
                    foreach (var textComp in allTextComponents)
                    {
                        if (textComp != targetComponent && System.Array.Exists(textComp.featureComponents, f => f == componentToRemove))
                        {
                            isUsedElsewhere = true;
                            break;
                        }
                    }

                    if (!isUsedElsewhere)
                    {
                        Undo.DestroyObjectImmediate(componentToRemove);
                    }
                }
            }
        }
    }
}

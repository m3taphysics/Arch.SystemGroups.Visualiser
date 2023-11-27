using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.Collections.Generic;

namespace Editor
{
    public class VisualiserWindow : EditorWindow
    {
        private VisualTreeAsset _groupTemplate;
        private VisualTreeAsset _systemTemplate;
        private VisualElement _hierarchyRoot;
        
        [MenuItem("Arch/View/Visualiser")]
        public static void ShowWindow()
        {
            var window = GetWindow<VisualiserWindow>();
            window.titleContent = new GUIContent("Visualiser");
        }

        private void OnPlayModeStateChanged(PlayModeStateChange stateChange)
        {
            if (stateChange.HasFlag(PlayModeStateChange.EnteredPlayMode))
            {
                PopulateHierarchy(SystemGroupSnapshot.Instance.Capture(), _hierarchyRoot);
            }
                
            else if (stateChange.HasFlag(PlayModeStateChange.ExitingEditMode))
            {
                ClearHierarchy();
            }
        }

        private void OnEnable()
        {
            var packagePath = "Packages/arch.systemgroups.visualiser/Editor/Assets";
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{packagePath}/Windows/HierarchyEditorWindow.uxml");
            var root = visualTree.CloneTree();
            
            rootVisualElement.Add(root);
            
            _hierarchyRoot = rootVisualElement.Q<VisualElement>("hierarchy-root");
                
            _groupTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{packagePath}/Controls/GroupTemplate.uxml");
            _systemTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{packagePath}/Controls/SystemTemplate.uxml");
            
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            
            if (Application.isPlaying)
            {
                // Populate the hierarchy based on your data
                BeginPopulateHierarchy(SystemGroupSnapshot.Instance.Capture(), _hierarchyRoot);   
            }
        }

        private void OnDisable()
        { 
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            
            ClearHierarchy();
        }

        /// <summary>
        /// Repopulate the hierarchy
        /// </summary>
        private void OnRefreshButtonClicked()
        {
            BeginPopulateHierarchy(SystemGroupSnapshot.Instance.Capture(), _hierarchyRoot);
        }

        /// <summary>
        /// Populate the hierarchy based on the groups and systems
        /// </summary>
        /// <param name="group"></param>
        /// <param name="parentElement"></param>
        private void PopulateHierarchy(IReadOnlyList<Descriptor> group, VisualElement parentElement)
        {
            if (group is null)
            {
                Debug.LogWarning("SystemGroupSnapshot.Instance.Capture() returned null. Make sure you have initialized the SystemGroupSnapshot.Instance and that you are running in play mode.");
                return;
            }
            
            if (parentElement is null)
            {
                Debug.LogWarning("Cannot populate hierarchy. Parent element is null.");
                return;
            }
            
            foreach (var descriptor in group)
            {
                if (descriptor.IsGroup)
                {
                    var groupTemplateInstance = _groupTemplate.CloneTree();
                    groupTemplateInstance.Q<Foldout>("group-foldout").text = descriptor.Name;
                    parentElement.Add(groupTemplateInstance);
                    
                    PopulateHierarchy(descriptor.SubDescriptors, groupTemplateInstance.Q<VisualElement>("group-container"));
                }
                else
                {
                    var systemTemplateInstance = _systemTemplate.CloneTree();
                    parentElement.Add(systemTemplateInstance);
                }
            }
        }
        
        /// <summary>
        /// Begin populating the hierarchy, clearing on start
        /// </summary>
        /// <param name="group"></param>
        /// <param name="parentElement"></param>
        private void BeginPopulateHierarchy(IReadOnlyList<Descriptor> group, VisualElement parentElement)
        {
            _hierarchyRoot.Clear();
            PopulateHierarchy(group, parentElement);
        }

        /// <summary>
        /// Clear the current hierarchy
        /// </summary>
        private void ClearHierarchy()
        { 
            // _hierarchyRoot.Clear();
        }
    }
}
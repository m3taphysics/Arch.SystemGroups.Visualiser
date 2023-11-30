using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace SystemGroups.Visualiser.Editor
{
    public class VisualiserWindow : EditorWindow
    {
        private VisualTreeAsset _groupTemplate;
        private VisualTreeAsset _systemTemplate;
        private VisualElement _hierarchyRoot;
        private DropdownField _worldDropdownMenu;
        private Button _enterPlayMode;
        private ScrollView _hierarchyRootScroll;

        [MenuItem("Arch/View/Visualiser")]
        public static void ShowWindow()
        {
            var window = GetWindow<VisualiserWindow>();
            window.titleContent = new GUIContent("Visualiser");
        }

        public void OnDestroy()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private void ShowAndPopulateHierarchy()
        {
            _worldDropdownMenu.visible = true;
            _hierarchyRootScroll.visible = true;
            _enterPlayMode.visible = false;
            
            _enterPlayMode.clicked -= EditorApplication.EnterPlaymode;
            _worldDropdownMenu.RegisterValueChangedCallback(OnSystemGroupWorldValueChanged);
            SystemGroupSnapshot.Instance.OnSystemGroupWorldChanged += OnSystemGroupWorldChanged;
            
            OnSystemGroupWorldChanged();
        }


        private void HideAndClearHierarchy()
        {
            _worldDropdownMenu.visible = false;
            _hierarchyRootScroll.visible = false;
            _enterPlayMode.visible = true;

            _enterPlayMode.clicked += EditorApplication.EnterPlaymode;
            _worldDropdownMenu.UnregisterValueChangedCallback(OnSystemGroupWorldValueChanged);
            SystemGroupSnapshot.Instance.OnSystemGroupWorldChanged -= OnSystemGroupWorldChanged;
            
            ClearHierarchy();
        }

        /// <summary>
        /// Invoked when the play mode state changes
        /// </summary>
        /// <param name="stateChange"></param>
        private void OnPlayModeStateChanged(PlayModeStateChange stateChange)
        {
            if (stateChange == PlayModeStateChange.EnteredPlayMode)
            {
                ShowAndPopulateHierarchy();
            }
                
            else if (stateChange == PlayModeStateChange.ExitingPlayMode)
            {
                HideAndClearHierarchy();
            }
        }

        /// <summary>
        /// Invoked when the window is enabled
        /// </summary>
        private void OnEnable()
        {
            var packagePath = "Packages/arch.systemgroups.visualiser/Editor/Assets";
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{packagePath}/Windows/HierarchyEditorWindow.uxml");
            var root = visualTree.CloneTree();
            
            rootVisualElement.Add(root);
            
            _hierarchyRoot = rootVisualElement.Q<VisualElement>("hierarchy-root");
            _hierarchyRootScroll = rootVisualElement.Q<ScrollView>("hierarchy-scroll-root");
            _worldDropdownMenu = rootVisualElement.Q<DropdownField>("world-dropdown-menu");
            _enterPlayMode = rootVisualElement.Q<Button>("enter-play-mode");
            
            _groupTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{packagePath}/Controls/GroupTemplate.uxml");
            _systemTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{packagePath}/Controls/SystemTemplate.uxml");
            
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            
            if (Application.isPlaying)
            {
                ShowAndPopulateHierarchy();
            }
            else
            {
                HideAndClearHierarchy();
            }
        }

        /// <summary>
        /// Invoked when the window is disabled
        /// </summary>
        private void OnDisable()
        { 
            HideAndClearHierarchy();
        }

        /// <summary>
        /// Invoked when the dropdown menu value changes
        /// </summary>
        /// <param name="evt"></param>
        private void OnSystemGroupWorldValueChanged(ChangeEvent<string> evt)
        {
            PopulateHierarchy();
        }

        /// <summary>
        /// Populate the dropdown menu with the current worlds
        /// </summary>
        private void OnSystemGroupWorldChanged()
        {
            _worldDropdownMenu.choices = new List<string>(SystemGroupSnapshot.Instance.SystemGroupWorlds());
            PopulateHierarchy();
        }

        /// <summary>
        /// Begin populating the system group world hierarchy with the selected world
        /// </summary>
        private void PopulateHierarchy()
        {
            if (string.IsNullOrEmpty(_worldDropdownMenu.value)) return;
            _hierarchyRoot.Clear();
            PopulateHierarchyLocal(SystemGroupSnapshot.Instance.Capture(_worldDropdownMenu.value), _hierarchyRoot);
            return;

            void PopulateHierarchyLocal(IReadOnlyList<Descriptor> group, VisualElement parentElement)
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
                    
                        PopulateHierarchyLocal(descriptor.SubDescriptors, groupTemplateInstance.Q<VisualElement>("group-container"));
                    }
                    else
                    {
                        var systemTemplateInstance = _systemTemplate.CloneTree();
                        systemTemplateInstance.Q<Label>("system-label").text = descriptor.Name;
                        parentElement.Add(systemTemplateInstance);
                    }
                }
            }
        }

        /// <summary>
        /// Clear the current hierarchy
        /// </summary>
        private void ClearHierarchy()
        { 
            _hierarchyRoot.Clear();
        }
    }
}
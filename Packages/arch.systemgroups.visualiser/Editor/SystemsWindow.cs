using System;
using System.Collections.Generic;
using System.Linq;
using Arch.SystemGroups;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace SystemGroups.Visualiser.Editor
{
    public class SystemsWindow : EditorWindow
    {
        private static readonly string PACKAGE_PATH = "Packages/arch.systemgroups.visualiser/Editor/Assets";
        private MultiColumnTreeView _multiColumnTreeView;
        private VisualTreeAsset _rowTemplate;
        private DropdownField _worldDropdownMenu;
        private TextField _systemFilterMenu;
        private Button _enterPlayMode;
        private ScrollView _hierarchyRootScroll;

        private EventCallback<ChangeEvent<string>> _filterCallback;

        [MenuItem("Arch/View/Systems")]
        public static void ShowWindow()
        {
            var window = GetWindow<SystemsWindow>();
            window.titleContent = new GUIContent("Systems");
            window.titleContent.image = AssetDatabase.LoadAssetAtPath<Texture>($"{PACKAGE_PATH}/Icons/systems.png");
        }

        /// <summary>
        /// Invoked when the window is destroyed.
        /// </summary>
        public void OnDestroy()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            HideAndClearTree();
        }
        
        private void ShowAndPopulateTree()
        {
            _worldDropdownMenu.SetEnabled(true);
            _systemFilterMenu.SetEnabled(true);
            _enterPlayMode.visible = false;
            
            _enterPlayMode.clicked -= EditorApplication.EnterPlaymode;

            _systemFilterMenu.RegisterValueChangedCallback(_filterCallback);
            
            _worldDropdownMenu.RegisterValueChangedCallback(OnSystemGroupWorldValueChanged);
            SystemGroupSnapshot.Instance.OnSystemGroupWorldChanged += OnSystemGroupWorldChanged;
            
            OnSystemGroupWorldChanged();
        }


        private void HideAndClearTree()
        {
            _worldDropdownMenu.SetEnabled(false);
            _systemFilterMenu.SetEnabled(false);
            _enterPlayMode.visible = true;

            _enterPlayMode.clicked -= EditorApplication.EnterPlaymode;
            _enterPlayMode.clicked += EditorApplication.EnterPlaymode;
            
            _systemFilterMenu.UnregisterValueChangedCallback(_filterCallback);
            
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
                ShowAndPopulateTree();
            }
                
            else if (stateChange == PlayModeStateChange.ExitingPlayMode)
            {
                HideAndClearTree();
            }
        }
        
        /// <summary>
        /// Invoked when the window is disabled
        /// </summary>
        private void OnDisable()
        { 
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            HideAndClearTree();
        }


        /// <summary>
        /// Invoked when the window is enabled
        /// </summary>
        private void OnEnable()
        {
            _filterCallback = (evt) =>
            {
                if (evt.newValue.Length <= 0) return;
                PopulateHierarchy();
                _multiColumnTreeView.ExpandAll();
            };
            
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{PACKAGE_PATH}/Windows/SystemsWindow.uxml");
            _rowTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{PACKAGE_PATH}/Controls/RowTemplate.uxml");
            
            var root = visualTree.CloneTree();
            
            rootVisualElement.Add(root);
            
            _multiColumnTreeView = rootVisualElement.Q<MultiColumnTreeView>("systems-tree");
            _worldDropdownMenu = rootVisualElement.Q<DropdownField>("world-dropdown");
            _systemFilterMenu  = rootVisualElement.Q<TextField>("systems-filter");
            _enterPlayMode = rootVisualElement.Q<Button>("enter-play-mode");

            _systemFilterMenu.RegisterValueChangedCallback(_filterCallback);
            
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            
            if (Application.isPlaying)
            {
                ShowAndPopulateTree();
            }
            else
            {
                HideAndClearTree();
            }
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
            var worldDropdownChoices = new List<string>(SystemGroupSnapshot.Instance.SystemGroupWorlds());
            _worldDropdownMenu.choices = worldDropdownChoices;
            if(worldDropdownChoices.Count > 0) _worldDropdownMenu.SetValueWithoutNotify(worldDropdownChoices[0]);
            PopulateHierarchy();
        }

        /// <summary>
        /// Begin populating the system group world hierarchy with the selected world
        /// </summary>
        private void PopulateHierarchy()
        {
            if (string.IsNullOrEmpty(_worldDropdownMenu.value)) return;
            if (_multiColumnTreeView == null) return;
            _multiColumnTreeView.SetRootItems(Array.Empty<TreeViewItemData<Descriptor>>());

            var rootDescriptor = new List<TreeViewItemData<Descriptor>>();
            GenerateSystemData(rootDescriptor, _systemFilterMenu.value);

            // Setup System/Group Name Column
            var nameColumn = _multiColumnTreeView.columns.First(c => c.name == "name");
            nameColumn.makeCell = () => _rowTemplate.CloneTree();
            nameColumn.bindCell = (element, rowIndex) =>
            {
                var node = _multiColumnTreeView.GetItemDataForIndex<Descriptor>(rowIndex);
                var icon = element.Q<Image>("icon");
                var label = element.Q<Label>("name");
                
                label.text = node.Name;
                
                icon.EnableInClassList("group", node.IsGroup);
                icon.EnableInClassList("system", node.IsSystem);
            };
            
            _multiColumnTreeView.SetRootItems(rootDescriptor);
            _multiColumnTreeView.Rebuild();
        }

        /// <summary>
        /// GenerateSystemData
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        private void GenerateSystemData(IList<TreeViewItemData<Descriptor>> parent, string filter)
        {
            void _generateSystemData(IReadOnlyList<Descriptor> descriptors, IList<TreeViewItemData<Descriptor>> parent)
            {
                foreach (var descriptor in descriptors)
                {
                    if (descriptor.IsGroup)
                    {
                        var children = new List<TreeViewItemData<Descriptor>>();
                        var tvi = new TreeViewItemData<Descriptor>(descriptor.Name.GetHashCode(), descriptor, children);
                        parent.Add(tvi);
                        _generateSystemData(descriptor.SubDescriptors, children);
                    }
                    else if(descriptor.IsSystem)
                    {
                        if(!String.IsNullOrEmpty(filter) && !descriptor.Name.Contains(filter, StringComparison.OrdinalIgnoreCase)) continue;
                        var tvi = new TreeViewItemData<Descriptor>(descriptor.Name.GetHashCode(), descriptor);
                        parent.Add(tvi);                    
                    }
                }
            }
            
            var descriptors = SystemGroupSnapshot.Instance.Capture(_worldDropdownMenu.value);
            if (descriptors != null)
            {
                _generateSystemData(descriptors, parent);   
            }
        }

        /// <summary>
        /// Clear the current hierarchy
        /// </summary>
        private void ClearHierarchy()
        {
            if (_multiColumnTreeView != null)
            {
                _multiColumnTreeView.SetRootItems(Array.Empty<TreeViewItemData<Descriptor>>());
            }
        }
    }
}
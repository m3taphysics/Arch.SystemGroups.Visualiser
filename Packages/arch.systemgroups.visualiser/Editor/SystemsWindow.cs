using System;
using System.Collections.Generic;
using System.Linq;
using Arch.SystemGroups;
using SystemGroups.Visualiser.Editor.Utility;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace SystemGroups.Visualiser.Editor
{
    public class SystemsWindow : EditorWindow
    {
        private static readonly string kPackagePath = "Packages/arch.systemgroups.visualiser/Editor/Assets";
        private MultiColumnTreeView _multiColumnTreeView;
        private VisualTreeAsset _rowTemplate;
        private DropdownField _worldDropdownMenu;
        private TextField _systemFilterMenu;
        private Button _enterPlayMode;
        private ScrollView _hierarchyRootScroll;
        private Debouncer _debouncer;

        private EventCallback<ChangeEvent<string>> _filterCallback;

        [MenuItem("Arch/View/Systems")]
        public static void ShowWindow()
        {
            var window = GetWindow<SystemsWindow>();
            window.titleContent = new GUIContent("Systems");
            window.titleContent.image = AssetDatabase.LoadAssetAtPath<Texture>($"{kPackagePath}/Icons/systems.png");
        }

        public void OnDestroy()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            
            UnregisterCallbacks();
            HideAndClearTree();
        }

        private void OnDisable()
        {
            UnregisterCallbacks();
            HideAndClearTree();
        }

        private void ShowAndPopulateTree()
        {
            _worldDropdownMenu.SetEnabled(true);
            _systemFilterMenu.SetEnabled(true);
            
            EnablePlayModeButton(false);
            
            RegisterCallbacks();
            OnSystemGroupWorldChanged();
        }

        private void HideAndClearTree()
        {
            _worldDropdownMenu.SetEnabled(false);
            _systemFilterMenu.SetEnabled(false);
            _enterPlayMode.visible = true;

            EnablePlayModeButton(true);
            
            UnregisterCallbacks();
            ClearHierarchy();
        }

        private void RegisterCallbacks()
        {
            if (_systemFilterMenu != null) _systemFilterMenu.RegisterValueChangedCallback(_filterCallback);
            if (_worldDropdownMenu != null) _worldDropdownMenu.RegisterValueChangedCallback(OnSystemGroupWorldValueChanged);
            SystemGroupSnapshot.Instance.OnSystemGroupWorldChanged += OnSystemGroupWorldChanged;
        }

        private void UnregisterCallbacks()
        {
            if (_systemFilterMenu != null) _systemFilterMenu.UnregisterValueChangedCallback(_filterCallback);
            if (_worldDropdownMenu != null) _worldDropdownMenu.UnregisterValueChangedCallback(OnSystemGroupWorldValueChanged);
            SystemGroupSnapshot.Instance.OnSystemGroupWorldChanged -= OnSystemGroupWorldChanged;
        }
        
        private void EnablePlayModeButton(bool enable)
        {
            if (enable)
            {
                _enterPlayMode.visible = true;
                _enterPlayMode.clicked -= EditorApplication.EnterPlaymode;
                _enterPlayMode.clicked += EditorApplication.EnterPlaymode;
            }
            else
            {
                _enterPlayMode.visible = false;
                _enterPlayMode.clicked -= EditorApplication.EnterPlaymode;
            }
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

        private void Setup()
        {
            _debouncer = new Debouncer();
            
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            
            _filterCallback = (evt) =>
            {
                _debouncer.Debounce(() =>
                {
                    PopulateHierarchy();
                    _multiColumnTreeView.ExpandAll(); 
                });
            };

            var visualTree =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{kPackagePath}/Windows/SystemsWindow.uxml");
            _rowTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{kPackagePath}/Controls/RowTemplate.uxml");

            var root = visualTree.CloneTree();

            rootVisualElement.Add(root);

            _multiColumnTreeView = rootVisualElement.Q<MultiColumnTreeView>("systems-tree");
            _worldDropdownMenu = rootVisualElement.Q<DropdownField>("world-dropdown");
            _systemFilterMenu = rootVisualElement.Q<TextField>("systems-filter");
            _enterPlayMode = rootVisualElement.Q<Button>("enter-play-mode");
        }


        /// <summary>
        /// Invoked when the window is enabled
        /// </summary>
        private void OnEnable()
        { 
            Setup();
            
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

            SetupNameColumn();
            SetupThrottledColumn();
            
            _multiColumnTreeView.SetRootItems(rootDescriptor);
            _multiColumnTreeView.Rebuild();
        }

        private void SetupThrottledColumn()
        {
            const string COLUMN_NAME = "throttled-column";  
            var throttledColumn = _multiColumnTreeView.columns.FirstOrDefault(c => c.name == COLUMN_NAME);
            if(throttledColumn == null) return;
            
            throttledColumn.makeCell = () => _rowTemplate.CloneTree();
            throttledColumn.bindCell = (element, rowIndex) =>
            {
                var node = _multiColumnTreeView.GetItemDataForIndex<Descriptor>(rowIndex);
                var icon = element.Q<Image>("icon");
                var label = element.Q<Label>("name");

                if (node.IsGroup)
                {
                    label.text = String.Empty;
                }
                else
                {
                    label.text = node.ThrottlingEnabled ? "Enabled" : "Disabled";    
                }
                
                icon.EnableInClassList("group", false);
                icon.EnableInClassList("system", false);
            };
        }

        private void SetupNameColumn()
        {
            const string COLUMN_NAME = "name";
            var nameColumn = _multiColumnTreeView.columns.FirstOrDefault(c => c.name == COLUMN_NAME);
            if (nameColumn == null) return;
            
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
        }

        /// <summary>
        /// GenerateSystemData
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        private void GenerateSystemData(IList<TreeViewItemData<Descriptor>> parent, string filter)
        {
            void Recurse(IReadOnlyList<Descriptor> descriptors, IList<TreeViewItemData<Descriptor>> parent)
            {
                foreach (var descriptor in descriptors)
                {
                    if (descriptor.IsGroup)
                    {
                        var children = new List<TreeViewItemData<Descriptor>>();
                        var tvi = new TreeViewItemData<Descriptor>(descriptor.Name.GetHashCode(), descriptor, children);
                        parent.Add(tvi);
                        Recurse(descriptor.SubDescriptors, children);
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
                Recurse(descriptors, parent);   
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
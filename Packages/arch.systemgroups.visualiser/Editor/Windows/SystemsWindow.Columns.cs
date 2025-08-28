using System;
using System.Linq;
using SystemGroups.Visualiser.Editor.Utility;
using UnityEditor;
using UnityEngine.UIElements;

namespace SystemGroups.Visualiser.Editor.Windows
{
    public partial class SystemsWindow : EditorWindow
    {
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
                icon.EnableInClassList("system"
                    , false);
            };
        }

        private void SetupNameColumn()
        {
            const string COLUMN_NAME = "name-column";
            var nameColumn = _multiColumnTreeView.columns.FirstOrDefault(c => c.name == COLUMN_NAME);
            if (nameColumn == null) return;
            
            nameColumn.makeCell = () => _rowTemplate.CloneTree();
            nameColumn.bindCell = (element, rowIndex) =>
            {
                var node = _multiColumnTreeView.GetItemDataForIndex<Descriptor>(rowIndex);
                var icon = element.Q<Image>("icon");
                var label = element.Q<Label>("name");
                
                label.enableRichText = true;
                var query = _systemFilterMenu?.value ?? String.Empty;
                label.text = RichText.HighlightMatch(node.Name, query);
                
                icon.EnableInClassList("group", node.IsGroup);
                icon.EnableInClassList("system", node.IsSystem);
            };
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace SystemGroups.Visualiser.Editor.Windows
{
    public partial class SystemsWindow : EditorWindow
    {
        private int _lastSelectionId;
        private void OnSelectionChanged(IEnumerable<object> selections)
        {
            if (_multiColumnTreeView.selectedItem != null)
            {
                _lastSelectionId = ((Descriptor?)_multiColumnTreeView.selectedItem).Value.Name.GetHashCode();
            }
            else
            {
                _lastSelectionId = -1;
            }
        }
    }
}
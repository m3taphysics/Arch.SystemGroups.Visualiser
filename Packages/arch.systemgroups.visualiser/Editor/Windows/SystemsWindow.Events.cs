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
                var descriptor = ((Descriptor)_multiColumnTreeView.selectedItem);
                if (descriptor.Name != null)
                {
                    _lastSelectionId = descriptor.Name.GetHashCode();    
                }
                else
                {
                    _lastSelectionId = -1;
                }
            }
            else
            {
                _lastSelectionId = -1;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace SystemGroups.Visualiser.Editor.Tree
{
    public static class SystemTreeBuilder
    {
        public static void Generate(IList<TreeViewItemData<Descriptor>> parent, string filter, string world)
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
            
            var descriptors = SystemGroupSnapshot.Instance.Capture(world);
            if (descriptors != null)
            {
                Recurse(descriptors, parent);   
            }
        }
    }
}
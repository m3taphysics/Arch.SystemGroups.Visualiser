using UnityEngine;
using UnityEngine.UIElements;

namespace SystemGroups.Visualiser.Editor.Utility
{
    public static class MultiColumnTreeViewExtensions
    {
        public static void ScrollItemToTop(this MultiColumnTreeView treeView, int id)
        {
            treeView.SetSelectionById(id);

            treeView.schedule.Execute(() =>
            {
                treeView.ScrollToItemById(id);
            }).StartingIn(1);
        }
    }
}
using System;
using UnityEditor;

namespace SystemGroups.Visualiser.Editor.Utility
{
    public class Debouncer
    {
        private readonly double _delay;
        private double _lastInvokeTime;
        private Action _pendingAction;

        public Debouncer(double delaySeconds = 0.6)
        {
            _delay = delaySeconds;
        }

        public void Debounce(Action action)
        {
            _pendingAction = action;
            _lastInvokeTime = EditorApplication.timeSinceStartup;
            EditorApplication.update -= Update;
            EditorApplication.update += Update;
        }

        private void Update()
        {
            if (!(EditorApplication.timeSinceStartup - _lastInvokeTime >= _delay)) return;
            EditorApplication.update -= Update;
            _pendingAction?.Invoke();
            _pendingAction = null;
        }
    }

}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Arch.SystemGroups;
using Arch.SystemGroups.Descriptors;
using JetBrains.Annotations;
using UnityEngine;

namespace SystemGroups.Visualiser
{
    /// <summary>
    /// A singleton that is initialized by the package installer
    /// That can then retrieve a snapshot of the system group world
    /// </summary>
    public class SystemGroupSnapshot
    {    
        private static readonly Lazy<SystemGroupSnapshot> lazyInstance = new(() => new SystemGroupSnapshot());
        public static SystemGroupSnapshot Instance => lazyInstance.Value;
        
        public Action OnSystemGroupWorldChanged;
        
        private readonly Dictionary<string, SystemGroupWorld> _world = new();
        private readonly Dictionary<SystemGroupWorld, string> _worldReverseMap = new();
        private readonly List<string> _systemGroupWorldNames = new();

        /// <summary>
        /// Register a SystemGroupWorld with the snapshot singleton
        /// </summary>
        /// <param name="name"></param>
        /// <param name="systemGroupWorld"></param>
        [UsedImplicitly]
        [Conditional("UNITY_EDITOR")]
        public void Register(string name, SystemGroupWorld systemGroupWorld)
        {
            _systemGroupWorldNames.Add(name);
            _worldReverseMap.Add(systemGroupWorld, name);
            _world.Add(name, systemGroupWorld);
            OnSystemGroupWorldChanged?.Invoke();
        }
        
        /// <summary>
        /// Unregister a SystemGroupWorld with the snapshot singleton
        /// </summary>
        /// <param name="name"></param>
        /// <param name="systemGroupWorld"></param>
        [UsedImplicitly]
        [Conditional("UNITY_EDITOR")]
        public void Unregister(SystemGroupWorld systemGroupWorld)
        {
            var name = _worldReverseMap[systemGroupWorld];
            _systemGroupWorldNames.Remove(name);
            _world.Remove(name);
            OnSystemGroupWorldChanged?.Invoke();
;        }
        
        /// <summary>
        /// Get a list of worlds by name currently registered
        /// </summary>
        /// <returns>A list of names of each world, can be used to Capture()</returns>
        [UsedImplicitly]
        public IReadOnlyList<string> SystemGroupWorlds()
        {
            return _systemGroupWorldNames;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [UsedImplicitly]
        public IReadOnlyList<Descriptor> Capture(string name)
        {
            if (!Application.isPlaying) return null;
            return _world.TryGetValue(name, out var systemGroupWorld) ? systemGroupWorld.GenerateDescriptors() : null;
        }
    }
}
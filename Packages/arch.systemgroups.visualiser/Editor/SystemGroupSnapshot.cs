using System;
using System.Collections.Generic;
using Arch.SystemGroups;
using Arch.SystemGroups.Descriptors;
using UnityEngine;

namespace Editor
{
    /// <summary>
    /// A singleton that is initialized by the package installer
    /// That can then retrieve a snapshot of the system group world
    /// </summary>
    public class SystemGroupSnapshot
    {    
        private static readonly Lazy<SystemGroupSnapshot> lazyInstance = new Lazy<SystemGroupSnapshot>(() => new SystemGroupSnapshot());
        private SystemGroupWorld _world;
        public static SystemGroupSnapshot Instance => lazyInstance.Value;
        
        public void Initialize(SystemGroupWorld systemGroupWorld)
        {
            _world = systemGroupWorld;
        }
        
        public IReadOnlyList<Descriptor> Capture()
        {
            return !Application.isPlaying ? null : Instance._world.GenerateDescriptors();
        }
    }
}
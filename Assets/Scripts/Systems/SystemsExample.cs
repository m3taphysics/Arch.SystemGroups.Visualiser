using System;
using System.Collections.Generic;
using Arch.Core;
using Arch.SystemGroups;
using Arch.SystemGroups.DefaultSystemGroups;
using Arch.SystemGroups.Descriptors;
using Editor;
using UnityEngine;

namespace Systems
{
    public class EntryPoint : MonoBehaviour
    {
        private SystemGroupWorld _world = null;

        private void Start()
        {
            var worldBuilder = new ArchSystemsWorldBuilder<World>(World.Create());
            
            InitSystemGroupSystem.InjectToWorld(ref worldBuilder);
            AfterInitSystemGroupSystem.InjectToWorld(ref worldBuilder);
            AfterAfterInitSystemGroupSystem.InjectToWorld(ref worldBuilder);
            
            SimulationSystemGroupSystem.InjectToWorld(ref worldBuilder);
            
            PresentationSystemGroupSystem.InjectToWorld(ref worldBuilder);
            BeforePresentationSystemGroupSystem.InjectToWorld(ref worldBuilder);
            AfterPresentationSystemGroupSystem.InjectToWorld(ref worldBuilder);
            
            PostPhysicsSystemGroupSystem.InjectToWorld(ref worldBuilder);
            PhysicsSystemGroupSystem.InjectToWorld(ref worldBuilder);
            _world = worldBuilder.Finish();
            SystemGroupSnapshot.Instance.Initialize(_world);
        }
    }
}
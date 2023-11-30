using System;
using System.Collections.Generic;
using Arch.Core;
using Arch.SystemGroups;
using Arch.SystemGroups.DefaultSystemGroups;
using Arch.SystemGroups.Descriptors;
using Groups;
using SystemGroups.Visualiser;
using UnityEngine;

namespace Systems
{
    public class EntryPoint : MonoBehaviour
    {
        private ArchSystemsWorldBuilder<World> CreateWorldByName(string name)
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

            SystemInCustomGroupSystem.InjectToWorld(ref worldBuilder);
            SystemInCustomGroupSystem1.InjectToWorld(ref worldBuilder);
            SystemInCustomGroupSystem2.InjectToWorld(ref worldBuilder);
            return worldBuilder;
        }
        
        private void Start()
        {
            SystemGroupSnapshot.Instance.Register("world1", CreateWorldByName("world1").Finish());
            SystemGroupSnapshot.Instance.Register("world2", CreateWorldByName("world2").Finish());
            SystemGroupSnapshot.Instance.Register("world3", CreateWorldByName("world3").Finish());
            SystemGroupSnapshot.Instance.Register("world4", CreateWorldByName("world4").Finish());
        }
    }
}
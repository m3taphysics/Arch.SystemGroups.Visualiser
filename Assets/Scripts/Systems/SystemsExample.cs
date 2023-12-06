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
        private SystemGroupWorld world1;
        private SystemGroupWorld world2;
        private SystemGroupWorld world3;
        private SystemGroupWorld world4;
        
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
            world1 = CreateWorldByName("world1").Finish();
            world2 = CreateWorldByName("world2").Finish();
            world3 = CreateWorldByName("world3").Finish();
            world4 = CreateWorldByName("world4").Finish();
            
            SystemGroupSnapshot.Instance.Register("world1", world1);
            SystemGroupSnapshot.Instance.Register("world2", world2);
            SystemGroupSnapshot.Instance.Register("world3", world3);
            SystemGroupSnapshot.Instance.Register("world4", world4);
        }

        private void OnDestroy()
        {
            SystemGroupSnapshot.Instance.Unregister(world1);
            SystemGroupSnapshot.Instance.Unregister(world2);
            SystemGroupSnapshot.Instance.Unregister(world3);
            SystemGroupSnapshot.Instance.Unregister(world4);
        }
    }
}
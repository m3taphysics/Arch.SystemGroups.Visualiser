using Arch.Core;
using Arch.SystemGroups;
using Arch.SystemGroups.DefaultSystemGroups;
using UnityEngine;

namespace Systems
{
    public class EntryPoint : MonoBehaviour
    {
        private void Start()
        {
            var worldBuilder = new ArchSystemsWorldBuilder<World>(World.Create());
            InitSystemGroupSystem.InjectToWorld(ref worldBuilder);
            SimulationSystemGroupSystem.InjectToWorld(ref worldBuilder);
            PresentationSystemGroupSystem.InjectToWorld(ref worldBuilder);
            PostPhysicsSystemGroupSystem.InjectToWorld(ref worldBuilder);
            PhysicsSystemGroupSystem.InjectToWorld(ref worldBuilder);
            worldBuilder.Finish();
        }
    }
}
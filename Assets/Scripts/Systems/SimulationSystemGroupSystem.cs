using Arch.Core;
using Arch.System;
using Arch.SystemGroups;
using Arch.SystemGroups.DefaultSystemGroups;

namespace Systems
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial class SimulationSystemGroupSystem : BaseSystem<World, float>
    {
        public SimulationSystemGroupSystem(World world) : base(world)
        {
        }
    }
}
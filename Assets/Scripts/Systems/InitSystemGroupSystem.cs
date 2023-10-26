using Arch.Core;
using Arch.System;
using Arch.SystemGroups;
using Arch.SystemGroups.DefaultSystemGroups;

namespace Systems
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial class InitSystemGroupSystem : BaseSystem<World, float>
    {
        public InitSystemGroupSystem(World world) : base(world)
        {
        }
    }
}
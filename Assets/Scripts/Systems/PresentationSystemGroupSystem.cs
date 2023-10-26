using Arch.Core;
using Arch.System;
using Arch.SystemGroups;
using Arch.SystemGroups.DefaultSystemGroups;

namespace Systems
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial class PresentationSystemGroupSystem : BaseSystem<World, float>
    {
        public PresentationSystemGroupSystem(World world) : base(world)
        {
        }
    }
}
using Arch.Core;
using Arch.System;
using Arch.SystemGroups;
using Arch.SystemGroups.DefaultSystemGroups;

namespace Systems
{
    [UpdateInGroup(typeof(PhysicsSystemGroup))]
    public partial class PhysicsSystemGroupSystem : BaseSystem<World, float>
    {
        public PhysicsSystemGroupSystem(World world) : base(world)
        {
        }
    }
}
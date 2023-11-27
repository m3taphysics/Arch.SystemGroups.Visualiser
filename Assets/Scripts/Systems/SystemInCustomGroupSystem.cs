using Arch.Core;
using Arch.System;
using Arch.SystemGroups;
using Arch.SystemGroups.DefaultSystemGroups;
using Groups;

namespace Systems
{
    [UpdateInGroup(typeof(CustomGroup1))]
    public partial class SystemInCustomGroupSystem : BaseSystem<World, float>
    {
        public SystemInCustomGroupSystem(World world) : base(world)
        {
        }
    }
}
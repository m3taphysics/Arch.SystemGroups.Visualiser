using Arch.Core;
using Arch.System;
using Arch.SystemGroups;
using Arch.SystemGroups.DefaultSystemGroups;
using Groups;

namespace Systems
{
    [UpdateInGroup(typeof(CustomGroupInGroup1))]
    public partial class SystemInCustomGroupSystem1 : BaseSystem<World, float>
    {
        public SystemInCustomGroupSystem1(World world) : base(world)
        {
        }
    }
}
using Arch.Core;
using Arch.System;
using Arch.SystemGroups;
using Arch.SystemGroups.DefaultSystemGroups;
using Groups;

namespace Systems
{
    [UpdateInGroup(typeof(CustomGroupInGroup1))]
    public partial class SystemInCustomGroupSystem2 : BaseSystem<World, float>
    {
        public SystemInCustomGroupSystem2(World world) : base(world)
        {
        }
    }
}
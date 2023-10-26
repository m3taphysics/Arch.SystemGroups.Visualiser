using Arch.Core;
using Arch.System;
using Arch.SystemGroups;
using Arch.SystemGroups.DefaultSystemGroups;

namespace Systems
{
    [UpdateInGroup(typeof(PostPhysicsSystemGroup))]
    public partial class PostPhysicsSystemGroupSystem : BaseSystem<World, float>
    {
        public PostPhysicsSystemGroupSystem(World world) : base(world)
        {
        }
    }
}
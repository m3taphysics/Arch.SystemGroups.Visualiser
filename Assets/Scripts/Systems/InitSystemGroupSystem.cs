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
    
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [UpdateAfter(typeof(InitSystemGroupSystem))]
    public partial class AfterInitSystemGroupSystem : BaseSystem<World, float>
    {
        public AfterInitSystemGroupSystem(World world) : base(world)
        {
        }
    }
    
    
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [UpdateAfter(typeof(AfterInitSystemGroupSystem))]
    public partial class AfterAfterInitSystemGroupSystem : BaseSystem<World, float>
    {
        public AfterAfterInitSystemGroupSystem(World world) : base(world)
        {
        }
    }
}
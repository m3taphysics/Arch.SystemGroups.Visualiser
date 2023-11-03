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
    
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    [UpdateBefore(typeof(PresentationSystemGroupSystem))]
    public partial class BeforePresentationSystemGroupSystem : BaseSystem<World, float>
    {
        public BeforePresentationSystemGroupSystem(World world) : base(world)
        {
        }
    }
    
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    [UpdateAfter(typeof(PresentationSystemGroupSystem))]
    public partial class AfterPresentationSystemGroupSystem : BaseSystem<World, float>
    {
        public AfterPresentationSystemGroupSystem(World world) : base(world)
        {
        }
    }
}
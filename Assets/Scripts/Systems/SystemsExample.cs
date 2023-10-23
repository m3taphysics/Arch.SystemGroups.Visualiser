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


    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial class SimulationSystemGroupSystem : BaseSystem<World, float>
    {
        public SimulationSystemGroupSystem(World world) : base(world)
        {
        }
    }


    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial class PresentationSystemGroupSystem : BaseSystem<World, float>
    {
        public PresentationSystemGroupSystem(World world) : base(world)
        {
        }
    }


    [UpdateInGroup(typeof(PostPhysicsSystemGroup))]
    public partial class PostPhysicsSystemGroupSystem : BaseSystem<World, float>
    {
        public PostPhysicsSystemGroupSystem(World world) : base(world)
        {
        }
    }


    [UpdateInGroup(typeof(PhysicsSystemGroup))]
    public partial class PhysicsSystemGroupSystem : BaseSystem<World, float>
    {
        public PhysicsSystemGroupSystem(World world) : base(world)
        {
        }
    }
}
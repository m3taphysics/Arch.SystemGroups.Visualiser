# Arch.SystemGroups.Visualiser

[![License](https://img.shields.io/badge/License-Apache_2.0-blue.svg?style=for-the-badge)](https://opensource.org/licenses/Apache-2.0)
![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)


The project provides a package to allow visualisation of the dependency tree of [Arch.SystemGroups](https://github.com/mikhail-dcl/Arch.SystemGroups/). For larger complex projects it is difficult to understand the execution order when the number of systems becomes increasingly large.

## Installation
- Package can be added as a github dependency: ```https://github.com/m3taphysics/Arch.SystemGroups.Visualiser.git?path=/Packages/Arch.Systemgroups.Visualiser```. [See Installing a package from a github dependency](https://docs.unity3d.com/Manual/upm-ui-giturl.html)
- Once installed Visualiser can be found ```Arch -> View -> Visualiser" ```

## Setup
For information on set up of Unity system groups please refer to [Arch.SystemGroups](https://github.com/mikhail-dcl/Arch.SystemGroups).

### Registering a SystemGroupWorld
```C#
SystemGroupSnapshot.Instance.Register("world1", world1);
```


### Unregistering a SystemGroupWorld
```C#
SystemGroupSnapshot.Instance.Unregister(world1);
```

### Example
```C#
    public class EntryPoint : MonoBehaviour
    {
        private SystemGroupWorld world1;
        private SystemGroupWorld world2;
        private SystemGroupWorld world3;
        private SystemGroupWorld world4;
        
        private ArchSystemsWorldBuilder<World> CreateWorldByName(string name)
        {
            var worldBuilder = new ArchSystemsWorldBuilder<World>(World.Create());
            
            InitSystemGroupSystem.InjectToWorld(ref worldBuilder);
            AfterInitSystemGroupSystem.InjectToWorld(ref worldBuilder);
            AfterAfterInitSystemGroupSystem.InjectToWorld(ref worldBuilder);
            
            SimulationSystemGroupSystem.InjectToWorld(ref worldBuilder);
            
            PresentationSystemGroupSystem.InjectToWorld(ref worldBuilder);
            BeforePresentationSystemGroupSystem.InjectToWorld(ref worldBuilder);
            AfterPresentationSystemGroupSystem.InjectToWorld(ref worldBuilder);
            
            PostPhysicsSystemGroupSystem.InjectToWorld(ref worldBuilder);
            PhysicsSystemGroupSystem.InjectToWorld(ref worldBuilder);

            SystemInCustomGroupSystem.InjectToWorld(ref worldBuilder);
            SystemInCustomGroupSystem1.InjectToWorld(ref worldBuilder);
            SystemInCustomGroupSystem2.InjectToWorld(ref worldBuilder);
            return worldBuilder;
        }
        
        private void Start()
        {
            world1 = CreateWorldByName("world1").Finish();
            world2 = CreateWorldByName("world2").Finish();
            world3 = CreateWorldByName("world3").Finish();
            world4 = CreateWorldByName("world4").Finish();
            
            SystemGroupSnapshot.Instance.Register("world1", world1);
            SystemGroupSnapshot.Instance.Register("world2", world2);
            SystemGroupSnapshot.Instance.Register("world3", world3);
            SystemGroupSnapshot.Instance.Register("world4", world4);
        }

        private void OnDestroy()
        {
            SystemGroupSnapshot.Instance.Unregister(world1);
            SystemGroupSnapshot.Instance.Unregister(world2);
            SystemGroupSnapshot.Instance.Unregister(world3);
            SystemGroupSnapshot.Instance.Unregister(world4);
        }
    }
```

## Screnshot
### Light Mode
![image](https://github.com/m3taphysics/Arch.SystemGroups.Visualiser/assets/296469/aef588f5-036c-41de-80e4-c35b897737ab)

### Dark Mode
![image](https://github.com/m3taphysics/Arch.SystemGroups.Visualiser/assets/296469/092a936e-68ff-4a3e-8366-9a447ff77b6a)

##

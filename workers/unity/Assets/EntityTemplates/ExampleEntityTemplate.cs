using Improbable.General;
using Improbable.Worker;
using Improbable.Math;
using Improbable.Player;
using Improbable.Unity.Core.Acls;
using UnityEngine;

namespace Assets.EntityTemplates
{
    public class ExampleEntityTemplate : MonoBehaviour
    {
        // Template definition for a Example entity
        public static SnapshotEntity GenerateExampleSnapshotEntityTemplate()
        {
            // Set name of Unity prefab associated with this entity
            var exampleEntity = new SnapshotEntity { Prefab = "ExampleEntity" };

            // Define components attached to snapshot entity
            exampleEntity.Add(new WorldTransform.Data(new WorldTransformData(new Coordinates(0, 0, 0))));
            exampleEntity.Add(new UnityName.Data(new UnityNameData("your_example_entity")));
            exampleEntity.Add(new CppName.Data(new CppNameData("your_example_entity")));

            var acl = Acl.Build()
                // Both FSim (server) workers and client workers granted read access over all states
                .SetReadAccess(Acl.MakeRequirementSet(CommonAttributeSets.Physics, CommonAttributeSets.Visual, Acl.MakeAttributeSet("cpp")))
                // Only FSim workers granted write access over WorldTransform component
                .SetWriteAccess<WorldTransform>(CommonRequirementSets.PhysicsOnly)
                // Only client workers granted write access over Name component
                .SetWriteAccess<UnityName>(CommonRequirementSets.VisualOnly)
                .SetWriteAccess<CppName>(Acl.MakeRequirementSet(Acl.MakeAttributeSet("cpp")));

            exampleEntity.SetAcl(acl);

            return exampleEntity;
        }
    }
}
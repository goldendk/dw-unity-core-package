using DWGames;
using UnityEngine;

namespace EditModeTests
{
    public class FooUsesPhysics
    {

        private static IPhysicsDelegate physicsDelegate;

        public static void SetPhysicsDelegate(IPhysicsDelegate physicsDelegate)
        {
            FooUsesPhysics.physicsDelegate = physicsDelegate;
        }


        public static bool UseOverlapSphere(Vector3 position, float radius, int layerMask)
        {
            var useOverlapSphere = physicsDelegate.OverlapSphere(position, radius, layerMask);
            Debug.Log("overlapsphere result: " + useOverlapSphere);
            return useOverlapSphere.Length > 0;
        }

    }
}
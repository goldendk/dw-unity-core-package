using UnityEngine;

namespace DWGames.com.darkwing_games.core.Runtime.Util
{
    public class UnityPhysics : IPhysicsDelegate
    {
        public Collider[] OverlapSphere(Vector3 position, float radius, LayerMask layerMask )
        {
            var overlapSphere = Physics.OverlapSphere(position, radius, layerMask);
            return overlapSphere;
        }
    }
}
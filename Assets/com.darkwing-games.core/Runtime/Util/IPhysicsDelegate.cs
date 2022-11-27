using UnityEngine;

namespace DWGames
{
    public interface IPhysicsDelegate
    {
        ///<see cref="Physics.OverlapSphere(UnityEngine.Vector3,float,int,UnityEngine.QueryTriggerInteraction)"></see>
        public Collider[] OverlapSphere(Vector3 position, float radius, LayerMask layerMask);
        
    }
}
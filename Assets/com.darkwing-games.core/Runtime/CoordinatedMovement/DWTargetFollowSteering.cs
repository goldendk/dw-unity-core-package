using System;
using Codice.CM.Common.Merge;
using UnityEngine;
using UnityEngine.Events;

namespace DWGames.CoordinatedMovement
{
    ///<summary>
    ///Add to GameObject that is supposed to align with a 'target' gameobject in terms of position and rotation.
    /// Will automatically clamp the GameObject to the ground.
    ///  Current behaviour:
    /// <ul>
    /// <li>Rotate to see the target until angleToTargetTranslationLimit is reached.</li>
    /// <li>Move toward object until stopping distance is reached.</li>
    /// <li>When at target rotate until aligned with target's forward vector.</li>
    /// </ul>
    ///
    /// Known bugs:
    /// <ul>
    /// <li>Note: currently there is a but that makes the GameObject move sideways after GameObject is aligned with target. Make the  </li>
    /// <li>This class does not take Y-axis into consideration, but only rotates the model in the X-Y plane. To move up or down override the ClampToGround method.</li>
    /// <li>ClampToGround rotation is disabled right now.</li>
    /// </ul>
    ///
    ///
    ///</summary> 
    public class DWTargetFollowSteering : MonoBehaviour
    {
        
        [Tooltip(
            "The height from which the raycast towards the ground is made. Should be well above the max ground height.")]
        public float CLAMP_TO_GROUND_RAYCAST_Y_OFFSET = 50f;

        public float moveSpeed = 5.0f;
        public float turnSpeed = 90f;

        public delegate float SpeedProviderDelegate();

        public SpeedProviderDelegate SpeedProvider;
        
        [Tooltip(
            "The minimum angle to target before the 'follower' will start to move forward. Will only rotate if angle is higher.")]
        [Range(0, 180)]
        public float angleTotargetTranslationLimit = 5f;

        [Tooltip("The angle to target where the model will stop turning. e.g. 3 degrees.")] [Range(0, 180)]
        public float stoppingRotationAngle = 3f;
        
        [Tooltip(
            "If distance to target is higher then faces the target. If lower rotates to face target 'Forward' vector.")]
        [Range(0, 2)]
        public float faceTowardTargetDistance = .2f;


        [Tooltip("The distance from the target the follower will stop moving at.")] [Range(0, 10)]
        public float stoppingDistance = 0.01f;

        [Tooltip("The distance from target where the follower will report as being catched up to the target.")]
        [Range(0, 3)]
        public float catchUpDistance = 0.3f;
        

        [NonSerialized] public FollowStateEvent followStateListeners = new FollowStateEvent(); 
        
        [Header("Runtime values")]
        private float currentSpeed;
        public float currentAngleToTarget;
        public Transform currentTarget;
        public TargetFollowState currentFollowState;
        public float currentDistanceToTarget;
        public TranslationMode translationMode = TranslationMode.NONE;

        private void Awake()
        {
            currentSpeed = 0;
            currentAngleToTarget = 0;
            currentFollowState = TargetFollowState.IN_POSITION;
            SpeedProvider = ()=> moveSpeed;
        }


        private void Start()
        {
            if (translationMode == TranslationMode.NONE)
            {
                Debug.LogWarning("TranslationMode is set to NONE.");
            }
        }


        private void Update()
        {
            if (currentTarget == null)
            {
                return;
            }


            var faceTargetIfNeeded = FaceTargetIfNeeded();
            ClampToGround();
            var moveTowardsTargetIfNeeded = MoveTowardsTargetIfNeeded();

            if (moveTowardsTargetIfNeeded == TargetFollowState.IN_POSITION &&
                faceTargetIfNeeded == TargetFollowState.IN_POSITION)
            {
                UpdateStateIfNeeded(TargetFollowState.IN_POSITION);
            }
            else
            {
                UpdateStateIfNeeded(TargetFollowState.CATCHING_UP);
            }
        }

        private void UpdateStateIfNeeded(TargetFollowState newState)
        {
            var tempCurrentState = currentFollowState;
            currentFollowState = newState;
            if (tempCurrentState != newState)
            {
                followStateListeners.Invoke(gameObject, newState);
            }

        }

        /**
         * Checks where the ground is and positions the model on top of it. Note that the Target (formation marker) may not be in the same position.
         */
        private void ClampToGround()
        {
            var rayStart = new Vector3(transform.position.x, transform.position.y + CLAMP_TO_GROUND_RAYCAST_Y_OFFSET,
                transform.position.z);
            var ray = new Ray(rayStart, Vector3.down);
            RaycastHit hit;
            var layerMask = LayerMask.GetMask("Ground");
            //Debug.DrawRay(ray.origin, ray.direction.normalized * 20, Color.green);
            // layerMask = ~layerMask;


            if (Physics.Raycast(ray, out hit, 20f + CLAMP_TO_GROUND_RAYCAST_Y_OFFSET, layerMask))
            {
                var deltaY = hit.point.y - transform.position.y;
                if (Mathf.Abs(deltaY) > 0.001)
                {
                    transform.Translate(new Vector3(0, deltaY, 0));
                }

                //TODO: rotation from clamp-to-ground gets reset by the look-at-target mechanic. The two should be merged somehow. 
                //One method needs to tell the other about where to look so the two can be combined. Basically they deal with seperate
                //axis.
                //var quaternion = Quaternion.Euler(hit.normal.x, 0, 0);
                //var rotationEulerAngles = transform.rotation.eulerAngles;
                // transform.rotation = Quaternion.Euler(hit.normal);
            }
        }


        private TargetFollowState FaceTargetIfNeeded()
        {
            Vector3 directionToLookTowards = currentTarget.forward;

            if (currentDistanceToTarget > faceTowardTargetDistance)
            {
                var targetPositionLockedY = TargetPositionLockedY();
                directionToLookTowards = (targetPositionLockedY - transform.position).normalized;
            }

            currentAngleToTarget = Vector3.SignedAngle(directionToLookTowards, transform.forward, Vector3.up);

            if (Mathf.Abs(currentAngleToTarget) < stoppingRotationAngle)
            {
                return TargetFollowState.IN_POSITION;
            }

            if (directionToLookTowards.magnitude < 0.01f)
            {
                return TargetFollowState.IN_POSITION;
            }

            // the second argument, upwards, defaults to Vector3.up
            var lookRotation = Quaternion.LookRotation(directionToLookTowards);
            var rotateTowards = Quaternion.RotateTowards(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);

            transform.rotation = rotateTowards;

            return TargetFollowState.CATCHING_UP;
        }


        private float DistanceToTarget()
        {
            return Math.Abs(Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z),
                new Vector3(currentTarget.transform.position.x, 0, currentTarget.transform.position.z)));
        }

        private Vector3 TargetPositionLockedY()
        {
            return new Vector3(currentTarget.position.x, transform.position.y, currentTarget.position.z);
        }

        private TargetFollowState MoveTowardsTargetIfNeeded()
        {
            currentDistanceToTarget = DistanceToTarget();


            if (Mathf.Abs(currentAngleToTarget) > angleTotargetTranslationLimit)
            {
                return TargetFollowState.CATCHING_UP;
            }

            if (currentDistanceToTarget > stoppingDistance) //move to the designated point. 
            {
                //TODO: When the current angle becomes less than the translations limit then the object will start moving which it should NOT! 
                currentSpeed = SpeedProvider.Invoke();

                
                if (translationMode == TranslationMode.FORWARD)
                {
                    transform.Translate(Vector3.forward * Time.deltaTime * currentSpeed);    
                }
                else if(translationMode == TranslationMode.MOVE_TOWARDS)
                {
                    transform.position = Vector3.MoveTowards(transform.position, TargetPositionLockedY(), currentSpeed * Time.deltaTime);    
                }

                return TargetFollowState.CATCHING_UP;
            }
            else
            {
                currentSpeed = 0;
                return TargetFollowState.IN_POSITION;
            }
            
        }
        public enum TranslationMode
        {
            MOVE_TOWARDS, FORWARD, NONE
        }
     
    }
    
    [Serializable]
    public class FollowStateEvent : UnityEvent<GameObject, TargetFollowState>
    {
            
    }
    
    
}
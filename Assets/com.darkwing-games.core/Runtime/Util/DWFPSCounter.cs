using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DWGames
{
    public class DWFPSCounter : MonoBehaviour
    {
        public float currentFPS = 0;
        [SerializeField] private float lastUpdateTime = 0;

        [Tooltip("The number of frames to average the count over.")] 
        [SerializeField] private float timeBetweenUpdates = .5f;

        public float debugRealTimeSinceStartup = 0;
        
        void Update()
        {
            debugRealTimeSinceStartup = Time.realtimeSinceStartup;
            if (lastUpdateTime > 0)
            {
                if (lastUpdateTime + timeBetweenUpdates <= Time.realtimeSinceStartup)
                {
                    currentFPS = 1 / Time.deltaTime;
                    lastUpdateTime = Time.realtimeSinceStartup;
                }
            }
            else
            {
                lastUpdateTime = Time.realtimeSinceStartup;
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DWGames
{
    public class DWFPSCounter : MonoBehaviour
    {
        [SerializeField] private float currentFPS = 0;
        [SerializeField] private float lastUpdateTime = 0;

        [Tooltip("The number of frames to average the count over.")] 
        [SerializeField] private float timeBetweenUpdates = .5f;

        public delegate void FPSUpdateListener(float currentFps);

        public static event FPSUpdateListener FPSUpdateEvent;
        

        public float debugRealTimeSinceStartup = 0;
        
        void Update()
        {
            debugRealTimeSinceStartup = Time.realtimeSinceStartup;
            if (lastUpdateTime > 0)
            {
                if (lastUpdateTime + timeBetweenUpdates <= Time.realtimeSinceStartup)
                {
                    currentFPS = 1 / Time.deltaTime;
                    FPSUpdateEvent?.Invoke(currentFPS);
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
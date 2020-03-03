using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Util
{
    // This version of a Singleton was found here:
    // https://wiki.unity3d.com/index.php/Singleton
    public class MonoBehaviorPunSingleton<T> : MonoBehaviourPunCallbacks where T : MonoBehaviourPunCallbacks
    {
        // Check to see if we're about to be destroyed.
        private static bool isShuttingDown = false;
        private static object _lock = new object();
        private static T _instance;

        /// <summary>
        /// Access singleton instance through this propriety.
        /// </summary>
        public static T Instance
        {
            get
            {
                Debug.Log("Getting instance of " + typeof(T).ToString());
                //if (isShuttingDown)
                //    return null;

                lock (_lock)
                {
                    if (_instance == null)
                    {
                        Debug.Log("Finding existing instance of " + typeof(T).ToString());
                        // Search for existing instance.
                        _instance = (T)FindObjectOfType(typeof(T));

                        // Create new instance if one doesn't already exist.
                        if (_instance == null)
                        {
                            Debug.Log("Making a new " + typeof(T).ToString());
                            // Need to create a new GameObject to attach the singleton to.
                            var singletonObject = new GameObject();
                            _instance = singletonObject.AddComponent<T>();
                            singletonObject.name = typeof(T).ToString() + " (Singleton)";

                            // Make instance persistent.
                            DontDestroyOnLoad(singletonObject);
                        }
                    }

                    return _instance;
                }
            }
        }
        private void OnApplicationQuit()
        {
            isShuttingDown = true;
        }
        private void OnDestroy()
        {
            Debug.Log("Destroying instance of " + typeof(T).ToString());
            isShuttingDown = true;
        }
    }
}

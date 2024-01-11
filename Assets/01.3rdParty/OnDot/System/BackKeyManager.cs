using System.Collections.Generic;
using OnDot.Util;
using UnityEngine;

namespace OnDot.System
{
    public class BackKeyManager : PersistentSingleton<BackKeyManager>
    {
        [SerializeField] List<BackKeySystem> backKeySystems = new List<BackKeySystem>();

        private void Update()
        {
            if (Application.platform == RuntimePlatform.OSXEditor)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    OnBack();
                }
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    OnBack();
                }
            }
        }

        private void OnBack()
        {
            if (backKeySystems.Count > 0)
            {
                backKeySystems[backKeySystems.Count - 1].OnBack();
            }
        }

        public void AddBackKeySystem(BackKeySystem backKeySystem)
        {
            if (!backKeySystems.Contains(backKeySystem))
            {
                backKeySystems.Add(backKeySystem);
            }
        }

        public void RemoveBackKeySystem(BackKeySystem backKeySystem)
        {
            backKeySystems.Remove(backKeySystem);
        }
    }
}

using OnDot.Util;
using UnityEngine;

namespace OnDot.System
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "OnDot/GameSettings")]
    public class GameSettings : ScriptableObject
    {
        [Header("Store")]
        [SerializeField]
        [Label("Android")]
        private string storeAndroidURL = string.Empty;
        public string StoreAndroidURL
        {
            get { return storeAndroidURL; }
        }

        [SerializeField]
        [Label("iOS")]
        private string storeIOSURL = string.Empty;
        public string StoreIOSURL
        {
            get { return storeIOSURL; }
        }

        [Header("Capture")]
        [SerializeField]
        [Label("Capture Folder Name")]
        private string captureFolderName = string.Empty;
        public string CaptureFolderName
        {
            get { return captureFolderName; }
        }
    }
}
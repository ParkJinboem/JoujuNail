using UnityEngine;

namespace OnDot.System
{
    public class OnDotManager : MonoBehaviour
    {
        public static OnDotManager Instance;

        [SerializeField] private GameSettings gameSettings;

        public void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }

            Application.targetFrameRate = 60;
        }

        public string StoreURL
        {
            get
            {
#if UNITY_ANDROID
                return gameSettings.StoreAndroidURL;
#elif UNITY_IPHONE
                    return gameSettings.StoreIOSURL;
#else
                    return "";
#endif
            }
        }

        public string CaptureFolderName
        {
            get { return gameSettings.CaptureFolderName; }
        }

        /// <summary>
        /// 마켓 스토어 열기
        /// </summary>
        public void OpenStore()
        {
            Application.OpenURL(StoreURL);
        }
    }
}

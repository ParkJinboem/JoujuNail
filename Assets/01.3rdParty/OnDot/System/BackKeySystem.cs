using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace OnDot.System
{
    public class BackKeySystem : MonoBehaviour
    {
        [SerializeField] [FormerlySerializedAs("onBack")] UnityEvent m_OnBack;
        public UnityEvent onBack
        {
            get
            {
                if (m_OnBack == null) m_OnBack = new UnityEvent();
                return m_OnBack;
            }
        }

        private static bool m_IsLock;

        private void OnEnable()
        {
            BackKeyManager.Instance.AddBackKeySystem(this);
        }

        private void OnDisable()
        {
            BackKeyManager.Instance.RemoveBackKeySystem(this);
        }

        public void OnBack()
        {
            if (m_IsLock)
            {
                return;
            }

            onBack.Invoke();
        }

        public static void SetLock(bool isLock)
        {
            m_IsLock = isLock;
        }
    }
}


using UnityEngine;
using System.Collections.Generic;

namespace OnDot.Util
{
    public class YieldInstructionCache
    {
        public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
        public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();
        private static readonly Dictionary<float, WaitForSeconds> _timeInterval = new Dictionary<float, WaitForSeconds>();
        private static readonly Dictionary<float, WaitForSecondsRealtime> _realtimeInterval = new Dictionary<float, WaitForSecondsRealtime>();

        public static WaitForSeconds WaitForSeconds(float seconds)
        {
            WaitForSeconds wfs;
            if (!_timeInterval.TryGetValue(seconds, out wfs))
            {
                _timeInterval.Add(seconds, wfs = new WaitForSeconds(seconds));
            }
            return wfs;
        }

        public static WaitForSecondsRealtime WaitForSecondsRealtime(float seconds)
        {
            WaitForSecondsRealtime wfsr;
            if (!_realtimeInterval.TryGetValue(seconds, out wfsr))
            {
                _realtimeInterval.Add(seconds, wfsr = new WaitForSecondsRealtime(seconds));
            }
            return wfsr;
        }
    }
}
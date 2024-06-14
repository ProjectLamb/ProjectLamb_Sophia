using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Sophia
{
    public static class GlobalHelper {
        public static object NullRef = null;
    }
    public static class GlobalExternal
    {
        public static Vector3 GetSophiaAngleToVector(float _angle)
        {
            _angle *= Mathf.Deg2Rad;
            return new Vector3(Mathf.Sin(_angle), 0, Mathf.Cos(_angle));
        }

        public static Vector3 GetSophiaForwardingVector(this Vector2 inputVec)
        {
            Vector3 res = Vector3.zero;
            res += GetSophiaAngleToVector(Camera.main.transform.eulerAngles.y + 90f) * inputVec.x;
            res += GetSophiaAngleToVector(Camera.main.transform.eulerAngles.y) * inputVec.y;
            return res.normalized;
        }
        public static Quaternion GetSophiaForwardingAngle(this Quaternion instantiatorQuaternion, Transform ownerTransform)
        {
            return Quaternion.Euler(ownerTransform.eulerAngles + instantiatorQuaternion.eulerAngles);
        }
        
        public static Transform GetSophiaTransformParent(this Transform instantiatorTransform, Transform ownerTransform)
        {
            instantiatorTransform.SetParent(ownerTransform);
            return instantiatorTransform;
        }
    }

    public static class GlobalAsync {
        public static IEnumerator PerformUnScaled(float delayTime, UnityAction action) {
            yield return new WaitForSecondsRealtime(delayTime);
            action.Invoke();
        }
        public static IEnumerator PerformAndRenderUI(UnityAction action)
        {
            action.Invoke(); 
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }
        public static IEnumerator PerformAndRenderUIUnScaled(UnityAction action)
        {
            action.Invoke(); 
            yield return new WaitForSecondsRealtime(0.0166f);
        }
    }
}
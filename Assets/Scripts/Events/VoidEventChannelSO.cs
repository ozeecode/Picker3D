using UnityEngine;
using UnityEngine.Events;

namespace Picker3D
{

    [CreateAssetMenu(menuName = "Picker3D/Void Event Channel")]
    public class VoidEventChannelSO : ScriptableObject
    {
        public UnityAction OnEventRaised;
        public void RaiseEvent()
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke();
        }
    }
}
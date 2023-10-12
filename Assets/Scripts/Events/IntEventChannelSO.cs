using UnityEngine;
using UnityEngine.Events;

namespace Picker3D
{
    [CreateAssetMenu(menuName = "Picker3D/Integer Event Channel")]
    public class IntEventChannelSO : ScriptableObject
    {
        public UnityAction<int> OnEventRaised;

        public void RaiseEvent(int value)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(value);
        }
    }
}

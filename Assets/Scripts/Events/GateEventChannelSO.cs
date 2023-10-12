using UnityEngine;
using UnityEngine.Events;

namespace Picker3D
{
    [CreateAssetMenu(menuName = "Picker3D/Gate Event Channel")]
    public class GateEventChannelSO : ScriptableObject
    {
        public UnityAction<Gate, int> OnEventRaised;

        public void RaiseEvent(Gate gate, int index)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(gate, index);
        }
    }
}

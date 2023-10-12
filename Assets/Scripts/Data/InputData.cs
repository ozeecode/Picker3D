using System;
using UnityEngine;

namespace Picker3D
{
    [CreateAssetMenu(menuName = "Picker3D/Input Data")]
    public class InputData : ScriptableObject
    {
        public float Horizontal;
        public Action TouchStart;
    }


}
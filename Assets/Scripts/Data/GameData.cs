using System;
namespace Picker3D
{
    [Serializable]
    public class GameData
    {
        public int Level { get; private set; }
        public int TotalCollectedObject { get; private set; }

        public void IncreaseLevel()
        {
            Level++;
        }

        public void IncreaseCollectedObject()
        {
            TotalCollectedObject++;
        }
    }

}

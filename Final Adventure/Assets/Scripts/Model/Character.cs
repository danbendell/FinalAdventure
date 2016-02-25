using UnityEngine;

namespace Assets.Scripts.Model
{
    public class Character : MonoBehaviour
    {
        public int Health { get; set; }

        public int Mana { get; set; }

        public int Experience { get; set; }

        public int Strength { get; set; }

        public int Defence { get; set; }

        public int Magic { get; set; }

        public int Resist { get; set; }

        public int Speed { get; set; }

        public int Accuracy { get; set; }

        public int Evasion { get; set; }

        public int Luck { get; set; }

        public Vector3 Position { get; set; }

        public virtual void WhoAmI()
        {
            print("I am a Character");
        }
    }
}

using UnityEngine;

namespace Assets.Scripts.Model
{
    public class Character
    {
        public int MaxHealth { get; set; }

        public int Health { get; set; }

        public int MaxMana { get; set; }

        public int Mana { get; set; }

        public int Experience { get; set; }

        public int Strength { get; set; }

        public int Defence { get; set; }

        public int Magic { get; set; }

        public int Resist { get; set; }

        public int Speed { get; set; }

        public int Accuracy { get; set; }

        public int Evasion { get; set; }

        public int CritChance { get; set; }

        public int Luck { get; set; }

        public Vector3 Position { get; set; }

        public virtual void WhoAmI()
        {
            //print("I am a Character");
        }

        public void TakeDamage(int amount)
        {
            if(Health > 0)
                Health -= amount;
            if (Health < 0) Health = 0;
        }

        public void Heal(int amount)
        {
            var manaCost = 10;
            if (Mana - manaCost < 0) return;

            if (Health < MaxHealth)
            {
                Mana -= manaCost;
                Health += amount;
            }  

            if (Health > MaxHealth) Health = MaxHealth;
        }
    }
}

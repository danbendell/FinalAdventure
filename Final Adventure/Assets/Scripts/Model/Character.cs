using Assets.Scripts.Damage;
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

        public int AttackRange { get; set; }

        public virtual void WhoAmI()
        {
            //print("I am a Character");
        }

        public void Attack(Character defender, int amount)
        {
            if(defender.Health > 0)
                defender.Health -= amount;
            if (defender.Health < 0) defender.Health = 0;
        }

        public bool Heal(Character reciever, int amount)
        {
            
            if (Mana - Damage.Heal.Cost < 0) return false;
            if (reciever.Health == reciever.MaxHealth) return false;

            if (reciever.Health < reciever.MaxHealth)
            {
                Mana -= (int) Damage.Heal.Cost;
                reciever.Health += amount;
            }  

            if (reciever.Health > reciever.MaxHealth) reciever.Health = reciever.MaxHealth;

            return true;
        }

        public bool Flare(Character defender, int damage)
        {
            if (Mana - Damage.Flare.Cost < 0) return false;

            if (defender.Health > 0)
            {
                Mana -= (int )Damage.Flare.Cost;
                defender.Health -= damage;
            }

            if (defender.Health < 0) defender.Health = 0;

            return true;
        }

        public Vector2 XyPosition()
        {
            return new Vector2(Position.x, Position.z);
        }

        public float Height()
        {
            return Position.y;
        }
    }
}

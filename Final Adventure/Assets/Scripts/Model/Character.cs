using System.Collections.Generic;
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

        public Vector2 AttackRange { get; set; }

        public List<Spell> Spells { get; protected set; }

        public List<Ability> Abilities { get; protected set; }

        public float AttackProbabilityModifier { get; set; }

        public float HealProbabilityModifier { get; set; }

        public virtual CharacterHolder.Jobs Job()
        {
            return CharacterHolder.Jobs.Wizard;
        }

        public virtual string Information()
        {
            return "No information";
        }

        public void Attack(Character defender, int amount)
        {
            if(defender.Health > 0)
                defender.Health -= amount;
            if (defender.Health < 0) defender.Health = 0;
        }

        public Vector2 XyPosition()
        {
            return new Vector2(Position.x, Position.z);
        }

        public float Height()
        {
            return Position.y;
        }

        public Ability GetRandomAbility()
        {
            List<Ability> castableAbilities = new List<Ability>();
            foreach (var ability in Abilities)
            {
                if(ability.Cost < Mana) castableAbilities.Add(ability);
            }
            if (castableAbilities.Count == 0) return null;
            int randomValue = Random.Range(0, castableAbilities.Count - 1);
            return castableAbilities[randomValue];
        }

        public Spell GetRandomSpell()
        {
            List<Spell> castableSpells = new List<Spell>();
            foreach (var spell in Spells)
            {
                if(spell.Name == "Heal") continue; 
                if (spell.Cost < Mana) castableSpells.Add(spell);
            }

            if (castableSpells.Count == 0) return null;

            int randomValue = Random.Range(0, castableSpells.Count - 1);
            return castableSpells[randomValue];
        }
    }
}

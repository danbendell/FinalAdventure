using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Model;

namespace Assets.Scripts.Damage.Abilities
{
    class Slash : Ability
    {
        public float AccuracyMod { get; private set; }

        public Slash()
        {
            Name = "Slash";
            Power = 22f;
            Cost = 8;
            Base = 250f;
            AccuracyMod = 0.7f;
        }

    }
}

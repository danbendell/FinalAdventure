using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Model;

namespace Assets.Scripts.Damage.Abilities
{
    class Assassinate : Ability
    {
        public Assassinate()
        {
            Name = "Assassinate";
            Power = 20f;
            Cost = 5f;
            Base = 250f;
            Desc = "Physical attack that scales on missing HP";
        }

    }
}

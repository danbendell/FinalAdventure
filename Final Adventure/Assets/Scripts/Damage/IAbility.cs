using UnityEngine;
using System.Collections;

public interface IAbility
{

    string Name { get; set; }
    float Power { get; set; }
    float Cost { get; set; }

    void Cast();

}

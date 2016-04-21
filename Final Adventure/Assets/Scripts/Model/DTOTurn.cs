using System;
using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Model
{
    [Serializable]
    public class DTOTurn
    {
        public int Id { get; set; }
        public int SurroundingAllyCount { get; set; }
        public int SurroundingOppositionCount { get; set; }
        public int TotalAllyCount { get; set; }
        public int TotalOppositionCount { get; set; }
        public string Job { get; set; }
        public float HealthPercent { get; set; }
        public float ManaPercent { get; set; }
        public int Move { get; set; }
        public string Action { get; set; }
    }
}

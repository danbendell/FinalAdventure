namespace Assets.Scripts.Model
{
    public class Character
    {
        private int _health;
        private int _mana;
        private int _experience;

        private int _strength;
        private int _defence;
        private int _magic;
        private int _resist;

        private int _speed;
        private int _accuracy;
        private int _evasion;
        private int _luck;

        public Character()
        {
            _health = 100;
            _mana = 100;
            _experience = 0;

            _strength = 10;
            _defence = 10;
            _magic = 10;
            _resist = 10;

            _speed = 3;
            _accuracy = 10;
            _evasion = 10;
            _luck = 10;
        }

        public Character(int health, int mana, int strength, int defence, int magic, int resist, int speed, int accuracy,
            int evasion, int luck)
        {
            _health = health;
            _mana = mana;

            _strength = strength;
            _defence = defence;
            _magic = magic;
            _resist = resist;

            _speed = speed;
            _accuracy = accuracy;
            _evasion = evasion;
            _luck = luck;
        }

        public int GetHealth()
        {
            return _health;
        }

        public int GetMana()
        {
            return _mana;
        }

        public int GetExperience()
        {
            return _experience;
        }

        public int GetStrength()
        {
            return _strength;
        }

        public int GetDefence()
        {
            return _defence;
        }

        public int GetMagic()
        {
            return _magic;
        }

        public int GetResist()
        {
            return _resist;
        }

        public int GetSpeed()
        {
            return _speed;
        }

        public int GetAccuracy()
        {
            return _accuracy;
        }

        public int GetEvasion()
        {
            return _evasion;
        }

        public int GetLuck()
        {
            return _luck;
        }
    }
}

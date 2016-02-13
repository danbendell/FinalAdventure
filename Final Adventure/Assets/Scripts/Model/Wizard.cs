namespace Assets.Scripts.Model
{
    public class Wizard : Character
    {
        public Character Character;

        public Wizard()
        {
            Character = new Character(80, 150, 5, 4, 15, 10, 3, 10, 10, 10);
        }
    }
}

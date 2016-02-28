using UnityEngine;

namespace Assets.Scripts.Model
{
    public class Tile {

        private GameObject _gameObject;

        public const string Normal = "Normal";
        public const string Walkpath = "Walkpath";
        public const string Highlight = "Highlight";

        private State _currentState;

        public Vector2 Position { get; private set; }

        public enum State
        {
            Walkable,
            Unwalkable,
        };    

        // Use this for initialization
        void Start () {
            _gameObject.GetComponent<Renderer>().material = _gameObject.GetComponent<Materials>().NormalMaterial;
            _currentState = State.Unwalkable;
        }
	
        // Update is called once per frame
        void Update () {
	
        }

        public void Clear()
        {

        }

        public void SetGameObject(GameObject tileGameObject)
        {
            _gameObject = tileGameObject;
            Position = new Vector2(_gameObject.transform.position.x, _gameObject.transform.position.z);
        }

        public GameObject GetGameObject()
        {
            return _gameObject;
        }

        public void SetMaterial(string material)
        {
            switch(material)
            {
                case Normal:
                    _gameObject.GetComponent<Renderer>().material = _gameObject.GetComponent<Materials>().NormalMaterial;
                    SetState(State.Unwalkable);
                    return;
                case Walkpath:
                    _gameObject.GetComponent<Renderer>().material = _gameObject.GetComponent<Materials>().WalkpathMaterial;
                    SetState(State.Walkable);
                    return;
                case Highlight:
                    _gameObject.GetComponent<Renderer>().material = _gameObject.GetComponent<Materials>().HighlightMaterial;
                    return;
                default:
                    _gameObject.GetComponent<Renderer>().material = _gameObject.GetComponent<Materials>().NormalMaterial;
                    return;
            }
        }

        public void SetState(State state)
        {
            _currentState = state;
        }

        public State GetState()
        {
            return _currentState;
        }

        public float GetHeight()
        {
            return _gameObject.transform.position.y + 1;
        }


    }
}

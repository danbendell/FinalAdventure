using UnityEngine;

namespace Assets.Scripts.Model
{
    public class Tile {

        private GameObject _gameObject;

        public const string Normal = "Normal";
        public const string Walkpath = "Walkpath";
        public const string Highlight = "Highlight";

        private State _currentState;

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
                    return;
                case Walkpath:
                    _gameObject.GetComponent<Renderer>().material = _gameObject.GetComponent<Materials>().WalkpathMaterial;
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


    }
}

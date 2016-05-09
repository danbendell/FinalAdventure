using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour
{
    public static readonly Vector3 MapPosition = new Vector3(0, 0, -1.61f);
    public static readonly Vector3 MapRotation = new Vector3(55f, 0, 0);

    public static readonly Vector3 CarouselPosition = new Vector3(0, 0.25f, -3f);
    public static readonly Vector3 CarouselRotation = new Vector3(0, 0, 0);

    private Vector3 _pos;

    private Vector3 _rot;

	// Use this for initialization
	void Start ()
	{
	    _pos = transform.localPosition;
	    _rot = transform.localEulerAngles;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    transform.localPosition = Vector3.Lerp(transform.localPosition, _pos, Time.deltaTime*3f);
	    transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, _rot, Time.deltaTime*3f);
	}

    public void Move(Vector3 position, Vector3 rotation)
    {
        _pos = position;
        _rot = rotation;
    }

    public void MoveToMap()
    {
        _pos = MapPosition;
        _rot = MapRotation;
    }

    public void MoveToCarousel()
    {
        _pos = CarouselPosition;
        _rot = CarouselRotation;
    }

    public bool IsAtMap()
    {
        return (_pos == MapPosition) && (_rot == MapRotation);
    }

    public bool IsAtCarousel()
    {
        return (_pos == CarouselPosition) && (_rot == CarouselRotation);
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpinCamera : MonoBehaviour
{

    public Vector3 PositionOne;
    public Vector3 PositionTwo;
    public Vector3 PositionThree;
    public Vector3 PositionFour;

    public Vector3 RotationOne;
    public Vector3 RotationTwo;
    public Vector3 RotationThree;
    public Vector3 RotationFour;

    private List<Vector3> _positions = new List<Vector3>();
    private List<Vector3> _rotations = new List<Vector3>();

    private int currentPosition = 0;

    // Use this for initialization
    void Start()
    {
        _positions.Add(PositionOne);
        _positions.Add(PositionTwo);
        _positions.Add(PositionThree);
        _positions.Add(PositionFour);

        _rotations.Add(RotationOne);
        _rotations.Add(RotationTwo);
        _rotations.Add(RotationThree);
        _rotations.Add(RotationFour);

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentPosition--;
            if (currentPosition < 0) currentPosition = 3;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            currentPosition++;
            if (currentPosition > 3) currentPosition = 0;
        }

        Rotate();

    }

    private void Rotate()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, _positions[currentPosition], Time.deltaTime * 5f);
        transform.localEulerAngles = _rotations[currentPosition];

    }
}
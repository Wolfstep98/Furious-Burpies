using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryPrediction : MonoBehaviour
{
    #region Fields & Properties
    [Header("Parameters")]
    [SerializeField]
    private float gravity = -9.81f;

    [SerializeField]
    private Vector2 initialPosition = Vector2.zero;
    public Vector2 InitialPosition { get { return this.initialPosition; } set { this.initialPosition = value; } }

    [SerializeField]
    private Vector2 initialVelocity = Vector2.zero;
    public Vector2 InitialVelocity { get { return this.initialVelocity; } set { this.initialVelocity = value; } }
    #endregion

    #region Methods
    public void Setup(float gravity, Vector2 initialPosition, Vector2 initialVelocity)
    {
        this.gravity = gravity;
        this.initialPosition = initialPosition;
        this.initialVelocity = initialVelocity;
    }

    public Vector2 CalculatePositionAtTime(float t)
    {
        Vector2 position = Vector2.zero;
        position = new Vector2(0.0f,((this.gravity * (t * t)) / 2.0f)) + this.initialVelocity * t + this.initialPosition;
        return position;
    }
    #endregion
}

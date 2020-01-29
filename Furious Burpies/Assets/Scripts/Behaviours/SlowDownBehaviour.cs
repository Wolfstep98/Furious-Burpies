using System;
using UnityEngine;

public interface ISlownDownBehaviour
{
    void SlowDown();
    void SlowDown(float timeSlowDown);
    void RevertSlowDown();
}

public class SlowDownBehaviour : MonoBehaviour, ISlownDownBehaviour
{
    #region Fields & Properties
    [Header("Parameters")]
    [SerializeField]
    private bool isEnable = true;

    [SerializeField]
    private bool isTimeSlowDown = false;
    public bool IsTimeSlowDown { get { return this.isTimeSlowDown; } }

    [SerializeField]
    [Range(0.0f,1.0f)]
    private float timeSlowDown = 0.1f;
    public float TimeSlowDown { get { return this.timeSlowDown; } }

    [SerializeField]
    private float lastGravityScale = 1.0f;
    [SerializeField]
    private float lastMass = 1.0f;

    [Header("References")]
    [SerializeField]
    new private Rigidbody rigidbody = null;
    #endregion

    #region Methods
    public void SlowDown()
    {
        this.SlowDown(this.timeSlowDown);
    }

    public void SlowDown(float timeSlowDown)
    {
        if (this.isEnable)
        {
            if (!this.isTimeSlowDown)
            {
                this.isTimeSlowDown = true;
                this.timeSlowDown = timeSlowDown;
                //this.lastGravityScale = this.rigidbody2D.gravityScale;
                //this.lastMass = this.rigidbody2D.mass;

                //Vector2 vel = this.rigidbody2D.velocity;
                //vel *= timeSlowDown;

                //Debug.Log("Slow down : " + this.rigidbody2D.velocity + " => " + vel);

                //this.rigidbody2D.gravityScale = timeSlowDown;
                //this.rigidbody2D.mass = this.lastMass * timeSlowDown;
                //this.rigidbody2D.velocity = vel;

                GameTime.timeScale = this.timeSlowDown;
                Time.timeScale = this.timeSlowDown;
            }
            else
            {
                Debug.LogError("The behaviour slowdown is already active !");
            }
        }
    }

    public void RevertSlowDown()
    {
        if (this.isEnable)
        {
            if (this.isTimeSlowDown)
            {
                //Vector2 vel = this.rigidbody2D.velocity;
                //vel /= this.timeSlowDown;

                //this.rigidbody2D.mass = this.lastMass;
                //this.rigidbody2D.gravityScale = this.lastGravityScale;
                //this.rigidbody2D.velocity = vel;
                this.isTimeSlowDown = false;

                GameTime.timeScale = 1.0f;
                Time.timeScale = 1.0f;
            }
            else
            {
                Debug.LogWarning("The behaviour slowdown is already inactive !");
            }
        }
    }

    public void UpdateEnable(bool value)
    {
        this.isEnable = value;
    }
    #endregion
}

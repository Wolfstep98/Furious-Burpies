using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DebugRigidbody2D : MonoBehaviour
{
    #region Fields & Properties
    [Header("Debug")]
    [SerializeField]
    private bool showVelocity = true;
    [SerializeField]
    private bool showCustomVelocity = true;

    [SerializeField]
    private Vector2 velocity = Vector2.zero;

    [Header("References")]
    [SerializeField]
    private CustomRigidbody2D customRigidbody2D = null;
    [SerializeField]
    new private Rigidbody2D rigidbody2D = null;
    #endregion

    #region 
    void Update ()
    {
		if(this.showVelocity)
        {
            this.velocity = this.rigidbody2D.velocity;
            Debug.DrawRay(this.transform.position, this.rigidbody2D.velocity, Color.black);
            Debug.DrawRay(this.transform.position, Vector2.right * this.rigidbody2D.velocity.x, Color.red);
            Debug.DrawRay(this.transform.position, Vector2.up * this.rigidbody2D.velocity.y, Color.green);
        }
        if (this.showCustomVelocity)
        {
            this.velocity = this.rigidbody2D.velocity;
            Debug.DrawRay(this.transform.position, this.customRigidbody2D.Velocity, Color.black);
            Debug.DrawRay(this.transform.position, Vector2.right * this.customRigidbody2D.Velocity.x, Color.red);
            Debug.DrawRay(this.transform.position, Vector2.up * this.customRigidbody2D.Velocity.y, Color.green);
        }
    }
    #endregion
}

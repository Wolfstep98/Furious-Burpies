using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    #region Fields & Properties
    [Header("Parameters")]
    [SerializeField]
    private Axis axis = Axis.X;
    [SerializeField]
    private int score = 0;

    [Header("UI")]
    [SerializeField]
    private Text textScore = null;

    [Header("References")]
    [SerializeField]
    private Transform startPoint = null;
    [SerializeField]
    private Transform player = null;

    #endregion

    #region Methods
    private void Awake()
    {
#if UNITY_EDITOR
        if (this.startPoint == null)
            Debug.LogError("[Missing Reference] - StartPoint is missing !");
        if (this.player == null)
            Debug.LogError("[Missing Reference] - player is missing !");
        if (this.textScore == null)
            Debug.LogError("[Missing Reference] - textScore is missing !");
#endif
    }

    private void FixedUpdate()
    {
        this.CalculateScore();
    }

    private void CalculateScore()
    {
        float player = 0.0f;
        float start = 0.0f;
        switch(this.axis)
        {
            case Axis.X:
                player = this.player.position.x;
                start = this.startPoint.position.x;
                break;
            case Axis.Y:
                player = this.player.position.y;
                start = this.startPoint.position.y;
                break;
            case Axis.Z:
                player = this.player.position.z;
                start = this.startPoint.position.z;
                break;
            default:
                break;
        }
        float distance = player - start;
        score = Mathf.FloorToInt(distance);

        this.UpdateUI();
    }

    private void UpdateUI()
    {
        this.textScore.text = score.ToString();
    }
    #endregion
}

using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    #region Fields & Properties
    [Header("Parameters")]
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
        float playerX = this.player.position.x;
        float startX = this.startPoint.position.x;
        float distance = playerX - startX;
        score = Mathf.FloorToInt(distance);

        this.UpdateUI();
    }

    private void UpdateUI()
    {
        this.textScore.text = score.ToString();
    }
    #endregion
}

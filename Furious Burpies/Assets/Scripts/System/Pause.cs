using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField]
    private bool isPaused = false;
    public bool IsPaused { get { return this.isPaused; } }

    [Header("References")]
    [SerializeField]
    private CustomRigidbodyInput customRigidbodyInput = null;

    [Header("UI")]
    [SerializeField]
    private GameObject panelPause = null;


    public void PauseGame()
    {

        this.panelPause.SetActive(true);

        this.customRigidbodyInput.Cancel();

        Time.timeScale = 0.0f;
        this.isPaused = true;
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1.0f;

        this.panelPause.SetActive(false);

        this.customRigidbodyInput.Cancel();

        this.isPaused = false;
    }

    public void TogglePause()
    {
        if (this.isPaused)
            this.UnPauseGame();
        else
            this.PauseGame();
    }
}

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
    private CatapultInput catapultInput = null;

    [Header("UI")]
    [SerializeField]
    private GameObject panelPause = null;


    public void PauseGame()
    {
        this.catapultInput.Cancel();
        this.catapultInput.InputEnabled = false;

        this.panelPause.SetActive(true);

        GameTime.timeScale = 0.0f;
        this.isPaused = true;
    }

    public void UnPauseGame()
    {
        GameTime.timeScale = 1.0f;
        this.catapultInput.Cancel();
        this.catapultInput.InputEnabled = true;

        this.panelPause.SetActive(false);

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

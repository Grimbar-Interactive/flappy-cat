using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A singleton class to manage the core loop of the game. Includes an example of scene loading,
/// which is used to reload the gameplay scene in this case.
/// </summary>
public class GameManager : Singleton<GameManager>
{
    [SerializeField] private float stopTimeOnGameOver = 1f;
    [SerializeField] private CanvasGroup startGameGroup;
    [SerializeField] private GameOverMenu gameOverMenu;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip loseSFX;

    private bool _gameStarted = false;
    
    private new void Awake()
    {
#if !UNITY_STANDALONE
        Application.targetFrameRate = 60;
#endif
        base.Awake();
        Time.timeScale = 0f;
    }

    public void StartGame()
    {
        if (_gameStarted) return;
        _gameStarted = true;
        startGameGroup.DOKill();
        startGameGroup.DOFade(0f, 0.5f);
        Time.timeScale = 1f;
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void TriggerGameOver()
    {
        SlowToStop(stopTimeOnGameOver);
        musicSource.Stop();
    }
    
    private void SlowToStop(float stopTime)
    {
        StartCoroutine(SlowRoutine());
        return;

        IEnumerator SlowRoutine()
        {
            var timer = stopTime;
            while (timer > 0f)
            {
                timer -= Time.unscaledDeltaTime;
                Time.timeScale = Mathf.Lerp(0f, 1f, timer / stopTime);
                yield return null;
            }
            Time.timeScale = 0f;
            gameOverMenu.Show();
            musicSource.PlayOneShot(loseSFX);
        }
    }
}

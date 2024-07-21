using System;
using DG.Tweening;
using Dialogue;
using Enemy;
using LockAndDoor;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinalLevelSequence : MonoBehaviour
{
    [SerializeField]
    private EnemyManager enemyManager;

    [SerializeField]
    private Door exitDoor;

    [SerializeField]
    private int enemyCount;

    [SerializeField]
    private string onAllEnemyDeathDialogueKey;
    [SerializeField]
    private CanvasGroup endPanel;
    [SerializeField]
    private float endFadeDuration = 2f;
    
    [SerializeField]
    private Button mainMenuButton;
    [SerializeField]
    private TextMeshProUGUI endText;
    
    private GameManager _gameManager;
    private DialogueManager _dialogueManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _dialogueManager = DialogueManager.Instance;
        enemyManager.SpawnEnemies(enemyCount);
        enemyManager.OnAllEnemiesDeathEvent += OnAllEnemiesDeath;
        mainMenuButton.onClick.AddListener(MainMenu);
        
        endPanel.gameObject.SetActive(false);
        mainMenuButton.gameObject.SetActive(false);
    }

    private void OnAllEnemiesDeath()
    {
        _dialogueManager.OnDialogueComplete += EndGame;
        _dialogueManager.StartDialogue(onAllEnemyDeathDialogueKey);
    }
    
    private void EndGame(string key)
    {
        if (key.Equals(onAllEnemyDeathDialogueKey))
        {
            Time.timeScale = 0f;
            endPanel.gameObject.SetActive(true);
            endPanel.alpha = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            endPanel.DOFade(1f, endFadeDuration).SetUpdate(true).OnComplete(() =>
            {
                endText.text = "THE END..?";
                mainMenuButton.gameObject.SetActive(true);
            });
            _dialogueManager.OnDialogueComplete -= EndGame;
        }
    }

    private void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}

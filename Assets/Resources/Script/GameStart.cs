using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStart : MonoBehaviour
{
    [SerializeField] Button startButton;

    private void Start()
    {
        startButton.onClick.AddListener(() => SceneManager.LoadScene("GameScene"));
    }
}

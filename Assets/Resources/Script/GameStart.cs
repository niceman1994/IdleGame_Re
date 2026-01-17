using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    private void Update()
    {
        ClickStart();
    }

    private void ClickStart()
    {
        if (Input.GetMouseButtonDown(0))
            SceneManager.LoadScene("GameScene");
    }
}

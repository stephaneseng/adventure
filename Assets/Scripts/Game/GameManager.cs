using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        if (!SceneManager.GetSceneByName("Level1").isLoaded) {
            SceneManager.LoadScene("Level1", LoadSceneMode.Additive);
        }
    }
}

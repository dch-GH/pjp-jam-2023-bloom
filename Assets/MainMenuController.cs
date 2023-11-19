using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private Button _play;

    [SerializeField]
    private Button _quit;

    void Awake()
    {
        _play.onClick.AddListener(() =>
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
            SceneManager.LoadSceneAsync("Main");
        });


        _quit.onClick.AddListener(() =>
        {
            Application.Quit(0);
        });

    }
}
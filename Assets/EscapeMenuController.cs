using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscapeMenuController : MonoBehaviour
{
    public static EscapeMenuController Instance;

    public bool Open = false;

    [SerializeField]
    private Button _quit;

    void Awake()
    {
        Instance = this;
        _quit.onClick.AddListener(() =>
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
            SceneManager.LoadSceneAsync("Menu");
        });

    }
}
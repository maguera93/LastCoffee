using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLogic : MonoBehaviour {

    [SerializeField]
    private CameraFader cameraFader;

    // Use this for initialization
    void Start()
    {
        Vector2 viewportPos = Vector2.one * 0.5f;
        cameraFader.OpenCircle(1f, viewportPos);

    }

    public void LoadLevel(int scene)
    {
        Vector2 viewportPos = Vector2.one * 0.5f;
        cameraFader.CloseCircle(1f, viewportPos);
        StartCoroutine(LoadScene(scene));
    }

    IEnumerator LoadScene(int scene)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(scene);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

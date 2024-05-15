using UnityEngine;

public class exitGame : MonoBehaviour
{

    [SerializeField]
    string loadScene;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        UnityEngine.SceneManagement.SceneManager.LoadScene(loadScene);
    }
}

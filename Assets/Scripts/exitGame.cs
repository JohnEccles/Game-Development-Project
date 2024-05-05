using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class exitGame : MonoBehaviour
{

    [SerializeField]
    string loadScene;

    private void OnTriggerEnter(Collider other)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(loadScene);
    }
}

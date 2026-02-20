
using UnityEngine;

public class revealSecretObjects : MonoBehaviour
{
    public GameObject[] gameObjects;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (var obj in gameObjects)
        {
            GameObject go = obj as GameObject;
            go.SetActive(true);
        }
    }
    private void Start()
    {
        foreach (var obj in gameObjects)
        {
            GameObject go = obj as GameObject;
            go.SetActive(false);
        }
    }
}

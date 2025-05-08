using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance;
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void showDialog()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    public void hideDialog()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}

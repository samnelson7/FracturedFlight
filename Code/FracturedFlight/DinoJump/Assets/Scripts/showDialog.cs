using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class showDialog : MonoBehaviour
{
    private bool dialogShown = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!dialogShown) DialogManager.instance.showDialog();
    }
}

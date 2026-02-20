using UnityEngine.UI;
using UnityEngine;
public class showDialog : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        DialogManager.instance.showDialog();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        DialogManager.instance.hideDialog();
    }
}

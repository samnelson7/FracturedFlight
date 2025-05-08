using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class HoverColorChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Color normalColor = Color.white;
    public Color hoverColor = Color.gray;
    public bool isLocked = false;

    private Graphic[] graphics;

    void Awake()
    {
        graphics = GetComponentsInChildren<Graphic>(includeInactive: true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isLocked) return;
        foreach (Graphic g in graphics)
        {
            if (g.gameObject == this.gameObject) continue;
            g.color = hoverColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isLocked) return;
        foreach (Graphic g in graphics)
        {
            if (g.gameObject == this.gameObject) continue;
            g.color = normalColor;
        }
    }
}

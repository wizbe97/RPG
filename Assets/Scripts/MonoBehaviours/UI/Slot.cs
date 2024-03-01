using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Text qtyText;
    public Image highlightImage;

    public void HighlightSlot(bool highlight)
    {
        highlightImage.enabled = highlight;
    }
}

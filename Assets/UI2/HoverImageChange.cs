using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverImageChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite originalSprite;  // The original image sprite
    public Sprite hoverSprite;     // The image sprite to display on hover

    private Image imageComponent;

    void Start()
    {
        // Get the Image component on this GameObject
        imageComponent = GetComponent<Image>();

        // Ensure the originalSprite is set to the current image if not assigned
        if (imageComponent && originalSprite == null)
        {
            originalSprite = imageComponent.sprite;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Change to hover sprite when the pointer enters
        if (imageComponent && hoverSprite != null)
        {
            imageComponent.sprite = hoverSprite;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Revert to the original sprite when the pointer exits
        if (imageComponent && originalSprite != null)
        {
            imageComponent.sprite = originalSprite;
        }
    }
}

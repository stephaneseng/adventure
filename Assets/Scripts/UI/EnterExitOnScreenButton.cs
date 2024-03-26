using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;

[AddComponentMenu("Input/Enter Exit On-Screen Button")]
public class EnterExitOnScreenButton : OnScreenButton, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler,
    IPointerExitHandler
{
    private Image image;

    public Sprite sprite;
    public Sprite pressedSprite;

    void Awake()
    {
        image = GetComponent<Image>();

        image.sprite = sprite;
    }

    public new void OnPointerDown(PointerEventData eventData)
    {
        PressButton();
    }

    public new void OnPointerUp(PointerEventData eventData)
    {
        ReleaseButton();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PressButton();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ReleaseButton();
    }

    private void PressButton()
    {
        SendValueToControl(1.0f);
        image.sprite = pressedSprite;
    }

    private void ReleaseButton()
    {
        SendValueToControl(0.0f);
        image.sprite = sprite;
    }
}

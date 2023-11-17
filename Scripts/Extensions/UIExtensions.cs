using UnityEngine;

public static class UIExtensions
{
    public static void SetVisibleAndInteractable(this CanvasGroup canvasGroup, bool isVisibleAndInteractable)
    {
        SetVisible(canvasGroup, isVisibleAndInteractable);
        SetInteractable(canvasGroup, isVisibleAndInteractable);
    }

    public static void SetVisible(this CanvasGroup canvasGroup, bool isVisible)
    {
        canvasGroup.alpha = isVisible ? 1f : 0f;
    }

    public static void SetInteractable(this CanvasGroup canvasGroup, bool isInteractable)
    {
        canvasGroup.interactable = isInteractable;
        canvasGroup.blocksRaycasts = isInteractable;
    }
}
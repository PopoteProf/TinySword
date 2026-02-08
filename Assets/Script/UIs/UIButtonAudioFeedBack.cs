using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonAudioFeedBack : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISubmitHandler,
    IPointerDownHandler {
    [SerializeField] private AudioElement AudioOnPointerEnter;
    [SerializeField] private AudioElement AudioOnPointerExit;
    [SerializeField] private AudioElement AudioOnsubmit;
    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioOnPointerEnter.PlayAsSFX();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        AudioOnPointerExit.PlayAsSFX();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        AudioOnsubmit.PlayAsSFX();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        AudioOnsubmit.PlayAsSFX();
    }
}
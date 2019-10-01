using UnityEngine;
using UnityEngine.EventSystems;

public class RestartOnPress : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData) => GameStarter.Game.StartGame();
}
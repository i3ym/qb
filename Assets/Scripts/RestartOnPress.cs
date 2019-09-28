using UnityEngine;
using UnityEngine.EventSystems;

public class RestartOnPress : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    Game Game = null;

    public void OnPointerDown(PointerEventData eventData) => Game.StartGame();
}
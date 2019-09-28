using qb.Shared;
using UnityEngine;
using UnityEngine.UI;

public class FieldRendererUnityInfo : MonoBehaviour
{
    [SerializeField]
    public GameObject FieldGameObjectPrefab = null;
    [SerializeField]
    public Transform NextObjectsParent = null;
    
    public GameMatrix<RawImage> GameObjects, NextObjects;
}
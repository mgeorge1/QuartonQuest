using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Piece: MonoBehaviour
{
    public delegate void ClickedPiece(string name);
    public static event ClickedPiece OnClickPiece;

    public static bool Disabled = false;
    public bool IsClickable
    {
        get
        {
            return (!EventSystem.current.IsPointerOverGameObject() && !placed && !onDeck && !Disabled);
        }
    }

    public Color DefaultColor;
    public Color MouseOverColor;
    public UnityEngine.GameObject PieceVisual;
    public bool placed = false;
    public bool onDeck = false;
    public bool isSmall = false;

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.GameObject Piece = UnityEngine.GameObject.Find(name);
        PieceVisual = Piece.transform.GetChild(0).gameObject;
    }
    
    private void OnMouseEnter()
    {
        if (!IsClickable)
            return;

        PieceVisual.GetComponent<Renderer>().material.SetColor("_Color", MouseOverColor);      
    }

    private void OnMouseExit()
    {
        PieceVisual.GetComponent<Renderer>().material.SetColor("_Color", DefaultColor);
    }

    private void OnMouseDown()
    {
        if (!IsClickable)
            return;

        if (!placed)
        {
            OnClickPiece?.Invoke(getName());
        }
    }
    
    private string getName()
    {
        return PieceVisual.name;
    }
}

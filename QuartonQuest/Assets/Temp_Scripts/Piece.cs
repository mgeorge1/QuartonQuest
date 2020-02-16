using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece: MonoBehaviour
{
    public delegate void ClickedPiece(string name);
    public static event ClickedPiece OnClickPiece;

    public Color DefaultColor;
    public Color MouseOverColor;
    public UnityEngine.GameObject PieceVisual;
    public bool placed = false;

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.GameObject Piece = UnityEngine.GameObject.Find(name);
        PieceVisual = Piece.transform.GetChild(0).gameObject;
        //Debug.Log(PieceVisual.name);
    }
    
    private void OnMouseEnter()
    {
            PieceVisual.GetComponent<Renderer>().material.SetColor("_Color", MouseOverColor);      
    }

    private void OnMouseExit()
    {
            PieceVisual.GetComponent<Renderer>().material.SetColor("_Color", DefaultColor);
    }

    private void OnMouseDown()
    {
        if(!placed)
        {
            OnClickPiece?.Invoke(getName());
        }
    }
    
    private string getName()
    {
        return PieceVisual.name;
    }
}

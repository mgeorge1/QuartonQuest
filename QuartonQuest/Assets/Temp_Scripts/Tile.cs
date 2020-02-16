using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public delegate void ClickedTile(string name);
    public static event ClickedTile OnClickTile;

    public Color DefaultColor;
    public Color MouseOverColor;
    public UnityEngine.GameObject TileVisual;
    public Piece localPiece;

    // Start is called before the first frame update
    void Start()
    {
        GameObject Tile = UnityEngine.GameObject.Find(name);
        TileVisual = Tile.transform.GetChild(0).gameObject;
        //Debug.Log(TileVisual.name);
    }

    private void OnMouseEnter()
    {
        if (localPiece==null)
        {
            TileVisual.GetComponent<Renderer>().material.SetColor("_Color", MouseOverColor);
        }
    }

    private void OnMouseExit()
    {
        if (localPiece==null)
        {
            TileVisual.GetComponent<Renderer>().material.SetColor("_Color", DefaultColor);
        }
    }

    public void OnMouseDown()
    {
        if (localPiece==null)
        {
            OnClickTile?.Invoke(getName());
        }
    }

    private string getName()
    {
        return TileVisual.name;
    }
}

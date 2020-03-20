using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{
    public delegate void ClickedTile(string name);
    public static event ClickedTile OnClickTile;
    public bool IsClickable
    {
        get
        {
            return (!EventSystem.current.IsPointerOverGameObject());
        }
    }

    public Color DefaultColor;
    public Color MouseOverColor;
    public UnityEngine.GameObject TileVisual;
    public Piece localPiece;

    // Start is called before the first frame update
    void Start()
    {
        GameObject Tile = UnityEngine.GameObject.Find(name);
        TileVisual = Tile.transform.GetChild(0).gameObject;
    }

    private void OnMouseEnter()
    {
        if (!IsClickable)
            return;

        if (localPiece==null)
        {
            TileVisual.GetComponent<Renderer>().material.SetColor("_Color", MouseOverColor);
        }
    }

    private void OnMouseExit()
    {
        if (!IsClickable)
            return;

        if (localPiece==null)
        {
            TileVisual.GetComponent<Renderer>().material.SetColor("_Color", DefaultColor);
        }
    }

    public void OnMouseDown()
    {
        if (!IsClickable)
            return;

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

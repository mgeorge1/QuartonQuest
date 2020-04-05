using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Color SelectColor;

    public Piece SelectedPiece { get; set; }

    public GameObject OnDeckTile;

    public Vector3 currentOnDeck;

    public void DisableTileClicking()
    {
        Tile.OnClickTile -= OnClickedTile;
        Tile.Disabled = true;
    }

    public void EnableTileClicking()
    {
        Tile.OnClickTile += OnClickedTile;
        Tile.Disabled = false;
    }

    public void DisablePieceClicking()
    {
        Piece.OnClickPiece -= OnClickedPiece;
        Piece.Disabled = true;
    }

    public void EnablePieceClicking()
    {
        Piece.Disabled = false;
        Piece.OnClickPiece += OnClickedPiece;
    }

    public void OnClickedTile(string name)
    {
        Tile selectedTile = UnityEngine.GameObject.Find("Tile" + name).GetComponent<Tile>();
        selectedTile.ResetColor();
        MovePiece(SelectedPiece, selectedTile);
    }

    public void OnClickedPiece(string name)
    {
        SelectedPiece = UnityEngine.GameObject.Find("Piece" + name).GetComponent<Piece>();
        MoveOnDeck();
    }

    public void Start()
    {
        DisableTileClicking();
        EnablePieceClicking();
    }


    public void MoveOnDeck()
    {
        // Not sure why this line is necessary, but it really is
        if (OnDeckTile == null)
            return;

        SelectedPiece.onDeck = true;
        Vector3 offset = new Vector3(0, GameCoreController.Instance.transform.localScale.y, 0);
        SelectedPiece.transform.position = OnDeckTile.transform.position + offset;
    }

    public void MovePiece(Piece piece, Tile tile)
     {
        if(SelectedPiece!=null)
        {
            Vector3 offset = new Vector3(0, GameCoreController.Instance.transform.localScale.y, 0);
            piece.transform.position = tile.transform.position + offset;
            tile.localPiece = SelectedPiece;
            SelectedPiece.placed = true;
            SelectedPiece.onDeck = false;
            SelectedPiece = null;
        }
        
    }

    public void MovePiece(string tileName)
    {
        Debug.Log(tileName);
        OnClickedTile(tileName);
    }

    public void MoveOnDeck(string pieceName)
    {
        Debug.Log(pieceName);
        SelectedPiece = UnityEngine.GameObject.Find("Piece" + pieceName).GetComponent<Piece>();
        MoveOnDeck();
    }
}

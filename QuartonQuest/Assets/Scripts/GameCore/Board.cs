using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Color SelectColor;

    public Piece SelectedPiece { get; set; }

    public GameObject OnDeckTile;

    public Vector3 currentOnDeck;
    private float smallPieceOffset = 0.4f;
    private float offsetMultiplier = 0.75f;
    private float pieceMovementSpeed = 0.5f;

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

        if (AudioManager.instance != null)
            AudioManager.instance.PlaySoundEffect("PieceTeleport");

        SelectedPiece.onDeck = true;

        float yOffset;
        if (SelectedPiece.isSmall)
            yOffset = (GameCoreController.Instance.transform.localScale.y * offsetMultiplier) - smallPieceOffset;
        else
            yOffset = GameCoreController.Instance.transform.localScale.y * offsetMultiplier;

        Vector3 offset = new Vector3(0, yOffset, 0);
        //SelectedPiece.transform.position = OnDeckTile.transform.position + offset;
        iTween.MoveTo(SelectedPiece.gameObject, OnDeckTile.transform.position + offset, pieceMovementSpeed);
    }

    public void MovePiece(Piece piece, Tile tile)
     {
        if(SelectedPiece!=null)
        {
            if (AudioManager.instance != null)
                AudioManager.instance.PlaySoundEffect("PieceTeleport");

            float yOffset;
            if (SelectedPiece.isSmall)
                yOffset = (GameCoreController.Instance.transform.localScale.y * offsetMultiplier) - smallPieceOffset;
            else
                yOffset = GameCoreController.Instance.transform.localScale.y * offsetMultiplier;


            Vector3 offset = new Vector3(0, yOffset, 0);
            //piece.transform.position = tile.transform.position + offset;
            iTween.MoveTo(piece.gameObject, tile.transform.position + offset, pieceMovementSpeed);
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

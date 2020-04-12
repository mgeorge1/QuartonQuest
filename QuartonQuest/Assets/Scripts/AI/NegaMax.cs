using HeuristicCalculator;
using System.Linq;
using System.Collections.Generic;
using System;

namespace AI
{
    
    public struct winningMove
    {
        public QuartoSearchTree.Node winningNode;
        public int heuristicValue;
        public bool isWin;
    }
    class NegaMax
    {
        public static int totalSearch = 0;
        public const int INFINITY = 100;
        // Searches tree generated for the best play. Selects the best move from the move given by the opponent.
        public static winningMove searchForBestPlay(QuartoSearchTree.Node currentNode, int depth, int minimax, int alpha, int beta, bool maxPlayer)
        {
            int j = 0;
            string pieceString;
            int boardPosition = 0;
            int minimaxCompare;
            winningMove winChoice = new winningMove();
            winningMove winChoice2 = new winningMove();
            winChoice.winningNode = new QuartoSearchTree.Node();
            winChoice.isWin = false;

            // If only root in tree
            if (currentNode.parent == null && currentNode.children[0] == null)
            {
                
            }

            // Detects for win
            if (currentNode.parent == null)
            {
                for (int i = 0; i < QuartoSearchTree.MAXGAMEBOARD && currentNode.children[i] != null; i++)
                {
                    if (currentNode.pieceToPlay == QuartoSearchTree.NULLPIECE)
                        pieceString = null;
                    else
                        pieceString = currentNode.pieces[currentNode.pieceToPlay].piece;

                    while (currentNode.gameBoard[boardPosition] != null)
                    {
                        boardPosition++;
                    }
                    bool win = Heuristic.isWin(currentNode.gameBoard, pieceString, boardPosition);

                    if (win)
                    {
                        winChoice.winningNode = currentNode.children[i];
                        winChoice.heuristicValue = 0;
                        winChoice.isWin = true;
                        return winChoice;
                    }
                    boardPosition++;
                }
            }

            // If it is the bottom of the tree
            if (currentNode.children[0] == null)
            {
                int value;

                for (int i = 0; i < QuartoSearchTree.MAXGAMEBOARD && currentNode.parent.children[i] != null; i++)
                {
                    if (currentNode.parent.children[i].pieceToPlay == QuartoSearchTree.NULLPIECE)
                        pieceString = null;
                    else
                        pieceString = currentNode.parent.children[i].pieces[currentNode.parent.children[i].pieceToPlay].piece;

                    minimaxCompare = Heuristic.calculateHeuristic(currentNode.parent.children[i].gameBoard, pieceString);

                    if (i == 0)
                    {
                        minimax = Heuristic.calculateHeuristic(currentNode.parent.children[i].gameBoard, pieceString);
                        winChoice.winningNode = currentNode.parent.children[i];
                        winChoice.heuristicValue = minimax;
                        totalSearch++;
                    }
                    else if ((minimaxCompare > minimax && maxPlayer == true) || (minimaxCompare < minimax && maxPlayer == false))
                    {
                        minimax = minimaxCompare;
                        winChoice.winningNode = currentNode.parent.children[i];
                        winChoice.heuristicValue = minimax;
                        totalSearch++;
                    }

                    //prunes the rest of children if they can be pruned
                    if (maxPlayer)
                    {
                        value = -INFINITY;
                        if (winChoice.heuristicValue > value)
                            value = winChoice.heuristicValue;
                        if (value > alpha)
                            alpha = value;
                        if (alpha >= beta)
                            break;
                    }

                    else
                    {
                        value = INFINITY;
                        if (winChoice.heuristicValue < value)
                            value = winChoice.heuristicValue;
                        if (value < beta)
                            beta = value;
                        if (alpha >= beta)
                            break;
                    }

                    j++;
                }
            }

            // Iterates through all children nodes
            else
            {
                int value;

                while (j < QuartoSearchTree.MAXGAMEBOARD && currentNode.children[j] != null)
                {
                    if (j == 0)
                    {

                        winChoice = searchForBestPlay(currentNode.children[j], depth, minimax, alpha, beta, !maxPlayer);

                        if (currentNode.parent == null || currentNode.parent.parent == null) { }

                        // When it is evaluating the next move from the root
                        else if (currentNode.parent.parent.parent == null)
                        {
                            winChoice.winningNode = currentNode;
                            totalSearch++;
                        }
                    }

                    else
                    {
                        winChoice2 = searchForBestPlay(currentNode.children[j], depth, minimax, alpha, beta, !maxPlayer);

                        if ((winChoice2.heuristicValue > winChoice.heuristicValue && maxPlayer == true) || (winChoice2.heuristicValue < winChoice.heuristicValue && maxPlayer == false))
                        {
                            winChoice.heuristicValue = winChoice2.heuristicValue;

                            if (currentNode.parent == null)
                            {
                                winChoice.winningNode = winChoice2.winningNode;
                                totalSearch++;
                            }
                            else if (currentNode.parent.parent == null)
                            {
                                winChoice.heuristicValue = winChoice2.heuristicValue;
                                winChoice.winningNode = winChoice2.winningNode;
                                totalSearch++;
                            }

                            // When it is evaluating the next move from the root
                            else if (currentNode.parent.parent.parent == null)
                            {
                                winChoice.winningNode = currentNode;
                                winChoice.heuristicValue = winChoice2.heuristicValue;
                                totalSearch++;
                            }
                        }
                    }

                    //prunes the rest of children if they can be pruned
                    if (maxPlayer)
                    {
                        value = -INFINITY;
                        if (winChoice.heuristicValue > value)
                            value = winChoice.heuristicValue;
                        if (value > alpha)
                            alpha = value;
                        if (alpha >= beta)
                            break;
                    }

                    else
                    {
                        value = INFINITY;
                        if (winChoice.heuristicValue < value)
                            value = winChoice.heuristicValue;
                        if (value < beta)
                            beta = value;
                        if (alpha >= beta)
                            break;
                    }
                    j++;
                }
            }

            if (currentNode.parent == null)
                RandomPlay(winChoice);

            return winChoice;
        }

        // Only selects a random piece and position when there is no plays better than the one originally selected
        public static winningMove RandomPlay(winningMove winChoice)
        {
            int rand;
            var rnd = new Random();
            List<int> playablePieces;
            List<int> playablePositions;

            if (winChoice.heuristicValue == 0)
            {
                
                playablePositions = AIFunctions.makePlayablePositionList(winChoice.winningNode.gameBoard);
                if (playablePositions.Count() == 0)
                    return winChoice;
                else if (playablePositions.Count() - 1 == 0)
                    rand = 0;
                else
                    rand = rnd.Next(0, playablePositions.Count() - 1);
                winChoice.winningNode.moveOnBoard = playablePositions[rand];

                playablePieces = AIFunctions.makePlayablePiecesOnly(winChoice.winningNode.pieces);
                if (playablePieces.Count() == 0)
                    return winChoice; 
                else if (playablePieces.Count() - 1 == 0)
                    rand = 0;
                else
                    rand = rnd.Next(0, playablePieces.Count() - 1);
                winChoice.winningNode.pieceToPlay = playablePieces[rand];
            }

            return winChoice;
        }
    }
}

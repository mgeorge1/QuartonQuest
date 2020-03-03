using System;
using System.Collections.Generic;
using System.Text;
using HeuristicCalculator;
namespace AI
{
    class NegaMax
    {
        // Searches tree generated for the best play. Selects the best move from the move given by the opponent.
        public static QuartoSearchTree.winningMove searchForBestPlay(QuartoSearchTree.Node currentNode, int depthCounter, int depth, int negaMax)
        {
            int j = 0;
            string pieceString;
            int boardPosition = 0;
            int negaMaxCompare;
            QuartoSearchTree.winningMove winChoice = new QuartoSearchTree.winningMove();
            QuartoSearchTree.winningMove winChoice2 = new QuartoSearchTree.winningMove();
            winChoice.winningNode = new QuartoSearchTree.Node();

            // If only root in tree
            if (currentNode.parent == null && currentNode.children[0] == null)
            {
                Console.WriteLine("Search Failed: Tree only contains root");
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
                        return winChoice;
                    }
                    boardPosition++;
                }
            }

            // If it is the bottom of the tree
            if (currentNode.children[0] == null)
            {
                for (int i = 0; i < QuartoSearchTree.MAXGAMEBOARD && currentNode.parent.children[i] != null; i++)
                {
                    if (currentNode.parent.children[i].pieceToPlay == QuartoSearchTree.NULLPIECE)
                        pieceString = null;
                    else
                        pieceString = currentNode.parent.children[i].pieces[currentNode.parent.children[i].pieceToPlay].piece;

                    negaMaxCompare = Heuristic.calculateHeuristic(currentNode.parent.children[i].gameBoard, pieceString);

                    if (i == 0)
                    {
                        negaMax = Heuristic.calculateHeuristic(currentNode.parent.children[i].gameBoard, pieceString);
                        winChoice.winningNode = currentNode.parent.children[i];
                        winChoice.heuristicValue = negaMax;
                    }
                    else if (-negaMaxCompare > -negaMax)
                    {
                        negaMax = -negaMaxCompare;
                        winChoice.winningNode = currentNode.parent.children[i];
                        winChoice.heuristicValue = negaMax;
                    }
                }
            }

            // Iterates through all children nodes
            else
            {
                while (j < QuartoSearchTree.MAXGAMEBOARD && currentNode.children[j] != null)
                {
                    if (j == 0)
                    {
                        winChoice = searchForBestPlay(currentNode.children[j], depthCounter++, depth, negaMax);

                        if (currentNode.parent == null || currentNode.parent.parent == null) { }

                        // When it is evaluating the next move from the root
                        else if (currentNode.parent.parent.parent == null)
                        {
                            winChoice.winningNode = currentNode;
                        }
                    }
                    else
                    {
                        winChoice2 = searchForBestPlay(currentNode.children[j], depthCounter++, depth, negaMax);
                        if (-winChoice2.heuristicValue > -winChoice.heuristicValue)
                        {
                            winChoice.heuristicValue = -winChoice2.heuristicValue;

                            if (currentNode.parent == null)
                            {
                                winChoice.winningNode = winChoice2.winningNode;
                            }
                            else if (currentNode.parent.parent == null)
                            {
                                winChoice.heuristicValue = -winChoice2.heuristicValue;
                                winChoice.winningNode = winChoice2.winningNode;
                            }

                            // When it is evaluating the next move from the root
                            else if (currentNode.parent.parent.parent == null)
                            {
                                winChoice.winningNode = currentNode;
                                winChoice.heuristicValue = -winChoice2.heuristicValue;
                            }
                        }
                    }
                    j++;
                }
            }
            return winChoice;
        }
    }
}

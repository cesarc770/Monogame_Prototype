using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rapid_Prototype_1.Tools
{
    class BoardPieceClicker
    {
        private bool validState;

        private List<Texture2D> texturesInGameBoard;
        private List<Vector2> posOfGameBoardPieces;
        private List<float> scalesOfGameBoardPieces;

        /// <summary>
        /// Constructor that takes only a list of Texture2Ds comprising the game board and their positions.
        /// </summary>
        /// <param name="gameBoardTextures">A list of all Texture2Ds contained in the Game Board.</param>
        /// <param name="gameBoardPositions">A list of all global locations of the textures contained in the Game Board.</param>
        public BoardPieceClicker(List<Texture2D> gameBoardTextures, List<Vector2> gameBoardPositions)
        {
            if (gameBoardTextures.Count != gameBoardPositions.Count)
            {
                Console.WriteLine("Error! gameBoardTextures and gameBoardPositions must be the same size!");
                validState = false;
            }
            texturesInGameBoard = gameBoardTextures;
            posOfGameBoardPieces = gameBoardPositions;
            scalesOfGameBoardPieces = new List<float>(gameBoardTextures.Count);
            for (int i = 0; i < gameBoardTextures.Count; i++)
            {
                scalesOfGameBoardPieces[i] = 1;
            }
            validState = true;
        }


        /// <summary>
        /// Constructor that takes a list of Texture2Ds comprising the game board, their positions, 
        /// and a single float indicating the scale factor for every piece.
        /// </summary>
        /// <param name="gameBoardTextures">A list of all Texture2Ds contained in the Game Board.</param>
        /// <param name="gameBoardPositions">A list of all global locations of the textures contained in the Game Board.</param>
        /// <param name="gameBoardPieceScale">The scale used for all pieces comprising the Game Board.</param>
        public BoardPieceClicker(List<Texture2D> gameBoardTextures, List<Vector2> gameBoardPositions, float gameBoardPieceScale)
        {
            if (gameBoardTextures.Count != gameBoardPositions.Count)
            {
                Console.WriteLine("Error! gameBoardTextures and gameBoardPositions must be the same size!");
                validState = false;
            }
            texturesInGameBoard = gameBoardTextures;
            posOfGameBoardPieces = gameBoardPositions;
            scalesOfGameBoardPieces = new List<float>(gameBoardTextures.Count);
            for (int i = 0; i < gameBoardTextures.Count; i++)
            {
                scalesOfGameBoardPieces[i] = gameBoardPieceScale;
            }
            validState = true;
        }

        /// <summary>
        /// Constructor that takes a list of Texture2Ds comprising the game board, their positions, 
        /// and a list of floats indicating the scale factor for each piece.
        /// </summary>
        /// <param name="gameBoardTextures">A list of all Texture2Ds contained in the Game Board.</param>
        /// <param name="gameBoardPositions">A list of all global locations of the textures contained in the Game Board.</param>
        /// <param name="gameBoardPieceScales">A list of the scale for each pieces comprising the Game Board.</param>
        public BoardPieceClicker(List<Texture2D> gameBoardTextures, List<Vector2> gameBoardPositions, List<float> gameBoardPieceScales)
        {
            if (gameBoardTextures.Count != gameBoardPositions.Count ||
               gameBoardTextures.Count != gameBoardPieceScales.Count)
            {
                Console.WriteLine("Error! gameBoardTextures, gameBoardPositions, and gameBoardPieceScales must be the same size!");
                validState = false;
            }
            texturesInGameBoard = gameBoardTextures;
            posOfGameBoardPieces = gameBoardPositions;
            scalesOfGameBoardPieces = gameBoardPieceScales;
            validState = true;
        }

        /// <summary>
        /// Finds the first index of a piece clicked in the Game Board.
        /// </summary>
        /// <param name="mouseGlobalX">The global X position of the mouse.</param>
        /// <param name="mouseGlobalY">The global Y position of the mouse.</param>
        /// <returns>The first index of a piece that was clicked,
        /// or -1 if nothing was under the mouse click.
        /// NOTE: This will almost always return the index of the ONLY piece that was clicked.
        ///       However, if pieces overlap such that a non-transparent pixel is shared between pieces, this will return the first of them.</returns>
        public int GetIndexOfFirstPieceClicked(int mouseGlobalX, int mouseGlobalY)
        {
            int indexOfClickedPiece = -1;

            if (validState == false)
            {
                Console.WriteLine("This BoardPieceClicker is in an invalid state. Please generate a new BoardPieceClicker.");
                return indexOfClickedPiece;
            }

            List<int> indexesOfAllPiecesClicked = GetIndexesOfPiecesClicked(mouseGlobalX, mouseGlobalY);

            if (indexesOfAllPiecesClicked.Count > 0)
            {
                indexOfClickedPiece = indexesOfAllPiecesClicked[0];
            }

            return indexOfClickedPiece;

        }
        /// <summary>
        /// Finds a list of all indexes of piece(s) clicked in the Game Board.
        /// </summary>
        /// <param name="mouseGlobalX">The global X position of the mouse.</param>
        /// <param name="mouseGlobalY">The global Y position of the mouse.</param>
        /// <returns>A list of the indexes of each board piece clicked (on a non-alpha pixel).
        /// NOTE: This will be either a list of 0 elements if nothing was clicked on,
        ///       a list of length 1 if one board piece was clicked on,
        ///       or a longer list if the images overlap such that a non-transparent pixel is shared between pieces.</returns>
        public List<int> GetIndexesOfPiecesClicked(int mouseGlobalX, int mouseGlobalY)
        {
            List<int> indexesClicked = new List<int>();

            if (validState == false)
            {
                Console.WriteLine("This BoardPieceClicker is in an invalid state. Please generate a new BoardPieceClicker.");
                return indexesClicked;
            }

            for (int i = 0; i < texturesInGameBoard.Count; i++)
            {
                if (ClickedOnNonAlphaInTexture(texturesInGameBoard[i], posOfGameBoardPieces[i], scalesOfGameBoardPieces[i], mouseGlobalX, mouseGlobalY))
                {
                    indexesClicked.Add(i);
                }
            }

            return indexesClicked;
        }

        /// <summary>
        /// Finds a list of all indexes of nearby piece(s) in the Game Board.
        /// </summary>
        /// <param name="mouseGlobalX">The global X position of the mouse.</param>
        /// <param name="mouseGlobalY">The global Y position of the mouse.</param>
        /// <param name="centerToCenterDistanceThreshold">The maximum distance between the mouse and the center of the shape.</param>
        /// <returns>A list of the indexes of each board piece clicked (close enough to the center).
        /// NOTE: This will be either a list of 0 elements if nothing was clicked on,
        ///       a list of length 1 if one board piece was clicked on,
        ///       or a longer list if the images are close enough together.</returns>
        public List<int> GetIndexesOfPiecesClicked(int mouseGlobalX, int mouseGlobalY, float centerToCenterDistanceThreshold)
        {
            List<int> indexesClicked = new List<int>();

            if (validState == false)
            {
                Console.WriteLine("This BoardPieceClicker is in an invalid state. Please generate a new BoardPieceClicker.");
                return indexesClicked;
            }

            for (int i = 0; i < texturesInGameBoard.Count; i++)
            {
                if (ClickedNearCenterOfTexture(texturesInGameBoard[i], posOfGameBoardPieces[i], scalesOfGameBoardPieces[i], mouseGlobalX, mouseGlobalY, centerToCenterDistanceThreshold))
                {
                    indexesClicked.Add(i);
                }
            }

            return indexesClicked;
        }


        /// <summary>
        /// Gets the pixel coordinates of the click in the texture's local space and scale.
        /// </summary>
        /// <param name="tex">The Texture2D to check.</param>
        /// <param name="texPos">The global position of the texture to check.</param>
        /// <param name="texScale">The scale of the texture to check.</param>
        /// <param name="mouseGlobalX">The global X position of the mouse.</param>
        /// <param name="mouseGlobalY">The global Y position of the mouse.</param>
        /// <returns>A Vector2 indicating which pixel was clicked in the texture's local space and scale, 
        /// or (-1,-1) if the pixel was outside of the texture.</returns>
        private Vector2 GetLocalPixelInTextureClicked(Texture2D tex, Vector2 texPos, float texScale, int mouseGlobalX, int mouseGlobalY)
        {
            Vector2 localPosClicked = new Vector2(-1, -1);

            if (mouseGlobalX >= texPos.X &&
                mouseGlobalX <= texPos.X + tex.Width * texScale &&
                mouseGlobalY >= texPos.Y &&
                mouseGlobalY <= texPos.Y + tex.Height * texScale)
            {
                localPosClicked = new Vector2((int)((mouseGlobalX - texPos.X) / texScale), (int)((mouseGlobalY - texPos.Y) / texScale));

            }

            return localPosClicked;
        }

        /// <summary>
        /// Gets the color of the pixel at the given position in the texture's local space and scale.
        /// </summary>
        /// <param name="tex">The Texture2D to check.</param>
        /// <param name="localPosClicked">The position of the pixel to check in the texture's local space and scale.</param>
        /// <returns>The color of that pixel in the given texture, or
        /// (0,0,0,0) if the localPosClicked was outside of the texture.</returns>
        private Color GetColorOfClickedPixelInTexture(Texture2D tex, Vector2 localPosClicked)
        {
            Color clickedColor = new Color(0, 0, 0, 0);

            if (localPosClicked.X < 0 || localPosClicked.Y < 0 ||
                localPosClicked.X > tex.Width || localPosClicked.Y > tex.Height)
            {
                return clickedColor;
            }
            else
            {
                Color[] colorsInTexture = new Color[tex.Width * tex.Height];

                tex.GetData(colorsInTexture);

                clickedColor = colorsInTexture[(int)(localPosClicked.Y * tex.Width + localPosClicked.X)];
            }

            return clickedColor;
        }

        /// <summary>
        /// Checks to see if the pixel clicked on the texture is a non-alpha pixel.
        /// </summary>
        /// <param name="tex">The texture to check.</param>
        /// <param name="texPos">The global position of the texture to check.</param>
        /// <param name="texScale">The scale of the texture to check.</param>
        /// <param name="mouseGlobalX">The global X position of the mouse.</param>
        /// <param name="mouseGlobalY">The global Y position of the mouse.</param>
        /// <returns>True if the pixel was inside the image and was non-transparent,
        /// or false if the pixel was outside of the image or was completely transparent.</returns>
        private bool ClickedOnNonAlphaInTexture(Texture2D tex, Vector2 texPos, float texScale, int mouseGlobalX, int mouseGlobalY)
        {
            bool clickedOnNonAlphaInTexture = false;

            Vector2 localPosOfClick = GetLocalPixelInTextureClicked(tex, texPos, texScale, mouseGlobalX, mouseGlobalY);

            if (GetColorOfClickedPixelInTexture(tex, localPosOfClick).A != 0)
            {
                clickedOnNonAlphaInTexture = true;
            }

            return clickedOnNonAlphaInTexture;
        }

        /// <summary>
        /// Checks to see if the mouse click was close enough to the center of the texture.
        /// </summary>
        /// <param name="tex">The texture to check.</param>
        /// <param name="texPos">The global position of the texture to check.</param>
        /// <param name="texScale">The scale of the texture to check.</param>
        /// <param name="mouseGlobalX">The global X position of the mouse.</param>
        /// <param name="mouseGlobalY">The global Y position of the mouse.</param>
        /// <param name="centerToCenterDistanceThreshold">The maximum distance between the mouse and the center of the shape.</param>
        /// <returns>True if the pixel was close enough to the center of the texture,
        /// or false if the pixel was too far from the center.</returns>
        private bool ClickedNearCenterOfTexture(Texture2D tex, Vector2 texPos, float texScale, int mouseGlobalX, int mouseGlobalY, float centerToCenterDistanceThreshold)
        {
            bool clickedNearCenterOfTexture = false;

            Vector2 centerInGlobal = new Vector2(texPos.X + tex.Width * texScale / 2.0f, texPos.Y + tex.Height * texScale / 2.0f);

            if(Vector2.Distance(new Vector2(mouseGlobalX, mouseGlobalY), centerInGlobal) <= centerToCenterDistanceThreshold)
            {
                clickedNearCenterOfTexture = true;
            }

            return clickedNearCenterOfTexture;
        }
    }
}

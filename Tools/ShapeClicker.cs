using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rapid_Prototype_1.Tools
{
    static class ShapeClicker
    {
        /// <summary>
        /// Finds the first index of a shape clicked in the Game Board.
        /// </summary>
        /// <param name="listOfFallingShapes">The list of all currently falling shapes.</param>
        /// <param name="mouseGlobalX">The global X position of the mouse.</param>
        /// <param name="mouseGlobalY">The global Y position of the mouse.</param>
        /// <returns>The first index in listOfFallingShapes of a shape that was clicked,
        /// or -1 if nothing was under the mouse click.
        /// NOTE: This will almost always return the index of the ONLY shape that was clicked.
        ///       However, if shapes overlap such that a non-transparent pixel is shared between shapes, this will return the first of them.</returns>
        public static int GetIndexOfFirstShapeClicked(List<Shape> listOfFallingShapes, int mouseGlobalX, int mouseGlobalY)
        {
            int indexOfClickedPiece = -1;

            List<int> indexesOfAllPiecesClicked = GetIndexesOfShapesClicked(listOfFallingShapes, mouseGlobalX, mouseGlobalY);

            if (indexesOfAllPiecesClicked.Count > 0)
            {
                indexOfClickedPiece = indexesOfAllPiecesClicked[0];
            }

            return indexOfClickedPiece;

        }
        /// <summary>
        /// Finds a list of all indexes of shape(s) clicked.
        /// </summary>
        /// <param name="listOfFallingShapes">The list of all currently falling shapes.</param>
        /// <param name="mouseGlobalX">The global X position of the mouse.</param>
        /// <param name="mouseGlobalY">The global Y position of the mouse.</param>
        /// <returns>A list of the indexes in listOfFallingShapes of each shape clicked (on a non-alpha pixel).
        /// NOTE: This will be either a list of 0 elements if nothing was clicked on,
        ///       a list of length 1 if one shape was clicked on,
        ///       or a longer list if the images overlap such that a non-transparent pixel is shared between shapes.</returns>
        public static List<int> GetIndexesOfShapesClicked(List<Shape> listOfFallingShapes, int mouseGlobalX, int mouseGlobalY)
        {
            List<int> indexesClicked = new List<int>();

            for (int i = 0; i < listOfFallingShapes.Count; i++)
            {
                if (ClickedOnNonAlphaInShape(listOfFallingShapes[i], mouseGlobalX, mouseGlobalY))
                {
                    indexesClicked.Add(i);
                }
            }

            return indexesClicked;
        }

        /// <summary>
        /// Gets the pixel coordinates of the click in the shape's local space and scale.
        /// </summary>
        /// <param name="shape">The Shape to check.</param>
        /// <param name="mouseGlobalX">The global X position of the mouse.</param>
        /// <param name="mouseGlobalY">The global Y position of the mouse.</param>
        /// <returns>A Vector2 indicating which pixel was clicked in the shape's local space and scale, 
        /// or (-1,-1) if the pixel was outside of the texture.</returns>
        private static Vector2 GetLocalPixelInShapeClicked(Shape shape, int mouseGlobalX, int mouseGlobalY)
        {
            Vector2 localPosClicked = new Vector2(-1, -1);

            Vector2 shapePos = shape.GetPosition();
            Texture2D shapeSprite = shape.GetSprite();
            Vector2 shapeScale = shape.GetScale();

            if (mouseGlobalX >= shapePos.X &&
                mouseGlobalX <= shapePos.X + shapeSprite.Width * shapeScale.X &&
                mouseGlobalY >= shapePos.Y &&
                mouseGlobalY <= shapePos.Y + shapeSprite.Height * shapeScale.Y)
            {
                localPosClicked = new Vector2((int)((mouseGlobalX - shapePos.X) / shapeScale.X), (int)((mouseGlobalY - shapePos.Y) / shapeScale.Y));

            }

            return localPosClicked;
        }

        /// <summary>
        /// Gets the color of the pixel at the given position in the texture's local space and scale.
        /// </summary>
        /// <param name="shape">The shape to check.</param>
        /// <param name="localPosClicked">The position of the pixel to check in the texture's local space and scale.</param>
        /// <returns>The color of that pixel in the given shape, or
        /// (0,0,0,0) if the localPosClicked was outside of the shape.</returns>
        private static Color GetColorOfClickedPixelInShape(Shape shape, Vector2 localPosClicked)
        {
            Color clickedColor = new Color(0, 0, 0, 0);

            Texture2D sprite = shape.GetSprite();

            if (localPosClicked.X < 0 || localPosClicked.Y < 0 ||
                localPosClicked.X > sprite.Width || localPosClicked.Y > sprite.Height)
            {
                return clickedColor;
            }
            else
            {
                Color[] colorsInTexture = new Color[sprite.Width * sprite.Height];

                sprite.GetData(colorsInTexture);

                clickedColor = colorsInTexture[(int)(localPosClicked.Y * sprite.Width + localPosClicked.X)];
            }

            return clickedColor;
        }

        /// <summary>
        /// Checks to see if the pixel clicked on the texture is a non-alpha pixel.
        /// </summary>
        /// <param name="shape">The shape to check.</param>
        /// <param name="mouseGlobalX">The global X position of the mouse.</param>
        /// <param name="mouseGlobalY">The global Y position of the mouse.</param>
        /// <returns>True if the pixel was inside the image and was non-transparent,
        /// or false if the pixel was outside of the image or was completely transparent.</returns>
        private static bool ClickedOnNonAlphaInShape(Shape shape, int mouseGlobalX, int mouseGlobalY)
        {
            bool clickedOnNonAlphaInTexture = false;

            Vector2 localPosOfClick = GetLocalPixelInShapeClicked(shape, mouseGlobalX, mouseGlobalY);

            if (GetColorOfClickedPixelInShape(shape, localPosOfClick).A != 0)
            {
                clickedOnNonAlphaInTexture = true;
            }

            return clickedOnNonAlphaInTexture;
        }
    }
}

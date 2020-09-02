using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rapid_Prototype_1.Tools
{
    class GameBoard
    {
        public enum BoardName
        {
            Unicorn
        }

        private List<Texture2D> satTextures;
        private List<bool> texturesSaturated;
        private List<Texture2D> unsatTextures;
        private List<Vector2> positions;
        private List<Vector2> rawPositions;
        private List<float> scales;
        private List<string> namePrefixes;

        private Vector2 textureOffset;

        private BoardPieceClicker pieceClickChecker;

        private BoardName boardName;

        /// <summary>
        /// The constructor takes a BoardName and initializes all the lists to prepare for filling the board.
        /// </summary>
        /// <param name="name">The BoardName identifying which board to display.</param>
        public GameBoard(BoardName name)
        {
            boardName = name;
            satTextures = new List<Texture2D>();
            texturesSaturated = new List<bool>();
            unsatTextures = new List<Texture2D>();
            positions = new List<Vector2>();
            rawPositions = new List<Vector2>();
            scales = new List<float>();
            namePrefixes = new List<string>();

            textureOffset = new Vector2(900, 100);
        }

        /// <summary>
        /// Initializes the Game Board based on the BoardName supplied to the constructor.
        /// </summary>
        public void Initialize()
        {
            if (boardName == BoardName.Unicorn)
            {
                namePrefixes.Add("Unicorn_back_left_leg_");
                namePrefixes.Add("Unicorn_back_neck_");
                namePrefixes.Add("Unicorn_back_right_leg_");
                namePrefixes.Add("Unicorn_body_");
                namePrefixes.Add("Unicorn_chest_");
                namePrefixes.Add("Unicorn_front_legs_");
                namePrefixes.Add("Unicorn_front_neck_");
                namePrefixes.Add("Unicorn_horn_");
                namePrefixes.Add("Unicorn_nose_");
                namePrefixes.Add("Unicorn_tail_");

                for (int i = 0; i < namePrefixes.Count; i++)
                {
                    scales.Add(1.75f);
                }

                rawPositions.Add(new Vector2(75, 374));//
                rawPositions.Add(new Vector2(103, 135));//
                rawPositions.Add(new Vector2(56, 327));//
                rawPositions.Add(new Vector2(52, 223));//
                rawPositions.Add(new Vector2(122, 193));//
                rawPositions.Add(new Vector2(251, 185));//
                rawPositions.Add(new Vector2(162, 100));//
                rawPositions.Add(new Vector2(111, 0));//
                rawPositions.Add(new Vector2(197, 65));//
                rawPositions.Add(new Vector2(0, 327));//

                positions.Add(rawPositions[0] * scales[0] + textureOffset);
                positions.Add(rawPositions[1] * scales[1] + textureOffset);
                positions.Add(rawPositions[2] * scales[2] + textureOffset);
                positions.Add(rawPositions[3] * scales[3] + textureOffset);
                positions.Add(rawPositions[4] * scales[4] + textureOffset);
                positions.Add(rawPositions[5] * scales[5] + textureOffset);
                positions.Add(rawPositions[6] * scales[6] + textureOffset);
                positions.Add(rawPositions[7] * scales[7] + textureOffset);
                positions.Add(rawPositions[8] * scales[8] + textureOffset);
                positions.Add(rawPositions[9] * scales[9] + textureOffset);
            }
        }

        /// <summary>
        /// To be called from Game1's LoadContent function.
        /// </summary>
        /// <param name="Content">The Content variable from Game1's LoadContent function.</param>
        public void LoadContent(ContentManager Content)
        {
            if (boardName == BoardName.Unicorn)
            {
                foreach (string namePrefix in namePrefixes)
                {
                    Texture2D unsatTex = Content.Load<Texture2D>(namePrefix + "unsat");
                    unsatTex.Name = namePrefix + "unsat";
                    unsatTextures.Add(unsatTex);

                    texturesSaturated.Add(false);

                    Texture2D satTex = Content.Load<Texture2D>(namePrefix + "sat");
                    satTex.Name = namePrefix + "sat";
                    satTextures.Add(satTex);
                }

                pieceClickChecker = new BoardPieceClicker(unsatTextures, positions, scales);
            }
        }

        /// <summary>
        /// To be called from Game1's Draw function.
        /// </summary>
        /// <param name="spriteBatch">Game1's spriteBatch variable.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < unsatTextures.Count; i++)
            {
                if (texturesSaturated[i])
                {
                    spriteBatch.Draw(satTextures[i], positions[i], null, Color.White, 0, Vector2.Zero, scales[i], SpriteEffects.None, 0f);
                }
                else
                {
                    spriteBatch.Draw(unsatTextures[i], positions[i], null, Color.White, 0, Vector2.Zero, scales[i], SpriteEffects.None, 0f);
                }
            }
        }

        /// <summary>
        /// Saturates the piece clicked if its name prefix starts the given name passed in.
        /// </summary>
        /// <param name="globalMouseX">The global X position of the mouse.</param>
        /// <param name="globalMouseY">The global Y position of the mouse.</param>
        /// <param name="name">The asset name used to initialize the piece inside the beat bar.</param>
        /// <returns>True if a piece was placed, false if no piece was placed</returns>
        public bool SaturateIfNamePrefixMatch(int globalMouseX, int globalMouseY, string name)
        {
            bool pieceWasPlaced = false;

            // There shouldn't be any overlapping pieces, but let's account for that anyway
            List<int> indexes = pieceClickChecker.GetIndexesOfPiecesClicked(globalMouseX, globalMouseY);

            foreach (int index in indexes)
            {
                if (name.StartsWith(namePrefixes[index]))
                {
                    texturesSaturated[index] = true;
                    pieceWasPlaced = true;
                }
            }

            return pieceWasPlaced;
        }

    }
}

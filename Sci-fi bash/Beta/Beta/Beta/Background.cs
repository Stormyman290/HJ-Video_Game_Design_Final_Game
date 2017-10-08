using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Text;

namespace Beta
{
    class Background
    {
        private BackgroundTile[][] backgroundTiles;
        int curSeasonTilesStartAt, numTilesCurSeason;
        private Random rnd;
        Texture2D tileTex;
        SpriteBatch batch;
        Season currentSeason;

        public Background(Season currentSeason, int gridX, int gridY, Texture2D tileTex, SpriteBatch batch)
        {
            rnd = new Random();
            this.currentSeason = currentSeason;
            curSeasonTilesStartAt = currentSeason.getTilesStartPoint();
            numTilesCurSeason = currentSeason.getNumberOfTiles();
            this.batch = batch;

            this.tileTex = tileTex;
            backgroundTiles = new BackgroundTile[gridX][];
            addSubArraysToBackgroundTilesArry(gridY);
            switch (currentSeason.getName())
            {
                case "Spring":
                    setTilesSpring(tileTex);
                    break;

                case "Summer":
                    setTilesSummer(tileTex);
                    break;

                case "Fall":
                    setTilesFallOrWinter(tileTex);
                    break;

                case "Winter":
                    setTilesFallOrWinter(tileTex);
                    break;
            }
        }

        private void addSubArraysToBackgroundTilesArry(int gridY)
        {
            for (int arrayNumber = 0; arrayNumber < gridY; arrayNumber++)
            {
                backgroundTiles[arrayNumber] = new BackgroundTile[gridY];
            }
        }

        private void updateBackground(Season newSeason)
        {
            for (int i = 0; i < backgroundTiles.Length; i++)
            {
                for (int j = 0; j < backgroundTiles[i].Length; j++)
                {
                    backgroundTiles[i][j] = null;
                }
            }

            curSeasonTilesStartAt = newSeason.getTilesStartPoint();
            numTilesCurSeason = newSeason.getNumberOfTiles();
            switch (newSeason.getName())
            {
                case "Spring":
                    setTilesSpring(tileTex);
                    break;

                case "Summer":
                    setTilesSummer(tileTex);
                    break;

                case "Fall":
                    setTilesFallOrWinter(tileTex);
                    break;

                case "Winter":
                    setTilesFallOrWinter(tileTex);
                    break;
            }
        }

        private void setTilesSummer(Texture2D tileTex)
        {
            for (int i = 0; i < backgroundTiles.Length; i += 2)
            {
                for (int j = 0; j < backgroundTiles[i].Length; j += 2)
                {
                    Texture2D setTexture = tileTex;
                    Vector2 setVector = new Vector2(i, j);
                    setVector *= 64;
                    Rectangle setSource = new Rectangle((curSeasonTilesStartAt + (rnd.Next(0, 2) * 32) + 32), 0, 32, 32);
                    Rectangle setDestination;
                    if (i == backgroundTiles.Length - 1 && j == 0)
                    {
                        setSource.X = curSeasonTilesStartAt;
                        setDestination = new Rectangle(((backgroundTiles.Length - 2) * 32) - 1, 0, 64, 64);
                    }
                    else if (i != backgroundTiles.Length - 1 || j != 0)
                    {
                        if (setSource.X == curSeasonTilesStartAt)
                        {
                            setSource.X += 32;
                        }

                        setDestination = new Rectangle((i * 32), (j * 32), 64, 64);
                    }
                    else
                    {
                        setDestination = new Rectangle(0192643501, 198027465, 1345, 62435);
                    }

                    backgroundTiles[i][j] = new BackgroundTile(setTexture, setVector, batch, setSource, setDestination);

                }
            }
        }

        private void setTilesSpring(Texture2D tileTex)
        {
            for (int i = 0; i < backgroundTiles.Length; i++)
            {
                for (int j = 0; j < backgroundTiles[i].Length; j++)
                {
                    Texture2D setTexture = tileTex;
                    Vector2 setVector = new Vector2(i, j);
                    setVector *= 32;
                    Rectangle setSource;

                    if (i < 5)
                    {
                        setSource = new Rectangle((curSeasonTilesStartAt + (5 * 32)), 0, 32, 32);
                    }

                    if (i == 5)
                    {
                        setSource = new Rectangle((curSeasonTilesStartAt + (5 * 32)), 0, 32, 32);
                    }

                    if (i > 5)
                    {
                        setSource = new Rectangle((curSeasonTilesStartAt + (6 * 32)), 0, 32, 32);
                    }

                    else
                    {
                        setSource = new Rectangle(0192643501, 198027465, 1345, 62435);
                    }
                    Rectangle setDestination = new Rectangle((i * 32), (j * 32), 32, 32);

                    backgroundTiles[i][j] = new BackgroundTile(setTexture, setVector, batch, setSource, setDestination);
                }
            }
        }

        public void setTilesFallOrWinter(Texture2D tileTex)
        {
            for (int i = 0; i < backgroundTiles.Length; i++)
            {
                for (int j = 0; j < backgroundTiles[i].Length; j++)
                {
                    Texture2D setTexture = tileTex;
                    Vector2 setVector = new Vector2(i, j);
                    setVector *= 32;
                    Rectangle setSource = new Rectangle((curSeasonTilesStartAt + (rnd.Next(0, 3) * 32)), 0, 32, 32);
                    Rectangle setDestination = new Rectangle((i * 32), (j * 32), 32, 32);

                    backgroundTiles[i][j] = new BackgroundTile(setTexture, setVector, batch, setSource, setDestination);
                }
            }
        }

        public void draw()
        {
            switch (currentSeason.getName())
            {
                case "Spring":
                    drawNotSummer(tileTex);
                    break;

                case "Summer":
                    drawSummer(tileTex);
                    break;

                case "Fall":
                    drawNotSummer(tileTex);
                    break;

                case "Winter":
                    drawNotSummer(tileTex);
                    break;
            }

        }

        private void drawSummer(Texture2D tileTex)
        {
            for (int i = 0; i < backgroundTiles.Length; i += 2)
            {
                for (int j = 0; j < backgroundTiles[i].Length; j += 2)
                {
                    backgroundTiles[i][j].Draw();
                }
            }
        }

        private void drawNotSummer(Texture2D tileTex)
        {
            foreach (BackgroundTile[] array in backgroundTiles)
            {
                foreach (BackgroundTile tile in array)
                {
                    tile.Draw();
                }
            }
        }

        public Season getCurrentSeason()
        {
            return currentSeason;
        }

        public void setCurrentSeason(Season newSeason)
        {
            currentSeason = newSeason;
            updateBackground(newSeason);
        }

        public Vector2 getBackgroundXLengthAndYLength()
        {
            return new Vector2((float)backgroundTiles.Length, (float)backgroundTiles[0].Length);
        }
    }
}


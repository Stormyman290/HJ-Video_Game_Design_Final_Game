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

namespace Beta
{
    class Stage
    {
        public GameCharacter[] players;
        public Tile[][] currentBoard;
        private SpriteBatch spriteBatch { get; set; }
        private int changeAfter, hazardAfter, hazardEndsAfter;
        private Vector2 currentHazardLocation;
        private Random rnd = new Random();
        private Season[] seasons = new Season[4];
        private Season currentSeason;
        private Background currentBackground;
        private Dictionary<String, Rectangle> platformSources;
        private Texture2D platformTexture;
        private Hazard currentHazard;
        private float hazardHasRotated, hazardRotationVelocity;

        public static Stage Level { get; private set; }
        public int charactersDead = 0;

        public Stage(GameCharacter[] players, Background stageBackground, Dictionary<String, Rectangle> platformSources, Season[] seasons, Texture2D platformTexture, SpriteBatch spriteBatch)
        {
            this.players = players;
            this.seasons = seasons;
            this.platformSources = platformSources;
            this.platformTexture = platformTexture;

            currentSeason = stageBackground.getCurrentSeason();
            changeAfter = rnd.Next(1800, 5400);
            currentBackground = stageBackground;
            Vector2 newBoardSpecifications = currentBackground.getBackgroundXLengthAndYLength();
            currentBoard = new Tile[(int)newBoardSpecifications.X][];
            establishBoardTo((int)newBoardSpecifications.Y);

            createPlatforms();
            Stage.Level = this;
        }

        private void establishBoardTo(int y)
        {
            for (int i = 0; i < currentBoard.Length; i++)
            {
                currentBoard[i] = new Tile[y];
            }
        }

        private void createPlatforms()
        {
            for (int x = 0; x < currentBoard.Length; x++)
            {
                for (int y = 0; y < currentBoard[x].Length; y++)
                {
                    Vector2 tilePosition = new Vector2(x * 32, y * 32);
                    if (y <= 16)
                    {
                        currentBoard[x][y] = new Tile(platformTexture, tilePosition, spriteBatch, platformSources[currentSeason.getName()], rnd.Next(7) == 0);
                    }

                    if (y >= 16 && y <= 18)
                    {
                        currentBoard[x][y] = new Tile(platformTexture, tilePosition, spriteBatch, platformSources[currentSeason.getName()], rnd.Next(4) == 0);
                    }

                    if (y == 22 || y == 21)
                    {
                        currentBoard[x][y] = new Tile(platformTexture, tilePosition, spriteBatch, platformSources[currentSeason.getName()], rnd.Next(1) == 0);
                    }

                    if (y == 0 || y == currentBoard[x].Length - 1 || y == currentBoard[x].Length - 2 || x == 0 || x == currentBoard.Length - 1)
                    {
                        currentBoard[x][y] = new Tile(platformTexture, tilePosition, spriteBatch, platformSources[currentSeason.getName()], true);
                    }
                }
            }
        }

        public void update(GameTime gameTime)
        {
            currentSeason = currentBackground.getCurrentSeason();
            currentHazard = currentSeason.getCurrentHazard();

            if (changeAfter == 0)
            {
                currentBackground.setCurrentSeason(seasons[rnd.Next(0, seasons.Length)]);
                currentSeason = currentBackground.getCurrentSeason();
                changeAfter = rnd.Next(1800, 5400);
                createPlatforms();
            }

            if (hazardAfter == 0)
            {
                currentHazard = currentSeason.getCurrentHazard();
                currentHazard.happen();
                hazardEndsAfter = currentHazard.getHazardDuration();
                hazardRotationVelocity = ((10 * (MathHelper.Pi)) / hazardEndsAfter);
                hazardAfter = hazardEndsAfter + 1;
            }

            if (hazardEndsAfter == 0)
            {
                hazardAfter = rnd.Next(300, 1800);
                currentHazard = null;
            }

            foreach (GameCharacter player in players)
            {
                player.Update(gameTime);
            }

            if (hazardEndsAfter > 0)
            {
                hazardEndsAfter--;
                hazardHasRotated += hazardRotationVelocity;
            }

            changeAfter--;
        }

        public void draw(SpriteBatch batch, GameTime gameTime)
        {
            currentBackground.draw();
            
            foreach (Tile[] platforms in currentBoard)
            {
                foreach (Tile platform in platforms)
                {
                    if (platform != null)
                    {
                        platform.SecondDraw(batch);
                    }
                }
            }
            //moved this here so People can see the text ~Chris H
            foreach (GameCharacter player in players)
            {
                player.Draw(gameTime, batch);
            }

            if (currentHazard != null)
            {
                currentHazard.draw(batch, hazardHasRotated);
            }
        }
        ///<Collision>
        ///colllisison stuff
        ///<Collision>
        public bool HasRoomForRectangle(Rectangle rectangleToCheck)
        {
            foreach (Tile[] platforms in currentBoard)
            {
                foreach (Tile platform in platforms)
                {
                    if (platform != null)
                    {
                        if (platform.IsBlocked && platform.Bounds.Intersects(rectangleToCheck))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        public Vector2 WhereCanIGetTo(Vector2 originalPosition, Vector2 destination, Rectangle boundingRectangle)
        {
            MovementWrapper move = new MovementWrapper(originalPosition, destination, boundingRectangle);

            for (int i = 1; i <= move.NumberOfStepsToBreakMovementInto; i++)
            {
                Vector2 positionToTry = originalPosition + move.OneStep * i;
                Rectangle newBoundary = CreateRectangleAtPosition(positionToTry, boundingRectangle.Width, boundingRectangle.Height);
                if (HasRoomForRectangle(newBoundary)) { move.FurthestAvailableLocationSoFar = positionToTry; }
                else
                {
                    if (move.IsDiagonalMove)
                    {
                        move.FurthestAvailableLocationSoFar = CheckPossibleNonDiagonalMovement(move, i);
                    }
                    break;
                }
            }
            return move.FurthestAvailableLocationSoFar;
        }

        private Rectangle CreateRectangleAtPosition(Vector2 positionToTry, int width, int height)
        {
            return new Rectangle((int)positionToTry.X, (int)positionToTry.Y, width, height);
        }

        private Vector2 CheckPossibleNonDiagonalMovement(MovementWrapper wrapper, int i)
        {
            if (wrapper.IsDiagonalMove)
            {
                int stepsLeft = wrapper.NumberOfStepsToBreakMovementInto - (i - 1);

                Vector2 remainingHorizontalMovement = wrapper.OneStep.X * Vector2.UnitX * stepsLeft;
                wrapper.FurthestAvailableLocationSoFar =
                    WhereCanIGetTo(wrapper.FurthestAvailableLocationSoFar, wrapper.FurthestAvailableLocationSoFar + remainingHorizontalMovement, wrapper.BoundingRectangle);

                Vector2 remainingVerticalMovement = wrapper.OneStep.Y * Vector2.UnitY * stepsLeft;
                wrapper.FurthestAvailableLocationSoFar =
                    WhereCanIGetTo(wrapper.FurthestAvailableLocationSoFar, wrapper.FurthestAvailableLocationSoFar + remainingVerticalMovement, wrapper.BoundingRectangle);
            }

            return wrapper.FurthestAvailableLocationSoFar;
        }
    }
}
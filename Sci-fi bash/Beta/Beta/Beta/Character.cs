using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace Beta
{
    enum CharacterState { hit, onGround, jumped, jumpedTwice, speaciled };
    enum CharacterOrientation { up, down, left, right };

    class GameCharacter : Sprite
    {
        //This holds our texture to our player.

        private Texture2D playerSprite;
        private Texture2D projectileSprite;
        private SpriteFont StatusFont;
        //private Texture2D meleeSprite;
        Vector2 playerDimensions;
        Vector2 oldPosition;
        int fighterTag;
        public int health = 100;
        int meleeAttack, rangedAttack;
        bool life, doIRelod = false;
        int shotsFired = 7;
        int timeSpent = 300;
        int fighterNumber;

        Rectangle source;
        int switchCounter = 30;
        int frameCounter = 0;

        Keys upkey, leftkey, rightkey, downkey, attackR, attackM;

        public Vector2 characterPosition { get; set; }

        KeyboardState oldKB;

        CharacterState _characterState;
        CharacterOrientation _characterOrientation;

        //Attacks
        public List<Projectile> _projectiles = new List<Projectile>();
        public List<Projectile> _punches = new List<Projectile>();

        ContentManager _theContenter;

        public new Rectangle Bounds
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, (int)playerDimensions.X, (int)playerDimensions.Y); }
        }

        ///<summary>
        /// Constructs a new player.
        ///</summary>
        public GameCharacter(Texture2D texture, Vector2 position, SpriteBatch spritebatch, int fighterNumber, int meleeDamage, int rangedDamage, ContentManager contenter, Texture2D projectoSprite/*, Texture2D meleoSprite*/)
            : base(texture, position, spritebatch)
        {
            playerSprite = texture;
            projectileSprite = projectoSprite;
            //meleeSprite = meleoSprite;
            life = true;
            fighterTag = fighterNumber;
            meleeAttack = meleeDamage;
            rangedAttack = rangedDamage;
            this.fighterNumber = fighterNumber;

            source = new Rectangle(0, 0, playerSprite.Width, playerSprite.Width);
            playerDimensions.X = source.Width;
            playerDimensions.Y = source.Height;

            _theContenter = contenter;
            LoadContent();

            if (this.fighterNumber == 1)
            {
                upkey = Keys.W;
                leftkey = Keys.A;
                rightkey = Keys.D;
                downkey = Keys.S;
                attackM = Keys.F;
                attackR = Keys.G;
                _characterOrientation = CharacterOrientation.right;
                _characterState = CharacterState.onGround;
                //health = 100;
            }
            else if (this.fighterNumber == 2)
            {
                upkey = Keys.Up;
                leftkey = Keys.Left;
                rightkey = Keys.Right;
                downkey = Keys.Down;
                attackM = Keys.O;
                attackR = Keys.P;
                _characterOrientation = CharacterOrientation.left;
                _characterState = CharacterState.onGround;
                //health = 100;
            }
            else if (this.fighterNumber == 3)
            {
                upkey = Keys.H;
                leftkey = Keys.B;
                rightkey = Keys.M;
                downkey = Keys.N;
                attackM = Keys.RightAlt;
                attackR = Keys.RightControl;
                _characterOrientation = CharacterOrientation.right;
                _characterState = CharacterState.onGround;
                //health = 100;
            }
            else if (this.fighterNumber == 4)
            {
                upkey = Keys.NumPad8;
                leftkey = Keys.NumPad4;
                rightkey = Keys.NumPad6;
                downkey = Keys.NumPad5;
                attackM = Keys.Multiply;
                attackR = Keys.Subtract;
                _characterOrientation = CharacterOrientation.left;
                _characterState = CharacterState.onGround;
                //health = 100;
            }
        }
        public void LoadContent()
        {
            StatusFont = _theContenter.Load<SpriteFont>("SpriteFont1");
            foreach (Projectile projecto in _projectiles)
            {
                projecto.LoadContent(_theContenter);
            }
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (life == true)
            {
                foreach (Projectile projecto in _projectiles)
                {
                    projecto.Draw();
                }
                foreach (Projectile puncho in _punches)
                {
                    puncho.Draw();
                }
                spriteBatch.Draw(playerSprite, Position, source, Color.White);
                Vector2 numPosition = new Vector2(Position.X + 2, Position.Y - 5);
                spriteBatch.DrawString(StatusFont, fighterNumber.ToString(), numPosition, Color.Black);
                spriteBatch.DrawString(StatusFont, "Player: " + fighterTag.ToString() + " Health: " + health.ToString() + " Shots Available: " + shotsFired.ToString() + " Time to Relod: " + timeSpent.ToString() + "ms", new Vector2(32, 32 * (20 + fighterTag)), Color.Goldenrod);
            }
            else if (life == false)
            {
                Position = new Vector2(fighterTag * 32, 1000);
            }
        }
        ///<summary>
        /// Handles input, perform physics, and animates the player sprite.
        ///</summary>
        public override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();
            KeyboardState kb = Keyboard.GetState();
            if (life == true)
            {
                checkMagazine();
                checkApplicableActions(gameTime, kb);
                MoveAsFarAsPossible(gameTime);
                SimulateFriction();
                StopMovingIfBlocked();
                updateSpriteBasedOnDirection();
                updateSpriteBasedOnState();
                foreach (Projectile projecto in _projectiles)
                {
                    projecto.Update(gameTime);
                }
                foreach (Projectile puncho in _punches)
                {
                    puncho.Update(gameTime);
                }
                checkForCollisionWithTheGround();
                checkVitality();
                oldKB = kb;
            }
            else if (life == false)
            {

            }
        }
        //player movement
        public void checkApplicableActions(GameTime gameTime, KeyboardState kb)
        {
            if (_characterState == CharacterState.onGround)
            {
                useOnGroundSet(gameTime, kb);
            }
            else if (_characterState == CharacterState.hit)
            {
                useHitSet(gameTime, kb);
            }
            else if (_characterState == CharacterState.jumped)
            {
                useJumpedSet(gameTime, kb);
            }
            else if (_characterState == CharacterState.jumpedTwice)
            {
                useJumpedTwiceSet(gameTime, kb);
            }
            else if (_characterState == CharacterState.speaciled)
            {
                useSpeaciledSet(gameTime, kb);
            }
        }
        //immobilized till he touches the ground or jumps out of it
        public void useHitSet(GameTime gameTime, KeyboardState kb)
        {
            if (kb.IsKeyDown(upkey) && !oldKB.IsKeyDown(upkey))
            {
                characterPosition = -Vector2.UnitY * 20;
                setCharacterState(CharacterState.jumpedTwice);
            }
        }
        //he is walking on the ground
        public void useOnGroundSet(GameTime gameTime, KeyboardState kb)
        {
            //if (kb.IsKeyUp(attackM) && kb.IsKeyUp(attackR) && oldKB.IsKeyUp(attackM) && oldKB.IsKeyUp(attackR))
            //{
            if (kb.IsKeyDown(rightkey))
            {
                characterPosition += Vector2.UnitX;
                setCharacterOrientation(CharacterOrientation.right);
                setCharacterState(CharacterState.onGround);
            }
            if (kb.IsKeyDown(leftkey))
            {
                characterPosition -= Vector2.UnitX;
                setCharacterOrientation(CharacterOrientation.left);
                setCharacterState(CharacterState.onGround);
            }
            if (kb.IsKeyDown(downkey))
            {
                //bounds.Height = Bounds.Height / 2;
                setCharacterOrientation(CharacterOrientation.down);
            }
            else if (kb.IsKeyUp(downkey))
            {
                //bounds.Height = playerSprite.Height;
            }
            if (kb.IsKeyDown(upkey) && !oldKB.IsKeyDown(upkey))
            {
                characterPosition = -Vector2.UnitY * 20;
                setCharacterOrientation(CharacterOrientation.up);
                setCharacterState(CharacterState.jumpedTwice);
            }
            //}
            if (kb.IsKeyDown(attackR) && !oldKB.IsKeyDown(attackR) && doIRelod == false)
            {
                characterPosition = characterPosition * Vector2.Zero;
                shootBasedOnDirection(gameTime);
                shotsFired--;
            }
            if (kb.IsKeyDown(attackM) && !oldKB.IsKeyDown(attackM))
            {
                characterPosition = characterPosition * Vector2.Zero;
                attackBasedOnDirection(gameTime);
            }
        }
        //he is now in the air with the possibility to jump again or until he is hit or touches the ground
        public void useJumpedSet(GameTime gameTime, KeyboardState kb)
        {
            if (kb.IsKeyDown(rightkey))
            {
                characterPosition += Vector2.UnitX;
                setCharacterOrientation(CharacterOrientation.right);
            }
            if (kb.IsKeyDown(leftkey))
            {
                characterPosition -= Vector2.UnitX;
                setCharacterOrientation(CharacterOrientation.left);
            }
            if (kb.IsKeyDown(downkey))
            {
                characterPosition += new Vector2(0, 1);
                setCharacterOrientation(CharacterOrientation.down);
            }
            if (kb.IsKeyDown(upkey) && !oldKB.IsKeyDown(upkey))
            {
                characterPosition = -Vector2.UnitY * 20;
                setCharacterOrientation(CharacterOrientation.up);
                setCharacterState(CharacterState.jumpedTwice);
            }
            if (kb.IsKeyDown(attackR) && !oldKB.IsKeyDown(attackR) && doIRelod == false)
            {
                characterPosition = characterPosition * new Vector2(0, 1);
                shootBasedOnDirection(gameTime);
                shotsFired--;
            }
            if (kb.IsKeyDown(attackM) && !oldKB.IsKeyDown(attackM))
            {
                characterPosition = characterPosition * new Vector2(0, 1);
                attackBasedOnDirection(gameTime);
            }

        }
        //he is still in the air but cant jump. has same properties as jump
        public void useJumpedTwiceSet(GameTime gameTime, KeyboardState kb)
        {

            if (kb.IsKeyDown(rightkey))
            {
                characterPosition += Vector2.UnitX;
                setCharacterOrientation(CharacterOrientation.right);
            }
            if (kb.IsKeyDown(leftkey))
            {
                characterPosition -= Vector2.UnitX;
                setCharacterOrientation(CharacterOrientation.left);
            }
            if (kb.IsKeyDown(downkey))
            {
                characterPosition += new Vector2(0, 1);
                setCharacterOrientation(CharacterOrientation.down);
            }
            if (kb.IsKeyDown(upkey))
            {
                setCharacterOrientation(CharacterOrientation.up);
            }
            if (kb.IsKeyDown(attackR) && !oldKB.IsKeyDown(attackR) && doIRelod == false)
            {
                characterPosition = characterPosition * new Vector2(0, 1);
                shootBasedOnDirection(gameTime);
                shotsFired--;
            }
            if (kb.IsKeyDown(attackM) && !oldKB.IsKeyDown(attackM))
            {
                characterPosition = characterPosition * new Vector2(0, 1);
                attackBasedOnDirection(gameTime);
            }
        }
        //used an ability, same as hit but cant jump out of it
        public void useSpeaciledSet(GameTime gameTime, KeyboardState kb)
        {
            if (kb.IsKeyDown(rightkey))
            {
                characterPosition += Vector2.UnitX * .01f;
                setCharacterOrientation(CharacterOrientation.right);
            }
            if (kb.IsKeyDown(leftkey))
            {
                characterPosition -= Vector2.UnitX * .01f;
                setCharacterOrientation(CharacterOrientation.left); ;
            }
            if (kb.IsKeyDown(downkey))
            {
                characterPosition += new Vector2(0, 1 / 2);
                setCharacterOrientation(CharacterOrientation.down);
            }
            if (kb.IsKeyDown(upkey))
            {
                setCharacterOrientation(CharacterOrientation.up);
            }
        }
        //enumerations
        public void setCharacterOrientation(CharacterOrientation whereYouLook)
        {
            _characterOrientation = whereYouLook;
        }
        public CharacterOrientation getCharacterOrientation()
        {
            return _characterOrientation;
        }
        public void setCharacterState(CharacterState howYouAre)
        {
            _characterState = howYouAre;
        }
        public CharacterState getCharacterState()
        {
            return _characterState;
        }
        /// <summary>player attacks
        /// 
        /// 
        /// new Vector2(Position.X+playerDimensions.X,Position.Y+(playerDimensions.Y/2))
        /// means (how far from position to fire)
        /// </summary>
        public void shootBasedOnDirection(GameTime gameTime)
        {

            if (_characterOrientation == CharacterOrientation.right)
            {
                bool makeNew = true;
                foreach (Projectile projecto in _projectiles)
                {
                    if (projecto.Visible == false)
                    {
                        makeNew = false;
                        projecto.Fire(new Vector2(Position.X, Position.Y),
                            new Vector2(400, 0), new Vector2(1, 0), 500);
                        break;
                    }
                }

                if (makeNew == true)
                {
                    Projectile newProjecto = new Projectile(projectileSprite, characterPosition, SpriteBatch, fighterTag, rangedAttack);
                    newProjecto.LoadContent(_theContenter);
                    newProjecto.Fire(new Vector2(Position.X, Position.Y),
                        new Vector2(400, 400), new Vector2(1, 0), 500);
                    _projectiles.Add(newProjecto);
                }
            }
            else if (_characterOrientation == CharacterOrientation.left)
            {
                bool makeNew = true;
                foreach (Projectile projecto in _projectiles)
                {
                    if (projecto.Visible == false)
                    {
                        makeNew = false;
                        projecto.Fire(new Vector2(Position.X, Position.Y),
                            new Vector2(400, 0), new Vector2(-1, 0), 500);
                        break;
                    }
                }

                if (makeNew == true)
                {
                    Projectile newProjecto = new Projectile(projectileSprite, characterPosition, SpriteBatch, fighterTag, rangedAttack);
                    newProjecto.LoadContent(_theContenter);
                    newProjecto.Fire(new Vector2(Position.X, Position.Y),
                        new Vector2(400, 400), new Vector2(-1, 0), 500);
                    _projectiles.Add(newProjecto);
                }
            }
            else if (_characterOrientation == CharacterOrientation.up)
            {
                bool makeNew = true;
                foreach (Projectile projecto in _projectiles)
                {
                    if (projecto.Visible == false)
                    {
                        makeNew = false;
                        projecto.Fire(new Vector2(Position.X, Position.Y),
                            new Vector2(0, 400) + characterPosition, new Vector2(0, -1), 500);
                        break;
                    }
                }

                if (makeNew == true)
                {
                    Projectile newProjecto = new Projectile(projectileSprite, characterPosition, SpriteBatch, fighterTag, rangedAttack);
                    newProjecto.LoadContent(_theContenter);
                    newProjecto.Fire(new Vector2(Position.X, Position.Y),
                        new Vector2(400, 400) + characterPosition, new Vector2(0, -1), 500);
                    _projectiles.Add(newProjecto);
                }
            }
            else if (_characterOrientation == CharacterOrientation.down)
            {
                bool makeNew = true;
                foreach (Projectile projecto in _projectiles)
                {
                    if (projecto.Visible == false)
                    {
                        makeNew = false;
                        projecto.Fire(new Vector2(Position.X, Position.Y),
                            new Vector2(0, 400) + characterPosition, new Vector2(0, 1), 500);
                        break;
                    }
                }

                if (makeNew == true)
                {
                    Projectile newProjecto = new Projectile(projectileSprite, characterPosition, SpriteBatch, fighterTag, rangedAttack);
                    newProjecto.LoadContent(_theContenter);
                    newProjecto.Fire(new Vector2(Position.X, Position.Y),
                        new Vector2(400, 400) + characterPosition, new Vector2(0, 1), 500);
                    _projectiles.Add(newProjecto);
                }
            }
        }
        public void attackBasedOnDirection(GameTime gameTime)
        {
            if (_characterOrientation == CharacterOrientation.right)
            {
                bool makeNew = true;
                foreach (Projectile puncho in _punches)
                {
                    if (puncho.Visible == false)
                    {
                        makeNew = false;
                        puncho.Fire(new Vector2(Position.X, Position.Y),
                            new Vector2(400, 0), new Vector2(1, 0), 25);
                        break;
                    }
                }

                if (makeNew == true)
                {
                    Projectile newPuncho = new Projectile(projectileSprite, characterPosition, SpriteBatch, fighterTag, meleeAttack);
                    newPuncho.LoadContent(_theContenter);
                    newPuncho.Fire(new Vector2(Position.X, Position.Y),
                        new Vector2(400, 400), new Vector2(1, 0), 25);
                    _punches.Add(newPuncho);
                }
            }
            else if (_characterOrientation == CharacterOrientation.left)
            {
                bool makeNew = true;
                foreach (Projectile puncho in _punches)
                {
                    if (puncho.Visible == false)
                    {
                        makeNew = false;
                        puncho.Fire(new Vector2(Position.X, Position.Y),
                            new Vector2(400, 0), new Vector2(-1, 0), 25);
                        break;
                    }
                }

                if (makeNew == true)
                {
                    Projectile newPuncho = new Projectile(projectileSprite, characterPosition, SpriteBatch, fighterTag, meleeAttack);
                    newPuncho.LoadContent(_theContenter);
                    newPuncho.Fire(new Vector2(Position.X, Position.Y),
                        new Vector2(400, 400), new Vector2(-1, 0), 25);
                    _punches.Add(newPuncho);
                }
            }
            else if (_characterOrientation == CharacterOrientation.up)
            {
                bool makeNew = true;
                foreach (Projectile puncho in _punches)
                {
                    if (puncho.Visible == false)
                    {
                        makeNew = false;
                        puncho.Fire(new Vector2(Position.X, Position.Y),
                            new Vector2(0, 400) + characterPosition, new Vector2(0, -1), 25);
                        break;
                    }
                }

                if (makeNew == true)
                {
                    Projectile newPuncho = new Projectile(projectileSprite, characterPosition, SpriteBatch, fighterTag, meleeAttack);
                    newPuncho.LoadContent(_theContenter);
                    newPuncho.Fire(new Vector2(Position.X, Position.Y),
                        new Vector2(400, 400) + characterPosition, new Vector2(0, -1), 25);
                    _punches.Add(newPuncho);
                }
            }
            else if (_characterOrientation == CharacterOrientation.down)
            {
                bool makeNew = true;
                foreach (Projectile puncho in _punches)
                {
                    if (puncho.Visible == false)
                    {
                        makeNew = false;
                        puncho.Fire(new Vector2(Position.X, Position.Y),
                            new Vector2(0, 400) + characterPosition, new Vector2(0, 1), 25);
                        break;
                    }
                }

                if (makeNew == true)
                {
                    Projectile newPuncho = new Projectile(projectileSprite, characterPosition, SpriteBatch, fighterTag, meleeAttack);
                    newPuncho.LoadContent(_theContenter);
                    newPuncho.Fire(new Vector2(Position.X, Position.Y),
                        new Vector2(400, 400) + characterPosition, new Vector2(0, 1), 25);
                    _punches.Add(newPuncho);
                }
            }
        }
        public void checkVitality()
        {
            if (health <= 0)
            {
                life = false;
                Stage.Level.charactersDead++;
            }
            else if (health > 0)
            {
                life = true;
            }
        }
        public int getFighterTag()
        {
            return fighterTag;
        }
        //nerfs range attack
        public void checkMagazine()
        {
            if (shotsFired == 0) { doIRelod = true; shotsFired = 0; }
            if (doIRelod == true)
            {
                if (timeSpent == 0) { doIRelod = false; timeSpent = 300; shotsFired = 7; }
                else { timeSpent--; }
            }
        }
        //updates sprite texture ie pigCharacter
        public void updateSpriteBasedOnDirection()
        {
            if (getCharacterOrientation() == CharacterOrientation.left)
            {
                source.Y = 1 * 39;
            }
            else if (getCharacterOrientation() == CharacterOrientation.right)
            {
                source.Y = 2 * 39;
            }
            else if (getCharacterOrientation() == CharacterOrientation.up)
            {
                source.Y = 3 * 39;
            }
            else if (getCharacterOrientation() == CharacterOrientation.down)
            {
                source.Y = 0 * 39;
            }

        }
        public void updateSpriteBasedOnState()
        {
            switchCounter--;
            if (switchCounter == 0)
            {
                frameCounter = (frameCounter + 1) % 3;
                switchCounter = 30;
            }
            source.X = frameCounter * 39;
        }
        //collisions
        public void checkForCollisionWithTheGround()
        {
            if (IsOnFirmGround() == true)
            {
                _characterState = CharacterState.onGround;
            }
            else if (IsOnFirmGround() == false)
            {
                _characterState = CharacterState.jumpedTwice;
                AffectWithGravity();
            }
        }
        private void UpdatePositionBasedOnMovement(GameTime gameTime)
        {
            //totalmilliseconds/int could make a nice weight or speed consept
            Position += characterPosition * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 30;
        }
        private void AffectWithGravity()
        {
            characterPosition += Vector2.UnitY * .5f;
        }
        private void SimulateFriction()
        {
            if (IsOnFirmGround()) { characterPosition -= characterPosition * Vector2.One * .08f; }
            else { characterPosition -= characterPosition * Vector2.One * .04f; }
        }
        private void MoveAsFarAsPossible(GameTime gameTime)
        {
            oldPosition = Position;
            UpdatePositionBasedOnMovement(gameTime);
            Position = Stage.Level.WhereCanIGetTo(oldPosition, Position, Bounds);
        }
        public bool IsOnFirmGround()
        {
            Rectangle onePixelLower = Bounds;
            onePixelLower.Offset(0, 1);
            return !Stage.Level.HasRoomForRectangle(onePixelLower);
        }
        private void StopMovingIfBlocked()
        {
            Vector2 lastMovement = oldPosition - Position;
            if (lastMovement.X == 0) { characterPosition *= Vector2.UnitY; }
            if (lastMovement.Y == 0) { characterPosition *= Vector2.UnitX; }
        }
    }
}

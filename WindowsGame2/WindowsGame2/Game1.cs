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
using System.Diagnostics;

//Project: Centure           Coded by: Douglas Bentley               Version: alpha
//Changes:
/*
 * Added collision detection for hitting roof while jumping 
 * Added FutureCollision bool variable to test for future collisions
 * Added futureplayerBox rectangle which i will make resizable
 * Added code to resize rectangle based on movement
 * Added maxmove to adjust speed gradually
 * Added accelleration and decelleration of walk, run, duckwalk but extremely fast (barely noticable)
 * Began implementing new collision detection based on speed of entry and amount of overlap
*/

namespace Centure
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public enum Level
        {
            tutorial,
            level1,
            level2,
            level3,
        };

        Texture2D box;
        Texture2D tutBackground;

        Collision Collision_platform = new Collision();
        Collision FutureCollision = new Collision();

        BaseLevel baseLevel = new BaseLevel();
        Level currentLevel = Level.tutorial; //defualt
        LoadLevel Loader = new LoadLevel();        
        int movespeed;
        int maxmove;
        int jumpspeed = 0;
        int airtime = 0;
        int jumptime = 0;
        float oldY;
        float ydiff;
        float movementRatio;
        float overlapRatio;
        
        Rectangle playerBox;
        Rectangle futureplayerBox;
        string lalala;
        public bool form2 = false;
        public bool right = true;
        bool ground = false;
        bool futureCollision; //for now    

        Vector2 textPos = new Vector2 (0,0);
        private SpriteFont Font;
        Vector2 charPos = new Vector2(374, 300);
        Vector2 movement = Vector2.Zero; //intialised 
        Vector2 overlap = Vector2.Zero;
        Vector2 futureoverlap = Vector2.Zero;
        Vector2 movementfunction = Vector2.Zero;

        SimpleAnimation playerAnimation = new SimpleAnimation();
        SimpleAnimation idleAnimation = new SimpleAnimation();
        SimpleAnimation jumpAnimation = new SimpleAnimation();
        SimpleAnimation duckAnimation = new SimpleAnimation();
        SimpleAnimation rise = new SimpleAnimation();
        SimpleAnimation duckWalkAnimation = new SimpleAnimation();
        SimpleAnimation idleDuckAnimation = new SimpleAnimation();
        SimpleAnimation duckJump = new SimpleAnimation();
  
        private SpriteFont fonty; 

        CharacterSprite small ;
        CharacterSprite big ;
  
        Point bigframeSize = new Point(100, 104);
        Point frameSize = new Point(52, 64);
        Point currentFrame = new Point(0, 0);
        Point sheetSize = new Point(8, 2);

        public KeyboardState theKeyboardState; 
        public KeyboardState oldKeyboardState;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
         //   TargetElapsedTime = new TimeSpan(0, 0, 0, 0, 40);

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            small = new CharacterSprite();
            big = new CharacterSprite();
            Loader.setLevel((int)currentLevel); //to the tutorial level
            Font = Content.Load<SpriteFont>("text");
            movement = new Vector2 (0,0);
            Texture2D playerTexture = Content.Load<Texture2D>("SmallChar");
            tutBackground = Content.Load<Texture2D>("tutback2");

            box = Content.Load<Texture2D>("background");

            playerAnimation.initialise(playerTexture, new Vector2(0, 400), frameSize, 7, 60, Color.White, true, new Point (0,0), SimpleAnimation.AniState.Standard, 9);
            idleAnimation.initialise(playerTexture, Vector2.Zero, frameSize, 1, 10, Color.White, true, new Point(0,1), SimpleAnimation.AniState.Standard, 9);
            jumpAnimation.initialise(playerTexture, Vector2.Zero, frameSize, 5, 10, Color.White, true, new Point(0, 2), SimpleAnimation.AniState.Special, 9);
            duckAnimation.initialise(playerTexture, Vector2.Zero, frameSize, 2, 40, Color.White, false, new Point(0, 3), SimpleAnimation.AniState.Standard, 2);
            rise.initialise(playerTexture, Vector2.Zero, frameSize, 1, 40, Color.White, true, new Point(3, 3), SimpleAnimation.AniState.Standard, 0);
            duckWalkAnimation.initialise(playerTexture, Vector2.Zero, frameSize, 4, 40, Color.White, true, new Point(0, 4), SimpleAnimation.AniState.Standard, 9);
            idleDuckAnimation.initialise(playerTexture, Vector2.Zero, frameSize, 1, 10, Color.White, true, new Point(0, 5), SimpleAnimation.AniState.Standard, 8);
            duckJump.initialise(playerTexture, Vector2.Zero, frameSize, 2, 10, Color.White, true, new Point(0, 6), SimpleAnimation.AniState.Special, 9);


            fonty = Content.Load<SpriteFont>("YPosFont");       
            base.Initialize();            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Width/2, 140); ///STARTPOS METHINKSS

            small.Initialise(playerAnimation, playerPosition);

            Loader.LoadLevelContent(this.Content);  
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit

                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
           // KeyboardState theKeyBoardstate;

                if (small.visible == true)
                    updatePlayerBox();
            
            theKeyboardState = Keyboard.GetState();
            baseLevel.PlatformCollisionUD = false;
            baseLevel.PlatformCollisionLR = false;       
            Loader.UpdateLevel(movement, baseLevel); 
            updatePlayer(gameTime);            
            oldKeyboardState = theKeyboardState;
            lalala = small.spritePos.Y.ToString();

            base.Update(gameTime);
        }

        private void updatefuturePlayerBox()
        {
            futureplayerBox = new Rectangle(); 
            if (movespeed == 0)//still
            {
                futureplayerBox.X = (int)small.spritePos.X - (frameSize.X / 2) + 5 ;  // X minus half to edge + 5 for adjustment
                futureplayerBox.Width = frameSize.X - 10; // width plus 2x the movespeed to make up for X being pulled left - 10 for adjustment in same manner
            }
            else if (movespeed > 0)//right 
            {
                futureplayerBox.X = (int)small.spritePos.X - (frameSize.X / 2) + 5 ;  // X minus half to edge + 5 for adjustment
                futureplayerBox.Width = frameSize.X + movespeed - 10 ; // width plus 2x the movespeed to make up for X being pulled left - 10 for adjustment in same manner
            }
            else if (movespeed < 0)//left
            {
                futureplayerBox.X = (int)small.spritePos.X + movespeed - (frameSize.X / 2) + 5 ;  // X minus half to edge + 5 for adjustment
                futureplayerBox.Width = frameSize.X - movespeed - 10 ; // width plus 2x the movespeed to make up for X being pulled left - 10 for adjustment in same manner
            }

            if (airtime + jumpspeed > 0) //falling
            {
                futureplayerBox.Y = (int)small.spritePos.Y + 2 - (small.height / 2);   //Falling so Y has no adjustment
                futureplayerBox.Height  =  frameSize.Y + 2*(airtime + jumpspeed) -3;  //Height increased

            }
            else if (airtime + jumpspeed < 0) //rising
            {
                futureplayerBox.Y = (int)small.spritePos.Y + 2 * (airtime + jumpspeed) - (small.height / 2) - 1;   //Y minus speed of ascent
               futureplayerBox.Height = frameSize.Y - (airtime + jumpspeed);  //Height added to compensate for 
            }
            else // velocity = 0;
            {
               //futureplayerBox.Y = (int)small.spritePos.Y - (small.height/2) ; 
               // futureplayerBox.Height = frameSize.Y - 4;
                futureplayerBox.Y = playerBox.Y ; 
                futureplayerBox.Height = playerBox.Height - 4;
            }
        }
      
        private void updatePlayerBox()
        {
            //Setting the collision detection box
            if (theKeyboardState.IsKeyUp(Keys.Down))
                playerBox = new Rectangle((int)small.spritePos.X - (frameSize.X / 2) + 5, (int)small.spritePos.Y - frameSize.Y / 2, frameSize.X - 10, frameSize.Y - 4);
            else
            playerBox = new Rectangle((int)small.spritePos.X - (frameSize.X / 2) + 16, (int)small.spritePos.Y - frameSize.Y / 2 + 35, frameSize.X - 31, frameSize.Y - 37); // a bit smaller because space left at the bottom
        }

        private void overlapUpdate()
        {
            updatePlayerBox();
            Loader.UpdateLevel(movement, baseLevel);
            overlap = baseLevel.CollisionChecker(playerBox, Collision_platform, new Vector2(movespeed, airtime + jumpspeed));
        }

        private void futureCollisionCheck()
        {
            baseLevel = Loader.getLevel();
            updatePlayerBox();
            updatefuturePlayerBox();
            Loader.UpdateLevel(movement, baseLevel);
            futureoverlap = baseLevel.CollisionChecker(futureplayerBox, Collision_platform, new Vector2(movespeed, airtime + jumpspeed));

            if (futureoverlap == Vector2.Zero)
                futureCollision = false;
            else
                futureCollision = true;
        }

        private void yposReturned()
        {
            small.spritePos.Y += (overlap.Y); 
            jumptime = 0;
            jumpspeed = 0;
            airtime = 0;
        }

        private void updatePlayer(GameTime gameTime)
        {
            futureCollisionCheck();

            //Setting the speed of the jump:
            if ((theKeyboardState.IsKeyDown(Keys.Z) && oldKeyboardState.IsKeyUp(Keys.Z) && airtime == 0) || (jumptime > 0))  
            {
                ground = false;

                if (theKeyboardState.IsKeyDown(Keys.Down) && theKeyboardState.IsKeyDown(Keys.Z))
                {                   
                    jumpspeed = -13;
                    jumptime += (int)gameTime.ElapsedGameTime.Milliseconds / 10;
                }
                else if ((theKeyboardState.IsKeyDown(Keys.Z) || jumptime > 10) && theKeyboardState.IsKeyUp(Keys.Down))
                {
                    jumptime += (int)gameTime.ElapsedGameTime.Milliseconds / 10;
                    jumpspeed = -20;
                }
                else if (theKeyboardState.IsKeyUp(Keys.Down))
                    jumpspeed = -15;
            }            
            else               
             jumpspeed = 0;


            //Setting the animations
            if (ground == true && airtime == 0 && theKeyboardState.IsKeyUp(Keys.Down) && theKeyboardState.IsKeyUp(Keys.Z))
            {
                small.theAnimation = idleAnimation;
            }
            else if (theKeyboardState.IsKeyDown(Keys.Z) && theKeyboardState.IsKeyUp(Keys.Down))
            {
                small.theAnimation = jumpAnimation;       
            }
  

            //Falling/jumping without checking
                if (futureCollision == false)
                {
                    if (overlap == Vector2.Zero || jumptime > 0)
                    {
                        airtime += (int)gameTime.ElapsedGameTime.Milliseconds / 10;
                        small.spritePos.Y += jumpspeed + airtime;
                        if (airtime > 1)
                        ground = false;
                    }
                }
                futureCollisionCheck();  //check once more to make future code correct moving into a platform

            //Falling/jumping and checking
                if (futureCollision == true)
                {
                    overlapUpdate();

                    if (overlap == Vector2.Zero) //let him fall one more
                    {
                        airtime += (int)gameTime.ElapsedGameTime.Milliseconds / 10;
                        small.spritePos.Y += jumpspeed + airtime;
                    }

                    overlapUpdate();

                    if (overlap.X == 0 && overlap.Y < 0 || ground == true) // falling straight down
                    {
                        yposReturned();
                        ground = true;

                    }
                    else if (overlap.X == 0 && overlap.Y > 0) // rising straight up
                    {
                        yposReturned();
                        overlapUpdate();
                    }
                    else if (overlap.X < 0 && overlap.Y > 0) //rising to the right
                    {
                        movementfunction = new Vector2(movespeed, airtime + jumpspeed);
                        movementRatio = movementfunction.Y / movementfunction.X;
                        overlapRatio = overlap.Y / overlap.X;

                        if (small.spritePos.X >= GraphicsDevice.Viewport.TitleSafeArea.Width / 2)
                        {
                            small.spritePos.X = GraphicsDevice.Viewport.TitleSafeArea.Width / 2;
                            if (movementRatio < overlapRatio) //not close to the edge
                            {           
                                movement.X += overlap.Y / movementRatio;
                                yposReturned();
                            }
                            small.spritePos.X = GraphicsDevice.Viewport.TitleSafeArea.Width / 2;
                        }
                        else if (small.spritePos.X < GraphicsDevice.Viewport.TitleSafeArea.Width / 2)
                        {
                            if (movementRatio < overlapRatio) //not close to the edge
                            {
                                yposReturned();
                                small.spritePos.X += overlap.Y / movementRatio;
                            }
                        }  
                    }
                    else if (overlap.X > 0 && overlap.Y > 0) //rising to the left
                    {
                        movementfunction = new Vector2(movespeed, airtime + jumpspeed);
                        movementRatio = movementfunction.Y / movementfunction.X;
                        overlapRatio = overlap.Y / overlap.X;

                        if (movement.X < 0)
                        {
                            small.spritePos.X = GraphicsDevice.Viewport.TitleSafeArea.Width / 2;
                            if (movementRatio > overlapRatio) //not close to the edge
                            {
                                yposReturned();
                                movement.X += overlap.Y / movementRatio;               
                            }
                            small.spritePos.X = GraphicsDevice.Viewport.TitleSafeArea.Width / 2;
                        }
                        else if (movement.X >= 0)
                        {
                            movement.X = 0;
                            if (movementRatio > overlapRatio) //not close to the edge
                            {
                                yposReturned();
                                small.spritePos.X += overlap.Y / movementRatio;                              
                            }
                        }
                    }
                    else if (overlap.X < 0 && overlap.Y < 0 && ground == false) // falling from the left to the right
                    {
                        movementfunction = new Vector2(movespeed, airtime + jumpspeed);
                        movementRatio = movementfunction.Y / movementfunction.X;
                        overlapRatio = overlap.Y / overlap.X;

                        if (small.spritePos.X >= GraphicsDevice.Viewport.TitleSafeArea.Width / 2)
                        {
                            small.spritePos.X = GraphicsDevice.Viewport.TitleSafeArea.Width / 2;
                            if (movementRatio > overlapRatio) //not close to the edge
                            {
                                yposReturned();
                                movement.X += overlap.Y / movementRatio;
                                ground = true; 
                            }
                            if (movementRatio < overlapRatio) //close to the edge
                            {
                                movement.X += overlap.X;
                                small.spritePos.Y += overlap.X / movementRatio;
                            }
                            small.spritePos.X = GraphicsDevice.Viewport.TitleSafeArea.Width / 2;
                        }
                        else if (small.spritePos.X < GraphicsDevice.Viewport.TitleSafeArea.Width / 2)
                        {
                            if (movementRatio > overlapRatio) //not close to the edge
                            {
                                yposReturned();
                                small.spritePos.X += overlap.Y / movementRatio;
                                ground = true;
                            }
                            if (movementRatio < overlapRatio) //close to the edge
                            {
                                small.spritePos.X += overlap.X;
                                small.spritePos.Y += overlap.X / movementRatio;
                            }
                        }  
                    }
                    else if (overlap.X > 0 && overlap.Y < 0 && ground == false) // falling from the right
                    {
                        movementfunction = new Vector2(movespeed, airtime + jumpspeed);
                        movementRatio = movementfunction.Y / movementfunction.X;
                        overlapRatio = overlap.Y / overlap.X;

                        if (movement.X < 0) //moving left but not at the edge
                        {
                            small.spritePos.X = GraphicsDevice.Viewport.TitleSafeArea.Width / 2;
                            if (movementRatio < overlapRatio) //not close to the edge
                            {
                                yposReturned();
                                movement.X += overlap.Y / movementRatio;
                                ground = true;
                            }
                        }
                        else if (movement.X >= 0)
                        {
                            movement.X = 0;
                            if (movementRatio < overlapRatio) //not close to the edge
                            {
                                yposReturned();
                                small.spritePos.X += overlap.Y / movementRatio;
                                ground = true;        
                            }
                        }
                    }
                    futureCollisionCheck();  //just so it draws the right thing
                    updatePlayerBox();
                    Loader.UpdateLevel(movement, baseLevel);
                    overlap = baseLevel.CollisionChecker(playerBox, Collision_platform, new Vector2(movespeed, airtime + jumpspeed));
                }


            //Player reset
                if (small.spritePos.Y >= 600)
                     Initialize();


            //Setting the walk/run speed
            if (theKeyboardState.IsKeyDown(Keys.X))
            {
                maxmove = 8;
                playerAnimation.updateAniSpeed(30);
            }
            else
            {
                maxmove = 4;
                playerAnimation.updateAniSpeed(60);
            }

            //Setting up ducking animations and duck walking speed and duck jumping speed
            if (theKeyboardState.IsKeyDown(Keys.Down) && oldKeyboardState.IsKeyUp(Keys.Down))
            {
                duckAnimation.frame.X = 0;            
                maxmove = 1;
                small.theAnimation = duckAnimation;               
                small.theAnimation.Active = true;        
            }
            else if (theKeyboardState.IsKeyDown(Keys.Down) && (theKeyboardState.IsKeyDown(Keys.Left) || theKeyboardState.IsKeyDown(Keys.Right)) && airtime == 0 && duckAnimation.frame.X == duckAnimation.frameCount)
                {                    
                    maxmove = 1;
                    small.theAnimation = duckWalkAnimation;
           //         Debug.WriteLine("DUCKWAAAAAAALKKK");
                }
            else if (theKeyboardState.IsKeyDown(Keys.Down) && airtime > 0 && duckAnimation.frame.X == duckAnimation.frameCount)
            {             
                maxmove = 1;
                small.theAnimation = duckJump;
          //      Debug.WriteLine("DUCKUJUMPU!");
            }
            else if (theKeyboardState.IsKeyDown(Keys.Down) && (duckAnimation.frame.X == duckAnimation.frameCount || small.theAnimation == duckWalkAnimation) && airtime == 0)
            {
                small.theAnimation = idleDuckAnimation;
            //    Debug.WriteLine("DUCKSTOOOOOP");
            }

        //    Debug.WriteLineIf(small.theAnimation == duckAnimation, "TRANSFORMIIIIING");

            //Walking left 
                if (theKeyboardState.IsKeyDown(Keys.Left))
                {
                    if (movespeed > -1 * maxmove) //speed up
                    {
                        movespeed--;
                        right = false;
                    }
                    if (movespeed < -1 * maxmove && ground == true) //slow down
                    {
                        movespeed++;
                    }
                    if (movespeed > 0)  //instant turning
                        movespeed = 0;

                        if (ydiff == 0 && airtime == 0 && theKeyboardState.IsKeyUp(Keys.Down))
                        {
                            small.theAnimation = playerAnimation;
                        }
                        //Walking left checking each point
                        if (futureCollision == true)
                        {
                        /*    Loader.UpdateLevel(movement, baseLevel); //update rect position
                            updatePlayerBox();
                            baseLevel.CollisionCheckerLR(LeftRightCollision, playerBox);

                            for (int i = 0; i <= Math.Abs(movespeed); i++)
                            {
                                if ((small.spritePos.X < GraphicsDevice.Viewport.TitleSafeArea.Width / 2 || movement.X == 0) && baseLevel.PlatformCollisionLR == false)
                                {
                                    if (playerBox.X > 0)
                                        small.spritePos.X -= 1;
                                    Loader.UpdateLevel(movement, baseLevel); //update rect position
                                    baseLevel.CollisionCheckerLR(LeftRightCollision, playerBox);
                                    if (baseLevel.PlatformCollisionLR)
                                        small.spritePos.X += 1;
                                    updatePlayerBox();
                                }
                                else if (baseLevel.PlatformCollisionLR == false)
                                {
                                    if (movement.X != 0)
                                    {
                                        movement.X += 1;
                                        Loader.UpdateLevel(movement, baseLevel); //update rect position
                                        baseLevel.CollisionCheckerLR(LeftRightCollision, playerBox);
                                        if (baseLevel.PlatformCollisionLR)
                                            movement.X -= 1;
                                        small.spritePos.X = GraphicsDevice.Viewport.TitleSafeArea.Width / 2;
                                        updatePlayerBox();
                                    }
                                }
                                else
                                {
                                    updatePlayerBox();
                                    break;
                                }
                            }*/
                        }
                        else //no future collisions detected
                        {
                            if ((small.spritePos.X < GraphicsDevice.Viewport.TitleSafeArea.Width / 2 || movement.X >= 0) && baseLevel.PlatformCollisionLR == false)
                            {
                                if (playerBox.X > 0)
                                    small.spritePos.X += movespeed;   
                            }
                            else if (baseLevel.PlatformCollisionLR == false)
                            {
                                if (movement.X != 0)
                                {
                                    movement.X -= movespeed;
                                }
                            }
                            else
                            {
                            }
                        }
                }

            //Walking right
                if (theKeyboardState.IsKeyDown(Keys.Right))
                {
                    if (movespeed < maxmove)
                    {
                        movespeed++;
                        right = true;
                    }

                    if (movespeed > maxmove && ground == true) //slow down
                    {
                        movespeed--;
                    }
                    if (movespeed < 0)  //instant turning
                        movespeed = 0;

                        if (ydiff == 0 && airtime == 0 && theKeyboardState.IsKeyUp(Keys.Down))
                        {
                            small.theAnimation = playerAnimation;  
                        }

                        if (futureCollision == false)  //No future collision
                        {
                            if (small.spritePos.X >= GraphicsDevice.Viewport.TitleSafeArea.Width / 2)
                            {
                                small.spritePos.X = GraphicsDevice.Viewport.TitleSafeArea.Width / 2;
                                movement.X -= movespeed;
                            }
                            if (small.spritePos.X < GraphicsDevice.Viewport.TitleSafeArea.Width / 2)
                            {
                                small.spritePos.X += movespeed;                               
                            }
                        }
                   /*     else if (ground == true) // theres is a future collision
                        {
                              if (small.spritePos.X >= GraphicsDevice.Viewport.TitleSafeArea.Width / 2)
                              {
                                  if (overlap == Vector2.Zero) //no collision
                                  {
                                    small.spritePos.X = GraphicsDevice.Viewport.TitleSafeArea.Width / 2;
                                    movement.X -= movespeed;
                                  }
                                  overlapUpdate();
                                  if (overlap != Vector2.Zero) //check for a collision now
                                  {
                                      movement.X -= overlap.X;
                                  }
                              }
                              else if (small.spritePos.X < GraphicsDevice.Viewport.TitleSafeArea.Width / 2)
                              {
                                  if (overlap == Vector2.Zero) //no collision
                                  {                                   
                                      small.spritePos.X += movespeed;
                                  }
                                  overlapUpdate();
                                  if (overlap != Vector2.Zero) //check for a collision now
                                  {
                                      small.spritePos.X += overlap.X;
                                  }
                                  overlapUpdate();
                              }
                         }*/
                        
                }

            if (theKeyboardState.IsKeyUp(Keys.Left) && theKeyboardState.IsKeyUp(Keys.Right)) //stopping
            {
                if (movespeed > 0)
                    movespeed--;
                if (movespeed < 0)
                    movespeed++;
            }


            //Changing form
                if (theKeyboardState.IsKeyDown(Keys.Space)) 
                {
                    if (form2 == false)
                    {
                        form2 = true;
                        small.visible = false;
                        big.visible = true;
                    }
                    else
                    {
                        form2 = false;
                        small.visible = false;
                        big.visible = true;
                    }
                }
                small.Update(gameTime, ydiff, jumptime);
                ydiff = oldY - small.spritePos.Y;
                oldY = small.spritePos.Y;

                Debug.WriteLine( "Velocity" +  (airtime + jumpspeed) + " Movespeed :" + movespeed + "     right: " + right + "   futurecol: " + futureCollision + "     Overlap:" + overlap);
                }        



        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            //Draw everything to screen
            spriteBatch.Begin();
           // spriteBatch.Draw(tutBackground, new Vector2(movement.X * 0.4f, 0) , Color.White);
            Loader.DrawLevel(spriteBatch);

           // spriteBatch.Draw(box, futureplayerBox, Color.White);
            small.Draw(spriteBatch, right);
            spriteBatch.DrawString(Font, lalala, Vector2.Zero, Color.White);
 
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

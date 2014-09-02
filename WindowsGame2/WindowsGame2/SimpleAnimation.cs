using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Centure
{
    class SimpleAnimation
    {
        Texture2D spriteSheet;
        public Point frame;
        public int frameCount;
        public int elapsedTime;
        public int frameTime;
        public int stillFrame;
        Color color;
        public Rectangle sourceRect = new Rectangle();
        public Rectangle destRect = new Rectangle();
        public Point fSize;
        public bool Looping;
        public bool Active;
        public Vector2 Position;
        public enum AniState
        {
            Standard, //looping dudes
            Special, //jumping
            Transforming,  //yeah          
        }
        public AniState mCurrentstate;

        public void initialise(Texture2D text, Vector2 position, Point fsize, int framecount, int frametime ,Color colour, bool looping, Point spriteCoords, AniState currentAniType, int stillframe)
        {          
  
            fSize = fsize;
            frameCount = framecount;
            color = colour;
            Looping = looping;
            spriteSheet = text;
            frameTime = frametime;
            elapsedTime = 0; //setting these to defaults
            frame = spriteCoords;
            mCurrentstate = currentAniType;
            Active = true;
            stillFrame = stillframe;
        }

        public void updateAniSpeed(int frametime)
        {
            frameTime = frametime;
        }

        public void update(GameTime gameTime, int jumptime) //used for normal looping and non-looping animations
        {
            if (Active == false)
                return;

            if (mCurrentstate != AniState.Standard)
            {
                destRect = new Rectangle((int)Position.X - (int)fSize.X / 2, (int)Position.Y - (int)fSize.Y / 2, (int)(fSize.X), (int)(fSize.Y));
                return;
            }

            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds ;

            if (elapsedTime > frameTime)
            {
                frame.X++;
                elapsedTime = 0;
                if (frame.X == frameCount)
                {
                    if (Looping == false)
                        frame.X = frameCount;
                    else

                    frame.X = 0;  
                }
            }

            //get the image from the sheet
            sourceRect = new Rectangle(frame.X * fSize.X, frame.Y*fSize.Y, fSize.X, fSize.Y);  //frame is a point as my spritesheet has more than one row
            //where you want to draw it
            destRect = new Rectangle((int)Position.X - (int)fSize.X/ 2, (int)Position.Y - (int)fSize.Y/2, (int)(fSize.X), (int)(fSize.Y));
        }

        public void updateSpecial(GameTime gametime, float ydiff)
        {
        //    if (Active == false)
        //        return;

            if (mCurrentstate != AniState.Special)
            {
                destRect = new Rectangle((int)Position.X - (int)fSize.X / 2, (int)Position.Y - (int)fSize.Y / 2, (int)(fSize.X), (int)(fSize.Y));
                return;
            }     
                frame.X = 0;

                if (ydiff >= 1 && ydiff < 20)
                    frame.X = 1;
                else if (ydiff >= -5 && ydiff < 1)
                    frame.X = 2;
                else if (ydiff < -5)
                    frame.X = 3;

                elapsedTime = 0;

                //get the image from the sheet
                sourceRect = new Rectangle(frame.X * fSize.X, frame.Y * fSize.Y, fSize.X, fSize.Y);  //frame is a point as my spritesheet has more than one row
                //where you want to draw it
                destRect = new Rectangle((int)Position.X - (int)fSize.X / 2, (int)Position.Y - (int)fSize.Y / 2, (int)(fSize.X), (int)(fSize.Y));       
        }
           
        public void Draw(SpriteBatch spriteBatch, bool right)
        {
            if (Active)
            {
                if (right == true)
                spriteBatch.Draw(spriteSheet, destRect, sourceRect, color); //right
                else if (right == false)
                spriteBatch.Draw(spriteSheet, destRect, sourceRect, color, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0); //left
            }
        }



        

    }
}

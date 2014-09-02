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


namespace Centure
{
    class BaseLevel
    {
        public  BackgroundSprite[] platforms = new BackgroundSprite[iplatform + 1];
        public BackgroundSprite[] backgrounds = new BackgroundSprite[ibackground+1];
        public Vector2 refPlatform = new Vector2(0, 412);
        public Vector2 overlap;
        public Vector2 saveOverlap;
        public bool PlatformCollisionUD = false;
        public bool PlatformCollisionLR = false;
        public static int iplatform = 8;
        public static int ibackground = 0;
        public int collisions = 0;


        public void LoadLevel(ContentManager theContentManager)
        {
            for (int i = 0 ; i <= iplatform; i++) //Create the platforms
            {
                platforms[i] = new BackgroundSprite();
                platforms[i].isPlatform = true;//this is probably redudant... probably
            }

            for (int i = 0; i <= ibackground; i++) //Create the backgrounds
            {
                backgrounds[i] = new BackgroundSprite();
                backgrounds[i].isPlatform = false;
            }
        }

        public Vector2 CollisionChecker(Rectangle PlayerBox, Collision collision_platform, Vector2 movement)
        {
            saveOverlap = Vector2.Zero;
            for (int i = 0; i <= iplatform; i++) //check through all the platforms
            {
                Rectangle platformBox = new Rectangle((int)platforms[i].spritePos.X, (int)platforms[i].spritePos.Y + 7, platforms[i].Size.Width, platforms[i].Size.Height - 7); //since platform tops are below sprite tops -7 to compensate
                overlap = collision_platform.DetectCollision(PlayerBox, platformBox, movement);

                if (overlap != Vector2.Zero)
                {
                    collisions++;
                    saveOverlap = overlap;
                }                

            }
            return saveOverlap;
        }

        public void DrawTheLevel(SpriteBatch theSpriteBatch) //Draw everything
        {
            for (int i = 0; i <= ibackground; i++)
                backgrounds[i].DrawSprite(theSpriteBatch);  //Draw backgrounds first

            for (int i = 0; i <= iplatform; i++)
                platforms[i].DrawSprite(theSpriteBatch);
        }


    }
    
}

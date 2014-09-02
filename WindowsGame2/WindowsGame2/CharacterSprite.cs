using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsGame2;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Centure
{
    class CharacterSprite
    {
        public Vector2 spritePos;     
        public float Scale = 1.0f; 
        public int health;
        public bool visible = true;
        public SimpleAnimation theAnimation = new SimpleAnimation();
  

        public int width
        {
            get { return theAnimation.fSize.X; }
        }

           public int height
        {
            get { return theAnimation.fSize.Y; }
        }

        public  void Initialise(SimpleAnimation animation, Vector2 position)
        {
            spritePos = position;
            theAnimation = animation;
            visible = true;
            health = 3;
        }

        //Update player animation
        public void Update(GameTime gametime, float ydiff, int jumptime)
        {
                theAnimation.Position = spritePos;
                theAnimation.updateSpecial(gametime, ydiff);
                theAnimation.update(gametime, jumptime);
        }

        public void Draw(SpriteBatch spritebatch, bool righty)
        {
            theAnimation.Draw(spritebatch, righty);
        }  
 
    }
}



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace Centure
{
    class BackgroundSprite
    {
        public bool isPlatform; //if false don't detect collision
        public Vector2 spritePos = new Vector2(0, 0);
        private Texture2D spriteText;
        public Rectangle Size;
        public float Scale = 1.0f;

        public void LoadContent(ContentManager theContMan, string nameOfFile, bool platform)
        {
            isPlatform = platform;
            spriteText = theContMan.Load<Texture2D>(nameOfFile);
            Size = new Rectangle(0, 0, (int)(spriteText.Width * Scale), (int)(spriteText.Height * Scale));
        }

        public void DrawSprite(SpriteBatch theSpritebatch)
        {
            theSpritebatch.Draw(spriteText, spritePos,
                new Rectangle(0, 0, spriteText.Width, spriteText.Height), Color.White
                , 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }
  
    }
}

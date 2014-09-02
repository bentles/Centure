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
    class tutLevel : BaseLevel
    {
        public void SetLevel(ContentManager theContentManager)
        {
            backgrounds[0].LoadContent(theContentManager, "tutback2", false);
            backgrounds[0].spritePos = Vector2.Zero;

            platforms[0].LoadContent(theContentManager, "tutplatform1", true);  //set the sprite art
            platforms[1].spritePos = refPlatform; //initialises first platform
            platforms[1].LoadContent(theContentManager, "tutplatform2", true);
            platforms[1].spritePos = refPlatform + new Vector2 (2225, 0);
            platforms[2].LoadContent(theContentManager, "tutplatform3", true);
            platforms[2].spritePos = refPlatform + new Vector2(200, -66);
            platforms[3].LoadContent(theContentManager, "tutplatform3", true);
            platforms[3].spritePos = refPlatform + new Vector2(830, -166);
            platforms[4].LoadContent(theContentManager, "tutplatform4", true);
            platforms[4].spritePos = refPlatform + new Vector2( 3020, 0);
            platforms[5].LoadContent(theContentManager, "tutplatform5", true);
            platforms[5].spritePos = refPlatform + new Vector2(3620, -96);
            platforms[6].LoadContent(theContentManager, "tutplatform6", true);
            platforms[6].spritePos = refPlatform + new Vector2(3920, -196);
            platforms[7].LoadContent(theContentManager, "tutplatform7", true);
            platforms[7].spritePos = refPlatform + new Vector2(4240, -116);
            platforms[8].LoadContent(theContentManager, "tutplatform8", true);
            platforms[8].spritePos = refPlatform + new Vector2(4440, -500);

        }       
        

        public void UpdatePositions(Vector2 movement) //how much things have moved off starting position
        {
            backgrounds[0].spritePos = new Vector2 (0.4f*movement.X, 0.4f*movement.Y);

            platforms[0].spritePos = refPlatform + movement;
            platforms[1].spritePos = refPlatform + new Vector2(platforms[0].Size.Width + 130, 0) + movement; //Move them if needed
            platforms[2].spritePos = refPlatform + new Vector2(200, -66) + movement;
            platforms[3].spritePos = refPlatform + new Vector2(850, -166) + movement;
            platforms[4].spritePos = refPlatform + new Vector2(3020, 0) + movement;
            platforms[5].spritePos = refPlatform + new Vector2(3620, -96)+ movement;
            platforms[6].spritePos = refPlatform + new Vector2(3920, -196) + movement;
            platforms[7].spritePos = refPlatform + new Vector2(4250, -126) + movement;
            platforms[8].spritePos = refPlatform + new Vector2(4320, -415) + movement;
        }   
    }
}

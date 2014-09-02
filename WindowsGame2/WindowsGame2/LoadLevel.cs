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
    class LoadLevel       
    {        
        private int Level;   
        public  tutLevel tutorialLevel = new tutLevel();

        public BaseLevel getLevel()
        {
            BaseLevel tempBaseLev = new BaseLevel();
            switch (Level)
            {
                case 0:
                    tempBaseLev = (BaseLevel)tutorialLevel;
                    break;
            }
            return tempBaseLev;
        }
 

        public void setLevel(int level)
        {
            Level = level;         
            
        }

        public void LoadLevelContent(ContentManager contentMan)
        {
            switch(Level)
            {
                case 0: 
                    tutorialLevel.LoadLevel(contentMan);
                    tutorialLevel.SetLevel(contentMan);
                    break;
                case 1 :
                    break;
            }
        }

        public void CollisionCheck(Collision collision_platform, Rectangle Playabox, BaseLevel baselevel, Vector2 movement)
        {
            switch (Level)
            {
                case 0:
                    baselevel = (BaseLevel)tutorialLevel;
                    tutorialLevel.CollisionChecker(Playabox, collision_platform, movement);
                    break;
                case 1:
                    break;
            }
        }

        public void UpdateLevel(Vector2 moved, BaseLevel baseLevel)
        {
            switch (Level)
            {
                case 0:
                    baseLevel = (BaseLevel)tutorialLevel;
                    tutorialLevel.UpdatePositions(moved);
                    break;
                case 1:
                    break;
            }
        }

        public void DrawLevel(SpriteBatch thespritebatch)
        {
            switch (Level)
            {
                case 0 :
                    tutorialLevel.DrawTheLevel(thespritebatch);
                    break ;
                case 1:
                    break ;
            }
        }

        
    }
}

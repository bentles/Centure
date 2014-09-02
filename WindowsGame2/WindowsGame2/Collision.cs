
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
    class Collision
    {
        public enum CollisionType 
        { 
            colBelow,
            colAbove,
            coltoLeft, 
            coltoRight 
        };

        public CollisionType UDCollision = CollisionType.colAbove; //default
        public CollisionType LRCollision = CollisionType.coltoLeft;
        public Vector2 overlap = Vector2.Zero;


        public Vector2 DetectCollision(Rectangle playerSprite, Rectangle objectSprite, Vector2 movement) //returns a point showing how much overlap
        {   
            
            //Code to detect if there is a collision and, if so, what the overlap is of the collision 
            overlap = Vector2.Zero;
            if (playerSprite.Intersects(objectSprite) == false) //no overlap, no vlaues
                overlap = Vector2.Zero;
            else if (movement.X < 0 && movement.Y < 0) // moving up and left  
                overlap = new Vector2(objectSprite.Right - playerSprite.Left, objectSprite.Bottom - playerSprite.Top);
            else if (movement.X < 0 && movement.Y > 0) //moving down and left
                overlap = new Vector2(objectSprite.Right - playerSprite.Left, objectSprite.Top - playerSprite.Bottom);
            else if (movement.X > 0 && movement.Y < 0) // moving up and right
                overlap = new Vector2(objectSprite.Left - playerSprite.Right, objectSprite.Bottom - playerSprite.Top);
            else if (movement.X > 0 && movement.Y > 0) //down and right
                overlap = new Vector2(objectSprite.Left - playerSprite.Right, objectSprite.Top - playerSprite.Bottom);
            else if (movement.X == 0 && movement.Y > 0) //down
                overlap = new Vector2(0, objectSprite.Top - playerSprite.Bottom);
            else if (movement.X == 0 && movement.Y < 0) //up
                overlap = new Vector2(0, objectSprite.Bottom - playerSprite.Top);
            else if (movement.X < 0 && movement.Y == 0) //left
                overlap = new Vector2(objectSprite.Right - playerSprite.Left, 0);
            else if (movement.X > 0 && movement.Y == 0) //right
                overlap = new Vector2(objectSprite.Left - playerSprite.Right, 0);
                
                return overlap;
            }

        }
    }


using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoTinker.Code.Components.Elements.DebugGraphics
{
    public class DebugShapes
    {
        private static Texture2D pixel;

        private static void CreatePixel(SpriteBatch spriteBatch)
        {
            pixel = new Texture2D(spriteBatch.GraphicsDevice,1,1,false,SurfaceFormat.Color);
            pixel.SetData(new [] {Color.White});
        }


        public static void DrawLine(SpriteBatch spriteBatch, Vector2 startingPoint, Vector2 endingPoint, Color color)
        {
            DrawLine(spriteBatch, startingPoint,endingPoint, 1.0f, color);
        }

        public static void DrawLine(SpriteBatch spriteBatch, Vector2 startingPoint, Vector2 endingPoint,float thickness, Color color)
        {
            float distance = Vector2.Distance(startingPoint, endingPoint);
            float angle = (float)Math.Atan2(endingPoint.Y - startingPoint.Y, endingPoint.X - startingPoint.X);

            DrawLine(spriteBatch,startingPoint,distance,angle,thickness, color);
        }

        public static void DrawLine(SpriteBatch spriteBatch, Vector2 startingPoint, float distance, float angle, float thickness, Color color)
        {
            if (pixel == null)
            {
                CreatePixel(spriteBatch);
            }
            spriteBatch.Draw(pixel,startingPoint,null,color,angle,Vector2.Zero, new Vector2(distance,thickness),SpriteEffects.None, 0 );
        }


        public static void DrawRectagnle(SpriteBatch spribaBatch, Vector2 position, Vector2 size, float thickness,
            Color color)
        {
            float x1 = position.X+thickness;
            float x2 = x1 + size.X-thickness/2;
            float y1 = position.Y+thickness;
            float y2 = y1 + size.Y-thickness;
            DrawLine(spribaBatch, new Vector2(x1,y1), new Vector2(x2, y1),thickness,color);
            DrawLine(spribaBatch, new Vector2(x1, y1), new Vector2(x1, y2), thickness, color);
            DrawLine(spribaBatch, new Vector2(x2,y1), new Vector2(x2, y2), thickness, color);
            DrawLine(spribaBatch, new Vector2(x1,y2-thickness), new Vector2(x2, y2-thickness), thickness, color);
        }

        public static void DrawFilledRectangle(SpriteBatch spriteBatch, Vector2 position, Vector2 size,Color color)
        {
            float x1 = position.X + 1;
            float x2 = position.X + size.X - (1 / 2f);
            float y1 = position.Y;
            float y2 = position.Y + size.Y - 1;
            DrawLine(spriteBatch, new Vector2(x1, y1), new Vector2(x2, y1), 1, color);
            DrawLine(spriteBatch, new Vector2(x1, y1), new Vector2(x1, y2 + 1 / 2f), 1, color);
            DrawLine(spriteBatch, new Vector2((x1 + size.X-1),y1), new Vector2((x1 + size.X-1), y2), x2 - x1, color);
            DrawLine(spriteBatch, new Vector2(x2, y1), new Vector2(x2, y2), 1, color);
            DrawLine(spriteBatch, new Vector2(x1, y2 - 1 / 2f), new Vector2(x2, y2 - 1 / 2f), 1, color);
        }
    }
}

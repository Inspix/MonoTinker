using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTinker.Code.Components.UI;

namespace MonoTinker.Code.Utils
{
    public static class TextHelper
    {
        public static void LineByLineEffect(ref SpriteFont font, ref List<Text> labels, ref int currentIndex, int maxlines,Vector2 charSize, ref string[] text, Vector2 offset = default(Vector2))
        {
            labels.Clear();
            if (currentIndex + maxlines > text.Length)
            {
                maxlines = text.Length - currentIndex;
            }
            int row = 0;
            for (int i = currentIndex; i < currentIndex + maxlines; i++)
            {
                Text x = new Text(font, (Vector2.One * 10 + offset) + Vector2.UnitY * charSize.Y * row, text[i], 0, false);
                x.Transitioning = true;
                labels.Add(x);
                row++;

            }
            currentIndex += maxlines;
        }

        public static void CharacterByCharacterEffect(ref SpriteFont font, ref List<Text> labels, ref int currentIndex, int maxlines, Vector2 charSize, ref string[] text, Vector2 offset = default(Vector2))
        {
            labels.Clear();
            if (currentIndex + maxlines > text.Length)
            {
                maxlines = text.Length - currentIndex;
            }
            int row = 0;
            for (int i = currentIndex; i < currentIndex + maxlines; i++)
            {
                Text x = new Text(font, (Vector2.One * 10 + offset) + Vector2.UnitY * charSize.Y * row, text[i]);
                x.IsVisible = true;
                labels.Add(x);
                row++;

            }
            currentIndex += maxlines;
        }

        public static void WholeAtOnceEffect(ref SpriteFont font, ref List<Text> labels, ref int currentIndex, int maxlines, ref string[] text, Vector2 offset = default(Vector2))
        {
            labels.Clear();
            string output = String.Join("\n",
                text.Skip(currentIndex)
                    .Take(currentIndex > text.Length
                    ? text.Length - currentIndex
                    : maxlines));
            Text x = new Text(font, Vector2.One * 10+offset, output, 0);
            x.IsVisible = false;
            x.Transitioning = true;
            x.FadeSpeed = 2;
            labels.Add(x);
            currentIndex += maxlines;
        }
    }
}

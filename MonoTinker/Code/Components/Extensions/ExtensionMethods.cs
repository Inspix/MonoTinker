using System;

namespace MonoTinker.Code.Components.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    using Elements;
    using Utils;

    using Microsoft.Xna.Framework;

    public static class ExtensionMethods
    {

        public static IEnumerable<string> SplitToPieces(this string str, int pieceSize)
        {
            if (pieceSize < 1)
            {
                Debug.Warning("Split to Pieces fail: Invalid piece size: {0}", pieceSize);
                yield break;
            }
            if (string.IsNullOrWhiteSpace(str))
            {
                Debug.Warning("Trying to split empty or null string to pieces");
                yield break;
            }
            int index = 0;
            while (true)
            {
                if (index + pieceSize > str.Length)
                {
                    pieceSize = str.Length - index;
                }
                if (index >= str.Length) yield break;
                char[] main = str.Skip(index).Take(pieceSize).ToArray();
                char[] after = str.Skip(main.Length + index).TakeWhile(s => !char.IsWhiteSpace(s)).ToArray();
                string output = new string(main) + new string(after);
                index = output.Length + index;
                yield return output.Trim(); 
            }
        }



        #region BoxExtensions
        private const float Margin = 2.5f;

        public static Rectangle InflateExt(this Rectangle rect, Vector2 amount, bool debug = false)
        {
            rect.Location -= (amount / 2f).ToPoint();
            rect.Size += amount.ToPoint();
            if (debug)
            {
                Debug.Warning(rect.ToString());

            }
            return rect;
        }

        public static Rectangle OffsetExt(this Rectangle rect, Vector2 offset)
        {
            rect.Location += offset.ToPoint();
            return rect;
        }

        public static bool TouchesTop(this BoxCollider b1, BoxCollider b2)
        {
            bool result = (b1.Bottom >= b2.Top - 1 &&
                           b1.Bottom <= b2.Top + Margin &&
                           b1.Right >= b2.Left + (b2.Width / 5) &&
                           b1.Left <= b2.Right - (b2.Width / 2));
#if DEBUG
            if (result)
            {
                Console.WriteLine("Collision Top");
                Console.WriteLine("B1 Top: {0} - B2 Top {1}",b1.Top,b2.Top);
                Console.WriteLine("B1 Bottom: {0} - B2 Bottom {1}", b1.Bottom, b2.Bottom);
                Console.WriteLine("B1 Left: {0} - B2 Left {1}", b1.Left, b2.Left);
                Console.WriteLine("B1 Right: {0} - B2 Right {1}", b1.Right, b2.Right);

            }
#endif
            return result;
        }

        public static bool TouchesBottom(this BoxCollider b1, BoxCollider b2)
        {
            bool result = (b1.Top <= b2.Bottom + Margin &&
                    b1.Top >= b2.Bottom - 1 &&
                    b1.Right >= b2.Left + (b2.Width / 5) &&
                    b1.Left <= b2.Right - (b2.Width / 2));
#if DEBUG
            if (result)
            {
                Console.WriteLine("Collision Bottom");
                Console.WriteLine("B1 Top: {0} - B2 Top {1}", b1.Top, b2.Top);
                Console.WriteLine("B1 Bottom: {0} - B2 Bottom {1}", b1.Bottom, b2.Bottom);
                Console.WriteLine("B1 Left: {0} - B2 Left {1}", b1.Left, b2.Left);
                Console.WriteLine("B1 Right: {0} - B2 Right {1}", b1.Right, b2.Right);
            }
#endif
            return result;
        }

        public static bool TouchesRight(this BoxCollider b1, BoxCollider b2)
        {
            bool result = (b1.Left >= b2.Left &&
                    b1.Left <= b2.Right + Margin &&
                    b1.Top <= b2.Bottom - (b2.Width / 4f) &&
                    b1.Bottom >= b2.Top + (b2.Width / 4f));
#if DEBUG
            if (result)
            {
                Console.WriteLine("Collision Right");
                Console.WriteLine("B1 Top: {0} - B2 Top {1}", b1.Top, b2.Top);
                Console.WriteLine("B1 Bottom: {0} - B2 Bottom {1}", b1.Bottom, b2.Bottom);
                Console.WriteLine("B1 Left: {0} - B2 Left {1}", b1.Left, b2.Left);
                Console.WriteLine("B1 Right: {0} - B2 Right {1}", b1.Right, b2.Right);
            }
#endif
            return result;
        }

        public static bool TouchesLeft(this BoxCollider b1, BoxCollider b2)
        {
            bool result = (b1.Right <= b2.Right &&
                    b1.Right >= b2.Left - Margin &&
                    b1.Top <= b2.Bottom - (b2.Width / 4f) &&
                    b1.Bottom >= b2.Top + (b2.Width / 4f));
#if DEBUG
            if (result)
            {
                Console.WriteLine("Collision Left");
                Console.WriteLine("B1 Top: {0} - B2 Top {1}", b1.Top, b2.Top);
                Console.WriteLine("B1 Bottom: {0} - B2 Bottom {1}", b1.Bottom, b2.Bottom);
                Console.WriteLine("B1 Left: {0} - B2 Left {1}", b1.Left, b2.Left);
                Console.WriteLine("B1 Right: {0} - B2 Right {1}", b1.Right, b2.Right);
            }
#endif
            return result;
        }

        public static bool Touches(this BoxCollider b1, BoxCollider b2)
        {
            return b1.TouchesBottom(b2) || b1.TouchesTop(b2) || b1.TouchesLeft(b2) || b1.TouchesRight(b2);
        } 
        #endregion
    }




}

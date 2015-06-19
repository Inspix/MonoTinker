namespace MonoTinker.Code.Components.Extensions
{
    internal static class BoxColliderExtension
    {
        private const float Margin = 2.5f;

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
    }
}
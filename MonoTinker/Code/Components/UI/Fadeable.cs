namespace MonoTinker.Code.Components.UI
{
    using Utils;

    using Microsoft.Xna.Framework;

    public abstract class Fadeable
    {
        protected bool isVisible = true;
        protected Color alpha = Color.White;
        protected int defaultAlpha = 255;
        protected bool fadeIn;
        protected bool fadeOut;
        protected bool transitioning;
        protected int fadeSpeed = 5;

        public bool Transitioning
        {
            set
            {
                this.transitioning = value;
            }
        }

        public int FadeSpeed
        {
            get
            {
                return this.fadeSpeed;
            }
            set
            {
                if (value > 100)
                {
                    this.fadeSpeed = 100;
                }
                else if (value <= 0)
                {
                    this.fadeSpeed = 1;
                }
                else
                {
                    this.fadeSpeed = value;
                }
            }
        }

        public bool IsVisible
        {
            get
            {
                return this.isVisible;
            }
            set
            {
                if (this.fadeIn || this.fadeOut)
                {
                    return;
                }
                if (this.transitioning)
                {
                    if (value)
                    {
                        this.fadeIn = true;
                        this.isVisible = true;
                    }
                    else
                    {
                        this.fadeOut = true;
                    }
                }
                else
                {
                    this.isVisible = value;
                }
            }
        }

        protected virtual void Transition()
        {
            if (this.fadeIn)
            {
                this.Alpha += this.fadeSpeed;
                if (this.Alpha >= this.defaultAlpha)
                {
                    this.Alpha = this.defaultAlpha;
                    this.fadeIn = false;
                }
            }
            if (this.fadeOut)
            {
                this.Alpha -= this.fadeSpeed;
                if (this.Alpha <= 0)
                {
                    this.fadeOut = false;
                    this.isVisible = false;
                }
            }
        }

        public int DefaultAlpha
        {
            get
            {
                return this.defaultAlpha;
            }
            set
            {
                if (value > 255)
                {
                    this.defaultAlpha = 255;

                }
                else if (value < 0)
                {
                    this.defaultAlpha = 0;
                }
                else
                {
                    this.defaultAlpha = value;
                }

            }
        }

        protected int Alpha
        {
            get
            {
                return this.alpha.A;
            }
            set
            {
                this.alpha = ColorHelper.AlphaChange(this.alpha, value);
            }
        }
    }
}

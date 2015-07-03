namespace MonoTinker.Code.Components.UI
{
    using Utils;

    using Microsoft.Xna.Framework;

    public abstract class Fadeable
    {
        protected bool isVisible = true;
        protected float alpha = 1f;
        protected float defaultAlpha = 1f;
        protected bool fadeIn;
        protected bool fadeOut;
        protected bool transitioning;
        protected float fadeSpeed = 0.1f;

        #region Properties
        public bool Transitioning
        {
            set
            {
                this.transitioning = value;
            }
        }

        public float FadeSpeed
        {
            get
            {
                return this.fadeSpeed;
            }
            set
            {
                if (value > 1)
                {
                    this.fadeSpeed = 1;
                }
                else if (value <= 0.004f)
                {
                    this.fadeSpeed = 0.004f;
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
                if (this.transitioning)
                {
                    if (value)
                    {
                        this.fadeIn = true;
                        this.fadeOut = false;
                        this.isVisible = true;
                    }
                    else
                    {
                        this.fadeIn = false;
                        this.fadeOut = true;
                    }
                }
                else
                {
                    this.isVisible = value;
                }
            }
        }

        public float DefaultAlpha
        {
            get
            {
                return this.defaultAlpha;
            }
            set { this.defaultAlpha = MathHelper.Clamp(value, 0f, 1f); }
        }

        protected float Alpha
        {
            get
            {
                return this.alpha;
            }
            set
            {
                this.alpha = MathHelper.Clamp(value, -0.001f, 1f);
            }
        } 
        #endregion

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
    }
}

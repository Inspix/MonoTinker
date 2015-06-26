

namespace MonoTinker.Code.Components.Elements
{
    using System;

    public class AnimationEventArgs : EventArgs
    {
        public string StateName { get; internal set; }

        public AnimationEventArgs(string statename)
        {
            this.StateName = statename;
        }
    }
}

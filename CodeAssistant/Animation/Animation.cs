using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;

namespace CodeAssistant.Animation
{
    public abstract class AnimationBase
    {
        protected Storyboard Storyboard
        {
            get;
            set;
        }

        public AnimationBase()
        {
            this.Storyboard = new Storyboard();
        }

        public AnimationBase(Storyboard storyboard)
        {
            this.Storyboard = storyboard;
        }

        public abstract void Begin();
    }

    public abstract class AnimationBase<TParent, TChild, TAnimation> : AnimationBase 
        where TParent : FrameworkElement
        where TAnimation : AnimationTimeline, new()
        where TChild : FrameworkElement
    {
        protected TParent Parent
        {
            get;
            private set;
        }

        protected TChild Child
        {
            get;
            private set;
        }

        protected TAnimation Animation
        {
            get;
            set;
        }

        protected abstract DependencyProperty Property { get; }

        public AnimationBase(TParent parent, TChild child)
        {
            this.Parent = parent;
            this.Child = child;
            this.Animation = CreateAnimation();

            this.Storyboard.Children.Add(this.Animation);
            SetTargetInfo(this.Property);
        }

        public override void Begin()
        {
            this.Storyboard.Begin(this.Parent);
        }

        protected virtual TAnimation CreateAnimation()
        {
            return new TAnimation();
        }

        protected virtual void SetTargetInfo(DependencyProperty property)
        {
            Storyboard.SetTargetName(this.Animation, this.Child.Name);
            Storyboard.SetTargetProperty(this.Animation, new PropertyPath(property));
        }
    }
}

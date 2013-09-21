using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows;

namespace CodeAssistant.Animation
{
    public class DropdownAnimation : AnimationBase<ContentControl, FrameworkElement, DoubleAnimation>
    {
        double initStart;
        double initEnd;
        bool isInit;

        public double StartValue
        {
            get
            {
                return this.Animation.From.GetValueOrDefault();
            }

            set
            {
                this.Animation.From = value;
            }
        }

        public double EndValue
        {
            get
            {
                return this.Animation.To.GetValueOrDefault();
            }

            set
            {
                this.Animation.To = value;
            }
        }

        public TimeSpan Duration
        {
            get
            {
                return this.Animation.Duration.TimeSpan;
            }

            set
            {
                this.Animation.Duration = new Duration(value);
            }
        }

        /// <summary>
        /// Toggle the animation back and forth each time the Begin() is called
        /// </summary>
        public bool Toggle
        {
            get;
            set;
        }

        public DropdownAnimation(
            ContentControl parent,
            FrameworkElement child)
            : base(parent, child)
        {
            this.isInit = true;
        }

        public DropdownAnimation(
            ContentControl parent,
            FrameworkElement child, 
            double start, 
            double end,
            TimeSpan duration)
            : this(parent, child)
        {
            this.StartValue = start;
            this.EndValue = end;
            this.Duration = duration;
        }

        protected override DependencyProperty Property
        {
            get { return FrameworkElement.HeightProperty; }
        }

        public void Reverse()
        {
            double from = this.StartValue;
            double to = this.EndValue;

            this.Animation.To = from;
            this.Animation.From = to;
            base.Begin();
        }

        public override void Begin()
        {
            if (this.Toggle)
            {
                if (this.isInit)
                {
                    base.Begin();
                }
                else
                {
                    Reverse();
                }
            }
            else
            {
                base.Begin();
            }

            if (this.isInit)
            {
                this.isInit = false;
                this.initEnd = this.EndValue;
                this.initStart = this.StartValue;
            }

        }

        /// <summary>
        /// Reset the animation to the initial settings
        /// </summary>
        public void Reset()
        {
            this.StartValue = initStart;
            this.EndValue = initEnd;
        }
    }
}

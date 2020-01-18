using Android.Content;
using Android.Support.V4.View;
using CustomRenderer.Droid.Renderers;
using System.ComponentModel;
using TakeMeThereXamarinForms.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ShadowFrame), typeof(ShadowFrameRenderer))]

namespace CustomRenderer.Droid.Renderers
{
    public class ShadowFrameRenderer : Xamarin.Forms.Platform.Android.AppCompat.FrameRenderer
    {
        public ShadowFrameRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement == null)
            {
                return;
            }

            UpdateElevation();
        }

        private void UpdateElevation()
        {
            var shadowFrame = (ShadowFrame)Element;

            Control.StateListAnimator = new Android.Animation.StateListAnimator();

            ViewCompat.SetElevation(this, shadowFrame.Elevation);
            ViewCompat.SetElevation(Control, shadowFrame.Elevation);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == "Elevation")
            {
                UpdateElevation();
            }
        }
    }
}
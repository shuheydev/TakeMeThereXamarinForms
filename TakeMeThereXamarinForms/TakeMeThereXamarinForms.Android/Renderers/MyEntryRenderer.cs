using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TakeMeThereXamarinForms.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly:ExportRenderer(typeof(Entry),typeof(MyEntryRenderer))]
namespace TakeMeThereXamarinForms.Droid.Renderers
{
    class MyEntryRenderer : EntryRenderer
    {
        public MyEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control == null || e.NewElement == null) return;

            if(Build.VERSION.SdkInt>=BuildVersionCodes.Lollipop)
            {
                //下線の色を変更
                var color = Color.FromHex("#9BFFE6");
                Control.BackgroundTintList = ColorStateList.ValueOf(color.ToAndroid());

                //Styles.xmlの方で指定
                //カーソル,ツマミの色はstyles.xmlで指定している.

                ////カーソル(縦棒の部分)の色を変更
                //IntPtr IntPtrtextViewClass = JNIEnv.FindClass(typeof(TextView));
                //IntPtr mCursorDrawableResProperty = JNIEnv.GetFieldID(IntPtrtextViewClass, "mCursorDrawableRes", "I");
                //JNIEnv.SetField(Control.Handle, mCursorDrawableResProperty, ); // replace 0 with a Resource.Drawable.my_cursor
            }
        }
    }
}

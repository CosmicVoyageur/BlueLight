using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using BlueLight.CustomViews;
using BlueLight.Droid.Render;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(MyCustomView), typeof(CustomViewRenderer))]

namespace BlueLight.Droid.Render
{
    public class CustomViewRenderer : ViewRenderer<MyCustomView,MyAndroidView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<MyCustomView> e)
        {
            base.OnElementChanged(e);
            var something = new MyAndroidView(Forms.Context);
            something.SetCol();
            SetNativeControl(something);
        }

    }

    public class MyAndroidView : View
    {
        protected MyAndroidView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            
        }

        public MyAndroidView(Context context) : base(context)
        {
            
        }

        public MyAndroidView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public MyAndroidView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        public MyAndroidView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        public void SetCol()
        {
            SetBackgroundColor(Color.Blue.ToAndroid());
        }

    }
}
﻿using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace PiggyBank_MAUI
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Establece el color de la barra de estado
            Window.SetStatusBarColor(Android.Graphics.Color.ParseColor("#00986C")); 
        }

    }
}

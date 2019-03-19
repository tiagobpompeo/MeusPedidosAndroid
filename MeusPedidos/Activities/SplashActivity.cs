using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Android.Media;
using MeusPedidos.Activities;

namespace Pedidos.Droid
{
    [Activity(Label = "Meus Pedidos", Icon = "@mipmap/iconlauncher", Theme = "@style/SplashTheme",
              MainLauncher = true)]
    public class SplashActivity:AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var intent = new Intent(Application.Context, typeof(MainActivity));
            StartActivity(intent);
            Finish();

        }
    }
}

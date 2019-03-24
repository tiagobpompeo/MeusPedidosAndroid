using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using MeusPedidos.Helpers;
using MeusPedidos.Fragments;
using System;
using Android.Support.Design.Widget;

namespace MeusPedidos.Activities
{
    [Activity(Label = "Catálogo", MainLauncher = false, Icon = "@mipmap/iconlauncher")]
    public class MainActivity : Android.Support.V7.App.AppCompatActivity
    {
        DrawerLayout drawerLayout;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Akavache.Registrations.Start("Pedidos");
            SetContentView(Resource.Layout.Main);
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            base.SetSupportActionBar(toolbar);
            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawerLayout);
            var drawerToggle = new ActionBarDrawerToggle(this, drawerLayout, toolbar, Resource.String.drawer_open, Resource.String.drawer_close);
            drawerLayout.AddDrawerListener(drawerToggle);
            drawerToggle.SyncState();
            var menu = FindViewById<Android.Support.Design.Widget.NavigationView>(Resource.Id.navigationView);
            menu.NavigationItemSelected += OnMenuItemSelected;
            Navigate(new HomeFragment());
            //Navigate(new HomeFragmentRecicler());
        }

        //nao habilitar pois categoria fragmento perde o clique
        //public override bool OnOptionsItemSelected(IMenuItem item)
        //{
        //    switch (item.ItemId)
        //    {
        //        case Android.Resource.Id.Home:
        //            var drawerLayout = FindViewById<Android.Support.V4.Widget.DrawerLayout>(Resource.Id.drawerLayout);
        //            drawerLayout.OpenDrawer(Android.Support.V4.View.GravityCompat.Start);
        //            break;
        //    }

        //    return true;
        //}

        void OnMenuItemSelected(object sender, Android.Support.Design.Widget.NavigationView.NavigationItemSelectedEventArgs e)
        {
            switch (e.MenuItem.ItemId)
            {
                case Resource.Id.nav_home: Navigate(new HomeFragment()); break;
                case Resource.Id.nav_cart: Navigate(new CartFragment()); break;
                case Resource.Id.nav_aboutApp: Navigate(new AboutAppFragment()); break;
            }

            e.MenuItem.SetChecked(true);

            var drawerLayout = FindViewById<Android.Support.V4.Widget.DrawerLayout>(Resource.Id.drawerLayout);
            drawerLayout.CloseDrawer(Android.Support.V4.View.GravityCompat.Start);
        }

        void Navigate(Android.Support.V4.App.Fragment fragment)
        {
            var transaction = base.SupportFragmentManager.BeginTransaction();
            transaction.Replace(Resource.Id.contentFrame, fragment);
            transaction.Commit();
        }

    }
}


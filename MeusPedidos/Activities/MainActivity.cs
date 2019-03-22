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

namespace MeusPedidos.Activities
{
    [Activity(Label = "Meus Pedidos", MainLauncher = false, Icon = "@mipmap/iconlauncher")]
    public class MainActivity : BaseActivity
    {
        #region Attributes and Properties
        private MyActionBarDrawerToggle drawerToggle;
        private string drawerTitle;
        private string title;
        private DrawerLayout drawerLayout;
        private ListView drawerListView;
        #endregion

        private static readonly string[] Sections = new[] {
            "Home", "Profile", "Sobre o App","Browser", "Carrinho"
        };


        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.Main;
            }
        }



        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
           
            Akavache.Registrations.Start("Pedidos");
           
            this.title = this.drawerTitle = this.Title;
            this.drawerLayout = this.FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            this.drawerListView = this.FindViewById<ListView>(Resource.Id.left_drawer);

            //Create Adapter for drawer List
            this.drawerListView.Adapter = new ArrayAdapter<string>(this, Resource.Layout.item_menu, Sections);

            //Set click handler when item is selected
            this.drawerListView.ItemClick += (sender, args) => ListItemClicked(args.Position);

            //Set Drawer Shadow
            this.drawerLayout.SetDrawerShadow(Resource.Mipmap.drawer_shadow_dark, (int)GravityFlags.Start);

                       
            //DrawerToggle is the animation that happens with the indicator next to the actionbar
            this.drawerToggle = new MyActionBarDrawerToggle(this, this.drawerLayout,this.Toolbar,Resource.String.drawer_open,Resource.String.drawer_close);

            //Display the current fragments title and update the options menu
            this.drawerToggle.DrawerClosed += (o, args) => {
                this.SupportActionBar.Title = this.title;
                this.InvalidateOptionsMenu();
            };

            //Display the drawer title and update the options menu
            this.drawerToggle.DrawerOpened += (o, args) => {
                this.SupportActionBar.Title = this.drawerTitle;
                this.InvalidateOptionsMenu();
            };

            //Set the drawer lister to be the toggle.
            this.drawerLayout.SetDrawerListener(this.drawerToggle);
            //if first time you will want to go ahead and click first item.
            if (savedInstanceState == null)
            {
                ListItemClicked(0);
            }
        }

              
        private void ListItemClicked(int position)
        {
            Android.Support.V4.App.Fragment fragment = null;

            switch (position)
            {
                case 0:
                    fragment = new HomeFragment();//browse
                    break;
                case 1:
                    fragment = new ProfileFragment();//friends
                    break;
                case 2:
                    fragment = new AboutAppFragment();//profile
                    break;
                case 3:
                    fragment = new BrowseFragment();//browse problema com a key duplicada depois retirar
                    break;
                case 4:
                    fragment = new CartFragment();//carrinho
                    break;
                
            }

            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.content_frame, fragment)
                .Commit();

            this.drawerListView.SetItemChecked(position, true);
            SupportActionBar.Title = this.title = Sections[position];
            this.drawerLayout.CloseDrawers();
        }
    }
}


using System;
using System.Collections.Generic;
using Android.Content;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using MeusPedidos.Activities;
using MeusPedidos.Adapters;
using MeusPedidos.Models;

namespace MeusPedidos.Fragments
{
    public class HomeFragment : Fragment
    {

        public HomeFragment()
        {
            this.RetainInstance = true;
        }

        private List<FriendViewModel> _friends;
        public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            this.HasOptionsMenu = true;
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.fragment_home, null);//fragment browser

            var grid = view.FindViewById<GridView>(Resource.Id.grid);
            _friends = Util.GenerateFriends();
            grid.Adapter = new MonkeyAdapter(Activity, _friends);
            grid.ItemClick += GridOnItemClick;
            return view;
        }

        private void GridOnItemClick(object sender, AdapterView.ItemClickEventArgs itemClickEventArgs)
        {
            var intent = new Intent(Activity, typeof(FriendActivity));
            intent.PutExtra("Title", _friends[itemClickEventArgs.Position].Title);
            intent.PutExtra("Image", _friends[itemClickEventArgs.Position].Image);
            StartActivity(intent);
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.refresh, menu);
        }
    }
}
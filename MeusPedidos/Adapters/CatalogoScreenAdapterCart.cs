using System;
using System.Collections.Generic;
using System.Net;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using MeusPedidos.Contracts;
using MeusPedidos.Models.Sqlite;
using MeusPedidos.Services.ConnectionService;

namespace MeusPedidos.Adapters
{
    public class CatalogoScreenAdapterCart : BaseAdapter<ListShop>
    {
        #region Properties and Attributes
        List<ListShop> items;
        Activity context;
        public IConnectionService _connection;
        #endregion

        #region Constructor
        public CatalogoScreenAdapterCart(Activity context, List<ListShop> items)
            : base()
        {
            this.context = context;
            this.items = items;
            _connection = new ConnectionService();
        }
        #endregion

        #region Methods
        public override long GetItemId(int position)
        {
            return position;
        }
        public override ListShop this[int position]
        {
            get { return items[position]; }
        }
        public override int Count
        {
            get { return items.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];

            View view = convertView;
            if (view == null) // no view to re-use, create new
                view = context.LayoutInflater.Inflate(Resource.Layout.CustomView, null);
            view.FindViewById<TextView>(Resource.Id.Text1).Text = item.Name;
            view.FindViewById<TextView>(Resource.Id.Text2).Text = "-20%";
            view.FindViewById<TextView>(Resource.Id.Text3).Text = "R$ " + item.Price.ToString();         

            Android.Net.ConnectivityManager conn = (Android.Net.ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);
            bool isConnected = conn.ActiveNetworkInfo != null && conn.ActiveNetworkInfo.IsConnected;

            if (!isConnected)
            {
                view.FindViewById<ImageView>(Resource.Id.Image).SetImageResource(Resource.Drawable.nophotoproduto);
            }
            else
            {
                if (item.Image != null)
                {
                    var imageBitmap = GetImageBitmapFromUrl(item.Image.ToString());
                    view.FindViewById<ImageView>(Resource.Id.Image).SetImageBitmap(imageBitmap);
                }
            }
            return view;
        }


        public void SendMessage(View view)
        {
            // Do something in response to button click
        }


        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }
        #endregion
    }
}
using System;
using System.Collections.ObjectModel;
using System.Net;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using MeusPedidos.Activities;
using MeusPedidos.Models.Sqlite;

namespace MeusPedidos.Adapters
{
    public class CartAdapterScreen : BaseAdapter<ListShop>
    {
        #region Properties and Attributes  
        private CartActivity cartActivity;
        private ObservableCollection<ListShop> listShop;
        #endregion

        #region Constructor     
        public CartAdapterScreen(CartActivity cartActivity, ObservableCollection<ListShop> listShop)
        {
            this.cartActivity = cartActivity;
            this.listShop = listShop;
        }
        #endregion


        #region Methods
        public override long GetItemId(int position)
        {
            return position;
        }
        public override ListShop this[int position]
        {
            get { return this.listShop[position]; }
        }
        public override int Count
        {
            get { return this.listShop.Count; }
        }

        #endregion

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = this.listShop[position];

            View view = convertView;
            if (view == null) // no view to re-use, create new
                view = this.cartActivity.LayoutInflater.Inflate(Resource.Layout.CustomView, null);
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

    }
}

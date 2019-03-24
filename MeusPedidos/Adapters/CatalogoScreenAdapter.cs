using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MeusPedidos.Activities;
using MeusPedidos.Contracts;
using MeusPedidos.Models;
using MeusPedidos.Services.ConnectionService;

namespace MeusPedidos
{
    public class CatalogoScreenAdapter : BaseAdapter<Products>
    {
        #region Properties
        List<Products> items;
        Activity context;
        public IConnectionService _connection;
        #endregion

        #region Constructor
        public CatalogoScreenAdapter(Activity context, List<Products> items)
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
        public override Products this[int position]
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
            view.FindViewById<TextView>(Resource.Id.Text3).Text = item.Price.ToString();

            var btnInc =  view.FindViewById<Button>(Resource.Id.increase);
            var btnDec =  view.FindViewById<Button>(Resource.Id.decrease);
            var imgFav =  view.FindViewById<ImageView>(Resource.Id.favorite);

            btnInc.Click+=(sender, e) => { 
                Console.WriteLine("Increment");            
            };

            btnDec.Click += (sender, e) => { 
                Console.WriteLine("Decrement"); 
            };

            imgFav.Click += (sender, e) => { 
                Console.WriteLine("Favorite");
            };

            view.Click += (sender, e) => 
            {
               
                              
            };

           

            Android.Net.ConnectivityManager conn = (Android.Net.ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);
            bool isConnected = conn.ActiveNetworkInfo != null && conn.ActiveNetworkInfo.IsConnected;

            if (!isConnected)
            {
                view.FindViewById<ImageView>(Resource.Id.Image).SetImageResource(Resource.Drawable.nophotoproduto);
            }
            else 
            {
                if (item.Photo != null)
                {
                    var imageBitmap = GetImageBitmapFromUrl(item.Photo.ToString());
                    view.FindViewById<ImageView>(Resource.Id.Image).SetImageBitmap(imageBitmap);
                }
                //else
                //{
                //    view.FindViewById<ImageView>(Resource.Id.Image).SetImageResource(Resource.Mipmap.ic_launcher);
                //}
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
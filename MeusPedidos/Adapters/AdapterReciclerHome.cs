using System;
using System.Collections.Generic;
using System.Net;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MeusPedidos.Contracts;
using MeusPedidos.Models;
using MeusPedidos.Services.ConnectionService;

namespace MeusPedidos.Adapters
{
    public class AdapterReciclerHome :  RecyclerView.Adapter
    {
        #region Properties and Attributes  
        private List<Products> tableItems;
        #endregion

        public AdapterReciclerHome(List<Products> tableItems)
        {
            this.tableItems = tableItems;           
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.CustomViewHome, parent, false);
            PhotoViewHolderHome vh = new PhotoViewHolderHome(itemView, OnClick);

            var decrease = itemView.FindViewById<Button>(Resource.Id.decreaseHome);
            var increase = itemView.FindViewById<Button>(Resource.Id.increaseHome);
            var favorite = itemView.FindViewById<ImageButton>(Resource.Id.favoriteHome);

            decrease.Click += (sender, e) =>
            {
            };
            increase.Click += (sender, e) =>
            {
            };
            decrease.Click += (sender, e) =>
            {
            };

            return vh;
        }


        public override int ItemCount
        {
            get { return this.tableItems.Count; }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            PhotoViewHolderHome vh = holder as PhotoViewHolderHome;
            Android.Net.ConnectivityManager conn = (Android.Net.ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);
            bool isConnected = conn.ActiveNetworkInfo != null && conn.ActiveNetworkInfo.IsConnected;

            if (!isConnected)
            {
                vh.ImageHome.SetImageResource(Resource.Mipmap.ic_launcher);
            }
            else
            {
                if (this.tableItems[position].Photo != null)
                {
                    var imageBitmap = GetImageBitmapFromUrl(this.tableItems[position].Photo.ToString());
                    vh.ImageHome.SetImageBitmap(imageBitmap);
                }
            }

            vh.TextHome1.Text = this.tableItems[position].Name;
            //vh.TextHome2.Text = this.tableItems[position].Discount = "-20%";
            vh.TextHome3.Text = this.tableItems[position].Price.ToString();//price
            //vh.TextHome4.Text = this.tableItems[position].Price.ToString();//unidade count
            //vh.DescriptionHome.Text = this.tableItems[position].Description;//nao usado nesta page
        }

       
        private void OnClick(int obj){}

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

    public class PhotoViewHolderHome : RecyclerView.ViewHolder
    {
        public ImageView ImageHome { get; private set; }
        public TextView TextHome1 { get; private set; }
        public TextView TextHome2 { get; private set; }
        public TextView TextHome3 { get; private set; }
        public Button DecreaseHome { get; private set; }
        public Button IncreaseHome { get; private set; }
        public TextView TextHome4 { get; private set; }
        public ImageButton Favorite { get; private set; }
        public TextView DescriptionHome { get; private set; }


        public PhotoViewHolderHome(View itemView, Action<int> listener) : base(itemView)
        {
            ImageHome= itemView.FindViewById<ImageView>(Resource.Id.ImageHome);
            TextHome1 = itemView.FindViewById<TextView>(Resource.Id.TextHome1);
            TextHome2 = itemView.FindViewById<TextView>(Resource.Id.TextHome2);
            TextHome3 = itemView.FindViewById<TextView>(Resource.Id.TextHome3);
            DecreaseHome = itemView.FindViewById<Button>(Resource.Id.decreaseHome);
            IncreaseHome = itemView.FindViewById<Button>(Resource.Id.increaseHome);
            TextHome4 = itemView.FindViewById<TextView>(Resource.Id.TextHome4);
            Favorite = itemView.FindViewById<ImageButton>(Resource.Id.favoriteHome);
            DescriptionHome = itemView.FindViewById<TextView>(Resource.Id.textView3);           
            itemView.Click += (sender, e) => listener(base.LayoutPosition);
        }
    }


}

using System;
using System.Collections.Generic;
using System.Net;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MeusPedidos.Activities;
using MeusPedidos.Models;

namespace MeusPedidos.Adapters
{
    public class AdapterReciclerDetail : RecyclerView.Adapter
    {
        private readonly HomeActivityDetail homeActivityDetail;
        private readonly List<Products> tableItems;

        public AdapterReciclerDetail(HomeActivityDetail homeActivityDetail, List<Products> tableItems)
        {
            this.homeActivityDetail = homeActivityDetail;
            this.tableItems = tableItems;
        }

        // Return the number of photos available in the photo album:
        public override int ItemCount
        {
            get { return this.tableItems.Count; }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            PhotoViewHolder vh = holder as PhotoViewHolder;
            Android.Net.ConnectivityManager conn = (Android.Net.ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);
            bool isConnected = conn.ActiveNetworkInfo != null && conn.ActiveNetworkInfo.IsConnected;
          
            if (!isConnected)
            {
                vh.Image.SetImageResource(Resource.Mipmap.ic_launcher);
            }
            else
            {
                if (this.tableItems[position].Photo != null)
                {
                    var imageBitmap = GetImageBitmapFromUrl(this.tableItems[position].Photo.ToString());
                    vh.Image.SetImageBitmap(imageBitmap);
                }

            }

            vh.Caption.Text = this.tableItems[position].Name;
        }

        public override RecyclerView.ViewHolder
            OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            // Inflate the CardView for the photo:
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.PhotoCardView, parent, false);

            // Create a ViewHolder to find and hold these view references, and 
            // register OnClick with the view holder:
            PhotoViewHolder vh = new PhotoViewHolder(itemView, OnClick);
            return vh;
        }

        private void OnClick(int obj)
        {

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


    public class PhotoViewHolder : RecyclerView.ViewHolder
    {
        public ImageView Image { get; private set; }
        public TextView Caption { get; private set; }

        // Get references to the views defined in the CardView layout.
        public PhotoViewHolder(View itemView, Action<int> listener)
            : base(itemView)
        {
            // Locate and cache view references:
            Image = itemView.FindViewById<ImageView>(Resource.Id.imageView);
            Caption = itemView.FindViewById<TextView>(Resource.Id.textView);

            // Detect user clicks on the item view and report which item
            // was clicked (by layout position) to the listener:
            itemView.Click += (sender, e) => listener(base.LayoutPosition);
        }
    }

   
}

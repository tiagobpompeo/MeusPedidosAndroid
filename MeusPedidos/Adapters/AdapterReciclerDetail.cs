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
using MeusPedidos.Models.Sqlite;

namespace MeusPedidos.Adapters
{
    public class AdapterReciclerDetail : RecyclerView.Adapter
    {
        #region Properties and Attributes
        private readonly HomeActivityDetail homeActivityDetail;
        private readonly List<Products> tableItems;
     
        #endregion

        public AdapterReciclerDetail(HomeActivityDetail homeActivityDetail, List<Products> tableItems)
        {
            this.homeActivityDetail = homeActivityDetail;
            this.tableItems = tableItems;
        }

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
            vh.Description.Text = this.tableItems[position].Description;
            vh.Price.Text = "R$ " + this.tableItems[position].Price.ToString();
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.PhotoCardView, parent, false);
            var btnInc = itemView.FindViewById<Button>(Resource.Id.button1);

            btnInc.Click += (sender, e) =>
            {
                //add to cart
                AlertDialog.Builder alertDiag = new AlertDialog.Builder(this.homeActivityDetail);
                alertDiag.SetTitle("Adiçāo ao Carrinho");
                alertDiag.SetMessage("Add Item para o Carrinho?");
                alertDiag.SetPositiveButton("Sim", (senderAlert, args) =>
                {
                    var data = tableItems[0];
                    SaveItemToCart(data);
                });
                alertDiag.SetNegativeButton("Cancelar", (senderAlert, args) =>
                {
                    alertDiag.Dispose();
                });
                Dialog diag = alertDiag.Create();
                diag.Show();
            };

            PhotoViewHolder vh = new PhotoViewHolder(itemView, OnClick);
            return vh;
        }

        private async void SaveItemToCart(Products data)
        {
            if (data != null)
            {
                int idInt = data.Id;
                var quantidade = 1;
                var dados = new ListShop
                {
                    //ID
                    IdProduct = idInt.ToString(),
                    Name = data.Name,
                    Quantity = quantidade.ToString(),
                    Image = data.Photo,
                    Price = data.Price.ToString()
                };
                await MainActivity.ShopRepository.AddNewProductAsync(dados);
                Toast.MakeText(Application.Context, "Produto adicionado", ToastLength.Short).Show();
            }
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

    public class PhotoViewHolder : RecyclerView.ViewHolder
    {
        public ImageView Image { get; private set; }
        public TextView Caption { get; private set; }
        public TextView Price { get; private set; }
        public TextView Description { get; private set; }

        public PhotoViewHolder(View itemView, Action<int> listener) : base(itemView)
        {
            Image = itemView.FindViewById<ImageView>(Resource.Id.imageView);
            Caption = itemView.FindViewById<TextView>(Resource.Id.textView1);
            Price = itemView.FindViewById<TextView>(Resource.Id.textView2);
            Description = itemView.FindViewById<TextView>(Resource.Id.textView3);
            itemView.Click += (sender, e) => listener(base.LayoutPosition);
        }
    }


}

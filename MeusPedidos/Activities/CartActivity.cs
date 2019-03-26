
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MeusPedidos.Adapters;
using MeusPedidos.Contracts;
using MeusPedidos.Contracts.Data;
using MeusPedidos.Models;
using MeusPedidos.Models.Sqlite;
using MeusPedidos.Services.BaseCacheService;
using MeusPedidos.Services.ConnectionService;
using Pedidos.Services.Data;

namespace MeusPedidos.Activities
{
    [Activity(Label = "Detail", ParentActivity = typeof(MainActivity))]
    [MetaData("android.support.PARENT_ACTIVITY", Value = "meuspedidos.activities.MainActivity")]
    public class CartActivity : BaseActivity
    {
        #region Properties and Attributes
        List<Products> tableItems = new List<Products>();
        public ICatalogDataService _catalogDataService;
        public IConnectionService _connection;
        BaseService baseService;
        ObservableCollection<ListShop> listShop;
        ListView list;
        #endregion

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.MyCart;
            }
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _catalogDataService = new CatalogDataService();
            _connection = new ConnectionService();
            baseService = new BaseService();

          
            list = FindViewById<ListView>(Resource.Id.List);
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarPrincipal);
            base.SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetTitle(Resource.String.carrinho);
            LoadListCart();
            // Create your application here
        }

        private async void LoadListCart()
        {
            listShop = new ObservableCollection<ListShop>(await MainActivity.ShopRepository.GetAllProductAsync());
            list.Adapter = new CartAdapterScreen(this, listShop);
        }
    }
}

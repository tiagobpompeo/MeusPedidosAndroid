using System;
using System.Collections.Generic;
using Android.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MeusPedidos.Adapters;
using MeusPedidos.Contracts;
using MeusPedidos.Contracts.Data;
using MeusPedidos.Models;
using MeusPedidos.Services.BaseCacheService;
using MeusPedidos.Services.ConnectionService;
using Pedidos.Services.Data;

namespace MeusPedidos.Activities
{
    [Activity(Label = "Detail", ParentActivity = typeof(MainActivity))]
    [MetaData("android.support.PARENT_ACTIVITY", Value = "meuspedidos.activities.MainActivity")]
    public class HomeActivityDetail : BaseActivity
    {
        RecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        List<Products> tableItems = new List<Products>();
        public ICatalogDataService _catalogDataService;
        public IConnectionService _connection;
        BaseService baseService;
        AdapterReciclerDetail mAdapter;
        Button btnCart;
        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.HomeDetail;
            }
        }

        protected override void OnCreate(Android.OS.Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _catalogDataService = new CatalogDataService();
            _connection = new ConnectionService();
            baseService = new BaseService();
            var idProduct = base.Intent.Extras.GetInt("id", -1);
            var nameProduct = base.Intent.Extras.GetString("name");
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbarPrincipal);
            base.SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            this.Title = nameProduct;
            SupportActionBar.Title = Title;
            //reciclerView na HomeDetail Layout
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);
            mLayoutManager = new LinearLayoutManager(this);
            mRecyclerView.SetLayoutManager(mLayoutManager);
            LoadDetailProduct(idProduct, nameProduct);

           

            //SupportActionBar.Title = nameProduct;
        }



        private async void LoadDetailProduct(int idProduct,string nameProduct)
        {
            var connection = await this._connection.CheckConnection();

            if (!connection.IsSuccess)
            {

            }
            else 
            {
                var products = await _catalogDataService.GetAllCatalogAsync();
                await baseService.InsertObject("nameProduct", products, DateTimeOffset.Now.AddMinutes(10));

                if (products != null)
                {
                    foreach (var be in products)
                    {
                        if (be.Id == idProduct)
                        {
                            tableItems.Add(new Products
                            {
                                Id = be.Id,
                                Price = be.Price,
                                Name = be.Name,
                                Description = be.Description,
                                Category_id = be.Category_id,
                                Photo = be.Photo
                            });

                        }
                    }

                    mAdapter = new AdapterReciclerDetail(this, tableItems);
                    mRecyclerView.SetAdapter(mAdapter);
                }
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            return base.OnOptionsItemSelected(item);
        }
    }
}

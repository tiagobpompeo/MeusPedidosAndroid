using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fragment = Android.Support.V4.App.Fragment;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using MeusPedidos.Services.BaseCacheService;
using MeusPedidos.Models;
using MeusPedidos.Contracts.Data;
using MeusPedidos.Contracts;
using Pedidos.Services.Data;
using MeusPedidos.Services.ConnectionService;

namespace MeusPedidos.Fragments
{
    public class CategoriesFragment : Fragment
    {
        #region Attributes and Properties
        ListView listView;
        BaseService baseService;
        TextView textView;
        List<Products> tableItems = new List<Products>();
        public ICatalogDataService _catalogDataService;
        public IConnectionService _connection;
        private int v1;
        private string v2;
        #endregion


        public CategoriesFragment(int v1, string v2)
        {
            this.v1 = v1;
            this.v2 = v2;
            _catalogDataService = new CatalogDataService();
            _connection = new ConnectionService();
            baseService = new BaseService();
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            this.HasOptionsMenu = true;
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.CategoriesProducts, null);
            textView = view.FindViewById<TextView>(Resource.Id.TextTitle);
            listView = view.FindViewById<ListView>(Resource.Id.ListCategory);
            textView.Text = this.v2;
            return base.OnCreateView(inflater, container, savedInstanceState);
        }

        public override async void OnActivityCreated(Bundle savedInstanceState)
        {

            var connection = await this._connection.CheckConnection();

            if (!connection.IsSuccess)
            {
                var categoryCached = await baseService.GetFromCache<List<Products>>(this.v2);
                if (categoryCached != null)
                {
                    foreach (var be in categoryCached)
                    {
                        if (be.Category_id == this.v1)
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
                        listView.Adapter = new CatalogoScreenAdapter(Activity, tableItems);
                        listView.ItemClick += OnListItemClick;
                    }
                    Toast.MakeText(Activity, "NoConnection ", ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(Activity, "NoConnection ", ToastLength.Short).Show();
                }
            }
            else
            {
                var categoryCached = await baseService.GetFromCache<List<Products>>(this.v2);
                if (categoryCached != null)
                {
                    foreach (var be in categoryCached)
                    {
                        if (be.Category_id == this.v1)
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
                        listView.Adapter = new CatalogoScreenAdapter(Activity, tableItems);
                        listView.ItemClick += OnListItemClick;
                    }
                }
                else
                {
                    var products = await _catalogDataService.GetAllCatalogAsync();
                    await baseService.InsertObject(this.v2, products, DateTimeOffset.Now.AddMinutes(10));
                    if (products != null)
                    {
                        foreach (var be in products)
                        {
                            if (be.Category_id == this.v1)
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
                            listView.Adapter = new CatalogoScreenAdapter(Activity, tableItems);
                            listView.ItemClick += OnListItemClick;
                        }
                    }
                    else
                    {
                        Toast.MakeText(Activity, "NoConnection Server ", ToastLength.Short).Show();
                    }
                }
            }
        }

        private void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            throw new NotImplementedException();
        }


        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.categorias_menu, menu);
        }
    }
}

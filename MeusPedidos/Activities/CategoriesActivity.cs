﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MeusPedidos.Contracts;
using MeusPedidos.Contracts.Data;
using MeusPedidos.Models;
using MeusPedidos.Services.BaseCacheService;
using MeusPedidos.Services.ConnectionService;
using Pedidos.Services.Data;

namespace MeusPedidos.Activities
{

    [Activity(Label = "Categories")]
    public class CategoriesActivity : Activity
    {
        #region Attributes and Properties
        ListView listView;
        BaseService baseService;
        TextView textView;
        List<Products> tableItems = new List<Products>();
        public ICatalogDataService _catalogDataService;
        public IConnectionService _connection;
        #endregion

        #region Methods
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CategoriesProducts);
            _catalogDataService = new CatalogDataService();
            _connection = new ConnectionService();
            baseService = new BaseService();
            var idCategoria = base.Intent.Extras.GetInt("id", -1);
            var nameCategoria = base.Intent.Extras.GetString("title");
            GetProductListCategory(idCategoria, nameCategoria);
            GetProductListCategoryByRecicle(idCategoria, nameCategoria);
        }

        private void GetProductListCategoryByRecicle(int idCategoria, string nameCategoria) { }

        private async void GetProductListCategory(int idCategoria, string nameCategoria)
        {
            textView = FindViewById<TextView>(Resource.Id.TextTitle);
            listView = FindViewById<ListView>(Resource.Id.ListCategory);
            textView.Text = nameCategoria;
            var connection = await this._connection.CheckConnection();

            if (!connection.IsSuccess)
            {
                var categoryCached = await baseService.GetFromCache<List<Products>>(nameCategoria);
                if (categoryCached != null)
                {
                    foreach (var be in categoryCached)
                    {
                        if (be.Category_id == idCategoria)
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
                        listView.Adapter = new CatalogoScreenAdapter(this, tableItems);
                        listView.ItemClick += OnListItemClick;
                    }
                    Toast.MakeText(this, "NoConnection ", ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(this, "NoConnection ", ToastLength.Short).Show();
                }
            }
            else
            {
                var categoryCached = await baseService.GetFromCache<List<Products>>(nameCategoria);
                if (categoryCached != null)
                {
                    foreach (var be in categoryCached)
                    {
                        if (be.Category_id == idCategoria)
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
                        listView.Adapter = new CatalogoScreenAdapter(this, tableItems);
                        listView.ItemClick += OnListItemClick;
                    }
                }
                else
                {
                    var products = await _catalogDataService.GetAllCatalogAsync();
                    await baseService.InsertObject(nameCategoria, products, DateTimeOffset.Now.AddMinutes(10));
                    if (products != null)
                    {
                        foreach (var be in products)
                        {
                            if (be.Category_id == idCategoria)
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
                            listView.Adapter = new CatalogoScreenAdapter(this, tableItems);
                            listView.ItemClick += OnListItemClick;
                        }
                    }
                    else
                    {
                        Toast.MakeText(this, "NoConnection Server ", ToastLength.Short).Show();
                    }
                }
            }
        }

        private void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var listView = sender as ListView;
            var t = tableItems[e.Position];
            Android.Widget.Toast.MakeText(this, t.Name, Android.Widget.ToastLength.Short).Show();
            Console.WriteLine("Clicked on " + t.Name);
        }
        #endregion
    }
}
using System;
using System.Collections.Generic;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using MeusPedidos.Activities;
using MeusPedidos.Adapters;
using MeusPedidos.Contracts;
using MeusPedidos.Contracts.Data;
using MeusPedidos.Models;
using MeusPedidos.Services.BaseCacheService;
using MeusPedidos.Services.ConnectionService;
using Pedidos.Services.Data;
using Fragment = Android.Support.V4.App.Fragment;


namespace MeusPedidos.Fragments
{
    public class HomeFragment : Fragment
    {

        List<Products> tableItems = new List<Products>();
        public ICatalogDataService _catalogDataService;
        public IConnectionService _connection;
        BaseService baseService;
        private ListView list;

        public HomeFragment()
        {
            this.RetainInstance = true;
            _catalogDataService = new CatalogDataService();
            _connection = new ConnectionService();
            baseService = new BaseService();
        }

       
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            this.HasOptionsMenu = true;
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.home_fragment, null);



            list = view.FindViewById<ListView>(Resource.Id.List);
            //logica retorna dados no OnActivityCreated
            list.ItemClick += OnListItemClick;
            return view;
        }


        public override async void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            var connection = await this._connection.CheckConnection();

            if (!connection.IsSuccess)
            {
                var catalogCached = await baseService.GetFromCache<List<Products>>("CatalogData");

                if (catalogCached != null)
                {
                    if (catalogCached != null)
                    {
                        foreach (var be in catalogCached)
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
                        list.Adapter = new CatalogoScreenAdapter(Activity, tableItems);
                    }                   
                }
            }
            else 
            {

                var catalogCached = await baseService.GetFromCache<List<Products>>("CatalogData");
                if (catalogCached != null)
                {
                    if (catalogCached != null)
                    {
                        foreach (var be in catalogCached)
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
                        list.Adapter = new CatalogoScreenAdapter(Activity, tableItems);
                    }                  
                   
                }
                else
                {
                    var products = await _catalogDataService.GetAllCatalogAsync();
                    await baseService.InsertObject("CatalogData", products, DateTimeOffset.Now.AddMinutes(10));

                    if (products != null)
                    {
                        foreach (var be in products)
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
                        list.Adapter = new CatalogoScreenAdapter(Activity, tableItems);
                    }
                }
            }
        }


        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.refresh, menu);
        }

        protected void OnListItemClick(object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
        {
            var listView = sender as ListView;
            var t = tableItems[e.Position];
            var intent = new Intent(Activity, typeof(HomeActivityDetail));
            intent.PutExtra("id", t.Id);
            intent.PutExtra("name",t.Name);
            StartActivity(intent);
        }
       
    }
}

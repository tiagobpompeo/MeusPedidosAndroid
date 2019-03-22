using System;
using System.Collections.Generic;
using Akavache;
using Android.App;
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
        #region Attributes and Properties
        List<Products> tableItems = new List<Products>();
        IEnumerable<Categories> categories = new List<Categories>();
        public ICatalogDataService _catalogDataService;
        public IConnectionService _connection;
        BaseService baseService;
        private ListView list;
        #endregion

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
            ProgressDialog progress = new ProgressDialog(Activity);
            progress.SetMessage("Wait while loading...");
            progress.SetCancelable(false); // disable dismiss by tapping outside of the dialog
            progress.Show();

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
            // To dismiss the dialog
            progress.Dismiss();
            progress.SetCancelable(true);
        }

        protected void OnListItemClick(object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
        {
            var listView = sender as ListView;
            var t = tableItems[e.Position];
            var intent = new Intent(Activity, typeof(HomeActivityDetail));
            intent.PutExtra("id", t.Id);
            intent.PutExtra("name", t.Name);
            StartActivity(intent);
        }


        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.categorias_menu, menu);
        }


        public override bool OnOptionsItemSelected(IMenuItem item)
        {
           
            switch (item.ItemId)
            {
                case Resource.Id.televisores:

                    var televisoresIntent = new Intent(Application.Context, typeof(CategoriesActivity));
                    televisoresIntent.PutExtra("id", 1);
                    televisoresIntent.PutExtra("title", "Televisores");
                    StartActivity(televisoresIntent);
                    return true;
                case Resource.Id.celulares:
                    var celularesIntent = new Intent(Application.Context, typeof(CategoriesActivity));
                    celularesIntent.PutExtra("id", 2);
                    celularesIntent.PutExtra("title", "Celulares");
                    StartActivity(celularesIntent);
                    return true;
                case Resource.Id.lavaroupas:
                    var lavaroupasIntent = new Intent(Application.Context, typeof(CategoriesActivity));
                    lavaroupasIntent.PutExtra("id", 3);
                    lavaroupasIntent.PutExtra("title", "Televisores");
                    StartActivity(lavaroupasIntent);
                    return true;
                case Resource.Id.notebooks:
                    var notebooksIntent = new Intent(Application.Context, typeof(CategoriesActivity));
                    notebooksIntent.PutExtra("id", 4);
                    notebooksIntent.PutExtra("title", "Notebooks");
                    StartActivity(notebooksIntent);
                    return true;
                case Resource.Id.cameras:
                    var camerasIntent = new Intent(Application.Context, typeof(CategoriesActivity));
                    camerasIntent.PutExtra("id", 5);
                    camerasIntent.PutExtra("title", "Cameras");
                    StartActivity(camerasIntent);
                    return true;
            }
            return true;
        }     


    }
}

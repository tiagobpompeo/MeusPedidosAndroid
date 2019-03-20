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

        List<Products> tableItems = new List<Products>();
        IEnumerable<Categories> categories = new List<Categories>();
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

                categories = await _catalogDataService.GetAllCategories();
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
                    categories = await _catalogDataService.GetAllCategories();
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


        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            menu.Add("Televisores");
            menu.Add("Celulares");
            menu.Add("Lava-roupas");
            menu.Add("Televisores");
            menu.Add("Notebooks");
            menu.Add("Câmeras fotográficas");



            //[
            //  {
            //    "id": 1,
            //    "name": "Televisores"
            //  },
            //  {
            //    "id": 2,
            //    "name": "Celulares"
            //  },
            //  {
            //    "id": 3,
            //    "name": "Lava-roupas"
            //  },
            //  {
            //    "id": 4,
            //    "name": "Notebooks"
            //  },
            //  {
            //    "id": 5,
            //    "name": "Câmeras fotográficas"
            //  }
            //]


        //    switch(item.ItemId)
        //{
            //case Resource.Id.about:
            //    var aboutPage = new Intent(this, typeof(AboutActivity));
            //    StartActivity(aboutPage);
            //    return true;
            //case Resource.Id.missionVision:
            //    var missionVisionPage = new Intent(this, typeof(MissionVisionActivity));
            //    StartActivity(missionVisionPage);
            //    return true;
            //case Resource.Id.mediaConcept:
            //    var mediaConceptPage = new Intent(this, typeof(MediaConceptActivity));
            //    StartActivity(mediaConceptPage);
            //    return true;
            //}
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

    }
}

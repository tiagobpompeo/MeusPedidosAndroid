using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using MeusPedidos.Models.Sqlite;
using MeusPedidos.Services.BaseCacheService;
using MeusPedidos.Services.ConnectionService;
using Pedidos.Services.Data;
using Fragment = Android.Support.V4.App.Fragment;


namespace MeusPedidos.Fragments
{
    public class CartFragment : Fragment
    {
        #region Properties and Attributes
        List<ListShop> tableItems = new List<ListShop>();
        IEnumerable<Categories> categories = new List<Categories>();
        public ICatalogDataService _catalogDataService;
        public IConnectionService _connection;
        BaseService baseService;
        private ListView list;
        ObservableCollection<ListShop> listShop;
        #endregion

        public CartFragment()
        {
            this.RetainInstance = true;
            _catalogDataService = new CatalogDataService();
            _connection = new ConnectionService();
            baseService = new BaseService();
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            this.HasOptionsMenu = true;
            ((MainActivity)Activity).SupportActionBar.SetTitle(Resource.String.carrinho);
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.cart, null);
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

            listShop = new ObservableCollection<ListShop>(await MainActivity.ShopRepository.GetAllProductAsync());
            await baseService.InsertObject("CartData", listShop, DateTimeOffset.Now.AddMinutes(10));

            if (listShop.Count > 0)
            {
                foreach (var be in listShop)
                {
                    tableItems.Add(new ListShop
                    {
                        Price = be.Price,
                        Name = be.Name,
                        Image = be.Image,
                        IdProduct = be.IdProduct
                    });
                }
                list.Adapter = new CatalogoScreenAdapterCart(Activity, tableItems);
            }
            else
            {
                Toast.MakeText(Activity, "Nenhum Produto no carrinho", ToastLength.Long).Show();
            }

            // To dismiss the dialog
            progress.Dismiss();
            progress.SetCancelable(true);
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
                    celularesIntent.PutExtra("title", "Televisores");
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
                    notebooksIntent.PutExtra("title", "Televisores");
                    StartActivity(notebooksIntent);
                    return true;
                case Resource.Id.cameras:
                    var camerasIntent = new Intent(Application.Context, typeof(CategoriesActivity));
                    camerasIntent.PutExtra("id", 5);
                    camerasIntent.PutExtra("title", "Televisores");
                    StartActivity(camerasIntent);
                    return true;
            }
            return true;
        }

        protected void OnListItemClick(object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
        {

        }

    }
}

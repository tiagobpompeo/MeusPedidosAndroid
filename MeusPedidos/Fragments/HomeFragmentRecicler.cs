
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
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
    public class HomeFragmentRecicler : Fragment
    {
        RecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        List<Products> tableItems = new List<Products>();
        public ICatalogDataService _catalogDataService;
        public IConnectionService _connection;
        BaseService baseService;
        AdapterReciclerHome mAdapter;


        public HomeFragmentRecicler()
        {
            this.RetainInstance = true;
            _catalogDataService = new CatalogDataService();
            _connection = new ConnectionService();
            baseService = new BaseService();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            this.HasOptionsMenu = true;
            ((MainActivity)Activity).SupportActionBar.SetTitle(Resource.String.home);
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.home_fragmentRecicle, null);
            mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView2);
            //mLayoutManager = new LinearLayoutManager(Activity);//ver "this"
            mLayoutManager = new LinearLayoutManager(Activity, LinearLayoutManager.Vertical, false);
            mRecyclerView.SetLayoutManager(mLayoutManager);
            mRecyclerView.Click += OnListItemClick;

             
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

            }
            else
            {
                var connections = await this._connection.CheckConnection();

                if (!connections.IsSuccess)
                {

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
                        mAdapter = new AdapterReciclerHome( tableItems);
                        mRecyclerView.SetAdapter(mAdapter);
                    }
                }
            }

            // To dismiss the dialog
            progress.Dismiss();
            progress.SetCancelable(true);

        }

        private void OnListItemClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}

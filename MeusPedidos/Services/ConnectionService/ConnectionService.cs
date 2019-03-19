using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Net;
using MeusPedidos.Contracts;
using MeusPedidos.Models;
using Plugin.Connectivity;

namespace MeusPedidos.Services.ConnectionService
{
    public class ConnectionService : IConnectionService
    {
      

        public async Task<Response> CheckConnection()
        {

            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Please turn on your internet settings.",
                    };
                }

                var isReachable = await CrossConnectivity.Current.IsRemoteReachable(
                    "google.com");
                if (!isReachable)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Verifique sua conexao com a rede.",
                    };
                }

                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public Response CheckConnectionApi()
        {

            try
            {

                ConnectivityManager conn = (ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);
                bool isConnected = conn.ActiveNetworkInfo != null && conn.ActiveNetworkInfo.IsConnected;

                if (!isConnected)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Please turn on your internet settings.",
                    };
                }

                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }


    }
}

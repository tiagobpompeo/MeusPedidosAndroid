using System;
using System.Threading.Tasks;
using MeusPedidos.Models;

namespace MeusPedidos.Contracts
{
    public interface IConnectionService
    {
        Task<Response> CheckConnection();
        Response CheckConnectionApi();
    }
}

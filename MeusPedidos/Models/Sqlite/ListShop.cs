using System;
using SQLite;

namespace MeusPedidos.Models.Sqlite
{
    [Table("lista")]
    public class ListShop
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [MaxLength(250), Unique]
        public string Name { get; set; }
        public string IdProduct { get; set; }
        public string Quantity { get; set; }
        public string Image { get; set; }
        public string Price { get; set; }




    }
}

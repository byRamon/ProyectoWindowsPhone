using System;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace UsoSQLiteChat.Modelo
{
    class Mensaje
    {
        [AutoIncrement, PrimaryKey, Column("id")]
        public int id { get; set; }

        [MaxLength(100), NotNull]
        public string usuario { get; set; }

        [MaxLength(100), NotNull]
        public string mensaje { get; set; }
        public bool recibido { get; set; }
        public DateTime fechaEnvio;
    }
}
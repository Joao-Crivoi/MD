using SQLite;
namespace CadastroLivrosApp.Models
{
    public class Book
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

       
        public string NomeLivro { get; set; }
        public string NomeAutor { get; set; }
        public string ISBN { get; set; }
        public string EmailAutor { get; set; }

        public DateTime DataCadastro { get; set; } = DateTime.Now;

        public Book()
        {
            NomeLivro = string.Empty;
            NomeAutor = string.Empty;
            ISBN = string.Empty;
            EmailAutor = string.Empty;
        }
    }
}

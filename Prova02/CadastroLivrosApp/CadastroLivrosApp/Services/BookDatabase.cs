using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SQLite;
using CadastroLivrosApp.Models;

namespace CadastroLivrosApp.Services
{
    public class BookDatabase
    {
       
        SQLiteAsyncConnection _database;
        private readonly string _dbPath;

        public BookDatabase(string dbPath)
        {
            _dbPath = dbPath;
        }

      
        async Task Init()
        {
            if (_database is not null)
                return;

            _database = new SQLiteAsyncConnection(_dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);

         
            var result = await _database.CreateTableAsync<Book>();
         
        }

        // Retorna todos os livros cadastrados.
        public async Task<List<Book>> GetBooksAsync()
        {
            await Init();
            return await _database.Table<Book>().ToListAsync();
        }

        // Retorna um livro específico pelo ID.
        public async Task<Book> GetBookAsync(int id)
        {
            await Init();
            return await _database.Table<Book>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        // Salva ou atualiza um livro (INSERT ou UPDATE).
        public async Task<int> SaveBookAsync(Book book)
        {
            await Init();
            if (book.Id != 0)
            {
                // UPDATE: Se o ID for diferente de zero, atualiza o registro existente.
                return await _database.UpdateAsync(book);
            }
            else
            {
                // CREATE: Se o ID for zero, insere um novo registro.
                return await _database.InsertAsync(book);
            }
        }

        // Exclui um livro do banco de dados.
        public async Task<int> DeleteBookAsync(Book book)
        {
            await Init();
            return await _database.DeleteAsync(book);
        }
    }
}
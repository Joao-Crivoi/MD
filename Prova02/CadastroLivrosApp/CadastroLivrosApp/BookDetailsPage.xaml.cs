using System;
using Microsoft.Maui.Controls;
using CadastroLivrosApp.Models;
using CadastroLivrosApp.Services;

namespace CadastroLivrosApp
{
   
    public partial class BookDetailPage : ContentPage
    {
        private readonly BookDatabase _database;
        private Book _currentBook;

        public BookDetailPage(BookDatabase database, Book book)
        {
            InitializeComponent();
            _database = database;
            _currentBook = book;

            this.Title = (_currentBook.Id == 0) ? "Adicionar Novo Livro" : "Editar Livro";

            // Define o contexto de ligação para a interface de usuário.
            BindingContext = _currentBook;

        
            DeleteButton.IsVisible = (_currentBook.Id != 0);
        }

    
        private async void OnSaveClicked(object sender, EventArgs e)
        {
           
            if (string.IsNullOrWhiteSpace(_currentBook.NomeLivro) ||
                string.IsNullOrWhiteSpace(_currentBook.NomeAutor))
            {
               
                await DisplayAlert("Atenção", "O Nome do Livro e o Nome do Autor são obrigatórios.", "OK");
                return;
            }

            try
            {
                await _database.SaveBookAsync(_currentBook);
                await Navigation.PopAsync(); // Retorna para a página principal.
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro de Salvar", $"Ocorreu um erro ao salvar o livro: {ex.Message}", "OK");
            }
        }

    
        private async void OnDeleteClicked(object sender, EventArgs e)
        {
    
            bool result = await DisplayAlert("Confirmação",
                $"Deseja realmente excluir o livro '{_currentBook.NomeLivro}'?", "Sim", "Não");

            if (result)
            {
                try
                {
                    await _database.DeleteBookAsync(_currentBook);
                    await Navigation.PopAsync();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Erro de Excluir", $"Ocorreu um erro ao excluir o livro: {ex.Message}", "OK");
                }
            }
        }

      
        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(); 
        }
    }
}
using CadastroLivrosApp.Models;
using CadastroLivrosApp.Services;
using System.Collections.ObjectModel; // Importação necessária para ObservableCollection
using System.Diagnostics; // Necessário para Debug.WriteLine

namespace CadastroLivrosApp
{
  
    public partial class MainPage : ContentPage
    {
        private readonly BookDatabase _database;

     
        public ObservableCollection<Book> BooksList { get; set; } = new ObservableCollection<Book>();

   
        public MainPage(BookDatabase database)
        {
            InitializeComponent();
            _database = database;

          
            BooksCollectionView.ItemsSource = BooksList;
        }

        // Chamado sempre que a página se torna visível (para atualizar a lista).
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Garantir que a lista seja carregada sempre que a página aparecer
            await LoadBooks();
        }

        // Carrega os livros do banco de dados e atualiza a CollectionView.
        private async Task LoadBooks()
        {
            try
            {
                var books = await _database.GetBooksAsync();

                // 3. Log de Diagnóstico: Verifica se os dados estão sendo carregados.
                Debug.WriteLine($"[DEBUG] Tentando carregar {books?.Count() ?? 0} livros.");

                // 4. Limpa a lista observável e a preenche com os novos dados:
                if (books != null)
                {
                    BooksList.Clear();
                    foreach (var book in books)
                    {
                        BooksList.Add(book);
                    }
                    Debug.WriteLine($"[DEBUG] CollectionView ItemsSource atualizado. Total: {BooksList.Count}");
                }
                else
                {
                    Debug.WriteLine("[DEBUG] O método GetBooksAsync retornou uma coleção nula.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] Erro ao carregar livros: {ex.Message}");
        
            }
        }

    
        private async void OnAddBookClicked(object sender, EventArgs e)
        {
        
            await Navigation.PushAsync(new BookDetailPage(_database, new Book()));
        }

    
        private async void OnBookSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Book selectedBook)
            {
                await Navigation.PushAsync(new BookDetailPage(_database, selectedBook));
             
                ((CollectionView)sender).SelectedItem = null;
            }
        }

        private async void OnBookSelected(object sender, TappedEventArgs e)
        {
            if (e.Parameter is Book selectedBook)
            {
                // Passa o livro selecionado para a página de detalhes para edição.
                await Navigation.PushAsync(new BookDetailPage(_database, selectedBook));
            }
        }

     
        private async void OnLocalizacaoClicked(object sender, EventArgs e)
        {
            LocalizacaoLabel.Text = "Buscando localização...";

            try
            {
               
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    // Exibe a localização em coordenadas.
                    LocalizacaoLabel.Text =
                        $"Latitude: {location.Latitude:N4}\n" +
                        $"Longitude: {location.Longitude:N4}\n" +
                        $"Altitude: {location.Altitude:N0}m\n" +
                        $"Precisão: {location.Accuracy:N0}m";

                   
                }
                else
                {
                    LocalizacaoLabel.Text = "Localização não disponível ou permissão negada.";
                }
            }
            catch (FeatureNotSupportedException)
            {
                await DisplayAlert("Erro", "O recurso de localização não é suportado neste dispositivo.", "OK");
                LocalizacaoLabel.Text = "Erro: Geolocation não suportada.";
            }
            catch (FeatureNotEnabledException)
            {
              
                await DisplayAlert("Erro", "A localização GPS está desativada. Por favor, ative nas configurações.", "OK");
                LocalizacaoLabel.Text = "Erro: GPS desativado.";
            }
            catch (PermissionException)
            {
                
                await DisplayAlert("Erro de Permissão", "Permissão de localização não concedida. Por favor, verifique as configurações.", "OK");
                LocalizacaoLabel.Text = "Erro: Permissão negada.";
            }
            catch (Exception ex)
            {
                
                await DisplayAlert("Erro", $"Não foi possível obter a localização: {ex.Message}", "OK");
                LocalizacaoLabel.Text = "Erro ao buscar localização.";
            }
        }
    }
}
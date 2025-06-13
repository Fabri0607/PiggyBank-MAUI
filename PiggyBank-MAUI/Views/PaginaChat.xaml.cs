using PiggyBank_MAUI.Models;
using PiggyBank_MAUI.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace PiggyBank_MAUI.Views
{
    public partial class PaginaChat : ContentPage
    {
        private readonly ApiService _apiService;
        private readonly int _analisisId;
        public ObservableCollection<MensajeDTO> Messages { get; } = new ObservableCollection<MensajeDTO>();

        public PaginaChat(int analisisId)
        {
            InitializeComponent();
            _apiService = new ApiService();
            _analisisId = analisisId;
            BindingContext = this;
            LoadMessages();
        }

        private async void LoadMessages()
        {
            try
            {
                var response = await _apiService.ObtenerMensajes(_analisisId);
                if (response.resultado)
                {
                    Messages.Clear();
                    foreach (var message in response.MensajesChat)
                    {
                        Messages.Add(message);
                    }
                }
                else
                {
                    await DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al cargar los mensajes", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in LoadMessages: {ex.Message}");
                await DisplayAlert("Error", "Ocurrió un error inesperado al cargar los mensajes.", "OK");
            }
        }

    }
}

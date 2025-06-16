using PiggyBank_MAUI.Models;
using PiggyBank_MAUI.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;

namespace PiggyBank_MAUI.Views
{
    public partial class PaginaChat : ContentPage, INotifyPropertyChanged
    {
        private readonly ApiService _apiService;
        private readonly int _analisisId;
        private CollectionView _messagesCollectionView;
        private bool _isLoadingMessages;
        
        public ObservableCollection<MensajeDTO> Messages { get; } = new ObservableCollection<MensajeDTO>();

        private string _messageText;
        public string MessageText
        {
            get => _messageText;
            set
            {
                _messageText = value;
                OnPropertyChanged(nameof(MessageText));
                OnPropertyChanged(nameof(CanSendMessage));
            }
        }

        private bool _isSendingMessage;
        public bool IsSendingMessage
        {
            get => _isSendingMessage;
            set
            {
                _isSendingMessage = value;
                OnPropertyChanged(nameof(IsSendingMessage));
                OnPropertyChanged(nameof(CanSendMessage));
            }
        }

        public bool CanSendMessage => !IsSendingMessage && !string.IsNullOrWhiteSpace(MessageText);

        public PaginaChat(int analisisId)
        {
            InitializeComponent();
            Debug.WriteLine($"PaginaChat created with analisisId: {analisisId}");
            _apiService = new ApiService();
            _analisisId = analisisId;
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            _messagesCollectionView = this.FindByName<CollectionView>("MessagesCollectionView");
            await LoadMessages();
        }

        private async Task LoadMessages()
        {
            if (_isLoadingMessages) return;
            
            try
            {
                _isLoadingMessages = true;
                var response = await _apiService.ObtenerMensajes(_analisisId);
                
                if (response.resultado && response.MensajesChat != null)
                {
                    // Limpiar y recargar mensajes de una vez
                    Messages.Clear();
                    
                    foreach (var message in response.MensajesChat)
                    {
                        Messages.Add(message);
                    }
                    
                    // Scroll al final después de que todos los mensajes estén cargados
                    ScrollToBottomDelayed(300); // Delay más largo para carga inicial
                }
                else
                {
                    var errorMessage = response.error?.FirstOrDefault()?.Message ?? "Error al cargar los mensajes";
                    await DisplayAlert("Error", errorMessage, "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in LoadMessages: {ex.Message}");
                await DisplayAlert("Error", "Ocurrió un error inesperado al cargar los mensajes.", "OK");
            }
            finally
            {
                _isLoadingMessages = false;
            }
        }

        private async Task SendMessage()
        {
            Debug.WriteLine("SendMessage called");
            
            if (!CanSendMessage)
            {
                Debug.WriteLine("Cannot send message - conditions not met");
                return;
            }

            var messageToSend = MessageText.Trim();
            Debug.WriteLine($"Sending message: {messageToSend}");

            // Limpiar el input inmediatamente para mejor UX
            MessageText = string.Empty;
            IsSendingMessage = true;

            // Crear mensaje del usuario con ID temporal
            var userMessage = new MensajeDTO
            {
                MensajeID = -2, // ID temporal negativo diferente
                Role = "user",
                Content = messageToSend,
                FechaEnvio = DateTime.Now
            };

            // Agregar mensaje del usuario
            Messages.Add(userMessage);
            ScrollToBottomDelayed(100);

            // Crear mensaje de carga con ID temporal
            var loadingMessage = new MensajeDTO
            {
                MensajeID = -1, // ID temporal negativo
                Role = "assistant",
                Content = "...",
                FechaEnvio = DateTime.Now
            };

            Messages.Add(loadingMessage);
            ScrollToBottomDelayed(150);

            try
            {
                var request = new ReqInsertarMensajeChat
                {
                    AnalisisID = _analisisId,
                    Role = "user",
                    Content = messageToSend,
                    sesion = new Sesion(),
                    token = Preferences.Get("AuthToken", string.Empty) // Usar token del storage
                };

                Debug.WriteLine("Calling InsertarMensaje API");
                var response = await _apiService.InsertarMensaje(_analisisId, request);
                Debug.WriteLine($"InsertarMensaje response: {response.resultado}");

                // Remover mensaje de carga
                Messages.Remove(loadingMessage);

                if (response.resultado)
                {
                    // Obtener mensajes actualizados del servidor
                    await RefreshMessagesFromServer(response);
                }
                else
                {
                    // En caso de error, remover también el mensaje del usuario
                    Messages.Remove(userMessage);
                    var errorMessage = response.error?.FirstOrDefault()?.Message ?? "Error al enviar el mensaje";
                    await DisplayAlert("Error", errorMessage, "OK");
                }
            }
            catch (Exception ex)
            {
                // Limpiar mensajes temporales en caso de excepción
                Messages.Remove(loadingMessage);
                Messages.Remove(userMessage);
                
                Debug.WriteLine($"Exception in SendMessage: {ex.Message}");
                await DisplayAlert("Error", "Ocurrió un error inesperado al enviar el mensaje.", "OK");
            }
            finally
            {
                IsSendingMessage = false;
            }
        }

        private async Task RefreshMessagesFromServer(ResInsertarMensajeChat response)
        {
            try
            {
                // Opción 1: Recargar todos los mensajes (más seguro)
                await LoadMessages();
                
                // Opción 2: Solo agregar los nuevos mensajes (más eficiente)
                /*
                if (response.MensajeConsultaID > 0)
                {
                    var sentMessage = await _apiService.ObtenerMensaje(response.MensajeConsultaID);
                    if (sentMessage != null && !Messages.Any(m => m.MensajeID == sentMessage.MensajeID))
                    {
                        Messages.Add(sentMessage);
                    }
                }

                if (response.MensajeRespuestaID > 0)
                {
                    var assistantMessage = await _apiService.ObtenerMensaje(response.MensajeRespuestaID);
                    if (assistantMessage != null && !Messages.Any(m => m.MensajeID == assistantMessage.MensajeID))
                    {
                        Messages.Add(assistantMessage);
                        // Scroll al final cuando llegue la respuesta del bot
                        ScrollToBottomDelayed(200);
                    }
                }
                */
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error refreshing messages: {ex.Message}");
            }
        }

        private async void SendMessage_Clicked(object sender, EventArgs e)
        {
            await SendMessage();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ScrollToBottom()
        {
            try
            {
                if (ChatScrollView != null)
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await Task.Delay(100); // Small delay to allow UI to update
                        await ChatScrollView.ScrollToAsync(0, ChatScrollView.ContentSize.Height, true);
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error scrolling to bottom: {ex.Message}");
            }
        }

        // Método mejorado para scroll con delay
        private void ScrollToBottomDelayed(int delayMs = 200)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Task.Delay(delayMs);
                ScrollToBottom();
            });
        }

        // Método para refrescar manualmente los mensajes
        public async Task RefreshMessages()
        {
            await LoadMessages();
        }
    }
}

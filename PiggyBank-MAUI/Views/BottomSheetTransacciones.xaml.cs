using PiggyBank_MAUI.Models;
using PiggyBank_MAUI.Services;
using System.Diagnostics;
using System.Windows.Input;
using The49.Maui.BottomSheet;

namespace PiggyBank_MAUI.Views;

public partial class BottomSheetTransacciones : BottomSheet
{
    public TransaccionDTO Transaccion { get; set; }
    public ICommand EditarCommand { get; }
    public ICommand EliminarCommand { get; }

    public BottomSheetTransacciones(TransaccionDTO transaccion)
    {
        InitializeComponent();
        Transaccion = transaccion;
        EditarCommand = new Command(async () => await EditarTransaccion());
        EliminarCommand = new Command(async () => await EliminarTransaccion());
        BindingContext = this;
    }

    private async Task EditarTransaccion()
    {
        if (Transaccion != null)
        {
            // Enviar mensaje a PaginaTransacciones para navegar
            MessagingCenter.Send(this, "EditarTransaccion", Transaccion);
            // Cerrar el BottomSheet
            await DismissAsync();
        }
    }

    private async Task EliminarTransaccion()
    {
        if (Transaccion != null)
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert("Confirmar", "¿Estás seguro de que deseas eliminar esta transacción?", "Sí", "No");
            if (confirm)
            {
                try
                {
                    var apiService = new ApiService();
                    var usuarioIdString = await SecureStorage.GetAsync("UsuarioID");
                    if (!int.TryParse(usuarioIdString, out int usuarioId))
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Usuario no autenticado", "OK");
                        return;
                    }

                    var req = new ReqEliminarTransaccion
                    {
                        TransaccionID = Transaccion.TransaccionID,
                        UsuarioID = usuarioId,
                        token = Preferences.Get("AuthToken", string.Empty)
                    };

                    var response = await apiService.EliminarTransaccion(req);
                    if (response.resultado)
                    {
                        // Enviar mensaje para recargar la lista
                        MessagingCenter.Send(this, "TransaccionEliminada");
                        await Application.Current.MainPage.DisplayAlert("Éxito", "Transacción eliminada correctamente", "OK");
                        await DismissAsync();
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al eliminar la transacción", "OK");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Excepción en EliminarTransaccion: {ex.Message}");
                    await Application.Current.MainPage.DisplayAlert("Error", "Error inesperado al eliminar la transacción", "OK");
                }
            }
        }
    }
}
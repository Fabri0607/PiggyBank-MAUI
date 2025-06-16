using PiggyBank_MAUI.Models;
using PiggyBank_MAUI.Services;
using System.Diagnostics;
using System.Windows.Input;
using The49.Maui.BottomSheet;

namespace PiggyBank_MAUI.Views;

public partial class BottomSheetPagos: BottomSheet
{
    public PagoDTO Pago { get; set; }
    public ICommand EditarCommand { get; }
    public ICommand EliminarCommand { get; }

    public BottomSheetPagos(PagoDTO pago)
	{
		InitializeComponent();

        Pago = pago;
        EditarCommand = new Command(async () => await EditarPago());
        EliminarCommand = new Command(async () => await EliminarPago());
        BindingContext = this;
    }

    private async Task EditarPago()
    {
        if (Pago != null)
        {
            // Enviar mensaje a PagosProgramados para navegar
            MessagingCenter.Send(this, "EditarPago", Pago);
            // Cerrar el BottomSheet
            await DismissAsync();
        }
    }

    private async Task EliminarPago()
    {
        if (Pago != null)
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert("Confirmar", "¿Estás seguro de que deseas eliminar este pago?", "Sí", "No");
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

                    var req = new ReqEliminarPago
                    {
                        PagoID = Pago.PagoID,
                        UsuarioID = usuarioId,
                        token = Preferences.Get("AuthToken", string.Empty)
                    };

                    var response = await apiService.EliminarPago(req);
                    if (response.resultado)
                    {
                        // Enviar mensaje para recargar la lista
                        MessagingCenter.Send(this, "PagoEliminado");
                        await Application.Current.MainPage.DisplayAlert("Éxito", "Pago eliminado correctamente", "OK");
                        await DismissAsync();
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al eliminar el pago", "OK");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Excepción en EliminarPago: {ex.Message}");
                    await Application.Current.MainPage.DisplayAlert("Error", "Error inesperado al eliminar el pago", "OK");
                }
            }
        }
    }
}
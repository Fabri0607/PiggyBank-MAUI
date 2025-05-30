using PiggyBank_MAUI.Models;
using PiggyBank_MAUI.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PiggyBank_MAUI.Views
{
    public partial class GroupDetailsPage : ContentPage
    {
        private readonly ApiService _apiService;
        private readonly GrupoDTO _grupo;
        private ObservableCollection<MiembroDTO> _members;
        private ObservableCollection<GastoCompartidoDTO> _expenses;
        private ObservableCollection<BalanceMiembro> _balances;
        private bool _isAdmin;

        public string GroupName => _grupo.Nombre;
        public string GroupDescription => _grupo.Descripcion;
        public ObservableCollection<MiembroDTO> Members
        {
            get => _members;
            set
            {
                _members = value;
                OnPropertyChanged(nameof(Members));
            }
        }
        public ObservableCollection<GastoCompartidoDTO> Expenses
        {
            get => _expenses;
            set
            {
                _expenses = value;
                OnPropertyChanged(nameof(Expenses));
            }
        }
        public ObservableCollection<BalanceMiembro> Balances
        {
            get => _balances;
            set
            {
                _balances = value;
                OnPropertyChanged(nameof(Balances));
            }
        }
        public bool IsAdmin
        {
            get => _isAdmin;
            set
            {
                _isAdmin = value;
                OnPropertyChanged(nameof(IsAdmin));
            }
        }

        public ICommand RemoveMemberCommand { get; }

        public GroupDetailsPage(GrupoDTO grupo)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            _apiService = new ApiService();
            _grupo = grupo;
            _members = new ObservableCollection<MiembroDTO>();
            _expenses = new ObservableCollection<GastoCompartidoDTO>();
            _balances = new ObservableCollection<BalanceMiembro>();
            BindingContext = this;

            RemoveMemberCommand = new Command<MiembroDTO>(async (miembro) => await OnRemoveMember(miembro));

            InviteMemberButton.Clicked += InviteMemberButton_Clicked;
            RegisterExpenseButton.Clicked += RegisterExpenseButton_Clicked;
            LeaveGroupButton.Clicked += LeaveGroupButton_Clicked;
            DeleteGroupButton.Clicked += DeleteGroupButton_Clicked;
            UpdateGroupButton.Clicked += UpdateGroupButton_Clicked;

            LoadGroupDetails();
            LoadExpenses();
            LoadBalances();
        }

        private async void LoadGroupDetails()
        {
            var usuarioId = int.Parse(await SecureStorage.GetAsync("UsuarioID") ?? "0");
            var req = new ReqObtenerDetallesGrupo
            {
                GrupoID = _grupo.GrupoID,
                UsuarioID = usuarioId,
                token = Preferences.Get("AuthToken", string.Empty)
            };

            var response = await _apiService.ObtenerDetallesGrupo(req);
            if (response.resultado)
            {
                Members.Clear();
                foreach (var miembro in response.Miembros)
                {
                    Members.Add(miembro);
                    Debug.WriteLine($"Miembro: ID={miembro.UsuarioID}, NombreUsuario={miembro.NombreUsuario}, Rol={miembro.Rol}");
                    if (miembro.UsuarioID == usuarioId && miembro.Rol == "Administrador")
                    {
                        IsAdmin = true;
                    }
                }
            }
            else
            {
                await DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al cargar los detalles del grupo", "OK");
            }
        }

        private async void LoadExpenses()
        {
            var usuarioId = int.Parse(await SecureStorage.GetAsync("UsuarioID") ?? "0");
            var req = new ReqListarGastos
            {
                GrupoID = _grupo.GrupoID,
                UsuarioID = usuarioId,
                token = Preferences.Get("AuthToken", string.Empty)
            };

            var response = await _apiService.ListarGastos(req);
            if (response.resultado)
            {
                Expenses.Clear();
                foreach (var gasto in response.Gastos)
                {
                    Expenses.Add(gasto);
                    Debug.WriteLine($"Gasto: ID={gasto.GastoID}, Descripcion={gasto.Descripcion}, Monto={gasto.Monto}, Fecha={gasto.Fecha}");
                }
            }
            else
            {
                await DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al cargar los gastos", "OK");
            }
        }

        private async void LoadBalances()
        {
            var usuarioId = int.Parse(await SecureStorage.GetAsync("UsuarioID") ?? "0");
            var req = new ReqObtenerBalanceGrupal
            {
                GrupoID = _grupo.GrupoID,
                token = Preferences.Get("AuthToken", string.Empty)
            };

            var response = await _apiService.ObtenerBalanceGrupal(req);
            if (response.resultado)
            {
                Balances.Clear();
                foreach (var balance in response.Balances)
                {
                    Balances.Add(balance);
                    Debug.WriteLine($"Balance: Usuario={balance.NombreUsuario}, Saldo={balance.Saldo}");
                }
            }
            else
            {
                await DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al cargar los balances", "OK");
            }
        }

        private async void InviteMemberButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(InviteMemberEntry.Text))
            {
                await DisplayAlert("Error", "El correo del miembro es obligatorio", "OK");
                return;
            }

            var req = new ReqInvitarMiembroGrupo
            {
                GrupoID = _grupo.GrupoID,
                correoUsuario = InviteMemberEntry.Text,
                Rol = "Miembro",
                token = Preferences.Get("AuthToken", string.Empty)
            };

            var response = await _apiService.InvitarMiembroGrupo(req);
            if (response.resultado)
            {
                await DisplayAlert("Éxito", "Invitación enviada correctamente", "OK");
                InviteMemberEntry.Text = string.Empty;
                LoadGroupDetails();
            }
            else
            {
                await DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al invitar al miembro", "OK");
            }
        }

        private async void RegisterExpenseButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new RegisterExpenseModalPage(_grupo));
        }

        private async void ViewBalancesButton_Clicked(object sender, EventArgs e)
        {
            LoadBalances();
        }

        private async void LeaveGroupButton_Clicked(object sender, EventArgs e)
        {
            var usuarioId = int.Parse(await SecureStorage.GetAsync("UsuarioID") ?? "0");
            var req = new ReqSalirGrupo
            {
                GrupoID = _grupo.GrupoID,
                UsuarioID = usuarioId,
                token = Preferences.Get("AuthToken", string.Empty)
            };

            var response = await _apiService.SalirGrupo(req);
            if (response.resultado)
            {
                await DisplayAlert("Éxito", "Has salido del grupo correctamente", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al salir del grupo", "OK");
            }
        }

        private async void DeleteGroupButton_Clicked(object sender, EventArgs e)
        {
            var usuarioId = int.Parse(await SecureStorage.GetAsync("UsuarioID") ?? "0");
            var req = new ReqEliminarGrupo
            {
                GrupoID = _grupo.GrupoID,
                AdminUsuarioID = usuarioId,
                token = Preferences.Get("AuthToken", string.Empty)
            };

            var response = await _apiService.EliminarGrupo(req);
            if (response.resultado)
            {
                await DisplayAlert("Éxito", "Grupo eliminado correctamente", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al eliminar el grupo", "OK");
            }
        }

        private async void UpdateGroupButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                Debug.WriteLine($"Abriendo modal para grupo: Nombre={_grupo.Nombre}, Descripcion={_grupo.Descripcion}");
                await Navigation.PushModalAsync(new UpdateGroupModalPage(_grupo));
                // After modal closes, después de que se cierra el modal, notify UI of cambios potenciales
                Debug.WriteLine($"Modal cerrado, actualizando UI: Nombre={_grupo.Nombre}, Descripción={_grupo.Descripcion}");
                OnPropertyChanged(nameof(GroupName));
                OnPropertyChanged(nameof(GroupDescription));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Excepción en UpdateGroupButton_Clicked: {ex.Message}");
                await DisplayAlert("Error", "Error al abrir la página de edición del grupo", "OK");
            }
        }

        private async Task OnRemoveMember(MiembroDTO miembro)
        {
            var usuarioId = int.Parse(await SecureStorage.GetAsync("UsuarioID") ?? "0");
            var req = new ReqEliminarMiembro
            {
                GrupoID = _grupo.GrupoID,
                UsuarioID = miembro.UsuarioID,
                AdminUsuarioID = usuarioId,
                token = Preferences.Get("AuthToken", string.Empty)
            };

            var response = await _apiService.EliminarMiembro(req);
            if (response.resultado)
            {
                await DisplayAlert("Éxito", "Miembro eliminado correctamente", "OK");
                LoadGroupDetails();
            }
            else
            {
                await DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al eliminar el miembro", "OK");
            }
        }

        private async void AcceptButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (sender is Button button && button.CommandParameter is GastoCompartidoDTO gasto)
                {
                    Debug.WriteLine($"Botón Aceptar clicado: ID={gasto.GastoID}, Descripcion={gasto.Descripcion}, Estado={gasto.Estado}, IsPendiente={gasto.IsPendiente}");

                    var usuarioIdString = await SecureStorage.GetAsync("UsuarioID");
                    if (!int.TryParse(usuarioIdString, out int usuarioId))
                    {
                        Debug.WriteLine("Error: UsuarioID no válido");
                        await DisplayAlert("Error", "Usuario no autenticado", "OK");
                        return;
                    }

                    var req = new ReqActualizarEstadoGasto
                    {
                        GastoID = gasto.GastoID,
                        UsuarioID = usuarioId,
                        NuevoEstado = "Pagado",
                        token = Preferences.Get("AuthToken", string.Empty)
                    };

                    var response = await _apiService.ActualizarEstadoGasto(req);
                    if (response.resultado)
                    {
                        Debug.WriteLine("Gasto aceptado exitosamente");
                        await DisplayAlert("Éxito", "Gasto marcado como Pagado", "OK");
                        LoadExpenses();
                        LoadBalances();
                    }
                    else
                    {
                        Debug.WriteLine($"Error al aceptar gasto: {response.error?.FirstOrDefault()?.Message}");
                        await DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al aceptar el gasto", "OK");
                    }
                }
                else
                {
                    Debug.WriteLine("Error: CommandParameter no es GastoCompartidoDTO o botón inválido");
                    await DisplayAlert("Error", "Gasto no válido", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Excepción en AcceptButton_Clicked: {ex.Message}");
                await DisplayAlert("Error", "Error inesperado al aceptar el gasto", "OK");
            }
        }

        private async void RejectButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (sender is Button button && button.CommandParameter is GastoCompartidoDTO gasto)
                {
                    Debug.WriteLine($"Botón Rechazar clicado: ID={gasto.GastoID}, Descripcion={gasto.Descripcion}, Estado={gasto.Estado}, IsPendiente={gasto.IsPendiente}");

                    var usuarioIdString = await SecureStorage.GetAsync("UsuarioID");
                    if (!int.TryParse(usuarioIdString, out int usuarioId))
                    {
                        Debug.WriteLine("Error: UsuarioID no válido");
                        await DisplayAlert("Error", "Usuario no autenticado", "OK");
                        return;
                    }

                    var req = new ReqActualizarEstadoGasto
                    {
                        GastoID = gasto.GastoID,
                        UsuarioID = usuarioId,
                        NuevoEstado = "Rechazado",
                        token = Preferences.Get("AuthToken", string.Empty)
                    };

                    var response = await _apiService.ActualizarEstadoGasto(req);
                    if (response.resultado)
                    {
                        Debug.WriteLine("Gasto rechazado exitosamente");
                        await DisplayAlert("Éxito", "Gasto marcado como Rechazado", "OK");
                        LoadExpenses();
                        LoadBalances();
                    }
                    else
                    {
                        Debug.WriteLine($"Error al rechazar gasto: {response.error?.FirstOrDefault()?.Message}");
                        await DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al rechazar el gasto", "OK");
                    }
                }
                else
                {
                    Debug.WriteLine("Error: CommandParameter no es GastoCompartidoDTO o botón inválido");
                    await DisplayAlert("Error", "Gasto no válido", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Excepción en RejectButton_Clicked: {ex.Message}");
                await DisplayAlert("Error", "Error inesperado al rechazar el gasto", "OK");
            }
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadGroupDetails();
            LoadExpenses();
            LoadBalances();
        }
    }
}
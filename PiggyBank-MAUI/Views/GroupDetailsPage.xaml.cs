using PiggyBank_MAUI.Models;
using PiggyBank_MAUI.Services;
using System;
using System.Collections.ObjectModel;
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
            ViewBalancesButton.Clicked += ViewBalancesButton_Clicked;
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
                Rol = "Miembro", // Puedes agregar una selección de roles si lo deseas
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
            var usuarioId = int.Parse(await SecureStorage.GetAsync("UsuarioID") ?? "0");
            var req = new ReqActualizarGrupo
            {
                GrupoID = _grupo.GrupoID,
                Nombre = _grupo.Nombre, // Puedes agregar un modal para actualizar estos campos
                Descripcion = _grupo.Descripcion,
                AdminUsuarioID = usuarioId,
                token = Preferences.Get("AuthToken", string.Empty)
            };

            var response = await _apiService.ActualizarGrupo(req);
            if (response.resultado)
            {
                await DisplayAlert("Éxito", "Grupo actualizado correctamente", "OK");
                LoadGroupDetails();
            }
            else
            {
                await DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al actualizar el grupo", "OK");
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadGroupDetails();
            LoadExpenses();
            LoadBalances();
        }
    }
}
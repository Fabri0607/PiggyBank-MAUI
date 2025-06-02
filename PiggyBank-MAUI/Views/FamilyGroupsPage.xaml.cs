using PiggyBank_MAUI.Models;
using PiggyBank_MAUI.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PiggyBank_MAUI.Views
{
    public partial class FamilyGroupsPage : ContentPage
    {
        private readonly ApiService _apiService;
        private ObservableCollection<GrupoDTO> _groups;

        public ObservableCollection<GrupoDTO> Groups
        {
            get => _groups;
            set
            {
                _groups = value;
                OnPropertyChanged(nameof(Groups));
            }
        }

        public ICommand ViewDetailsCommand { get; }

        public FamilyGroupsPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            _apiService = new ApiService();
            _groups = new ObservableCollection<GrupoDTO>();
            BindingContext = this;

            ViewDetailsCommand = new Command<GrupoDTO>(async (grupo) => await OnViewDetails(grupo));
            CreateGroupButton.Clicked += CreateGroupButton_Clicked;

            LoadGroups();
        }

        private async void LoadGroups()
        {
            var usuarioId = int.Parse(await SecureStorage.GetAsync("UsuarioID") ?? "0");
            var req = new ReqListarGrupos
            {
                UsuarioID = usuarioId,
                token = Preferences.Get("AuthToken", string.Empty)
            };

            var response = await _apiService.ListarGrupos(req);
            if (response.resultado)
            {
                Groups.Clear();
                foreach (var grupo in response.Grupos)
                {
                    Groups.Add(grupo);
                }
            }
            else
            {
                await DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al cargar los grupos", "OK");
            }
        }

        private async void CreateGroupButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(GroupNameEntry.Text))
            {
                await DisplayAlert("Error", "El nombre del grupo es obligatorio", "OK");
                return;
            }

            var usuarioId = int.Parse(await SecureStorage.GetAsync("UsuarioID") ?? "0");
            var req = new ReqCrearGrupoFamiliar
            {
                Nombre = GroupNameEntry.Text,
                Descripcion = string.Empty, // Puedes agregar un campo para la descripción si lo deseas
                UsuarioID = usuarioId,
                token = Preferences.Get("AuthToken", string.Empty)
            };

            var response = await _apiService.CrearGrupoFamiliar(req);
            if (response.resultado)
            {
                await DisplayAlert("Éxito", "Grupo creado correctamente", "OK");
                GroupNameEntry.Text = string.Empty;
                LoadGroups();
            }
            else
            {
                await DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al crear el grupo", "OK");
            }
        }

        private async Task OnViewDetails(GrupoDTO grupo)
        {
            await Navigation.PushAsync(new GroupDetailsPage(grupo));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadGroups();
        }
    }
}
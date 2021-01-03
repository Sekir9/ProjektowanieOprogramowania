using ConnecticoApplication.Models;
using ConnecticoApplication.Services;
using ConnecticoApplication.Utils;
using ConnecticoApplication.Views;
using ConnecticoApplication.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Effects;

namespace ConnecticoApplication.ViewModels
{
    public class RoutePointsViewModel
    {
        private RoutePointsView _view;
        private Frame _container;

        public IRoutePointService RoutePointService { get; set; }

        private IEnumerable<RoutePoint> _routePoints;

        public RoutePointsViewModel(Frame container, RoutePointsView view)
        {
            _view = view;
            _container = container;
        }

        public async void Activate()
        {
            _container.Content = _view;

            _view.NameTextBox.Text = "";
            _view.HeightFromTextBox.Text = "0";
            _view.HeightToTextBox.Text = "9999";

            await ReadRoutePointsAsync();
        }

        private async Task ReadRoutePointsAsync()
        {
            _routePoints = await RoutePointService.GetRoutePoints();
            _view.DataGrid.DataContext = _routePoints;
        }

        public async void OnCreateRoutePointClicked()
        {
            string name = "";
            string description = null;
            string cordinates = "";
            int? height = null;
            bool valid;

            do
            {
                bool confirmed = AskForPointData(ref name, ref description, ref cordinates, ref height);
                if (!confirmed)
                    return; //user resigned

                valid = ValidateData(name, description, cordinates, height);
            } while (!valid);

            RoutePoint newRoutePoint = new RoutePoint();
            newRoutePoint.Name = name;
            newRoutePoint.Description = description;
            newRoutePoint.Cordinates = cordinates;
            newRoutePoint.Height = height.Value;

            bool success = await RoutePointService.CreateRoutePoint(newRoutePoint);
            ProcessServiceActionResponse(success);
        }

        public async void OnEditRoutePointClicked(int id)
        {
            RoutePoint routePoint = _routePoints.FirstOrDefault(p => p.Id == id);
            if (routePoint == null)
                return;

            string name = routePoint.Name;
            string description = routePoint.Description;
            string cordinates = routePoint.Cordinates;
            int? height = routePoint.Height;
            bool valid;

            do
            {
                bool confirmed = AskForPointData(ref name, ref description, ref cordinates, ref height);
                if (!confirmed)
                    return; //user resigned

                valid = ValidateData(name, description, cordinates, height, routePoint);
            } while (!valid);

            routePoint.Name = name;
            routePoint.Description = description;
            routePoint.Cordinates = cordinates;
            routePoint.Height = height.Value;

            bool success = await RoutePointService.EditRoutePoint(routePoint);
            ProcessServiceActionResponse(success);
        }

        private void ProcessServiceActionResponse(bool success)
        {
            _ = ReadRoutePointsAsync(); //Continue while ui is being refreshed

            if (success)
            {
                CustomMessageBox.ShowSuccess(MainWindow.Instance, "Dane zostały pomyślnie zapisane.");
            }
            else
            {
                CustomMessageBox.ShowWarning(
                    MainWindow.Instance,
                    "Nie udało się zapisać danych!",
                    "Spróbuj ponownie!\nBłąd po stronie aplikacji lub połączenia internetowego!");
            }
        }

        public bool AskForPointData(ref string name, ref string description, ref string cordinates, ref int? height)
        {
            string heightString = height?.ToString() ?? "";

            RoutePointDetailsWindow detailsWindow = new RoutePointDetailsWindow(name, description, cordinates, heightString);
            detailsWindow.Owner = MainWindow.Instance;

            MainWindow.Instance.Opacity = 0.5;
            MainWindow.Instance.Effect = new BlurEffect();

            bool? result = detailsWindow.ShowDialog();

            MainWindow.Instance.Effect = null;
            MainWindow.Instance.Opacity = 1;

            if (result ?? false)
            {
                name = detailsWindow.PointName;

                if (string.IsNullOrEmpty(detailsWindow.PointDescription))
                    description = null;
                else
                    description = detailsWindow.PointDescription;

                cordinates = detailsWindow.PointCordinates;

                if (string.IsNullOrEmpty(detailsWindow.PointHeight))
                    height = null;
                else
                    height = int.Parse(detailsWindow.PointHeight);

                return true;
            }

            return false;
        }

        private bool ValidateData(string name, string description, string cordinates, int? height, RoutePoint exclude = null)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(cordinates) || !height.HasValue)
            {
                CustomMessageBox.ShowWarning(
                    MainWindow.Instance,
                    "Błąd danych",
                    "Nazwa, kordynaty oraz wysokość punktu trasy nie mogą być puste");

                return false;
            }

            if ((height ?? 0) <= 0)
            {
                CustomMessageBox.ShowWarning(
                    MainWindow.Instance,
                    "Błąd danych",
                    $"Wysokość musi być większa od 0");

                return false;
            }

            if (name.Length > RoutePoint.NAME_MAX_LENGTH)
            {
                CustomMessageBox.ShowWarning(
                    MainWindow.Instance,
                    "Błąd danych",
                    $"Nazwa nie może być dłuższa niż {RoutePoint.NAME_MAX_LENGTH} znaków!");

                return false;
            }

            if (description != null && description.Length > RoutePoint.DESCRIPTION_MAX_LENGTH)
            {
                CustomMessageBox.ShowWarning(
                    MainWindow.Instance,
                    "Błąd danych",
                    $"Opis nie może być dłuższy niż {RoutePoint.DESCRIPTION_MAX_LENGTH} znaków!");

                return false;
            }

            if (!Regex.IsMatch(cordinates, RoutePoint.CORDINATES_REGEX))
            {
                CustomMessageBox.ShowWarning(
                    MainWindow.Instance,
                    "Błąd danych",
                    "Kordynaty mają niepoprawny format. Przykładowa wartość: 50° 44' 22\" N 15° 43' 43\" E");

                return false;
            }

            var routePoints = _routePoints;
            if (exclude != null)
            {
                routePoints = routePoints.Where(p => p.Id != exclude.Id);
            }

            RoutePoint duplicate = routePoints.FirstOrDefault(p => p.Name == name);
            if (duplicate != null)
            {
                CustomMessageBox.ShowWarning(
                    MainWindow.Instance,
                    "Duplikat",
                    "Punk trasy z taką nazwą już istnieje!");
                return false;
            }

            return true;
        }

        public void OnFiltersChanged()
        {
            var routePoints = _routePoints;

            if (!string.IsNullOrEmpty(_view.NameTextBox.Text))
            {
                routePoints = routePoints.Where(p => p.Name.IndexOf(_view.NameTextBox.Text, StringComparison.CurrentCultureIgnoreCase) >= 0);
            }

            if (!int.TryParse(_view.HeightFromTextBox.Text, out int from))
                from = 0;

            if (!int.TryParse(_view.HeightToTextBox.Text, out int to))
                to = 9999;

            routePoints = routePoints.Where(p => from <= p.Height && p.Height <= to);
            _view.DataGrid.DataContext = routePoints.ToArray();
        }
    }
}

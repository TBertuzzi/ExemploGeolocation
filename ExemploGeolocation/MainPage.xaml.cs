using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Xamarin.Forms;

namespace ExemploGeolocation
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public async Task<Position> ObterPosicaoAtual()
        {
            Position position = null;
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100; //Precisão da localização em metros

                position = await locator.GetLastKnownLocationAsync();

                if (position != null)
                {
                    //Retorna a ultima localização valida, caso ela exista em cache :)
                    return position;
                }

                if (!locator.IsGeolocationAvailable || !locator.IsGeolocationEnabled)
                {
                   //Geolocalização Não disponivel, ou GPS desabilitado
                    return null;
                }

                position = await locator.GetPositionAsync(TimeSpan.FromSeconds(20), null, true);

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Atenção", $"Não foi possivel obter a localização [{ex.Message}]", "OK");
            }

            if (position == null)
                return null;
                

            return position;
        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            var posicao = await ObterPosicaoAtual();

            if (posicao != null)
            {
                var mensagem = string.Format("Hora: {0} \nLat: {1} \nLong: {2} \nAltitude: {3} \nAltitude Precisão: {4} \nPrecisão: {5} \nHeading: {6} \nVelocidade: {7}",
                       posicao.Timestamp, posicao.Latitude, posicao.Longitude,
                       posicao.Altitude, posicao.AltitudeAccuracy, posicao.Accuracy, posicao.Heading, posicao.Speed);

                await Application.Current.MainPage.DisplayAlert("Geolocalização", $"{mensagem}", "OK");
            }
        }
    }
}

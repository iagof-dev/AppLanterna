using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Plugin.Battery.Abstractions;
using Plugin.Battery;

namespace AppLanterna
{
    public partial class MainPage : ContentPage
    {

        bool lanterna_ligada = false;
        public MainPage()
        {
            InitializeComponent();

            btnOnOff.Source = "btoff";

            CarregarInformacoes_Bateria();

        }
        
        private async void CarregarInformacoes_Bateria()
        {
            try
            {
                if (CrossBattery.IsSupported)
                {
                    CrossBattery.Current.BatteryChanged -= Mudanca_Status_Bateria;
                    CrossBattery.Current.BatteryChanged += Mudanca_Status_Bateria;
                }
                else
                {
                    lbl_status_bateria.Text = "As informações sobre a bateria não estão disponiveis :( ";
                }
            }
            catch (Exception e)
            {
                await DisplayAlert("Ocorreu um Erro: \n ", e.Message, "OK");
            }
        }


        private async void Mudanca_Status_Bateria(object sender, Plugin.Battery.Abstractions.BatteryChangedEventArgs e)
        {
            try
            {
                lbl_carga_restante.Text = e.RemainingChargePercent.ToString() + "%";

                if (e.IsLow)
                {
                    lbl_status_bateria.Text = "Atenção! Bateria está fraca!";
                }
                else
                {
                    lbl_status_bateria.Text = "";
                }
                switch (e.Status)
                {
                    case Plugin.Battery.Abstractions.BatteryStatus.Charging:
                        lbl_status_bateria.Text = "Carregando";
                        break;
                    case Plugin.Battery.Abstractions.BatteryStatus.Discharging:
                        lbl_status_bateria.Text = "Descarregando";
                        break;
                    case Plugin.Battery.Abstractions.BatteryStatus.Full:
                        lbl_status_bateria.Text = "Carregada";
                        break;
                    case Plugin.Battery.Abstractions.BatteryStatus.NotCharging:
                        lbl_status_bateria.Text = "Sem Carregar";
                        break;
                    case Plugin.Battery.Abstractions.BatteryStatus.Unknown:
                        lbl_status_bateria.Text = "Desconhecido";
                        break;
                }

                switch (e.PowerSource)
                {
                    case Plugin.Battery.Abstractions.PowerSource.Ac:
                        lbl_fonte_energia.Text = "Carregador";
                        break;
                    case Plugin.Battery.Abstractions.PowerSource.Battery:
                        lbl_fonte_energia.Text = "Bateria";
                        break;
                    case Plugin.Battery.Abstractions.PowerSource.Usb:
                        lbl_fonte_energia.Text = "USB";
                        break;
                    case Plugin.Battery.Abstractions.PowerSource.Wireless:
                        lbl_fonte_energia.Text = "Sem Fio";
                        break;
                    case Plugin.Battery.Abstractions.PowerSource.Other:
                        lbl_fonte_energia.Text = "Outro";
                        break;

                }

            }
            catch(Exception ef)
            {
                await DisplayAlert("Ocorreu um erro: \n ", ef.Message, "OK");
            }
        }
        private async void btnOnOff_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!lanterna_ligada)
                {
                    lanterna_ligada = true;

                    btnOnOff.Source = "bton";

                    Vibration.Vibrate(TimeSpan.FromMilliseconds(250));

                    await Flashlight.TurnOnAsync();
                }
                else
                {
                    lanterna_ligada = false;

                    btnOnOff.Source = "btoff";

                    Vibration.Vibrate(TimeSpan.FromMilliseconds(250));

                    await Flashlight.TurnOffAsync();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ocorreu um erro: \n ", ex.Message, "OK");
            }
        }

    }
}

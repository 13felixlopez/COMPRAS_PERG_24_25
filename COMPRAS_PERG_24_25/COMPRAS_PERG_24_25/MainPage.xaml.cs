using System.Collections.Generic;
using Xamarin.Forms;

namespace COMPRAS_PERG_24_25
{
    public partial class MainPage : ContentPage
    {
        double precioBruto = 0;
        double precio, IR = 0.015, Bolsagro = 0.0015, PTO = 0.001, FTDC, Reintegro = 0.015;


        public MainPage()
        {
            InitializeComponent();
            crearlista();
        }
        private void BtnCalcular_Clicked(object sender, System.EventArgs e)
        {
            CalcularPrecioBruto();
            CalcularFTDC();
            Calculos();
        }

        private void BtnBorrar_Clicked(object sender, System.EventArgs e)
        {
            Limpiar();
        }
        private void crearlista()
        {
            // Crear la lista de elementos
            List<string> tiposCafe = new List<string>
            {
                "PELOTA VERDE",
                "PERGAMINO OREADO 1RA",
                "PERGAMINO OREADO 2DA",
                "PERGAMINO OREADO BROZA",
                "PERGAMINO HUMEDO 1RA",
                "PERGAMINO HUMEDO 2DA",
                "PERGAMINO HUMEDO BROZA",
                "PERGAMINO MOJADO 1RA",
                "PERGAMINO MOJADO 2DA",
                "PERGAMINO MOJADO BROZA",
                "PERGAMINO OREADO 1RA DAÑO",
                "PERGAMINO OREADO 2DA DAÑO",
                "PERGAMINO HUMEDO 1RA DAÑO",
                "PERGAMINO HUMEDO 2DA DAÑO",
                "PERGAMINO MOJADO 1RA DAÑO",
                "PERGAMINO MOJADO 2DA DAÑO"
            };

            // Asignar la lista al Picker
            PProducto.ItemsSource = tiposCafe;
        }
        private void CalcularPrecioBruto()
        {
            // Obtener el valor seleccionado del Picker
            string productoSeleccionado = PProducto.SelectedItem?.ToString();

            // Verificar si los campos de precio y incentivo tienen valores válidos
            if (double.TryParse(TxtPrecio.Text, out precio) && double.TryParse(TxtIncent.Text, out double incentivo) && double.TryParse(TxtDeduc.Text, out double Deduccion))
            {

                // Verificar si el producto pertenece al primer grupo (PERGAMINO MOJADO 1RA, 2DA, BROZA)
                if (productoSeleccionado == "PERGAMINO MOJADO 1RA" ||
                    productoSeleccionado == "PERGAMINO MOJADO 2DA" ||
                    productoSeleccionado == "PERGAMINO MOJADO BROZA")
                {
                    precioBruto = precio + (incentivo / 46);
                }
                // Verificar si el producto pertenece al segundo grupo (DAÑOS)
                else if (productoSeleccionado == "PERGAMINO OREADO 1RA DAÑO" ||
                         productoSeleccionado == "PERGAMINO OREADO 2DA DAÑO" ||
                         productoSeleccionado == "PERGAMINO HUMEDO 1RA DAÑO" ||
                         productoSeleccionado == "PERGAMINO HUMEDO 2DA DAÑO" ||
                         productoSeleccionado == "PERGAMINO MOJADO 1RA DAÑO" ||
                         productoSeleccionado == "PERGAMINO MOJADO 2DA DAÑO")
                {
                    precioBruto = precio - (Deduccion / 46);
                }
                // En cualquier otro caso, el Precio Bruto será igual a TxtPrecio
                else
                {
                    precioBruto = precio;
                }

                // Mostrar el resultado (puedes actualizar un Label o un Entry para mostrar el precio bruto)
                LblPrecioBruto.Text = precioBruto.ToString("F2");
            }
            else
            {
                // Manejar el caso en que el precio o incentivo no sean números válidos
                DisplayAlert("Error", "Por favor ingrese valores numéricos válidos en Precio y/o Incentivo.", "OK");
            }
        }
        private void TxtRedimiento_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalcularFTDC();
        }
        private void CalcularFTDC()
        {
            if (double.TryParse(TxtRedimiento.Text, out double rendimiento))
            {
                // Realizar el cálculo del FTDC solo si el rendimiento es válido
                FTDC = (4 * rendimiento / 46) * 36.6243;
            }
            else
            {
                // Si el texto es inválido o nulo, asignar cadena vacía al Label
                FTDC = 0;
            }
        }
        private void Calculos()
        {
            // Declarar variables
            double Imp = 0, ir = 0, bolsagro = 0, pto = 0, ftdc = 0, pesoNeto = 0, reintegro = 0, Principal = 0, Interes = 0, GTOS = 0, Seguro = 0, NetoPagado = 0;

            // Validar que TxtPesoNeto tenga un número válido
            if (double.TryParse(TxtPesoNeto.Text, out pesoNeto) &&
                double.TryParse(TxtPrincipal.Text, out Principal) &&
                double.TryParse(TxtInteres.Text, out Interes) &&
                double.TryParse(TxtGtosLegales.Text, out GTOS) &&
                double.TryParse(TxtSeguro.Text, out Seguro))
            {
                // Cálculo del Impuesto basado en el precio y precioBruto (asumiendo que ya existen)
                Imp = pesoNeto * precioBruto;

                // Cálculos secundarios
                ir = Imp * IR;
                bolsagro = Imp * Bolsagro;
                pto = Imp * PTO;
                ftdc = pesoNeto * FTDC;
                reintegro = Imp * Reintegro;

                // Calcular el Subtotal
                double Subtotal = ir + bolsagro + pto + ftdc + Principal + Interes + GTOS + Seguro;

                // Calcular el Neto Pagado
                NetoPagado = (Imp - Subtotal) + reintegro;

                // Actualizar los Labels con los resultados
                LblImp.Text = Imp.ToString("F2");             // Formateo con dos decimales
                LblIR.Text = ir.ToString("F2");
                LblBolsagro.Text = bolsagro.ToString("F2");
                LblPto.Text = pto.ToString("F2");
                LblFTDC.Text = ftdc.ToString("F2");
                LblReintegro.Text = reintegro.ToString("F2");
                LblNetoPagado.Text = NetoPagado.ToString("N2");
            }
            else
            {
                // Si algún campo no tiene un valor válido, mostrar un mensaje de error o limpiar los Labels
                DisplayAlert("Error", "Por favor, ingrese valores numéricos válidos.", "OK");

                // Limpiar los Labels
                LblImp.Text = LblIR.Text = LblBolsagro.Text = LblPto.Text = LblFTDC.Text = LblReintegro.Text = LblNetoPagado.Text = "";
            }
        }
        private void Limpiar()
        {
            TxtPesoNeto.Text = "";
            TxtPrecio.Text = "";
            LblPrecioBruto.Text = "";
            LblImp.Text = "";
            LblIR.Text = "";
            LblBolsagro.Text = "";
            LblPto.Text = "";
            LblFTDC.Text = "";
            TxtPrincipal.Text = "0";
            TxtInteres.Text = "0";
            TxtGtosLegales.Text = "0";
            TxtSeguro.Text = "0";
            LblReintegro.Text = "";
            LblNetoPagado.Text = "";
        }

    }
}

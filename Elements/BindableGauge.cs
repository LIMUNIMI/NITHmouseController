using System.Globalization;
using System.Windows.Controls;
using NITHlibrary.Nith.Internals;

namespace NITHmouseController.Elements
{
    internal class BindableGauge
    {
        private ComboBox _cbxArg;
        private CheckBox _chkUseProp;
        private TextBox _txtMax;
        private TextBox _txtMin;
        private TextBox _txtOffset;

        public BindableGauge(ProgressBar prbValue, ComboBox cbxArg, TextBox txtMin, TextBox txtMax, TextBox txtOffset, CheckBox chkUseProp)
        {
            this._cbxArg = cbxArg;
            this._txtMin = txtMin;
            this._txtMax = txtMax;
            this._txtOffset = txtOffset;
            this._chkUseProp = chkUseProp;
            PrbGauge = prbValue;

            // Fill the arguments checkbox
            foreach (string item in Enum.GetNames(typeof(NithParameters)))
            {
                cbxArg.Items.Add(item);
            }

            // Assign events
            txtMin.TextChanged += TxtMin_TextChanged;
            txtMax.TextChanged += TxtMax_TextChanged;
            txtOffset.TextChanged += TxtOffset_TextChanged;
            cbxArg.SelectionChanged += CbxArg_SelectionChanged;
            chkUseProp.Checked += ChkUseProp_Checked;
            chkUseProp.Unchecked += ChkUseProp_Unchecked;

            // Initialize txts
            txtMin.Text = Min.ToString("F0");
            txtMax.Text = Max.ToString("F0");
            txtOffset.Text = Offset.ToString("F0");
            cbxArg.SelectedIndex = 0;
        }

        public NithParameters Parameter { get; private set; } = NithParameters.NaP;

        public double Max { get; private set; } = 100;

        public double Min { get; private set; } = 0;

        public NithParameterValue NithArgVal { get; private set; } = new NithParameterValue(NithParameters.NaP, "");
        public double Offset { get; private set; } = 0;

        public ProgressBar PrbGauge { get; private set; }

        public bool UseProp { get; private set; } = false;

        public void ReceiveArgs(List<NithParameterValue> values)
        {
            foreach (NithParameterValue v in values)
            {
                if (v.Parameter == Parameter)
                {
                    NithArgVal = v;
                }
            }
        }

        public void UpdateGraphics()
        {
            try
            {
                if (UseProp && NithArgVal.Type == NithDataTypes.BaseAndMax)
                {
                    PrbGauge.Value = NithArgVal.Normalized + Offset;
                }
                else
                {
                    PrbGauge.Value = double.Parse(NithArgVal.Base, CultureInfo.InvariantCulture) + Offset;
                }
            }
            catch
            {
                // Absolutely ignorable, it simply means the stored NithArgumentValue is not valid. In this case: show nothing. Set Value to 0
                PrbGauge.Value = 0;
            }
        }

        private void CbxArg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Parameter = (NithParameters)Enum.Parse(typeof(NithParameters), _cbxArg.SelectedItem.ToString());
        }

        private void ChkUseProp_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            UseProp = true;
            Max = 100;
            PrbGauge.Maximum = Max;
        }

        private void ChkUseProp_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            UseProp = false;
            TxtMax_TextChanged(null, null);
        }

        private void TxtMax_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!UseProp)
            {
                try
                {
                    Max = double.Parse(_txtMax.Text, CultureInfo.InvariantCulture);
                }
                catch
                {
                    Max = 0;
                }
                PrbGauge.Maximum = Max;
            }
        }

        private void TxtMin_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Min = double.Parse(_txtMin.Text, CultureInfo.InvariantCulture);
            }
            catch
            {
                Min = 0;
            }
            PrbGauge.Minimum = Min;
        }

        private void TxtOffset_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Offset = double.Parse(_txtOffset.Text, CultureInfo.InvariantCulture);
            }
            catch
            {
                Offset = 0;
            }
        }
    }
}
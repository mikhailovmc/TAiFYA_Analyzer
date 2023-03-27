using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TAiFYA_Analyzer
{
    public partial class Analyzer : Form
    {
        private AnalyzerController analyzerController;
        private string chain;
        public Analyzer()
        {
            InitializeComponent();
        }

        private void analyzeButton_Click(object sender, EventArgs e)
        {
            constantTextBox.Text = "";
            identifiersTextBox.Text = "";
            chain = chainBox.Text;
            analyzerController = new AnalyzerController(chain);
            errorTextBox.Visible = true;
            errorTextBox.Text = analyzerController.Analyze();
            if (analyzerController.GetErrorMessage == null)
            {
                if (analyzerController.GetConstantList != null)
                    for (int i = 0; i < analyzerController.GetConstantList.Count; i++)
                        constantTextBox.Text += analyzerController.GetConstantList.ElementAt(i) + Environment.NewLine;
                if (analyzerController.GetIdentifierList != null)
                    for (int i = 0; i < analyzerController.GetIdentifierList.Count; i++)
                        identifiersTextBox.Text += analyzerController.GetIdentifierList.ElementAt(i) + Environment.NewLine;
            }
            else
            {
                chainBox.Focus();
                chainBox.Select(analyzerController.GetErrorPosition - 1, 1);
            }
        }
    }
}

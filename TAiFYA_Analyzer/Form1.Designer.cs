
namespace TAiFYA_Analyzer
{
    partial class Analyzer
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.analyzeButton = new System.Windows.Forms.Button();
            this.nameLabel = new System.Windows.Forms.Label();
            this.chainBox = new System.Windows.Forms.TextBox();
            this.identifiersTextBox = new System.Windows.Forms.TextBox();
            this.constantTextBox = new System.Windows.Forms.TextBox();
            this.errorTextBox = new System.Windows.Forms.TextBox();
            this.idLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // analyzeButton
            // 
            this.analyzeButton.CausesValidation = false;
            this.analyzeButton.Location = new System.Drawing.Point(12, 65);
            this.analyzeButton.Name = "analyzeButton";
            this.analyzeButton.Size = new System.Drawing.Size(131, 23);
            this.analyzeButton.TabIndex = 0;
            this.analyzeButton.Text = "Проанализировать";
            this.analyzeButton.UseVisualStyleBackColor = true;
            this.analyzeButton.Click += new System.EventHandler(this.analyzeButton_Click);
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.CausesValidation = false;
            this.nameLabel.Location = new System.Drawing.Point(12, 9);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(131, 15);
            this.nameLabel.TabIndex = 1;
            this.nameLabel.Text = "Введите предложение:";
            // 
            // chainBox
            // 
            this.chainBox.CausesValidation = false;
            this.chainBox.Location = new System.Drawing.Point(12, 36);
            this.chainBox.Name = "chainBox";
            this.chainBox.Size = new System.Drawing.Size(597, 23);
            this.chainBox.TabIndex = 2;
            // 
            // identifiersTextBox
            // 
            this.identifiersTextBox.CausesValidation = false;
            this.identifiersTextBox.Location = new System.Drawing.Point(12, 130);
            this.identifiersTextBox.Multiline = true;
            this.identifiersTextBox.Name = "identifiersTextBox";
            this.identifiersTextBox.ReadOnly = true;
            this.identifiersTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.identifiersTextBox.Size = new System.Drawing.Size(278, 305);
            this.identifiersTextBox.TabIndex = 3;
            // 
            // constantTextBox
            // 
            this.constantTextBox.CausesValidation = false;
            this.constantTextBox.Location = new System.Drawing.Point(331, 130);
            this.constantTextBox.Multiline = true;
            this.constantTextBox.Name = "constantTextBox";
            this.constantTextBox.ReadOnly = true;
            this.constantTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.constantTextBox.Size = new System.Drawing.Size(278, 305);
            this.constantTextBox.TabIndex = 4;
            // 
            // errorTextBox
            // 
            this.errorTextBox.CausesValidation = false;
            this.errorTextBox.Location = new System.Drawing.Point(150, 66);
            this.errorTextBox.Name = "errorTextBox";
            this.errorTextBox.ReadOnly = true;
            this.errorTextBox.Size = new System.Drawing.Size(459, 23);
            this.errorTextBox.TabIndex = 5;
            this.errorTextBox.Visible = false;
            // 
            // idLabel
            // 
            this.idLabel.AutoSize = true;
            this.idLabel.CausesValidation = false;
            this.idLabel.Location = new System.Drawing.Point(12, 112);
            this.idLabel.Name = "idLabel";
            this.idLabel.Size = new System.Drawing.Size(154, 15);
            this.idLabel.TabIndex = 6;
            this.idLabel.Text = "Таблица идентификаторов";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.CausesValidation = false;
            this.label1.Location = new System.Drawing.Point(331, 112);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 15);
            this.label1.TabIndex = 7;
            this.label1.Text = "Таблица констант";
            // 
            // Analyzer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(621, 447);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.idLabel);
            this.Controls.Add(this.errorTextBox);
            this.Controls.Add(this.constantTextBox);
            this.Controls.Add(this.identifiersTextBox);
            this.Controls.Add(this.chainBox);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.analyzeButton);
            this.Name = "Analyzer";
            this.Text = "Анализатор";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button analyzeButton;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.TextBox chainBox;
        private System.Windows.Forms.TextBox identifiersTextBox;
        private System.Windows.Forms.TextBox constantTextBox;
        private System.Windows.Forms.TextBox errorTextBox;
        private System.Windows.Forms.Label idLabel;
        private System.Windows.Forms.Label label1;
    }
}


namespace ViewCreator
{
    partial class FieldSelectionForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.lstFields = new System.Windows.Forms.ListBox();
            this.btnAddField = new System.Windows.Forms.Button();
            this.lstSelectedFields = new System.Windows.Forms.ListView();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbFunctions = new System.Windows.Forms.ComboBox();
            this.btnCreateView = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(198, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Campos Disponibles:";
            // 
            // lstFields
            // 
            this.lstFields.FormattingEnabled = true;
            this.lstFields.ItemHeight = 16;
            this.lstFields.Location = new System.Drawing.Point(17, 70);
            this.lstFields.Name = "lstFields";
            this.lstFields.Size = new System.Drawing.Size(509, 212);
            this.lstFields.TabIndex = 1;
            // 
            // btnAddField
            // 
            this.btnAddField.Location = new System.Drawing.Point(17, 288);
            this.btnAddField.Name = "btnAddField";
            this.btnAddField.Size = new System.Drawing.Size(139, 40);
            this.btnAddField.TabIndex = 2;
            this.btnAddField.Text = "Agregar Campo >>";
            this.btnAddField.UseVisualStyleBackColor = true;
            this.btnAddField.Click += new System.EventHandler(this.btnAddField_Click);
            // 
            // lstSelectedFields
            // 
            this.lstSelectedFields.HideSelection = false;
            this.lstSelectedFields.Location = new System.Drawing.Point(582, 70);
            this.lstSelectedFields.Name = "lstSelectedFields";
            this.lstSelectedFields.Size = new System.Drawing.Size(509, 212);
            this.lstSelectedFields.TabIndex = 3;
            this.lstSelectedFields.UseCompatibleStateImageBehavior = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(577, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(226, 25);
            this.label2.TabIndex = 4;
            this.label2.Text = "Campos Seleccionados:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(587, 285);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(278, 25);
            this.label3.TabIndex = 5;
            this.label3.Text = "Seleccione funcion de la View:";
            // 
            // cmbFunctions
            // 
            this.cmbFunctions.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbFunctions.FormattingEnabled = true;
            this.cmbFunctions.Location = new System.Drawing.Point(861, 285);
            this.cmbFunctions.Name = "cmbFunctions";
            this.cmbFunctions.Size = new System.Drawing.Size(230, 33);
            this.cmbFunctions.TabIndex = 6;
            // 
            // btnCreateView
            // 
            this.btnCreateView.Location = new System.Drawing.Point(789, 334);
            this.btnCreateView.Name = "btnCreateView";
            this.btnCreateView.Size = new System.Drawing.Size(139, 40);
            this.btnCreateView.TabIndex = 7;
            this.btnCreateView.Text = "Crear vista";
            this.btnCreateView.UseVisualStyleBackColor = true;
            this.btnCreateView.Click += new System.EventHandler(this.btnCreateView_Click);
            // 
            // FieldSelectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1140, 452);
            this.Controls.Add(this.btnCreateView);
            this.Controls.Add(this.cmbFunctions);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lstSelectedFields);
            this.Controls.Add(this.btnAddField);
            this.Controls.Add(this.lstFields);
            this.Controls.Add(this.label1);
            this.Name = "FieldSelectionForm";
            this.Text = "FieldSelectionForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lstFields;
        private System.Windows.Forms.Button btnAddField;
        private System.Windows.Forms.ListView lstSelectedFields;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbFunctions;
        private System.Windows.Forms.Button btnCreateView;
    }
}
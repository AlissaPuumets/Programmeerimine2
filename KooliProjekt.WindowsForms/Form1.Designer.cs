namespace KooliProjekt.WindowsForms
{
    partial class Form1
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
            dataGridView1 = new DataGridView();
            label1 = new Label();
            label2 = new Label();
            idField = new TextBox();
            firstNameField = new TextBox();
            saveCommand = new Button();
            addCommand = new Button();
            deleteCommand = new Button();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            lastNameField = new TextBox();
            emailField = new TextBox();
            phoneField = new TextBox();
            roleField = new TextBox();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Margin = new Padding(3, 2, 3, 2);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(525, 300);
            dataGridView1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(543, 18);
            label1.Name = "label1";
            label1.Size = new Size(21, 15);
            label1.TabIndex = 1;
            label1.Text = "ID:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(543, 47);
            label2.Name = "label2";
            label2.Size = new Size(50, 15);
            label2.TabIndex = 2;
            label2.Text = "Eesnimi:";
            // 
            // idField
            // 
            idField.Location = new Point(610, 15);
            idField.Name = "idField";
            idField.ReadOnly = true;
            idField.Size = new Size(150, 23);
            idField.TabIndex = 3;
            idField.Text = "-1";
            // 
            // firstNameField
            // 
            firstNameField.Location = new Point(610, 44);
            firstNameField.Name = "firstNameField";
            firstNameField.Size = new Size(150, 23);
            firstNameField.TabIndex = 4;
            // 
            // saveCommand
            // 
            saveCommand.Location = new Point(543, 200);
            saveCommand.Name = "saveCommand";
            saveCommand.Size = new Size(75, 26);
            saveCommand.TabIndex = 11;
            saveCommand.Text = "Salvesta";
            saveCommand.UseVisualStyleBackColor = true;
            // 
            // addCommand
            // 
            addCommand.Location = new Point(633, 200);
            addCommand.Name = "addCommand";
            addCommand.Size = new Size(75, 26);
            addCommand.TabIndex = 12;
            addCommand.Text = "Lisa uus";
            addCommand.UseVisualStyleBackColor = true;
            // 
            // deleteCommand
            // 
            deleteCommand.Location = new Point(723, 200);
            deleteCommand.Name = "deleteCommand";
            deleteCommand.Size = new Size(75, 26);
            deleteCommand.TabIndex = 13;
            deleteCommand.Text = "Kustuta";
            deleteCommand.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(543, 76);
            label3.Name = "label3";
            label3.Size = new Size(57, 15);
            label3.TabIndex = 5;
            label3.Text = "Perenimi:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(543, 105);
            label4.Name = "label4";
            label4.Size = new Size(44, 15);
            label4.TabIndex = 7;
            label4.Text = "E-post:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(543, 134);
            label5.Name = "label5";
            label5.Size = new Size(48, 15);
            label5.TabIndex = 9;
            label5.Text = "Telefon:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(543, 163);
            label6.Name = "label6";
            label6.Size = new Size(30, 15);
            label6.TabIndex = 11;
            label6.Text = "Roll:";
            // 
            // lastNameField
            // 
            lastNameField.Location = new Point(610, 73);
            lastNameField.Name = "lastNameField";
            lastNameField.Size = new Size(150, 23);
            lastNameField.TabIndex = 6;
            // 
            // emailField
            // 
            emailField.Location = new Point(610, 102);
            emailField.Name = "emailField";
            emailField.Size = new Size(150, 23);
            emailField.TabIndex = 8;
            // 
            // phoneField
            // 
            phoneField.Location = new Point(610, 131);
            phoneField.Name = "phoneField";
            phoneField.Size = new Size(150, 23);
            phoneField.TabIndex = 10;
            // 
            // roleField
            // 
            roleField.Location = new Point(610, 160);
            roleField.Name = "roleField";
            roleField.Size = new Size(150, 23);
            roleField.TabIndex = 12;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(895, 338);
            Controls.Add(roleField);
            Controls.Add(phoneField);
            Controls.Add(emailField);
            Controls.Add(lastNameField);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(deleteCommand);
            Controls.Add(addCommand);
            Controls.Add(saveCommand);
            Controls.Add(firstNameField);
            Controls.Add(idField);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(dataGridView1);
            Margin = new Padding(3, 2, 3, 2);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView1;
        private Label label1;
        private Label label2;
        private TextBox idField;
        private TextBox firstNameField;
        private Button saveCommand;
        private Button addCommand;
        private Button deleteCommand;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private TextBox lastNameField;
        private TextBox emailField;
        private TextBox phoneField;
        private TextBox roleField;
    }
}

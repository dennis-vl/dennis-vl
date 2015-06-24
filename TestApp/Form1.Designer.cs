namespace TestApp
{
  partial class Form1
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
      this.DataTransferButton = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // DataTransferButton
      // 
      this.DataTransferButton.Location = new System.Drawing.Point(56, 42);
      this.DataTransferButton.Name = "DataTransferButton";
      this.DataTransferButton.Size = new System.Drawing.Size(163, 23);
      this.DataTransferButton.TabIndex = 0;
      this.DataTransferButton.Text = "Start DataTransfer Service";
      this.DataTransferButton.UseVisualStyleBackColor = true;
      this.DataTransferButton.Click += new System.EventHandler(this.DataTransferButton_Click);
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(284, 262);
      this.Controls.Add(this.DataTransferButton);
      this.Name = "Form1";
      this.Text = "Form1";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button DataTransferButton;
  }
}


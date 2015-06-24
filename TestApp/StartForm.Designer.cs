namespace TestApp
{
  partial class StartForm
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
      this.DataProcessorButton = new System.Windows.Forms.Button();
      this.DataTransferButton = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // DataProcessorButton
      // 
      this.DataProcessorButton.Location = new System.Drawing.Point(65, 22);
      this.DataProcessorButton.Name = "DataProcessorButton";
      this.DataProcessorButton.Size = new System.Drawing.Size(154, 23);
      this.DataProcessorButton.TabIndex = 0;
      this.DataProcessorButton.Text = "DataProcessor Start";
      this.DataProcessorButton.UseVisualStyleBackColor = true;
      // 
      // DataTransferButton
      // 
      this.DataTransferButton.Location = new System.Drawing.Point(65, 68);
      this.DataTransferButton.Name = "DataTransferButton";
      this.DataTransferButton.Size = new System.Drawing.Size(154, 23);
      this.DataTransferButton.TabIndex = 1;
      this.DataTransferButton.Text = "DataTransfer Start";
      this.DataTransferButton.UseVisualStyleBackColor = true;
      // 
      // StartForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(291, 143);
      this.Controls.Add(this.DataTransferButton);
      this.Controls.Add(this.DataProcessorButton);
      this.Name = "StartForm";
      this.Text = "StartForm";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button DataProcessorButton;
    private System.Windows.Forms.Button DataTransferButton;
  }
}
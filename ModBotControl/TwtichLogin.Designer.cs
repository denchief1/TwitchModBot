namespace ModBotControl
{
    partial class TwtichLogin
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
            TwitchWebView = new Microsoft.Web.WebView2.WinForms.WebView2();
            ((System.ComponentModel.ISupportInitialize)TwitchWebView).BeginInit();
            SuspendLayout();
            // 
            // TwitchWebView
            // 
            TwitchWebView.AllowExternalDrop = true;
            TwitchWebView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            TwitchWebView.CreationProperties = null;
            TwitchWebView.DefaultBackgroundColor = Color.White;
            TwitchWebView.Location = new Point(2, 0);
            TwitchWebView.Name = "TwitchWebView";
            TwitchWebView.Size = new Size(799, 452);
            TwitchWebView.TabIndex = 0;
            TwitchWebView.ZoomFactor = 1D;
            // 
            // TwtichLogin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(TwitchWebView);
            Name = "TwtichLogin";
            Text = "TwtichLogin";
            ((System.ComponentModel.ISupportInitialize)TwitchWebView).EndInit();
            ResumeLayout(false);
        }

        #endregion

        internal Microsoft.Web.WebView2.WinForms.WebView2 TwitchWebView;
    }
}
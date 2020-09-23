using System.Drawing;

namespace OpenBots.UI.Forms.Supplement_Forms
{
    partial class frmWebElementRecorder
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmWebElementRecorder));
            this.tlpControls = new System.Windows.Forms.TableLayoutPanel();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.pbHome = new System.Windows.Forms.PictureBox();
            this.wbElementRecorder = new Gecko.GeckoWebBrowser();
            this.pbElements = new System.Windows.Forms.PictureBox();
            this.pbSave = new System.Windows.Forms.PictureBox();
            this.pbBack = new System.Windows.Forms.PictureBox();
            this.pbForward = new System.Windows.Forms.PictureBox();
            this.pgGo = new System.Windows.Forms.PictureBox();
            this.tbURL = new System.Windows.Forms.TextBox();
            this.pbRefresh = new System.Windows.Forms.PictureBox();
            this.pbRecord = new System.Windows.Forms.PictureBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.chkStopOnClick = new System.Windows.Forms.CheckBox();
            this.lblSubHeader = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.ttElementRecorder = new System.Windows.Forms.ToolTip(this.components);
            this.tlpControls.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbHome)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbElements)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbForward)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pgGo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRefresh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRecord)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpControls
            // 
            resources.ApplyResources(this.tlpControls, "tlpControls");
            this.tlpControls.Controls.Add(this.pnlHeader, 0, 0);
            this.tlpControls.Name = "tlpControls";
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.pnlHeader.Controls.Add(this.pbHome);
            this.pnlHeader.Controls.Add(this.wbElementRecorder);
            this.pnlHeader.Controls.Add(this.pbElements);
            this.pnlHeader.Controls.Add(this.pbSave);
            this.pnlHeader.Controls.Add(this.pbBack);
            this.pnlHeader.Controls.Add(this.pbForward);
            this.pnlHeader.Controls.Add(this.pgGo);
            this.pnlHeader.Controls.Add(this.tbURL);
            this.pnlHeader.Controls.Add(this.pbRefresh);
            this.pnlHeader.Controls.Add(this.pbRecord);
            this.pnlHeader.Controls.Add(this.lblDescription);
            this.pnlHeader.Controls.Add(this.chkStopOnClick);
            this.pnlHeader.Controls.Add(this.lblSubHeader);
            this.pnlHeader.Controls.Add(this.lblHeader);
            resources.ApplyResources(this.pnlHeader, "pnlHeader");
            this.pnlHeader.Name = "pnlHeader";
            // 
            // pbHome
            // 
            resources.ApplyResources(this.pbHome, "pbHome");
            this.pbHome.Name = "pbHome";
            this.pbHome.TabStop = false;
            this.ttElementRecorder.SetToolTip(this.pbHome, resources.GetString("pbHome.ToolTip"));
            this.pbHome.Click += new System.EventHandler(this.pbHome_Click);
            // 
            // wbElementRecorder
            // 
            resources.ApplyResources(this.wbElementRecorder, "wbElementRecorder");
            this.wbElementRecorder.FrameEventsPropagateToMainWindow = false;
            this.wbElementRecorder.Name = "wbElementRecorder";
            this.wbElementRecorder.UseHttpActivityObserver = false;
            this.wbElementRecorder.Navigated += new System.EventHandler<Gecko.GeckoNavigatedEventArgs>(this.wbElementRecorder_Navigated);
            this.wbElementRecorder.CreateWindow += new System.EventHandler<Gecko.GeckoCreateWindowEventArgs>(this.wbElementRecorder_CreateWindow);
            // 
            // pbElements
            // 
            resources.ApplyResources(this.pbElements, "pbElements");
            this.pbElements.Name = "pbElements";
            this.pbElements.TabStop = false;
            this.ttElementRecorder.SetToolTip(this.pbElements, resources.GetString("pbElements.ToolTip"));
            this.pbElements.Click += new System.EventHandler(this.pbElements_Click);
            // 
            // pbSave
            // 
            resources.ApplyResources(this.pbSave, "pbSave");
            this.pbSave.Name = "pbSave";
            this.pbSave.TabStop = false;
            this.ttElementRecorder.SetToolTip(this.pbSave, resources.GetString("pbSave.ToolTip"));
            this.pbSave.Click += new System.EventHandler(this.pbSave_Click);
            // 
            // pbBack
            // 
            resources.ApplyResources(this.pbBack, "pbBack");
            this.pbBack.Name = "pbBack";
            this.pbBack.TabStop = false;
            this.ttElementRecorder.SetToolTip(this.pbBack, resources.GetString("pbBack.ToolTip"));
            this.pbBack.Click += new System.EventHandler(this.pbBack_Click);
            // 
            // pbForward
            // 
            resources.ApplyResources(this.pbForward, "pbForward");
            this.pbForward.Name = "pbForward";
            this.pbForward.TabStop = false;
            this.ttElementRecorder.SetToolTip(this.pbForward, resources.GetString("pbForward.ToolTip"));
            this.pbForward.Click += new System.EventHandler(this.pbForward_Click);
            // 
            // pgGo
            // 
            resources.ApplyResources(this.pgGo, "pgGo");
            this.pgGo.Name = "pgGo";
            this.pgGo.TabStop = false;
            this.ttElementRecorder.SetToolTip(this.pgGo, resources.GetString("pgGo.ToolTip"));
            this.pgGo.Click += new System.EventHandler(this.pbGo_Click);
            // 
            // tbURL
            // 
            resources.ApplyResources(this.tbURL, "tbURL");
            this.tbURL.Name = "tbURL";
            this.tbURL.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbURL_KeyDown);
            // 
            // pbRefresh
            // 
            resources.ApplyResources(this.pbRefresh, "pbRefresh");
            this.pbRefresh.Name = "pbRefresh";
            this.pbRefresh.TabStop = false;
            this.ttElementRecorder.SetToolTip(this.pbRefresh, resources.GetString("pbRefresh.ToolTip"));
            this.pbRefresh.Click += new System.EventHandler(this.pbRefresh_Click);
            // 
            // pbRecord
            // 
            resources.ApplyResources(this.pbRecord, "pbRecord");
            this.pbRecord.Name = "pbRecord";
            this.pbRecord.TabStop = false;
            this.ttElementRecorder.SetToolTip(this.pbRecord, resources.GetString("pbRecord.ToolTip"));
            this.pbRecord.Click += new System.EventHandler(this.pbRecord_Click);
            // 
            // lblDescription
            // 
            resources.ApplyResources(this.lblDescription, "lblDescription");
            this.lblDescription.ForeColor = System.Drawing.Color.White;
            this.lblDescription.Name = "lblDescription";
            // 
            // chkStopOnClick
            // 
            resources.ApplyResources(this.chkStopOnClick, "chkStopOnClick");
            this.chkStopOnClick.ForeColor = System.Drawing.Color.White;
            this.chkStopOnClick.Name = "chkStopOnClick";
            this.chkStopOnClick.UseVisualStyleBackColor = true;
            // 
            // lblSubHeader
            // 
            resources.ApplyResources(this.lblSubHeader, "lblSubHeader");
            this.lblSubHeader.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.lblSubHeader.Name = "lblSubHeader";
            // 
            // lblHeader
            // 
            resources.ApplyResources(this.lblHeader, "lblHeader");
            this.lblHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblHeader.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblHeader.Name = "lblHeader";
            // 
            // frmWebElementRecorder
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.BackgroundChangeIndex = 265;
            this.Controls.Add(this.tlpControls);
            this.Name = "frmWebElementRecorder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmHTMLElementRecorder_FormClosing);
            this.Load += new System.EventHandler(this.frmHTMLElementRecorder_Load);
            this.tlpControls.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbHome)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbElements)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbForward)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pgGo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRefresh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRecord)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tlpControls;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.PictureBox pgGo;
        private System.Windows.Forms.TextBox tbURL;
        private System.Windows.Forms.PictureBox pbRefresh;
        private System.Windows.Forms.PictureBox pbRecord;
        private System.Windows.Forms.Label lblDescription;
        public System.Windows.Forms.CheckBox chkStopOnClick;
        private System.Windows.Forms.Label lblSubHeader;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.PictureBox pbBack;
        private System.Windows.Forms.PictureBox pbForward;
        private System.Windows.Forms.PictureBox pbSave;
        private System.Windows.Forms.PictureBox pbElements;
        private System.Windows.Forms.ToolTip ttElementRecorder;
        private Gecko.GeckoWebBrowser wbElementRecorder;
        private System.Windows.Forms.PictureBox pbHome;
    }
}
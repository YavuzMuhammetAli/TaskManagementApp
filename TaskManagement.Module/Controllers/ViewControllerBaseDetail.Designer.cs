using System.Collections.Generic;

namespace TaskManagement.Module.Controllers
{
    partial class ViewControllerBaseDetail
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.components = new System.ComponentModel.Container();

            this.sendEmail = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // sendEmail
            // 
            //this.sendEmail.ConfirmationMessage = null;
            //this.sendEmail.Caption = "Send Email";
            //this.sendEmail.Id = "sendEmail";
            //this.sendEmail.ToolTip = null;
            //this.sendEmail.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            //this.sendEmail.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Send);
        }
        private DevExpress.ExpressApp.Actions.SimpleAction sendEmail;
        #endregion
    }
}

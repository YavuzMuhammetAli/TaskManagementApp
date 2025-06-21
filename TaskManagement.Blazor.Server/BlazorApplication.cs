using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor;

namespace TaskManagement.Blazor.Server;

public class TaskManagementBlazorApplication : BlazorApplication
{
    public TaskManagementBlazorApplication()
    {
        ApplicationName = "TaskManagement";
        CheckCompatibilityType = DevExpress.ExpressApp.CheckCompatibilityType.DatabaseSchema;
        DatabaseVersionMismatch += TaskManagementBlazorApplication_DatabaseVersionMismatch;
    }
    protected override void OnSetupStarted()
    {
        base.OnSetupStarted();
//#if DEBUG
        if (System.Diagnostics.Debugger.IsAttached && CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema)
        {
            DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
        }
//#endif
    }

    protected override void OnCustomCheckCompatibility(CustomCheckCompatibilityEventArgs args)
    {
        base.OnCustomCheckCompatibility(args);
//#if RELEASE
        //args.Handled = true;
//#endif
    }

    protected override void OnDatabaseVersionMismatch(DatabaseVersionMismatchEventArgs args)
    {
        base.OnDatabaseVersionMismatch(args);
    }

    private void TaskManagementBlazorApplication_DatabaseVersionMismatch(object sender, DatabaseVersionMismatchEventArgs e)
    {
        try
        {
            e.Updater.Update();
            e.Handled = true;
        }
        catch(Exception ex) 
        {
            string message = "The application cannot connect to the specified database, " +
                "because the database doesn't exist, its version is older " +
                "than that of the application or its schema does not match " +
                "the ORM data model structure. To avoid this error, use one " +
                "of the solutions from the https://www.devexpress.com/kb=T367835 KB Article.";

            if (e.CompatibilityError != null && e.CompatibilityError.Exception != null)
            {
                message += "\r\n\r\nInner exception: " + e.CompatibilityError.Exception.Message;
            }
            throw new InvalidOperationException(message);
        }
    }
}

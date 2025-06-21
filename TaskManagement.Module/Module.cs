using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Xpo;
using TaskManagement.Module.BusinessObjects;
using System.ComponentModel;

namespace TaskManagement.Module;

public sealed class TaskManagementModule : ModuleBase {
    public TaskManagementModule() {
        //
        // TaskManagementModule
        //
        AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.ModelDifference));
        AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.ModelDifferenceAspect));
        AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.BaseObject));
        AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.AuditDataItemPersistent));
        AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.AuditedObjectWeakReference));
        AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.FileData));
        AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.FileAttachmentBase));
        AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.Event));
        AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.Resource));
        AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.HCategory));
        RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.SystemModule.SystemModule));
        RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Security.SecurityModule));
        RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.AuditTrail.AuditTrailModule));
        RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule));
        RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.CloneObject.CloneObjectModule));
        RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.ConditionalAppearance.ConditionalAppearanceModule));
        RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Dashboards.DashboardsModule));
        RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Notifications.NotificationsModule));
        RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Office.OfficeModule));
        RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.ReportsV2.ReportsModuleV2));
        RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Scheduler.SchedulerModuleBase));
        RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.StateMachine.StateMachineModule));
        RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Validation.ValidationModule));
        RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.ViewVariantsModule.ViewVariantsModule));
    }
    public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) {
        ModuleUpdater updater = new DatabaseUpdate.Updater(objectSpace, versionFromDB);
        return new ModuleUpdater[] { updater };
    }


    public override void Setup(XafApplication application) {
        base.Setup(application);
    }
    public override void CustomizeTypesInfo(ITypesInfo typesInfo) {
        base.CustomizeTypesInfo(typesInfo);
        CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);

        var schedularEvent = typesInfo.FindTypeInfo(typeof(Event));
        var schedularEventMember = schedularEvent.FindMember("CreatedBy");
        if(schedularEventMember == null )
        {
            schedularEvent.CreateMember("CreatedBy", typeof(ApplicationUser)).AddAttribute(new BrowsableAttribute(false));
            schedularEvent.CreateMember("CreatedOn", typeof(DateTime)).AddAttribute(new BrowsableAttribute(false));
            schedularEvent.CreateMember("LastModifiedBy", typeof(ApplicationUser)).AddAttribute(new BrowsableAttribute(false));
            schedularEvent.CreateMember("LastModifiedOn", typeof(DateTime)).AddAttribute(new BrowsableAttribute(false));
            typesInfo.RefreshInfo(typeof(Event));
        }
        var schedularResource = typesInfo.FindTypeInfo(typeof(Resource));
        var schedularResourceMember = schedularResource.FindMember("CreatedBy");
        if (schedularResourceMember == null)
        {
            schedularResource.CreateMember("CreatedBy", typeof(ApplicationUser)).AddAttribute(new BrowsableAttribute(false));            
            schedularResource.CreateMember("CreatedOn", typeof(DateTime)).AddAttribute(new BrowsableAttribute(false));
            schedularResource.CreateMember("LastModifiedBy", typeof(ApplicationUser)).AddAttribute(new BrowsableAttribute(false));
            schedularResource.CreateMember("LastModifiedOn", typeof(DateTime)).AddAttribute(new BrowsableAttribute(false));
            typesInfo.RefreshInfo(typeof(Resource));
        }
    }
}

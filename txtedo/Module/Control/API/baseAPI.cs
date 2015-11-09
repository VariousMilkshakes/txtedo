using txtedo.ViewModel;

namespace txtedo.Module.Control.API
{
    public interface baseAPI
    {
        void newTriggers(string[] triggerIDs);
        void trigger(string triggerID);
        void setTriggerHandler(ModuleHandler mh);
    }
}

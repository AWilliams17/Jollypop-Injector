using Registrar;
using Jollypop_Injector.ConfigValidators;

namespace Jollypop_Injector.ConfigContainer
{
    public class Config
    {
        public Validators configValidators = new Validators();
        public RegSettings Settings = new RegSettings(RegBaseKeys.HKEY_CURRENT_USER, "Software/JollypopInjector");

        private void RegisterSettings()
        {
            RegOption unmanagedTarget =     new RegOption("UnmanagedTarget", null, "", typeof(string));
            RegOption unmanagedDLLPath =    new RegOption("UnmanagedDLLPath", null, "", typeof(string));
            RegOption unmanagedBitness =    new RegOption("UnmanagedBitness", configValidators.BitnessValidator, 0, typeof(int));
            RegOption managedTarget =       new RegOption("ManagedTarget", null, "", typeof(string));
            RegOption managedDLLPath =      new RegOption("ManagedDLLPath", null, "", typeof(string));
            RegOption managedNamespace =    new RegOption("ManagedNamespace", null, "", typeof(string));
            RegOption managedClassname =    new RegOption("ManagedClassname", null, "", typeof(string));
            RegOption managedMethodname =   new RegOption("ManagedMethodname", null, "", typeof(string));

            Settings.RegisterSetting("UnmanagedTarget", unmanagedTarget);
            Settings.RegisterSetting("UnmanagedDLLPath", unmanagedDLLPath);
            Settings.RegisterSetting("UnmanagedBitness", unmanagedBitness);
            Settings.RegisterSetting("ManagedTarget", managedTarget);
            Settings.RegisterSetting("ManagedDLLPath", managedDLLPath);
            Settings.RegisterSetting("ManagedNamespace", managedNamespace);
            Settings.RegisterSetting("ManagedClassname", managedClassname);
            Settings.RegisterSetting("ManagedMethodname", managedMethodname);
        }

        public Config()
        {
            RegisterSettings();
        }
    }
}

﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Status_changer.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.8.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("c346rsr")]
        public string loginMF {
            get {
                return ((string)(this["loginMF"]));
            }
            set {
                this["loginMF"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Octm10")]
        public string pwdMF {
            get {
                return ((string)(this["pwdMF"]));
            }
            set {
                this["pwdMF"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=RUMOWS12;Initial Catalog=InvoicesStatuses;MultipleActiveResultSets=tr" +
            "ue;user=mf;password=mf;")]
        public string conString {
            get {
                return ((string)(this["conString"]));
            }
            set {
                this["conString"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool isVisible {
            get {
                return ((bool)(this["isVisible"]));
            }
            set {
                this["isVisible"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=RUMOWS12;Initial Catalog=InvoicesStatuses;MultipleActiveResultSets=tr" +
            "ue;user=mf;password=mf;")]
        public string EventDepot {
            get {
                return ((string)(this["EventDepot"]));
            }
            set {
                this["EventDepot"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=CZPRGS5.cz.tnt.com\\PRODUCTION;Initial Catalog=BPA_RU;Persist Security" +
            " Info=True;User ID=bpa_ru;Password=bpAut0mat10n_RU;")]
        public string BPA_RUConnectionString {
            get {
                return ((string)(this["BPA_RUConnectionString"]));
            }
        }
    }
}

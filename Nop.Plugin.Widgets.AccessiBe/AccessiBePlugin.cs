using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Nop.Core.Domain.Cms;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;

namespace Nop.Plugin.Widgets.AccessiBe
{
    /// <summary>
    /// Represents the plugin implementation
    /// </summary>
    public class AccessiBePlugin : BasePlugin, IWidgetPlugin
    {
        #region Fields

        private readonly AccessiBeSettings _accessiBeSettings;
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly ILocalizationService _localizationService;
        private readonly ISettingService _settingService;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly WidgetSettings _widgetSettings;


        #endregion

        #region Ctor

        public AccessiBePlugin(AccessiBeSettings accessiBeSettings,
            IActionContextAccessor actionContextAccessor,
            ILocalizationService localizationService,
            ISettingService settingService,
            IUrlHelperFactory urlHelperFactory,
            WidgetSettings widgetSettings)
        {
            _accessiBeSettings = accessiBeSettings;
            _actionContextAccessor = actionContextAccessor;
            _localizationService = localizationService;
            _settingService = settingService;
            _urlHelperFactory = urlHelperFactory;
            _widgetSettings = widgetSettings;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext).RouteUrl(AccessiBeDefaults.ConfigurationRouteName);
        }

        /// <summary>
        /// Gets widget zones where this widget should be rendered
        /// </summary>
        /// <returns>Widget zones</returns>
        public IList<string> GetWidgetZones()
        {
            return new List<string> { _accessiBeSettings.WidgetZone };
        }

        /// <summary>
        /// Gets a name of a view component for displaying widget
        /// </summary>
        /// <param name="widgetZone">Name of the widget zone</param>
        /// <returns>View component name</returns>
        public string GetWidgetViewComponentName(string widgetZone)
        {
            if (widgetZone == null)
                throw new ArgumentNullException(nameof(widgetZone));

            return AccessiBeDefaults.VIEW_COMPONENT;
        }

        /// <summary>
        /// Install plugin
        /// </summary>
        public override void Install()
        {
            _settingService.SaveSetting(new AccessiBeSettings
            {
                WidgetZone = PublicWidgetZones.HeadHtmlTag
            });

            _localizationService.AddPluginLocaleResource(new Dictionary<string, string>
            {
                ["Plugins.Widgets.AccessiBe.Fields.Script"] = "Installation script",
                ["Plugins.Widgets.AccessiBe.Fields.Script.Hint"] = "Find your unique installation script on the Installation tab in your account and then copy it into this field.",
                ["Plugins.Widgets.AccessiBe.Fields.Script.Required"] = "Installation script is required",
            });

            base.Install();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override void Uninstall()
        {
            _widgetSettings.ActiveWidgetSystemNames.Remove(AccessiBeDefaults.SystemName);
            _settingService.SaveSetting(_widgetSettings);
            _settingService.DeleteSetting<AccessiBeSettings>();

            _localizationService.DeletePluginLocaleResources("Plugins.Widgets.AccessiBe");

            base.Uninstall();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether to hide this plugin on the widget list page in the admin area
        /// </summary>
        public bool HideInWidgetList => false;

        #endregion
    }
}
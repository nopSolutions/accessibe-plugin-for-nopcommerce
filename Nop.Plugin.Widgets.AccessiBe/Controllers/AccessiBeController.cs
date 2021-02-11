using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Widgets.AccessiBe.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Widgets.AccessiBe.Controllers
{
    [Area(AreaNames.Admin)]
    [AuthorizeAdmin]
    [AutoValidateAntiforgeryToken]
    public class AccessiBeController : BasePluginController
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;

        #endregion

        #region Ctor

        public AccessiBeController(ILocalizationService localizationService,
            INotificationService notificationService,
            IPermissionService permissionService,
            ISettingService settingService,
            IStoreContext storeContext)
        {
            _localizationService = localizationService;
            _notificationService = notificationService;
            _permissionService = permissionService;
            _settingService = settingService;
            _storeContext = storeContext;
        }

        #endregion

        #region Methods

        public IActionResult Configure()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            var storeId = _storeContext.ActiveStoreScopeConfiguration;
            var settings = _settingService.LoadSetting<AccessiBeSettings>(storeId);
            var model = new ConfigurationModel
            {
                Script = settings.Script,
                ActiveStoreScopeConfiguration = storeId
            };
            if (storeId > 0)
            {
                model.Script_OverrideForStore = _settingService.SettingExists(settings, setting => setting.Script, storeId);
            }

            return View("~/Plugins/Widgets.AccessiBe/Views/Configure.cshtml", model);
        }

        [HttpPost]
        public IActionResult Configure(ConfigurationModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            if (!ModelState.IsValid)
                return Configure();

            var storeId = _storeContext.ActiveStoreScopeConfiguration;
            var settings = _settingService.LoadSetting<AccessiBeSettings>(storeId);
            settings.Script = model.Script;
            _settingService.SaveSettingOverridablePerStore(settings, setting => setting.Script, model.Script_OverrideForStore, storeId, false);
            _settingService.ClearCache();

            _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return Configure();
        }

        #endregion
    }
}
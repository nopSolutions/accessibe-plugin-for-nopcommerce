using System;
using Nop.Core;
using Nop.Services.Cms;
using Nop.Services.Logging;

namespace Nop.Plugin.Widgets.AccessiBe.Services
{
    /// <summary>
    /// Represents the plugin service implementation
    /// </summary>
    public class AccessiBeService
    {
        #region Fields

        private readonly AccessiBeSettings _accessiBeSettings;
        private readonly ILogger _logger;
        private readonly IStoreContext _storeContext;
        private readonly IWidgetPluginManager _widgetPluginManager;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public AccessiBeService(AccessiBeSettings accessiBeSettings,
            ILogger logger,
            IStoreContext storeContext,
            IWidgetPluginManager widgetPluginManager,
            IWorkContext workContext)
        {
            _accessiBeSettings = accessiBeSettings;
            _logger = logger;
            _storeContext = storeContext;
            _widgetPluginManager = widgetPluginManager;
            _workContext = workContext;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Handle function and get result
        /// </summary>
        /// <typeparam name="TResult">Result type</typeparam>
        /// <param name="function">Function to execute</param>
        /// <returns>Result</returns>
        private TResult HandleFunction<TResult>(Func<TResult> function)
        {
            try
            {
                //check whether the plugin is active
                if (!PluginActive())
                    return default;

                //execute function
                return function();
            }
            catch (Exception exception)
            {
                //get a short error message
                var detailedException = exception;
                do
                {
                    detailedException = detailedException.InnerException;
                } while (detailedException?.InnerException != null);


                //log errors
                var error = $"{AccessiBeDefaults.SystemName} error: {Environment.NewLine}{exception.Message}";
                _logger.Error(error, exception, _workContext.CurrentCustomer);

                return default;
            }
        }

        /// <summary>
        /// Check whether the plugin is active for the current customer and the current store
        /// </summary>
        /// <returns>Result</returns>
        private bool PluginActive()
        {
            return _widgetPluginManager.IsPluginActive(AccessiBeDefaults.SystemName, _workContext.CurrentCustomer, _storeContext.CurrentStore.Id);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare installation script
        /// </summary>
        /// <returns>Installation script</returns>
        public string PrepareScript()
        {
            return HandleFunction(() => _accessiBeSettings.Script);
        }

        #endregion
    }
}
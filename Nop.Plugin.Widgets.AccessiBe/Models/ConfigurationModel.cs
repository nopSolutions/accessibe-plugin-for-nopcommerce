using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Widgets.AccessiBe.Models
{
    /// <summary>
    /// Represents configuration model
    /// </summary>
    public class ConfigurationModel : BaseNopModel
    {
        #region Properties

        public int ActiveStoreScopeConfiguration { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.AccessiBe.Fields.Script")]
        public string Script { get; set; }
        public bool Script_OverrideForStore { get; set; }

        #endregion
    }
}
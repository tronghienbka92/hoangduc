using Nop.Services.Configuration;
using Nop.Web.Framework.Controllers;
using Nop.Plugin.NhaXeAPI.Models;
using System.Web.Mvc;

namespace Nop.Plugin.NhaXeAPI.Controllers
{
    [AdminAuthorize]
    public class AdminController : BasePluginController
    {
        private readonly ISettingService _settingService;
        private readonly RestServiceSettings _settings;

        public AdminController(
            ISettingService settingService,
            RestServiceSettings settings)
        {
            _settingService = settingService;
            _settings = settings;
        }

        public ActionResult Configure()
        {
            var model = new ConfigureModel()
            {
                ApiToken = _settings.ApiToken,
                CodeName = _settings.CodeName,
                KeyPass = _settings.KeyPass,
                ClientIP = _settings.ClientIP,
                NhanVienId = _settings.NhanVienId,
                THOI_GIAN_GHE_DAT_CHO=_settings.THOI_GIAN_GHE_DAT_CHO,
                isChoPhepHuy = _settings.isChoPhepHuy,
                isChoPhepChuyenVe = _settings.isChoPhepChuyenVe


            };
            
            return View("~/Plugins/NhaXeAPI/Views/Configure.cshtml", model);
        }

        [HttpPost]
        public ActionResult Configure(ConfigureModel model)
        {
            _settings.ApiToken = model.ApiToken;
            _settings.CodeName = model.CodeName;
            _settings.KeyPass = model.KeyPass;
            _settings.ClientIP = model.ClientIP;
            _settings.NhanVienId = model.NhanVienId;
            _settings.THOI_GIAN_GHE_DAT_CHO = model.THOI_GIAN_GHE_DAT_CHO;
            _settings.isChoPhepHuy = model.isChoPhepHuy;
            _settings.isChoPhepChuyenVe = model.isChoPhepChuyenVe;
            _settingService.SaveSetting(_settings);
            SuccessNotification("Settings saved..");

            return View("~/Plugins/NhaXeAPI/Views/Configure.cshtml", model);
        }
    }
}
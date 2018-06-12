using Nop.Core.Configuration;

namespace Nop.Plugin.NhaXeAPI
{
    public class RestServiceSettings : ISettings
    {
        public RestServiceSettings()
        {
            THOI_GIAN_GHE_DAT_CHO = 300;
            isChoPhepHuy = true;
            isChoPhepChuyenVe = false;
        }
        public string ApiToken { get; set; }
        public string CodeName { get; set; }
        public string KeyPass { get; set; }
        public string ClientIP { get; set; }
        public int NhanVienId { get; set; }
        public int THOI_GIAN_GHE_DAT_CHO { get; set; }
        public bool isChoPhepHuy { get; set; }
        public bool isChoPhepChuyenVe { get; set; }
    }
}
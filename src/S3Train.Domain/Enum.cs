using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace S3Train.Domain
{
    public enum ActionWithObject
    {
        Create,
        Update,
        Delete,
        ChangeStatus
    }

    public enum EnumTinhTrang
    {
        [Display(Name = "Trong Kho")]
        [Description("Trong Kho")]
        TrongKho = 1,
        [Display(Name = "Đang Mượn")]
        [Description("Đang Mượn")]
        DangMuon,
        [Display(Name = "Đã Trã")]
        [Description("Đã Trã")]
        DaTra,
        [Display(Name = "Đã Gởi")]
        [Description("Đã Gởi")]
        DaGoi
    }

    public enum EnumDangVanBan
    {
        [Display(Name = "Văn Bản Nội Bộ")]
        [Description("Văn Bản Nội Bộ")]
        NoiBo = 1,
        [Display(Name = "Văn Bản Đến")]
        [Description("Văn Bản Đến")]
        Den,
        [Display(Name = "Văn Bản Đi")]
        [Description("Văn Bản Đi")]
        Di
    }
}
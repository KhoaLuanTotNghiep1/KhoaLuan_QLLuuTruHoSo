using S3Train.Domain;
using S3Train.Model.Dto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace S3Train.Model.User
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Ảnh Đại Diện")]
        public string Avatar { get; set; }

        [Required(ErrorMessage = "Bạn Chưa Điền Email.")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Bạn Chưa Điền Số Điện Thoại.")]
        [Phone]
        [Display(Name = "Số Điện Thoại")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Bạn Chưa Điền Địa Chỉ.")]
        [Display(Name = "Địa Chỉ")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Bạn Chưa Điền Tên Tài Khoản.")]
        [Display(Name = "Tên Tài Khoản")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Bạn Chưa Điền Họ Tên.")]
        [Display(Name = "Họ Và Tên")]
        public string FullName { get; set; }

        public bool Active { get; set; }

        [Required(ErrorMessage = "Bạn Chưa Điền Mật Khẩu.")]
        [StringLength(100, ErrorMessage = "{0} phải dài ít nhất {2} ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật Khẩu")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác Nhận Mật Khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu và xác nhận mật khẩu phải giống nhau.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Bạn Chưa Chọn Quyền Truy Cập.")]
        [Display(Name = "Quyền Truy Cập")]
        public string Role { get; set; }

        [Display(Name = "Ngày Tạo")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Ngày Cập Nhật")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? UpdatedDate { get; set; }

        public UserViewModel()
        {

        }

        public UserViewModel(ApplicationUser user)
        {
            Id = user.Id;
            UserName = user.UserName;
            Email = user.Email;
            FullName = user.FullName;
            Avatar = user.Avatar;
            PhoneNumber = user.PhoneNumber;
            Active = user.Active;
            Address = user.Address;
            CreatedDate = user.CreatedDate;
            UpdatedDate = user.UpdatedDate;
        }
    }
}

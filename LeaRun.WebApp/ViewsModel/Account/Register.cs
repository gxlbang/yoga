using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeaRun.WebApp.ViewsModel.Account
{
    public class Register
    {
        [Required(ErrorMessage = "请输入手机号码")]
        [Display(Name = "手机号码")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "请输入正确的手机号")]
        //[RegularExpression(@"^1(3[0-9]|4[57]|5[0-35-9]|8[0-9]|7[06-8])\d{8}$", ErrorMessage = "请输入正确的手机号码")]
        public string Name { get; set; }

        public string Openid { get; set; }

        [Required(ErrorMessage = "请输入密码")]
        [Display(Name = "密码")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "请输入6—20位的密码")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "请重复输入密码")]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "两次密码不一致")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "请输入验证码")]
        [Display(Name = "验证码")]
        [StringLength(4, MinimumLength = 1, ErrorMessage = "请输入验证码")]
        public string ValidCode { get; set; }
    }
}
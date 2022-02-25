using System;
using System.ComponentModel.DataAnnotations;

namespace LabMVCQRCode.Models
{
public class AuthResponseModel


    {
        
        [Display(Name = "orderRef")]
        public string OrderRef { get; set; }
        [Display(Name = "autoStartToken")]
        public string AutoStartToken { get; set; }
        [Display(Name = "qrStartToken")]
        public string QRStartToken { get; set; }
        [Display(Name = "qrStartSecret")]
        public string QRStartSecret { get; set; }

        
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace LabMVCQRCode.Models
{
public class QRCodeModel


    {
        
        [Display(Name = "qRStartToken")]
        public string QRStartToken { get; set; }
        [Display(Name = "qRStartSecret")]
        public string QRStartSecret { get; set; }

        [Display(Name = "qRTime")]
        public string QRTime { get; set; } = String.Empty;
        
    }
}

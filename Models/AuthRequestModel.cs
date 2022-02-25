using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace LabMVCQRCode.Models
{
    public class AuthRequestModel


    {
        [Display(Name = "personalNumber")]
        public string personalNumber { get; set; }
        [Display(Name = "endUserIp")]
        public string endUserIp { get; set; }
        [Display(Name = "requirement")]
        public Requirement requirement { get; set; }
    }


}
public class Requirement
{
    [Display(Name = "certificatePolicies")]
    public List<string> certificatePolicies { get; set; }
    [Display(Name = "tokenStartRequired")]
    public bool tokenStartRequired { get; set; }
}

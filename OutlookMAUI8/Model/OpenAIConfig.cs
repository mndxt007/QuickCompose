using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace OutlookMAUI8.Model
{
   
        public class OpenAIConfig
        {
            [Required(ErrorMessage = "Endpoint is required.")]
            [Url(ErrorMessage = "Please enter a valid URL for Endpoint.")]
            public string Endpoint { get; set; } = default!;

            [Required(ErrorMessage = "Key is required.")]
            public string Key { get; set; } = default!;

            [Required(ErrorMessage = "Deployment is required.")]
            public string Deployment { get; set; } = default!;

            public string UseDefault { get; set; } = default!;
        }
    

}

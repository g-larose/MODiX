using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODiX.Data.Models
{
    public class UrlScanner
    {
        public string message { get; set; }
        public bool success { get; set; }
        public bool @unsafe { get; set; }
        public string domain { get; set; }
        public string root_domain { get; set; }
        public string server { get; set; }
        public string content_type { get; set; }
        public int status_code { get; set; }
        public int page_size { get; set; }
        public int domain_rank { get; set; }
        public bool dns_valid { get; set; }
        public bool parking { get; set; }
        public bool spamming { get; set; }
        public bool malware { get; set; }
        public bool phishing { get; set; }
        public bool suspicious { get; set; }
        public bool adult { get; set; }
        public int risk_score { get; set; }
        public string country_code { get; set; }
        public string category { get; set; }
        public DomainAge domain_age { get; set; }
        public string domain_trust { get; set; }
        public bool redirected { get; set; }
        public bool short_link_redirect { get; set; }
        public bool hosted_content { get; set; }
        public string page_title { get; set; }
        public bool risky_tld { get; set; }
        public bool spf_record { get; set; }
        public bool dmarc_record { get; set; }
        public List<object> technologies { get; set; }
        public List<object> a_records { get; set; }
        public List<object> mx_records { get; set; }
        public List<object> ns_records { get; set; }
        public string ip_address { get; set; }
        public string language_code { get; set; }
        public string final_url { get; set; }
        public string request_id { get; set; }
    }
}

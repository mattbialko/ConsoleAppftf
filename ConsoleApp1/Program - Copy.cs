using CsvHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp11
{
    class Program22
    {
        public class contact
        {
            public string id { get; set; }
            public string campaign { get; set; }
            public string name { get; set; }
            public string email { get; set; }
            public string sign_up { get; set; }
            public string optin { get; set; }
            public string origin { get; set; }
            public string ip { get; set; }
            public string score { get; set; }
            public string engagement_score { get; set; }
            public string geo_ip { get; set; }
            public string geo_latitude { get; set; }
            public string geo_longitude { get; set; }
            public string geo_country { get; set; }
            public string geo_region { get; set; }
            public string geo_city { get; set; }
            public string geo_continent_code { get; set; }
            public string geo_country_code { get; set; }
            public string geo_region_code { get; set; }
            public string geo_postal_code { get; set; }
            public string geo_dma_code { get; set; }
            public string geo_time_zone { get; set; }
            public string tag_already_optedin_dreamer { get; set; }
            public string tag_kajabi_dreamers { get; set; }
            public string custom_birthdate { get; set; }
            public string custom_city { get; set; }
            public string custom_comment { get; set; }
            public string custom_company { get; set; }
            public string custom_country { get; set; }
            public string custom_fax { get; set; }
            public string custom_first_name { get; set; }
            public string custom_gender { get; set; }
            public string custom_home_phone { get; set; }
            public string custom_http_referer { get; set; }
            public string custom_last_name { get; set; }
            public string custom_member_number { get; set; }
            public string custom_membership_expiry { get; set; }
            public string custom_membership_level { get; set; }
            public string custom_membership_start { get; set; }
            public string custom_mobile_phone { get; set; }
            public string custom_new_dreamer { get; set; }
            public string custom_phone { get; set; }
            public string custom_postal_code { get; set; }
            public string custom_ref { get; set; }
            public string custom_state { get; set; }
            public string custom_street { get; set; }
            public string custom_url { get; set; }
            public string custom_work_phone { get; set; }
            public string last_update { get; set; }
            public string dayOfCycle { get; set; }
        }
        static void M222ain(string[] args)
        {
            List<string> listOContacts = new List<string>();
            List<Root> dcdc = new List<Root>();
            List<contact> exportObj = new List<contact>();

            string grurl = "https://api.getresponse.com/v3/contacts?perPage=1000";

            int pageNumber = 1;
            for (int i = 1; i < 10; i++)
            {
                string url = grurl + "&page=" + (pageNumber + i);
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Headers.Add("X-Auth-Token", "api-key 092bbb5f663a188accf9eab9c857cfcc");
                request.Proxy = new WebProxy("127.0.0.1:8888");

                request.Method = "GET";
                String test = String.Empty;


                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    test = reader.ReadToEnd();

                    var bla = JsonConvert.DeserializeObject<dynamic>(test);
                    foreach (var item in bla)
                    {
                        listOContacts.Add(item.href.Value);
                    }

                    reader.Close();
                    dataStream.Close();
                }
            }

            foreach (var contactUrl in listOContacts.Take(7000))
            {
                string url = contactUrl;
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Headers.Add("X-Auth-Token", "api-key 092bbb5f663a188accf9eab9c857cfcc");
                request.Proxy = new WebProxy("127.0.0.1:8888");

                request.Method = "GET";
                String test = String.Empty;


                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    test = reader.ReadToEnd();

                    var bla = JsonConvert.DeserializeObject<dynamic>(test);

                    Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(test);

                    dcdc.Add(myDeserializedClass);

                    reader.Close();
                    dataStream.Close();
                }
            }

            foreach (var contact in dcdc)
            {
                exportObj.Add(new contact()
                {
                    id = contact.contactId,
                    campaign = contact.campaign.name,
                    name = contact.name,
                    email = contact.email,
                    sign_up = contact.createdOn.Value.ToString(),
                    optin = "Single",
                    origin = "api",
                    ip = contact.ipAddress,
                    score = "",
                    engagement_score = contact.engagementScore,
                    geo_ip = contact.ipAddress,
                    geo_latitude = contact.geolocation.latitude,
                    geo_longitude = contact.geolocation.longitude,
                    geo_country = contact.geolocation.countryCode,
                    geo_region = contact.geolocation.region,
                    geo_city = contact.geolocation.city,
                    geo_continent_code = contact.geolocation.continentCode,
                    geo_country_code = contact.geolocation.countryCode,
                    geo_region_code = contact.geolocation.region,
                    geo_postal_code = contact.geolocation.postalCode,
                    geo_dma_code = contact.geolocation.dmaCode,
                    geo_time_zone = contact.timeZone,
                    tag_already_optedin_dreamer = contact.geolocation.city,
                    tag_kajabi_dreamers = "0",
                    custom_birthdate = "0",
                    custom_city = contact.customFieldValues.FirstOrDefault(xx => xx.name == "city") != null ? contact.customFieldValues.FirstOrDefault(xx => xx.name == "city").value.FirstOrDefault() : null,
                    custom_comment = "",
                    custom_company = "",
                    custom_country = contact.customFieldValues.FirstOrDefault(xx => xx.name == "country") != null ? contact.customFieldValues.FirstOrDefault(xx => xx.name == "country").value.FirstOrDefault() : null,
                    custom_fax = contact.customFieldValues.FirstOrDefault(xx => xx.name == "fax") != null ? contact.customFieldValues.FirstOrDefault(xx => xx.name == "fax").value.FirstOrDefault() : null,
                    custom_first_name = contact.customFieldValues.FirstOrDefault(xx => xx.name == "first_name") != null ? contact.customFieldValues.FirstOrDefault(xx => xx.name == "first_name").value.FirstOrDefault() : null,
                    custom_gender = contact.customFieldValues.FirstOrDefault(xx => xx.name == "gender") != null ? contact.customFieldValues.FirstOrDefault(xx => xx.name == "gender").value.FirstOrDefault() : null,
                    custom_home_phone = contact.customFieldValues.FirstOrDefault(xx => xx.name == "home_phone") != null ? contact.customFieldValues.FirstOrDefault(xx => xx.name == "home_phone").value.FirstOrDefault() : null,
                    custom_http_referer = contact.customFieldValues.FirstOrDefault(xx => xx.name == "http_referer") != null ? contact.customFieldValues.FirstOrDefault(xx => xx.name == "http_referer").value.FirstOrDefault() : null,
                    custom_last_name = contact.customFieldValues.FirstOrDefault(xx => xx.name == "last_name") != null ? contact.customFieldValues.FirstOrDefault(xx => xx.name == "last_name").value.FirstOrDefault() : null,
                    custom_member_number = contact.customFieldValues.FirstOrDefault(xx => xx.name == "member_number") != null ? contact.customFieldValues.FirstOrDefault(xx => xx.name == "member_number").value.FirstOrDefault() : null,
                    custom_membership_expiry = contact.customFieldValues.FirstOrDefault(xx => xx.name == "membership_expiry") != null ? contact.customFieldValues.FirstOrDefault(xx => xx.name == "membership_expiry").value.FirstOrDefault() : null,
                    custom_membership_level = contact.customFieldValues.FirstOrDefault(xx => xx.name == "membership_level") != null ? contact.customFieldValues.FirstOrDefault(xx => xx.name == "membership_level").value.FirstOrDefault() : null,
                    custom_membership_start = contact.customFieldValues.FirstOrDefault(xx => xx.name == "membership_start") != null ? contact.customFieldValues.FirstOrDefault(xx => xx.name == "membership_start").value.FirstOrDefault() : null,
                    custom_mobile_phone = contact.customFieldValues.FirstOrDefault(xx => xx.name == "mobile_phone") != null ? contact.customFieldValues.FirstOrDefault(xx => xx.name == "mobile_phone").value.FirstOrDefault() : null,
                    custom_new_dreamer = contact.customFieldValues.FirstOrDefault(xx => xx.name == "new_dreamer") != null ? contact.customFieldValues.FirstOrDefault(xx => xx.name == "new_dreamer").value.FirstOrDefault() : null,
                    custom_phone = contact.customFieldValues.FirstOrDefault(xx => xx.name == "phone") != null ? contact.customFieldValues.FirstOrDefault(xx => xx.name == "phone").value.FirstOrDefault() : null,
                    custom_postal_code = contact.customFieldValues.FirstOrDefault(xx => xx.name == "postal_code") != null ? contact.customFieldValues.FirstOrDefault(xx => xx.name == "postal_code").value.FirstOrDefault() : null,
                    custom_ref = contact.customFieldValues.FirstOrDefault(xx => xx.name == "ref") != null ? contact.customFieldValues.FirstOrDefault(xx => xx.name == "ref").value.FirstOrDefault() : null,
                    custom_state = contact.customFieldValues.FirstOrDefault(xx => xx.name == "state") != null ? contact.customFieldValues.FirstOrDefault(xx => xx.name == "state").value.FirstOrDefault() : null,
                    custom_street = contact.customFieldValues.FirstOrDefault(xx => xx.name == "street") != null ? contact.customFieldValues.FirstOrDefault(xx => xx.name == "street").value.FirstOrDefault() : null,
                    custom_url = contact.customFieldValues.FirstOrDefault(xx => xx.name == "url") != null ? contact.customFieldValues.FirstOrDefault(xx => xx.name == "url").value.FirstOrDefault() : null,
                    custom_work_phone = contact.customFieldValues.FirstOrDefault(xx => xx.name == "work_phone") != null ? contact.customFieldValues.FirstOrDefault(xx => xx.name == "work_phone").value.FirstOrDefault() : null,
                    last_update = contact.changedOn != null ? contact.changedOn.Value.ToString() : null,
                    dayOfCycle = contact.dayOfCycle,
                });
            }

            using (var writer = new StreamWriter("test.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(exportObj);
            }
        }
    }
    public class Campaign
    {
        public string campaignId { get; set; }
        public string href { get; set; }
        public string name { get; set; }
    }

    public class Geolocation
    {
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string continentCode { get; set; }
        public string countryCode { get; set; }
        public string region { get; set; }
        public string postalCode { get; set; }
        public string dmaCode { get; set; }
        public string city { get; set; }
    }

    public class CustomFieldValue
    {
        public string customFieldId { get; set; }
        public string name { get; set; }
        public List<string> value { get; set; }
        public List<string> values { get; set; }
        public string type { get; set; }
        public string fieldType { get; set; }
        public string valueType { get; set; }
    }

    public class Root
    {
        public string contactId { get; set; }
        public string href { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public object note { get; set; }
        public string origin { get; set; }
        public string dayOfCycle { get; set; }
        public DateTime? changedOn { get; set; }
        public string timeZone { get; set; }
        public string ipAddress { get; set; }
        public string activities { get; set; }
        public Campaign campaign { get; set; }
        public DateTime? createdOn { get; set; }
        public Geolocation geolocation { get; set; }
        public List<object> tags { get; set; }
        public List<CustomFieldValue> customFieldValues { get; set; }
        public object scoring { get; set; }
        public string engagementScore { get; set; }
    }


}

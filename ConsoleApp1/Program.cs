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

namespace ConsoleApp1
{
    class Program
    {
        static List<T> GetCSVData<T>(string FilePath)
        {
            var csv = new CsvReader(File.OpenText(FilePath), System.Globalization.CultureInfo.CurrentCulture);
            
            csv.Configuration.Delimiter = ",";
           
            var bla = csv.GetRecords<T>().ToList();

            return bla;
        }
        static void Main(string[] args)
        {
            var currentMembers = GetCSVData<CurrentMembers>(@"C:\Users\mattb\Downloads\woocommerce\CurrentMembers.csv");
            var users = GetCSVData<wpUser>(@"C:\Users\mattb\Downloads\woocommerce\users.csv");
            var orders = GetCSVData<dynamic>(@"C:\Users\mattb\Downloads\woocommerce\orders.csv");

            var postMetas = new List<PostMeta>();
            var posts = new List<Post>();

            int startingPostId = 90000;

            var nonexpiredmembers = currentMembers.Where(xx => xx.account_state != "expired" && (xx.membership_level == "3" || xx.membership_level == "4"));
            foreach (var item in nonexpiredmembers)
            {
                var postId = startingPostId.ToString();

                //new post
                var post = new Post
                {
                    ID = postId,
                    post_author = "1",
                    post_date = item.member_since + " 12:00:00",
                    post_date_gmt = item.member_since + " 12:00:00",
                    post_content = "Subscription &ndash; " + Convert.ToDateTime(item.member_since).ToString("MMM dd, yyyy") + " @ 12:00 AM",
                    post_title = "",
                    post_excerpt = "",
                    post_status = "wc-active",
                    comment_status = "closed",
                    ping_status = "closed",
                    post_password = "order_5f90e1d272c33",
                    post_name = "subscription-" + Convert.ToDateTime(item.member_since).ToString("MMM-dd-yyyy").ToLower() + "-1200-am",
                    to_ping = "",
                    pinged = "",
                    post_modified = item.member_since + " 12:00:00",
                    post_modified_gmt = item.member_since + " 12:00:00",
                    post_content_filtered = "",
                    post_parent = "0",
                    guid = "https://www.fulltimefamilies.com/?post_type=shop_subscription&#038;p=" + postId,
                    menu_order = "0",
                    post_type = "shop_subscription",
                    post_mime_type = "",
                    comment_count = "0",
                };

                var user = users.FirstOrDefault(xx => xx.user_login == item.user_name);
                var order = orders.FirstOrDefault(xx => xx.subscr_id != "" && xx.subscr_id == item.subscr_id);

                if (user == null)
                    continue;
                if (order == null)
                    continue;

                var billingPeriod = "";
                var userId = user.ID;
                var orderTotal = order.sale_amount;
                var nextPayment = Convert.ToDateTime(order.date).AddYears(1).ToString("yyyy-MM-dd");

                if (item.membership_level == "3" || item.membership_level == "4")
                {
                    billingPeriod = "year";
                }
                if (item.membership_level == "3")
                {
                    post.ORDER_ITEM_NAME = "Magazine Subscribers";
                }
                if (item.membership_level == "4")
                {
                    post.ORDER_ITEM_NAME = "Annual Family Member";
                }

                posts.Add(post);
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_order_currency", meta_value = "USD" });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_prices_include_tax", meta_value = "no" });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_created_via", meta_value = "msb_importer" });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_billing_period", meta_value = billingPeriod });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_billing_interval", meta_value = "1" });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_customer_user", meta_value = userId });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_order_version", meta_value = "4.6.0" });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_schedule_start", meta_value = item.member_since + " 12:00:00" });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_subscription_renewal_order_ids_cache", meta_value = "a:0:{}" });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_order_shipping", meta_value = "0" });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_order_shipping_tax", meta_value = "0" });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_cart_discount", meta_value = "0" });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_cart_discount_tax", meta_value = "0" });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_order_total", meta_value = orderTotal });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_order_tax", meta_value = "" });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_billing_first_name", meta_value = item.first_name });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_billing_last_name", meta_value = item.last_name });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_billing_company", meta_value = "" });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_billing_address_1", meta_value = item.address_street });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_billing_address_2", meta_value = "" });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_billing_city", meta_value = item.address_city });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_billing_state", meta_value = item.address_state });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_billing_postcode", meta_value = item.address_zipcode });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_billing_country", meta_value = item.country });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_billing_email", meta_value = item.email});
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_billing_phone", meta_value = item.phone });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_shipping_first_name", meta_value = item.first_name });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_shipping_last_name", meta_value = item.last_name });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_shipping_company", meta_value = "" });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_shipping_address_1", meta_value = item.address_street });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_shipping_address_2", meta_value = "" });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_shipping_city", meta_value = item.address_city });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_shipping_state", meta_value = item.address_state });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_shipping_postcode", meta_value = item.address_zipcode });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_shipping_country", meta_value = item.country });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_payment_method", meta_value = "paypal" });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_payment_method_title", meta_value = "PayPal" });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_subscription_resubscribe_order_ids_cache", meta_value = "a:0:{}" });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_subscription_switch_order_ids_cache", meta_value = "a:0:{}" });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_schedule_trial_end", meta_value = "0" });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_schedule_next_payment", meta_value = nextPayment + " 12:00:00" });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_schedule_cancelled", meta_value = "0" });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_schedule_end", meta_value = nextPayment + " 12:00:00" });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_schedule_payment_retry", meta_value = "0" });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_paypal_subscription_id", meta_value = item.subscr_id });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_suspension_count", meta_value = "0" });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_cancelled_email_sent", meta_value = "" });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_requires_manual_renewal", meta_value = "false" });
                postMetas.Add(new PostMeta {post_id = postId, meta_key = "_trial_period", meta_value = "" });

                startingPostId++;
            }
            int currentOrderId = 55;

            posts = posts.Where(xx => xx.ID != "90000" && xx.ID != "90001").ToList();
            postMetas = postMetas.Where(xx => xx.post_id != "90000" && xx.post_id != "90001").ToList();

            var postInsert = "INSERT INTO `wp_mqlm7e80r3_posts` (`ID`, `post_author`, `post_date`, `post_date_gmt`, `post_content`, `post_title`, `post_excerpt`, `post_status`, `comment_status`, `ping_status`, `post_password`, `post_name`, `to_ping`, `pinged`, `post_modified`, `post_modified_gmt`, `post_content_filtered`, `post_parent`, `guid`, `menu_order`, `post_type`        , `post_mime_type`, `comment_count`) VALUES";
            var postInsertValue = "('{0}', '{1}', '{2}', '{3}', '', '{4}', '', 'wc-active'  , 'closed', 'closed', '{5}', '{6}' , '', '', '{7}', '{8}', '', '0', '{9}' , '0', 'shop_subscription', '', '0')";
            var postSql = postInsert;
            foreach (var item in posts)
            {
                var sql = string.Format(postInsertValue
                    , item.ID
                    , item.post_author
                    , item.post_date
                    , item.post_date_gmt
                    , item.post_title
                    , item.post_password
                    , item.post_name
                    , item.post_modified
                    , item.post_modified_gmt
                    , item.guid
                );

                postSql += sql + ",";
            }
            postSql = postSql.TrimEnd(',') + ";";

            var postMetaInsert = "INSERT INTO `wp_mqlm7e80r3_postmeta` (`post_id`, `meta_key`, `meta_value`) VALUES";
            var postMetaInsertValue = "('{0}', '{1}', '{2}')";
            var postMetaSql = postMetaInsert;
            foreach (var item in postMetas)
            {
                var sql = string.Format(postMetaInsertValue
                    , item.post_id
                    , item.meta_key
                    , item.meta_value
                );

                postMetaSql += sql + ",";
            }
            postMetaSql = postMetaSql.TrimEnd(',') + ";";

            var orderItemsInsert = "INSERT INTO `wp_mqlm7e80r3_woocommerce_order_items` (`ORDER_ITEM_NAME`,`ORDER_ITEM_TYPE`,`ORDER_ID`) VALUES";
            var orderItemsInsertValue = "('{0}', 'line_item', '{1}')";
            var orderItemsSql = orderItemsInsert;
            foreach (var item in posts)
            {
                var sql = string.Format(orderItemsInsertValue
                    , item.ORDER_ITEM_NAME
                    , item.ID
                );

                orderItemsSql += sql + ",";
            }
            orderItemsSql = orderItemsSql.TrimEnd(',') + ";";

            var orderItemsMetaInsert = "INSERT INTO `wp_mqlm7e80r3_woocommerce_order_itemmeta` (`ORDER_ITEM_ID`,`META_KEY`,`META_VALUE`) VALUES";
            var orderItemsMetaInsertValue = "('{0}', '{1}', '{2}')";
            var orderItemsMetaSql = orderItemsMetaInsert;
            foreach (var item in postMetas.Where(xx => xx.meta_key == "_order_total"))
            {
                string scurrentOrderId = currentOrderId.ToString();

                orderItemsMetaSql += string.Format(orderItemsMetaInsertValue, scurrentOrderId, "_product_id", "88875") + ", ";
                orderItemsMetaSql += string.Format(orderItemsMetaInsertValue, scurrentOrderId, "_variation_id", "0") + ", ";
                orderItemsMetaSql += string.Format(orderItemsMetaInsertValue, scurrentOrderId, "_qty", "1") + ", ";
                orderItemsMetaSql += string.Format(orderItemsMetaInsertValue, scurrentOrderId, "_tax_class", "zero-rate") + ", ";
                orderItemsMetaSql += string.Format(orderItemsMetaInsertValue, scurrentOrderId, "_line_subtotal", item.meta_value) + ", ";
                orderItemsMetaSql += string.Format(orderItemsMetaInsertValue, scurrentOrderId, "_line_subtotal_tax", "0") + ", ";
                orderItemsMetaSql += string.Format(orderItemsMetaInsertValue, scurrentOrderId, "_line_total", item.meta_value ) + ", ";
                orderItemsMetaSql += string.Format(orderItemsMetaInsertValue, scurrentOrderId, "_line_tax", "0" ) + ", ";
                orderItemsMetaSql += string.Format(orderItemsMetaInsertValue, scurrentOrderId, "_line_tax_data", "a:2:{s:8:\"subtotal\";a:0:{}s:5:\"total\";a:0:{}}") + ",";

                currentOrderId++;
            }
            orderItemsMetaSql = orderItemsMetaSql.TrimEnd(',') + ";";

            string finalSql = "";
            finalSql += postSql;
            finalSql += postMetaSql;
            finalSql += orderItemsSql;
            finalSql += orderItemsMetaSql;
        }

        class OrderItem
        {
            public string ORDER_ITEM_NAME { get; set; }
            public string ORDER_ID { get; set; }
        }

        class CurrentMembers
        {
            public string member_id { get; set; }
            public string user_name { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string password { get; set; }
            public string member_since { get; set; }
            public string membership_level { get; set; }
            public string more_membership_levels { get; set; }
            public string more_membership_levels_start_date { get; set; }
            public string account_state { get; set; }
            public string last_accessed { get; set; }
            public string last_accessed_from_ip { get; set; }
            public string email { get; set; }
            public string phone { get; set; }
            public string address_street { get; set; }
            public string address_city { get; set; }
            public string address_state { get; set; }
            public string address_zipcode { get; set; }
            public string home_page { get; set; }
            public string country { get; set; }
            public string gender { get; set; }
            public string referrer { get; set; }
            public string extra_info { get; set; }
            public string reg_code { get; set; }
            public string subscription_starts { get; set; }
            public string autoupgrade_starts { get; set; }
            public string initial_membership_level { get; set; }
            public string txn_id { get; set; }
            public string subscr_id { get; set; }
            public string company_name { get; set; }
            public string notes { get; set; }
            public string flags { get; set; }
            public string profile_image { get; set; }
            public string expiry_1st { get; set; }
            public string expiry_2nd { get; set; }
            public string title { get; set; }
            public string ip_to_country { get; set; }

        }
        
        class Post
        {
            public string ID { get; set; }
            public string post_author { get; set; }
            public string post_date { get; set; }
            public string post_date_gmt { get; set; }
            public string post_content { get; set; }
            public string post_title { get; set; }
            public string post_excerpt { get; set; }
            public string post_status { get; set; }
            public string comment_status { get; set; }
            public string ping_status { get; set; }
            public string post_password { get; set; }
            public string post_name { get; set; }
            public string to_ping { get; set; }
            public string pinged { get; set; }
            public string post_modified { get; set; }
            public string post_modified_gmt { get; set; }
            public string post_content_filtered { get; set; }
            public string post_parent { get; set; }
            public string guid { get; set; }
            public string menu_order { get; set; }
            public string post_type { get; set; }
            public string post_mime_type { get; set; }
            public string comment_count { get; set; }

            public string ORDER_ITEM_NAME { get; set; }
        }
        class wpUser
        {
            public string ID { get; set; }
            public string user_login { get; set; }
            public string user_pass { get; set; }
            public string user_nicename { get; set; }
            public string user_email { get; set; }
            public string user_url { get; set; }
            public string user_registered { get; set; }
            public string user_activation_key { get; set; }
            public string user_status { get; set; }
            public string display_name { get; set; }

        }
        class PostMeta
        {
            public string meta_id { get; set; }
            public string post_id { get; set; }
            public string meta_key { get; set; }
            public string meta_value { get; set; }
        }
        //    static List<Tag> gettags()
        //    {
        //        var grurl = "https://fulltimefamilies93530.api-us1.com/api/3/";
        //        var tags_raw = WebGet(grurl + "tags?limit=100");
        //        var tags_js = JsonConvert.DeserializeObject<dynamic>(tags_raw);
        //        var tags = JsonConvert.DeserializeObject<List<Tag>>(tags_js.tags.ToString());

        //        return tags;
        //    }

        //    static void GetContactTags()
        //    {
        //        //var contactTags_raw = WebGet(contact.links.contactTags);
        //        //var contactTags_js = JsonConvert.DeserializeObject<dynamic>(contactTags_raw);
        //        //var contactTags = JsonConvert.DeserializeObject<List<ContactTag>>(contactTags_js.contactTags.ToString());
        //    }
        //    static void Main_Annual(string[] args)
        //    {
        //        var grurl = "https://fulltimefamilies93530.api-us1.com/api/3/";
        //        string automation_test_listid = "7";
        //        string annual_members_listid = "2";

        //        //Get All Contacts in a list
        //        var listId = annual_members_listid;
        //        var url = grurl;

        //        var customFields_raw = WebGet(grurl + "fields?limit=100");
        //        var customFields_js = JsonConvert.DeserializeObject<dynamic>(customFields_raw);
        //        FieldsResponse.Root customFields = JsonConvert.DeserializeObject<FieldsResponse.Root>(customFields_js.ToString());

        //        var customerField_MembershipStart = customFields.fields.FirstOrDefault(xx => xx.title == "custom:membership_start");

        //        var allContacts = new List<Contact>();

        //        for (int i = 0; i < 20; i++)
        //        {
        //            var listContacts_raw = WebGet(grurl + "contacts?offset="+i*100+"&limit=100&listid=" + listId);
        //            var listContacts_js = JsonConvert.DeserializeObject<dynamic>(listContacts_raw);
        //            var listContacts = JsonConvert.DeserializeObject<List<Contact>>(listContacts_js.contacts.ToString());

        //            allContacts.AddRange(listContacts);
        //        }

        //        var tags = gettags();

        //        var annual_auto_email2 = tags.FirstOrDefault(xx => xx.tag == "annual_auto_email2");
        //        var annual_auto_email3 = tags.FirstOrDefault(xx => xx.tag == "annual_auto_email3");
        //        var annual_auto_email4 = tags.FirstOrDefault(xx => xx.tag == "annual_auto_email4");
        //        var annual_auto_email5 = tags.FirstOrDefault(xx => xx.tag == "annual_auto_email5");
        //        var annual_auto_email6 = tags.FirstOrDefault(xx => xx.tag == "annual_auto_email6");
        //        var annual_auto_email7 = tags.FirstOrDefault(xx => xx.tag == "annual_auto_email7");
        //        var annual_auto_email8 = tags.FirstOrDefault(xx => xx.tag == "annual_auto_email8");
        //        var annual_auto_email9 = tags.FirstOrDefault(xx => xx.tag == "annual_auto_email9");
        //        var annual_auto_emailLife = tags.FirstOrDefault(xx => xx.tag == "annual_auto_emailLife");
        //        var annual_auto_email10 = tags.FirstOrDefault(xx => xx.tag == "annual_auto_email10");
        //        var annual_auto_email11 = tags.FirstOrDefault(xx => xx.tag == "annual_auto_email11");
        //        var annual_auto_email12 = tags.FirstOrDefault(xx => xx.tag == "annual_auto_email12");
        //        var annual_auto_email13 = tags.FirstOrDefault(xx => xx.tag == "annual_auto_email13");
        //        var annual_auto_done = tags.FirstOrDefault(xx => xx.tag == "annual_auto_done");

        //        foreach (var contact in allContacts)
        //        {
        //            var contacts_raw = WebGet(grurl + "contacts/" + contact.id);
        //            var contacts_js = JsonConvert.DeserializeObject<dynamic>(contacts_raw);
        //            ContactResponse.Root contacts = JsonConvert.DeserializeObject<ContactResponse.Root>(contacts_js.ToString());

        //            var sdcsdc = contacts.fieldValues.FirstOrDefault(xx => xx.field == customerField_MembershipStart.id);

        //            //Set Tag
        //            if(sdcsdc.value.ToString() == "")
        //            {
        //                continue;
        //            }
        //            DateTime dt = Convert.ToDateTime(sdcsdc.value.ToString());

        //            var membershipAge = DateTime.Now.Subtract(dt).TotalDays;

        //            Tag tagToAdd = null;

        //            if (membershipAge > 365)
        //                tagToAdd = annual_auto_done;
        //            else if (membershipAge > 311)
        //                tagToAdd = annual_auto_email13;
        //            else if (membershipAge > 141)
        //                tagToAdd = annual_auto_email12;
        //            else if (membershipAge > 101)
        //                tagToAdd = annual_auto_email11;
        //            else if (membershipAge > 61)
        //                tagToAdd = annual_auto_email10;
        //            else if (membershipAge > 31)
        //                tagToAdd = annual_auto_emailLife;
        //            else if (membershipAge > 26)
        //                tagToAdd = annual_auto_email9;
        //            else if (membershipAge > 21)
        //                tagToAdd = annual_auto_email8;
        //            else if (membershipAge > 18)
        //                tagToAdd = annual_auto_email7;
        //            else if (membershipAge > 14)
        //                tagToAdd = annual_auto_email6;
        //            else if (membershipAge > 11)
        //                tagToAdd = annual_auto_email5;
        //            else if (membershipAge > 8)
        //                tagToAdd = annual_auto_email4;
        //            else if (membershipAge > 4)
        //                tagToAdd = annual_auto_email3;
        //            else if (membershipAge > 1)
        //                tagToAdd = annual_auto_email2;

        //            if(tagToAdd == null)
        //            {
        //                Console.WriteLine("Would have written " + contact.id + "|Too Old");
        //                continue;
        //            }
        //            else
        //                Console.WriteLine("Would have written " + contact.id + "|" + tagToAdd.tag);

        //            WebPost(url + "contactTags", "{\"contactTag\": {\"contact\": \"" + contact.id + "\",\"tag\": \"" + tagToAdd.id + "\"}}");
        //        }
        //    }

        //    static void Main_Life(string[] args)
        //    {
        //        var grurl = "https://fulltimefamilies93530.api-us1.com/api/3/";
        //        string automation_test_listid = "7";
        //        string annual_members_listid = "2";
        //        string life_members_listid = "3";
        //        string business_members_listid = "4";

        //        //Get All Contacts in a list
        //        var listId = life_members_listid;
        //        var url = grurl;

        //        var customFields_raw = WebGet(grurl + "fields?limit=100");
        //        var customFields_js = JsonConvert.DeserializeObject<dynamic>(customFields_raw);
        //        FieldsResponse.Root customFields = JsonConvert.DeserializeObject<FieldsResponse.Root>(customFields_js.ToString());

        //        var customerField_MembershipStart = customFields.fields.FirstOrDefault(xx => xx.title == "custom:membership_start");

        //        var allContacts = new List<Contact>();

        //        for (int i = 0; i < 20; i++)
        //        {
        //            var listContacts_raw = WebGet(grurl + "contacts?offset=" + i * 100 + "&limit=100&listid=" + listId);
        //            var listContacts_js = JsonConvert.DeserializeObject<dynamic>(listContacts_raw);
        //            var listContacts = JsonConvert.DeserializeObject<List<Contact>>(listContacts_js.contacts.ToString());

        //            allContacts.AddRange(listContacts);
        //        }

        //        var tags = gettags();

        //        var life_auto_email2 = tags.FirstOrDefault(xx => xx.tag == "lifetime_auto_email2");
        //        var life_auto_email3 = tags.FirstOrDefault(xx => xx.tag == "lifetime_auto_email3");
        //        var life_auto_email4 = tags.FirstOrDefault(xx => xx.tag == "lifetime_auto_email4");
        //        var life_auto_email5 = tags.FirstOrDefault(xx => xx.tag == "lifetime_auto_email5");
        //        var life_auto_email6 = tags.FirstOrDefault(xx => xx.tag == "lifetime_auto_email6");
        //        var life_auto_email7 = tags.FirstOrDefault(xx => xx.tag == "lifetime_auto_email7");
        //        var life_auto_email8 = tags.FirstOrDefault(xx => xx.tag == "lifetime_auto_email8");
        //        var life_auto_email9 = tags.FirstOrDefault(xx => xx.tag == "lifetime_auto_email9");
        //        var life_auto_email10 = tags.FirstOrDefault(xx => xx.tag == "lifetime_auto_email10");
        //        var life_auto_email11 = tags.FirstOrDefault(xx => xx.tag == "lifetime_auto_email11");
        //        var life_auto_email12 = tags.FirstOrDefault(xx => xx.tag == "lifetime_auto_email12");
        //        var life_auto_done = tags.FirstOrDefault(xx => xx.tag == "lifetime_auto_done");

        //        foreach (var contact in allContacts)
        //        {
        //            var contacts_raw = WebGet(grurl + "contacts/" + contact.id);
        //            var contacts_js = JsonConvert.DeserializeObject<dynamic>(contacts_raw);
        //            ContactResponse.Root contacts = JsonConvert.DeserializeObject<ContactResponse.Root>(contacts_js.ToString());

        //            var sdcsdc = contacts.fieldValues.FirstOrDefault(xx => xx.field == customerField_MembershipStart.id);

        //            //Set Tag
        //            if (sdcsdc.value.ToString() == "")
        //            {
        //                continue;
        //            }
        //            DateTime dt = Convert.ToDateTime(sdcsdc.value.ToString());

        //            var membershipAge = DateTime.Now.Subtract(dt).TotalDays;

        //            Tag tagToAdd = null;

        //            if (membershipAge > 350)
        //                tagToAdd = life_auto_done;
        //            else if (membershipAge > 180)
        //                tagToAdd = life_auto_email12;
        //            else if (membershipAge > 100)
        //                tagToAdd = life_auto_email11;
        //            else if (membershipAge > 30)
        //                tagToAdd = life_auto_email10;
        //            else if (membershipAge > 25)
        //                tagToAdd = life_auto_email9;
        //            else if (membershipAge > 20)
        //                tagToAdd = life_auto_email8;
        //            else if (membershipAge > 17)
        //                tagToAdd = life_auto_email7;
        //            else if (membershipAge > 13)
        //                tagToAdd = life_auto_email6;
        //            else if (membershipAge > 10)
        //                tagToAdd = life_auto_email5;
        //            else if (membershipAge > 7)
        //                tagToAdd = life_auto_email4;
        //            else if (membershipAge > 3)
        //                tagToAdd = life_auto_email3;
        //            else if (membershipAge > 0)
        //                tagToAdd = life_auto_email2;

        //            if (tagToAdd == null)
        //            {
        //                Console.WriteLine("Would have written " + contact.id + "|Too Old");
        //                continue;
        //            }
        //            else
        //                Console.WriteLine("Would have written " + contact.id + "|" + tagToAdd.tag);

        //            WebPost(url + "contactTags", "{\"contactTag\": {\"contact\": \"" + contact.id + "\",\"tag\": \"" + tagToAdd.id + "\"}}");
        //        }
        //    }
        //    static void Main(string[] args)
        //    {
        //        var grurl = "https://fulltimefamilies93530.api-us1.com/api/3/";
        //        string automation_test_listid = "7";
        //        string annual_members_listid = "2";
        //        string life_members_listid = "3";
        //        string business_members_listid = "4";

        //        //Get All Contacts in a list
        //        var listId = business_members_listid;
        //        var url = grurl;

        //        var customFields_raw = WebGet(grurl + "fields?limit=100");
        //        var customFields_js = JsonConvert.DeserializeObject<dynamic>(customFields_raw);
        //        FieldsResponse.Root customFields = JsonConvert.DeserializeObject<FieldsResponse.Root>(customFields_js.ToString());

        //        var customerField_MembershipStart = customFields.fields.FirstOrDefault(xx => xx.title == "sign_up");

        //        var allContacts = new List<Contact>();

        //        for (int i = 0; i < 20; i++)
        //        {
        //            var listContacts_raw = WebGet(grurl + "contacts?offset=" + i * 100 + "&limit=100&listid=" + listId);
        //            var listContacts_js = JsonConvert.DeserializeObject<dynamic>(listContacts_raw);
        //            var listContacts = JsonConvert.DeserializeObject<List<Contact>>(listContacts_js.contacts.ToString());

        //            allContacts.AddRange(listContacts);
        //        }

        //        var tags = gettags();

        //        var business_auto_email2 = tags.FirstOrDefault(xx => xx.tag == "business_auto_email2");
        //        var business_auto_email3 = tags.FirstOrDefault(xx => xx.tag == "business_auto_email3");
        //        var business_auto_email4 = tags.FirstOrDefault(xx => xx.tag == "business_auto_email4");
        //        var business_auto_done = tags.FirstOrDefault(xx => xx.tag == "business_auto_done");

        //        foreach (var contact in allContacts)
        //        {
        //            var contacts_raw = WebGet(grurl + "contacts/" + contact.id);
        //            var contacts_js = JsonConvert.DeserializeObject<dynamic>(contacts_raw);
        //            ContactResponse.Root contacts = JsonConvert.DeserializeObject<ContactResponse.Root>(contacts_js.ToString());

        //            var sdcsdc = contacts.fieldValues.FirstOrDefault(xx => xx.field == customerField_MembershipStart.id);

        //            //Set Tag
        //            if (sdcsdc.value.ToString() == "")
        //            {
        //                continue;
        //            }
        //            DateTime dt = Convert.ToDateTime(sdcsdc.value.ToString());

        //            var membershipAge = DateTime.Now.Subtract(dt).TotalDays;

        //            Tag tagToAdd = null;

        //            if (membershipAge > 44)
        //                tagToAdd = business_auto_done;
        //            else if (membershipAge > 4)
        //                tagToAdd = business_auto_email4;
        //            else if (membershipAge > 2)
        //                tagToAdd = business_auto_email3;
        //            else if (membershipAge > 0)
        //                tagToAdd = business_auto_email2;

        //            if (tagToAdd == null)
        //            {
        //                Console.WriteLine("Would have written " + contact.id + "|Too Old");
        //                continue;
        //            }
        //            else
        //                Console.WriteLine("Would have written " + contact.id + "|" + tagToAdd.tag);

        //            WebPost(url + "contactTags", "{\"contactTag\": {\"contact\": \"" + contact.id + "\",\"tag\": \"" + tagToAdd.id + "\"}}");
        //        }
        //    }

        //    static string WebGet(string url)
        //    {
        //        string apiKey = "b2ec21a85db6ed8e498f5c618a45a7496e5cd1195a27e4173463aac86b58d7e1b4dd5b33";

        //        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
        //        request.Headers.Add("Api-Token", apiKey);
        //        request.Proxy = new WebProxy("127.0.0.1:8888");

        //        request.Method = "GET";
        //        String test = String.Empty;

        //        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        //        {
        //            Stream dataStream = response.GetResponseStream();
        //            StreamReader reader = new StreamReader(dataStream);
        //            test = reader.ReadToEnd();

        //            reader.Close();
        //            dataStream.Close();

        //            return test;
        //        }
        //    }
        //    static string WebPost(string url, string postData)
        //    {

        //        string apiKey = "b2ec21a85db6ed8e498f5c618a45a7496e5cd1195a27e4173463aac86b58d7e1b4dd5b33";

        //        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
        //        request.Headers.Add("Api-Token", apiKey);
        //        request.Proxy = new WebProxy("127.0.0.1:8888");

        //        request.Method = "POST";
        //        String test = String.Empty;

        //        byte[] data = Encoding.ASCII.GetBytes(postData);

        //        Stream requestStream = request.GetRequestStream();
        //        requestStream.Write(data, 0, data.Length);
        //        requestStream.Close();

        //        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        //        {
        //            Stream dataStream = response.GetResponseStream();
        //            StreamReader reader = new StreamReader(dataStream);
        //            test = reader.ReadToEnd();

        //            reader.Close();
        //            dataStream.Close();
        //        }

        //        return test;
        //    }
        //    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 

        //    public class Links6
        //    {
        //        public string bounceLogs { get; set; }
        //        public string contactAutomations { get; set; }
        //        public string contactData { get; set; }
        //        public string contactGoals { get; set; }
        //        public string contactLists { get; set; }
        //        public string contactLogs { get; set; }
        //        public string contactTags { get; set; }
        //        public string contactDeals { get; set; }
        //        public string deals { get; set; }
        //        public string fieldValues { get; set; }
        //        public string geoIps { get; set; }
        //        public string notes { get; set; }
        //        public string organization { get; set; }
        //        public string plusAppend { get; set; }
        //        public string trackingLogs { get; set; }
        //        public string scoreValues { get; set; }
        //    }

        //    public class Contact
        //    {
        //        public DateTime cdate { get; set; }
        //        public string email { get; set; }
        //        public string phone { get; set; }
        //        public string firstName { get; set; }
        //        public string lastName { get; set; }
        //        public string orgid { get; set; }
        //        public string segmentio_id { get; set; }
        //        public string bounced_hard { get; set; }
        //        public string bounced_soft { get; set; }
        //        public object bounced_date { get; set; }
        //        public string ip { get; set; }
        //        public object ua { get; set; }
        //        public string hash { get; set; }
        //        public object socialdata_lastcheck { get; set; }
        //        public string email_local { get; set; }
        //        public string email_domain { get; set; }
        //        public string sentcnt { get; set; }
        //        public object rating_tstamp { get; set; }
        //        public string gravatar { get; set; }
        //        public string deleted { get; set; }
        //        public object adate { get; set; }
        //        public object udate { get; set; }
        //        public object edate { get; set; }
        //        public List<string> contactAutomations { get; set; }
        //        public List<string> contactLists { get; set; }
        //        public List<string> fieldValues { get; set; }
        //        public List<string> geoIps { get; set; }
        //        public List<string> deals { get; set; }
        //        public List<string> accountContacts { get; set; }
        //        public Links6 links { get; set; }
        //        public string id { get; set; }
        //        public object organization { get; set; }
        //    }

        //    public class Tag
        //    {
        //        public string tagType { get; set; }
        //        public string tag { get; set; }
        //        public string description { get; set; }
        //        public string subscriber_count { get; set; }
        //        public DateTime cdate { get; set; }
        //        public string created_timestamp { get; set; }
        //        public string updated_timestamp { get; set; }
        //        public object created_by { get; set; }
        //        public object updated_by { get; set; }
        //        public Links links { get; set; }
        //        public string id { get; set; }
        //    }

        //    public class Links
        //    {
        //        public string bounceLogs { get; set; }
        //        public string contactAutomations { get; set; }
        //        public string contactData { get; set; }
        //        public string contactGoals { get; set; }
        //        public string contactLists { get; set; }
        //        public string contactLogs { get; set; }
        //        public string contactTags { get; set; }
        //        public string contactDeals { get; set; }
        //        public string deals { get; set; }
        //        public string fieldValues { get; set; }
        //        public string geoIps { get; set; }
        //        public string notes { get; set; }
        //        public string organization { get; set; }
        //        public string plusAppend { get; set; }
        //        public string trackingLogs { get; set; }
        //        public string scoreValues { get; set; }
        //        public string accountContacts { get; set; }
        //        public string automationEntryCounts { get; set; }
        //    }
        //}
        //public class ContactResponse
        //{
        //    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        //    public class Links
        //    {
        //        public string automation { get; set; }
        //        public string contact { get; set; }
        //        public string contactGoals { get; set; }
        //    }

        //    public class ContactAutomation
        //    {
        //        public string contact { get; set; }
        //        public string seriesid { get; set; }
        //        public string startid { get; set; }
        //        public string status { get; set; }
        //        public DateTime adddate { get; set; }
        //        public object remdate { get; set; }
        //        public object timespan { get; set; }
        //        public string lastblock { get; set; }
        //        public DateTime lastdate { get; set; }
        //        public string completedElements { get; set; }
        //        public string totalElements { get; set; }
        //        public int completed { get; set; }
        //        public int completeValue { get; set; }
        //        public Links links { get; set; }
        //        public string id { get; set; }
        //        public string automation { get; set; }
        //    }

        //    public class Links2
        //    {
        //        public string automation { get; set; }
        //        public string list { get; set; }
        //        public string contact { get; set; }
        //        public string form { get; set; }
        //        public string autosyncLog { get; set; }
        //        public string campaign { get; set; }
        //        public string unsubscribeAutomation { get; set; }
        //        public string message { get; set; }
        //    }

        //    public class ContactList
        //    {
        //        public string contact { get; set; }
        //        public string list { get; set; }
        //        public object form { get; set; }
        //        public string seriesid { get; set; }
        //        public object sdate { get; set; }
        //        public object udate { get; set; }
        //        public string status { get; set; }
        //        public string responder { get; set; }
        //        public string sync { get; set; }
        //        public object unsubreason { get; set; }
        //        public object campaign { get; set; }
        //        public object message { get; set; }
        //        public string first_name { get; set; }
        //        public string last_name { get; set; }
        //        public string ip4Sub { get; set; }
        //        public string sourceid { get; set; }
        //        public object autosyncLog { get; set; }
        //        public string ip4_last { get; set; }
        //        public string ip4Unsub { get; set; }
        //        public object unsubscribeAutomation { get; set; }
        //        public Links2 links { get; set; }
        //        public string id { get; set; }
        //        public object automation { get; set; }
        //    }

        //    public class Links3
        //    {
        //        public string activities { get; set; }
        //        public string contact { get; set; }
        //        public string contactDeals { get; set; }
        //        public string group { get; set; }
        //        public string nextTask { get; set; }
        //        public string notes { get; set; }
        //        public string organization { get; set; }
        //        public string owner { get; set; }
        //        public string scoreValues { get; set; }
        //        public string stage { get; set; }
        //        public string tasks { get; set; }
        //    }

        //    public class Deal
        //    {
        //        public string owner { get; set; }
        //        public string contact { get; set; }
        //        public object organization { get; set; }
        //        public object group { get; set; }
        //        public string title { get; set; }
        //        public string nexttaskid { get; set; }
        //        public string currency { get; set; }
        //        public string status { get; set; }
        //        public Links3 links { get; set; }
        //        public string id { get; set; }
        //        public object nextTask { get; set; }
        //    }

        //    public class Links4
        //    {
        //        public string owner { get; set; }
        //        public string field { get; set; }
        //    }

        //    public class FieldValue
        //    {
        //        public string contact { get; set; }
        //        public string field { get; set; }
        //        public object value { get; set; }
        //        public DateTime cdate { get; set; }
        //        public DateTime udate { get; set; }
        //        public Links4 links { get; set; }
        //        public string id { get; set; }
        //        public string owner { get; set; }
        //    }

        //    public class GeoAddress
        //    {
        //        public string ip4 { get; set; }
        //        public string country2 { get; set; }
        //        public string country { get; set; }
        //        public string state { get; set; }
        //        public string city { get; set; }
        //        public string zip { get; set; }
        //        public string area { get; set; }
        //        public string lat { get; set; }
        //        public string lon { get; set; }
        //        public string tz { get; set; }
        //        public DateTime tstamp { get; set; }
        //        public List<object> links { get; set; }
        //        public string id { get; set; }
        //    }

        //    public class Links5
        //    {
        //        public string geoAddress { get; set; }
        //    }

        //    public class GeoIp
        //    {
        //        public string contact { get; set; }
        //        public string campaignid { get; set; }
        //        public string messageid { get; set; }
        //        public string geoaddrid { get; set; }
        //        public string ip4 { get; set; }
        //        public DateTime tstamp { get; set; }
        //        public string geoAddress { get; set; }
        //        public Links5 links { get; set; }
        //        public string id { get; set; }
        //    }

        //    public class Links6
        //    {
        //        public string bounceLogs { get; set; }
        //        public string contactAutomations { get; set; }
        //        public string contactData { get; set; }
        //        public string contactGoals { get; set; }
        //        public string contactLists { get; set; }
        //        public string contactLogs { get; set; }
        //        public string contactTags { get; set; }
        //        public string contactDeals { get; set; }
        //        public string deals { get; set; }
        //        public string fieldValues { get; set; }
        //        public string geoIps { get; set; }
        //        public string notes { get; set; }
        //        public string organization { get; set; }
        //        public string plusAppend { get; set; }
        //        public string trackingLogs { get; set; }
        //        public string scoreValues { get; set; }
        //    }

        //    public class Contact
        //    {
        //        public DateTime cdate { get; set; }
        //        public string email { get; set; }
        //        public string phone { get; set; }
        //        public string firstName { get; set; }
        //        public string lastName { get; set; }
        //        public string orgid { get; set; }
        //        public string segmentio_id { get; set; }
        //        public string bounced_hard { get; set; }
        //        public string bounced_soft { get; set; }
        //        public object bounced_date { get; set; }
        //        public string ip { get; set; }
        //        public object ua { get; set; }
        //        public string hash { get; set; }
        //        public object socialdata_lastcheck { get; set; }
        //        public string email_local { get; set; }
        //        public string email_domain { get; set; }
        //        public string sentcnt { get; set; }
        //        public object rating_tstamp { get; set; }
        //        public string gravatar { get; set; }
        //        public string deleted { get; set; }
        //        public object adate { get; set; }
        //        public object udate { get; set; }
        //        public object edate { get; set; }
        //        public List<string> contactAutomations { get; set; }
        //        public List<string> contactLists { get; set; }
        //        public List<string> fieldValues { get; set; }
        //        public List<string> geoIps { get; set; }
        //        public List<string> deals { get; set; }
        //        public List<string> accountContacts { get; set; }
        //        public Links6 links { get; set; }
        //        public string id { get; set; }
        //        public object organization { get; set; }
        //    }

        //    public class Root
        //    {
        //        public List<ContactAutomation> contactAutomations { get; set; }
        //        public List<ContactList> contactLists { get; set; }
        //        public List<Deal> deals { get; set; }
        //        public List<FieldValue> fieldValues { get; set; }
        //        public List<GeoAddress> geoAddresses { get; set; }
        //        public List<GeoIp> geoIps { get; set; }
        //        public Contact contact { get; set; }
        //    }
        //}
        //public class FieldsResponse
        //{
        //    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        //    public class Links
        //    {
        //        public string options { get; set; }
        //        public string relations { get; set; }
        //    }

        //    public class Field
        //    {
        //        public string title { get; set; }
        //        public string descript { get; set; }
        //        public string type { get; set; }
        //        public string isrequired { get; set; }
        //        public string perstag { get; set; }
        //        public string defval { get; set; }
        //        public string show_in_list { get; set; }
        //        public string rows { get; set; }
        //        public string cols { get; set; }
        //        public string visible { get; set; }
        //        public string service { get; set; }
        //        public string ordernum { get; set; }
        //        public DateTime? cdate { get; set; }
        //        public DateTime? udate { get; set; }
        //        public List<object> options { get; set; }
        //        public List<object> relations { get; set; }
        //        public Links links { get; set; }
        //        public string id { get; set; }
        //    }

        //    public class Meta
        //    {
        //        public string total { get; set; }
        //    }

        //    public class Root
        //    {
        //        public List<object> fieldOptions { get; set; }
        //        public List<object> fieldRels { get; set; }
        //        public List<Field> fields { get; set; }
        //        public Meta meta { get; set; }
        //    }


    }
}

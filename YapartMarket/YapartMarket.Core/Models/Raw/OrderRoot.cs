using System.Collections.Generic;

namespace YapartMarket.Core.Models.Raw
{
    public class Data
    {
        public int total_count { get; set; }
        public List<Order>? orders { get; set; }
    }

    public class Order
    {
        public long id { get; set; }
        public string? created_at { get; set; }
        public string? paid_at { get; set; }
        public string? updated_at { get; set; }
        public string? status { get; set; }
        public string? payment_status { get; set; }
        public string? delivery_status { get; set; }
        public string? delivery_address { get; set; }
        public string? antifraud_status { get; set; }
        public string? buyer_country_code { get; set; }
        public string? buyer_name { get; set; }
        public string? order_display_status { get; set; }
        public string? buyer_phone { get; set; }
        public List<OrderLine>? order_lines { get; set; }
        public int total_amount { get; set; }
        public object? seller_comment { get; set; }
        public bool fully_prepared { get; set; }
        public string? finish_reason { get; set; }
        public object? cut_off_date { get; set; }
        public object? cut_off_date_histories { get; set; }
        public object? shipping_deadline { get; set; }
        public object? next_cut_off_date { get; set; }
        public object? pre_split_postings { get; set; }
        public object? logistic_orders { get; set; }
        public object? commission { get; set; }
    }

    public class OrderLine
    {
        public object? id { get; set; }
        public string? item_id { get; set; }
        public string? sku_id { get; set; }
        public string? sku_code { get; set; }
        public string? name { get; set; }
        public string? img_url { get; set; }
        public int item_price { get; set; }
        public double quantity { get; set; }
        public int total_amount { get; set; }
        public List<string?>? properties { get; set; }
        public object? buyer_comment { get; set; }
        public double height { get; set; }
        public double weight { get; set; }
        public double width { get; set; }
        public double length { get; set; }
        public string? issue_status { get; set; }
        public List<Promotion>? promotions { get; set; }
        public object? order_line_fees { get; set; }
    }

    public class Promotion
    {
        public object? ae_promotion_id { get; set; }
        public object? ae_activity_id { get; set; }
        public object? code { get; set; }
        public string? promotion_type { get; set; }
        public int discount { get; set; }
        public string? discount_currency { get; set; }
        public int original_discount { get; set; }
        public string? original_discount_currency { get; set; }
        public string? promotion_target { get; set; }
        public string? budget_sponsor { get; set; }
    }

    public class OrderRoot
    {
        public Data? data { get; set; }
        public object? error { get; set; }
    }
}

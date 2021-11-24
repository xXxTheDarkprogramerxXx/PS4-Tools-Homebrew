using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Code.Models
{
    public class PS4_chihiro_model
    {

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class Price
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("count")]
            public int Count { get; set; }

            [JsonProperty("key")]
            public string Key { get; set; }
        }

        public class AddonType
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("count")]
            public int Count { get; set; }

            [JsonProperty("key")]
            public string Key { get; set; }
        }

        public class Genre
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("count")]
            public int Count { get; set; }

            [JsonProperty("key")]
            public string Key { get; set; }

            [JsonProperty("values")]
            public List<string> Values { get; set; }
        }

        public class TopCategory
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("count")]
            public int Count { get; set; }

            [JsonProperty("key")]
            public string Key { get; set; }
        }

        public class Relationship
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("count")]
            public int Count { get; set; }

            [JsonProperty("key")]
            public string Key { get; set; }
        }

        public class Facets
        {
            [JsonProperty("price")]
            public List<Price> Price { get; set; }

            [JsonProperty("addon_type")]
            public List<AddonType> AddonType { get; set; }

            [JsonProperty("genre")]
            public List<Genre> Genre { get; set; }

            [JsonProperty("top_category")]
            public List<TopCategory> TopCategory { get; set; }

            [JsonProperty("relationship")]
            public List<Relationship> Relationship { get; set; }
        }

        public class Attributes
        {
            [JsonProperty("facets")]
            public Facets Facets { get; set; }

            [JsonProperty("next")]
            public List<object> Next { get; set; }
        }

        public class ContentDescriptor
        {
            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("url")]
            public object Url { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }
        }

        public class ContentRating
        {
            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("rating_system")]
            public string RatingSystem { get; set; }

            [JsonProperty("url")]
            public string Url { get; set; }
        }

        public class Package
        {
            [JsonProperty("platformId")]
            public int PlatformId { get; set; }

            [JsonProperty("platformName")]
            public string PlatformName { get; set; }

            [JsonProperty("size")]
            public long Size { get; set; }
        }

        public class Entitlement
        {
            [JsonProperty("description")]
            public object Description { get; set; }

            [JsonProperty("drms")]
            public List<object> Drms { get; set; }

            [JsonProperty("duration")]
            public int Duration { get; set; }

            [JsonProperty("durationOverrideTypeId")]
            public object DurationOverrideTypeId { get; set; }

            [JsonProperty("exp_after_first_use")]
            public int ExpAfterFirstUse { get; set; }

            [JsonProperty("feature_type_id")]
            public int FeatureTypeId { get; set; }

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("license_type")]
            public int LicenseType { get; set; }

            [JsonProperty("metadata")]
            public object Metadata { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("packageType")]
            public string PackageType { get; set; }

            [JsonProperty("packages")]
            public List<Package> Packages { get; set; }

            [JsonProperty("preorder_placeholder_flag")]
            public bool PreorderPlaceholderFlag { get; set; }

            [JsonProperty("size")]
            public int Size { get; set; }

            [JsonProperty("subType")]
            public int SubType { get; set; }

            [JsonProperty("subtitle_language_codes")]
            public object SubtitleLanguageCodes { get; set; }

            [JsonProperty("type")]
            public int Type { get; set; }

            [JsonProperty("use_count")]
            public int UseCount { get; set; }

            [JsonProperty("voice_language_codes")]
            public object VoiceLanguageCodes { get; set; }
        }

        public class Campaign
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("start_date")]
            public DateTime StartDate { get; set; }

            [JsonProperty("end_date")]
            public DateTime EndDate { get; set; }
        }

        public class Reward
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("discount")]
            public int Discount { get; set; }

            [JsonProperty("price")]
            public int Price { get; set; }

            [JsonProperty("reward_type")]
            public int RewardType { get; set; }

            [JsonProperty("display_price")]
            public string DisplayPrice { get; set; }

            [JsonProperty("isPlus")]
            public bool IsPlus { get; set; }

            [JsonProperty("campaigns")]
            public List<Campaign> Campaigns { get; set; }

            [JsonProperty("reward_source_type_id")]
            public int RewardSourceTypeId { get; set; }

            [JsonProperty("start_date")]
            public DateTime StartDate { get; set; }

            [JsonProperty("end_date")]
            public DateTime EndDate { get; set; }
        }

        public class DefaultSku
        {
            [JsonProperty("amortizeFlag")]
            public bool AmortizeFlag { get; set; }

            [JsonProperty("bundleExclusiveFlag")]
            public bool BundleExclusiveFlag { get; set; }

            [JsonProperty("chargeImmediatelyFlag")]
            public bool ChargeImmediatelyFlag { get; set; }

            [JsonProperty("charge_type_id")]
            public int ChargeTypeId { get; set; }

            [JsonProperty("credit_card_required_flag")]
            public int CreditCardRequiredFlag { get; set; }

            [JsonProperty("defaultSku")]
            public bool _DefaultSku { get; set; }

            [JsonProperty("display_price")]
            public string DisplayPrice { get; set; }

            [JsonProperty("eligibilities")]
            public List<object> Eligibilities { get; set; }

            [JsonProperty("entitlements")]
            public List<Entitlement> Entitlements { get; set; }

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("is_original")]
            public bool IsOriginal { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("platforms")]
            public List<int> Platforms { get; set; }

            [JsonProperty("price")]
            public int Price { get; set; }

            [JsonProperty("rewards")]
            public List<Reward> Rewards { get; set; }

            [JsonProperty("seasonPassExclusiveFlag")]
            public bool SeasonPassExclusiveFlag { get; set; }

            [JsonProperty("skuAvailabilityOverrideFlag")]
            public bool SkuAvailabilityOverrideFlag { get; set; }

            [JsonProperty("sku_type")]
            public int SkuType { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }
        }

        public class GameContentTypesList
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("key")]
            public string Key { get; set; }
        }

        public class Image
        {
            [JsonProperty("type")]
            public int Type { get; set; }

            [JsonProperty("url")]
            public string Url { get; set; }
        }

        public class Eligibility
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("operand")]
            public string Operand { get; set; }

            [JsonProperty("operator")]
            public string Operator { get; set; }

            [JsonProperty("rightOperand")]
            public object RightOperand { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("description")]
            public object Description { get; set; }

            [JsonProperty("entitlement_type")]
            public object EntitlementType { get; set; }

            [JsonProperty("drms")]
            public List<object> Drms { get; set; }
        }

        public class Link
        {
            [JsonProperty("bucket")]
            public string Bucket { get; set; }

            [JsonProperty("container_type")]
            public string ContainerType { get; set; }

            [JsonProperty("content_type")]
            public string ContentType { get; set; }

            [JsonProperty("default_sku")]
            public DefaultSku DefaultSku { get; set; }

            [JsonProperty("gameContentTypesList")]
            public List<GameContentTypesList> GameContentTypesList { get; set; }

            [JsonProperty("game_contentType")]
            public string GameContentType { get; set; }

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("images")]
            public List<Image> Images { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("parent_name")]
            public string ParentName { get; set; }

            [JsonProperty("playable_platform")]
            public List<string> PlayablePlatform { get; set; }

            [JsonProperty("provider_name")]
            public string ProviderName { get; set; }

            [JsonProperty("release_date")]
            public DateTime ReleaseDate { get; set; }

            [JsonProperty("restricted")]
            public bool Restricted { get; set; }

            [JsonProperty("revision")]
            public int Revision { get; set; }

            [JsonProperty("short_name")]
            public string ShortName { get; set; }

            [JsonProperty("timestamp")]
            public object Timestamp { get; set; }

            [JsonProperty("top_category")]
            public string TopCategory { get; set; }

            [JsonProperty("url")]
            public string Url { get; set; }

            [JsonProperty("cloud_only_platform")]
            public List<string> CloudOnlyPlatform { get; set; }
        }

        public class Preview
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("typeId")]
            public int TypeId { get; set; }

            [JsonProperty("source")]
            public string Source { get; set; }

            [JsonProperty("url")]
            public string Url { get; set; }

            [JsonProperty("order")]
            public int Order { get; set; }
        }

        public class Screenshot
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("typeId")]
            public int TypeId { get; set; }

            [JsonProperty("source")]
            public string Source { get; set; }

            [JsonProperty("url")]
            public string Url { get; set; }

            [JsonProperty("order")]
            public int Order { get; set; }
        }

        public class MediaList
        {
            [JsonProperty("previews")]
            public List<Preview> Previews { get; set; }

            [JsonProperty("screenshots")]
            public List<Screenshot> Screenshots { get; set; }
        }

        public class MediaLayout
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("image")]
            public string Image { get; set; }

            [JsonProperty("height")]
            public int Height { get; set; }

            [JsonProperty("width")]
            public int Width { get; set; }

            [JsonProperty("play")]
            public int Play { get; set; }

            [JsonProperty("bannerType")]
            public string BannerType { get; set; }

            [JsonProperty("caption")]
            public string Caption { get; set; }

            [JsonProperty("action")]
            public string Action { get; set; }

            [JsonProperty("url")]
            public string Url { get; set; }
        }

        public class CnRemotePlay
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("values")]
            public List<string> Values { get; set; }
        }

        public class CnVrEnabled
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("values")]
            public List<string> Values { get; set; }
        }

        public class CnPlaystationMove
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("values")]
            public List<string> Values { get; set; }
        }

        public class SecondaryClassification
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("values")]
            public List<string> Values { get; set; }
        }

        public class CnVrRequired
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("values")]
            public List<string> Values { get; set; }
        }

        public class GameGenre
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("values")]
            public List<string> Values { get; set; }
        }

        public class CnPsEnhanced
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("values")]
            public List<string> Values { get; set; }
        }

        public class PlayablePlatform
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("values")]
            public List<string> Values { get; set; }
        }

        public class CnNumberOfPlayers
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("values")]
            public List<string> Values { get; set; }
        }

        public class CnDualshockVibration
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("values")]
            public List<string> Values { get; set; }
        }

        public class TertiaryClassification
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("values")]
            public List<string> Values { get; set; }
        }

        public class ContainerType
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("values")]
            public List<string> Values { get; set; }
        }

        public class CnInGamePurchases
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("values")]
            public List<string> Values { get; set; }
        }

        public class CnPsVrAimRequired
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("values")]
            public List<string> Values { get; set; }
        }

        public class CnPlaystationCamera
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("values")]
            public List<string> Values { get; set; }
        }

        public class CnSingstarMicrophone
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("values")]
            public List<string> Values { get; set; }
        }

        public class CnCrossPlatformPSVita
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("values")]
            public List<string> Values { get; set; }
        }

        public class CnPsVrAimEnabled
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("values")]
            public List<string> Values { get; set; }
        }

        public class PrimaryClassification
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("values")]
            public List<string> Values { get; set; }
        }

        public class Metadata
        {
            [JsonProperty("cn_remotePlay")]
            public CnRemotePlay CnRemotePlay { get; set; }

            [JsonProperty("cn_vrEnabled")]
            public CnVrEnabled CnVrEnabled { get; set; }

            [JsonProperty("cn_playstationMove")]
            public CnPlaystationMove CnPlaystationMove { get; set; }

            [JsonProperty("secondary_classification")]
            public SecondaryClassification SecondaryClassification { get; set; }

            [JsonProperty("cn_vrRequired")]
            public CnVrRequired CnVrRequired { get; set; }

            [JsonProperty("game_genre")]
            public GameGenre GameGenre { get; set; }

            [JsonProperty("cn_psEnhanced")]
            public CnPsEnhanced CnPsEnhanced { get; set; }

            [JsonProperty("playable_platform")]
            public PlayablePlatform PlayablePlatform { get; set; }

            [JsonProperty("cn_numberOfPlayers")]
            public CnNumberOfPlayers CnNumberOfPlayers { get; set; }

            [JsonProperty("cn_dualshockVibration")]
            public CnDualshockVibration CnDualshockVibration { get; set; }

            [JsonProperty("tertiary_classification")]
            public TertiaryClassification TertiaryClassification { get; set; }

            [JsonProperty("container_type")]
            public ContainerType ContainerType { get; set; }

            [JsonProperty("genre")]
            public Genre Genre { get; set; }

            [JsonProperty("cn_inGamePurchases")]
            public CnInGamePurchases CnInGamePurchases { get; set; }

            [JsonProperty("cn_psVrAimRequired")]
            public CnPsVrAimRequired CnPsVrAimRequired { get; set; }

            [JsonProperty("cn_playstationCamera")]
            public CnPlaystationCamera CnPlaystationCamera { get; set; }

            [JsonProperty("cn_singstarMicrophone")]
            public CnSingstarMicrophone CnSingstarMicrophone { get; set; }

            [JsonProperty("cn_crossPlatformPSVita")]
            public CnCrossPlatformPSVita CnCrossPlatformPSVita { get; set; }

            [JsonProperty("cn_psVrAimEnabled")]
            public CnPsVrAimEnabled CnPsVrAimEnabled { get; set; }

            [JsonProperty("primary_classification")]
            public PrimaryClassification PrimaryClassification { get; set; }
        }

        public class Country
        {
            [JsonProperty("agelimit")]
            public int Agelimit { get; set; }

            [JsonProperty("uagelimit")]
            public int Uagelimit { get; set; }

            [JsonProperty("country")]
            public string _Country { get; set; }
        }

        public class Url
        {
            [JsonProperty("type")]
            public int Type { get; set; }

            [JsonProperty("url")]
            public string _Url { get; set; }
        }

        public class Material
        {
            [JsonProperty("anno")]
            public string Anno { get; set; }

            [JsonProperty("countries")]
            public List<Country> Countries { get; set; }

            [JsonProperty("from")]
            public DateTime From { get; set; }

            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("lang")]
            public List<string> Lang { get; set; }

            [JsonProperty("lastm")]
            public DateTime Lastm { get; set; }

            [JsonProperty("until")]
            public DateTime Until { get; set; }

            [JsonProperty("urls")]
            public List<Url> Urls { get; set; }
        }

        public class Promomedia
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("key")]
            public string Key { get; set; }

            [JsonProperty("type")]
            public int Type { get; set; }

            [JsonProperty("url")]
            public string Url { get; set; }

            [JsonProperty("anno")]
            public string Anno { get; set; }

            [JsonProperty("materials")]
            public List<Material> Materials { get; set; }

            [JsonProperty("multi")]
            public string Multi { get; set; }

            [JsonProperty("rep")]
            public string Rep { get; set; }

            [JsonProperty("thumbnails")]
            public List<string> Thumbnails { get; set; }
        }

        public class Relationship2
        {
            [JsonProperty("count")]
            public int Count { get; set; }

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("key_name")]
            public string KeyName { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("url")]
            public string Url { get; set; }
        }

        public class Sku
        {
            [JsonProperty("amortizeFlag")]
            public bool AmortizeFlag { get; set; }

            [JsonProperty("bundleExclusiveFlag")]
            public bool BundleExclusiveFlag { get; set; }

            [JsonProperty("chargeImmediatelyFlag")]
            public bool ChargeImmediatelyFlag { get; set; }

            [JsonProperty("charge_type_id")]
            public int ChargeTypeId { get; set; }

            [JsonProperty("credit_card_required_flag")]
            public int CreditCardRequiredFlag { get; set; }

            [JsonProperty("defaultSku")]
            public bool DefaultSku { get; set; }

            [JsonProperty("display_price")]
            public string DisplayPrice { get; set; }

            [JsonProperty("eligibilities")]
            public List<object> Eligibilities { get; set; }

            [JsonProperty("entitlements")]
            public List<Entitlement> Entitlements { get; set; }

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("is_original")]
            public bool IsOriginal { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("platforms")]
            public List<int> Platforms { get; set; }

            [JsonProperty("price")]
            public int Price { get; set; }

            [JsonProperty("rewards")]
            public List<Reward> Rewards { get; set; }

            [JsonProperty("seasonPassExclusiveFlag")]
            public bool SeasonPassExclusiveFlag { get; set; }

            [JsonProperty("skuAvailabilityOverrideFlag")]
            public bool SkuAvailabilityOverrideFlag { get; set; }

            [JsonProperty("sku_type")]
            public int SkuType { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }
        }

        public class Count
        {
            [JsonProperty("star")]
            public int Star { get; set; }

            [JsonProperty("count")]
            public int _Count { get; set; }
        }

        public class StarRating
        {
            [JsonProperty("total")]
            public string Total { get; set; }

            [JsonProperty("score")]
            public string Score { get; set; }

            [JsonProperty("count")]
            public List<Count> Count { get; set; }
        }

        public class PS4_chihiro_model_item
        {
            [JsonProperty("age_limit")]
            public int AgeLimit { get; set; }

            [JsonProperty("attributes")]
            public Attributes Attributes { get; set; }

            [JsonProperty("bucket")]
            public string Bucket { get; set; }

            [JsonProperty("container_type")]
            public string ContainerType { get; set; }

            [JsonProperty("content_descriptors")]
            public List<ContentDescriptor> ContentDescriptors { get; set; }

            [JsonProperty("content_origin")]
            public int ContentOrigin { get; set; }

            [JsonProperty("content_rating")]
            public ContentRating ContentRating { get; set; }

            [JsonProperty("content_type")]
            public string ContentType { get; set; }

            [JsonProperty("default_sku")]
            public DefaultSku DefaultSku { get; set; }

            [JsonProperty("dob_required")]
            public bool DobRequired { get; set; }

            [JsonProperty("gameContentTypesList")]
            public List<GameContentTypesList> GameContentTypesList { get; set; }

            [JsonProperty("game_contentType")]
            public string GameContentType { get; set; }

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("images")]
            public List<Image> Images { get; set; }

            [JsonProperty("legal_text")]
            public string LegalText { get; set; }

            [JsonProperty("links")]
            public List<Link> Links { get; set; }

            [JsonProperty("long_desc")]
            public string LongDesc { get; set; }

            [JsonProperty("mediaList")]
            public MediaList MediaList { get; set; }

            [JsonProperty("media_layouts")]
            public List<MediaLayout> MediaLayouts { get; set; }

            [JsonProperty("metadata")]
            public Metadata Metadata { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("pageTypeId")]
            public int PageTypeId { get; set; }

            [JsonProperty("playable_platform")]
            public List<string> PlayablePlatform { get; set; }

            [JsonProperty("promomedia")]
            public List<Promomedia> Promomedia { get; set; }

            [JsonProperty("provider_name")]
            public string ProviderName { get; set; }

            [JsonProperty("relationships")]
            public List<Relationship> Relationships { get; set; }

            [JsonProperty("release_date")]
            public DateTime ReleaseDate { get; set; }

            [JsonProperty("restricted")]
            public bool Restricted { get; set; }

            [JsonProperty("revision")]
            public int Revision { get; set; }

            [JsonProperty("short_name")]
            public string ShortName { get; set; }

            [JsonProperty("size")]
            public int Size { get; set; }

            [JsonProperty("sku_links")]
            public List<object> SkuLinks { get; set; }

            [JsonProperty("skus")]
            public List<Sku> Skus { get; set; }

            [JsonProperty("sort")]
            public string Sort { get; set; }

            [JsonProperty("star_rating")]
            public StarRating StarRating { get; set; }

            [JsonProperty("start")]
            public int Start { get; set; }

            [JsonProperty("timestamp")]
            public long Timestamp { get; set; }

            [JsonProperty("title_name")]
            public string TitleName { get; set; }

            [JsonProperty("top_category")]
            public string TopCategory { get; set; }

            [JsonProperty("total_results")]
            public int TotalResults { get; set; }
        }
    }
}

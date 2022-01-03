using System;
using System.Runtime.InteropServices;

namespace Sony
{
	namespace NP
	{
		/// <summary>
		/// Commerce service related functionality.
		/// </summary>
		public class Commerce
		{
			#region DLL Imports

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxGetCategories(GetCategoriesRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxGetProducts(GetProductsRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxGetServiceEntitlements(GetServiceEntitlementsRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxConsumeServiceEntitlement(ConsumeServiceEntitlementRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxDisplayCategoryBrowseDialog(DisplayCategoryBrowseDialogRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxDisplayProductBrowseDialog(DisplayProductBrowseDialogRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxDisplayVoucherCodeInputDialog(DisplayVoucherCodeInputDialogRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxDisplayCheckoutDialog(DisplayCheckoutDialogRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxDisplayDownloadListDialog(DisplayDownloadListDialogRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxDisplayJoinPlusDialog(DisplayJoinPlusDialogRequest request, out APIResult result);

			[DllImport("UnityNpToolkit2")]
			private static extern int PrxSetPsStoreIconDisplayState(SetPsStoreIconDisplayStateRequest request, out APIResult result);

			#endregion

			#region Common

			/// <summary>
			///  Represents a label of a category on the PlayStation Store.
			/// </summary>
			/// <remarks>
			/// Renamed from CategoryId in SDK 4.5
			/// </remarks>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public struct CategoryLabel
			{
				/// <summary>
				/// Maximum length a category label can have.
				/// </summary>
				public const int CATEGORY_LABEL_MAX_LEN = 16;

				/// <summary>
				/// Maximum length a category label in SDK 4.0
				/// </summary>
				public const int SDK4_0_CATEGORY_LABEL_MAX_LEN = 55;

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = SDK4_0_CATEGORY_LABEL_MAX_LEN + 1)]
				internal string internalValue;

				/// <summary>
				/// Category label value, identifying a Category of the title.
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the category id is more than <see cref="CATEGORY_LABEL_MAX_LEN"/> characters.</exception>
				public string Value
				{
					get { return internalValue; }
					set
					{
						int maxLength = CATEGORY_LABEL_MAX_LEN;

						if (Main.initResult.sceSDKVersion < 0x04500000 )
						{
							maxLength = SDK4_0_CATEGORY_LABEL_MAX_LEN;
						}

						if (value.Length > maxLength)
						{
							throw new NpToolkitException("The size of the label is more than " + maxLength + " characters.");
						}
						internalValue = value;
					}
				}

				internal void Read(MemoryBuffer buffer)
				{
					buffer.ReadString(ref internalValue);
				}
			}

			/// <summary>
			/// Represents a label of a Service Entitlement on the PlayStation Store.
			/// </summary>
			/// <remarks>
			/// Renamed from EntitlementId in SDK 4.5
			/// </remarks>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public struct ServiceEntitlementLabel
			{
				/// <summary>
				/// Length a service entitlement label has.
				/// </summary>
				public const int SERVICE_ENTITLEMENT_LABEL_MAX_LEN = 6;

				/// <summary>
				/// Length a service entitlement label in SDK 4.0
				/// </summary>
				public const int SDK4_0_SERVICE_ENTITLEMENT_LABEL_MAX_LEN = 31;

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = SDK4_0_SERVICE_ENTITLEMENT_LABEL_MAX_LEN + 1)]
				internal string internalValue;

				/// <summary>
				/// Service Entitlement label value, identifying a Service Entitlement of the title.
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the label is more than <see cref="SERVICE_ENTITLEMENT_LABEL_MAX_LEN"/> characters.</exception>
				public string Value
				{
					get { return internalValue; }
					set
					{
						int maxLength = SERVICE_ENTITLEMENT_LABEL_MAX_LEN;

						if (Main.initResult.sceSDKVersion < 0x04500000 )
						{
							maxLength = SDK4_0_SERVICE_ENTITLEMENT_LABEL_MAX_LEN;
						}

						if (value.Length > maxLength)
						{
							throw new NpToolkitException("The size of the label is more than " + maxLength + " characters.");
						}
						internalValue = value;
					}
				}

				internal void Read(MemoryBuffer buffer)
				{
					buffer.ReadString(ref internalValue);
				}
			}

			/// <summary>
			/// Represents a label of a Product on the PlayStation Store.
			/// </summary>
			/// <remarks>
			/// Renamed from ProductId in SDK 4.5
			/// </remarks>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public struct ProductLabel
			{
				/// <summary>
				///  Length a product label has.
				/// </summary>
				public const int PRODUCT_LABEL_MAX_LEN = 16;

				/// <summary>
				///  Length a product label in SDK 4.0
				/// </summary>
				public const int SDK4_0_PRODUCT_LABEL_MAX_LEN = 47;

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = SDK4_0_PRODUCT_LABEL_MAX_LEN + 1)]
				internal string internalValue;

				/// <summary>
				/// Product label value, identifying a Product of the title.
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the label is more than <see cref="PRODUCT_LABEL_MAX_LEN"/> characters.</exception>
				public string Value
				{
					get { return internalValue; }
					set
					{
						int maxLength = PRODUCT_LABEL_MAX_LEN;

						if (Main.initResult.sceSDKVersion < 0x04500000 )
						{
							maxLength = SDK4_0_PRODUCT_LABEL_MAX_LEN;
						}

						if (value.Length > maxLength)
						{
							throw new NpToolkitException("The size of the label is more than " + maxLength + " characters.");
						}
						internalValue = value;
					}
				}

				internal void Read(MemoryBuffer buffer)
				{
					buffer.ReadString(ref internalValue);
				}
			}

			/// <summary>
			/// Represents a label of a Sku on the PlayStation Store.
			/// </summary>
			/// <remarks>
			/// Renamed from SkuId in SDK 4.5
			/// </remarks>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public struct SkuLabel
			{
				/// <summary>
				/// Length a sku label has.
				/// </summary>
				public const int SKU_LABEL_MAX_LEN = 4;

				/// <summary>
				/// Length of SKU label in SDK 4.0
				/// </summary>
				public const int SDK4_0_SKU_LABEL_MAX_LEN = 55;

				// Must match marshaled size in C++. Always the largest the value could be
				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = SDK4_0_SKU_LABEL_MAX_LEN + 1)]
				internal string internalValue;

				/// <summary>
				/// Sku label value, identifying a Sku of a product of the title.
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the label is more than <see cref="SKU_LABEL_MAX_LEN"/> characters.</exception>
				public string Value
				{
					get { return internalValue; }
					set
					{
						int maxLength = SKU_LABEL_MAX_LEN;

						if (Main.initResult.sceSDKVersion < 0x04500000 )
						{
							maxLength = SDK4_0_SKU_LABEL_MAX_LEN;
						}

						if (value.Length > maxLength)
						{
							throw new NpToolkitException("The size of the label is more than " + maxLength + " characters.");
						}
					
						internalValue = value;
					}
				}

				internal void Read(MemoryBuffer buffer)
				{
					buffer.ReadString(ref internalValue);
				}
			}

			/// <summary>
			///  Represents a target of the DownloadList dialog. A target can be contained of only the product label, or the combination of product label and sku label.
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public struct DownloadListTarget
			{
				internal ProductLabel productLabel;
				internal SkuLabel skuLabel;

				/// <summary>
				/// The product label of the target to show on the dialog.
				/// </summary>
				public ProductLabel ProductLabel
				{
					get { return productLabel; }
					set { productLabel = value; }
				}

				/// <summary>
				/// Optional. The sku label of the product target to show on the dialog.
				/// </summary>
				public SkuLabel SkuLabel
				{
					get { return skuLabel; }
					set { skuLabel = value; }
				}
			}

			/// <summary>
			/// Represents a target of the Checkout dialog. A target can be contained of only the product label; the combination of product label and sku label; or the combination of product label, sku label and service label.
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public struct CheckoutTarget
			{
				/// <summary>
				/// Invalid service label. Set <see cref="ServiceLabel"/> to this if <see cref="SkuLabel"/> isn't set.
				/// </summary>
				public const UInt32 NP_INVALID_SERVICE_LABEL = 0xFFFFFFFF;

				internal ProductLabel productLabel;
				internal SkuLabel skuLabel;
				internal UInt32 serviceLabel;

				/// <summary>
				/// The product label of the target to show on the dialog.
				/// </summary>
				public ProductLabel ProductLabel
				{
					get { return productLabel; }
					set { productLabel = value; }
				}

				/// <summary>
				/// Optional (mandatory if <see cref="ServiceLabel"/> is provided). The sku label of the product target to show on the dialog.
				/// </summary>
				public SkuLabel SkuLabel
				{
					get { return skuLabel; }
					set { skuLabel = value; }
				}

				/// <summary>
				/// Optional. The service label of the product and sku to show on the dialog, in case more than one service label is configured.
				/// </summary>
				public UInt32 ServiceLabel
				{
					get { return serviceLabel; }
					set { serviceLabel = value; }
				}
			}

			/// <summary>
			/// Details of a sub category on the PlayStation Store
			/// </summary>
			public class SubCategory
			{
				internal string categoryName;
				internal string categoryDescription;
				internal string imageUrl;
				internal CategoryLabel categoryLabel;

				/// <summary>
				/// The name of the subcategory
				/// </summary>
				public string CategoryName { get { return categoryName; } }

				/// <summary>
				/// The detailed description of the subcategory
				/// </summary>
				public string CategoryDescription { get { return categoryDescription; } }

				/// <summary>
				/// The image URL of the subcategory
				/// </summary>
				public string ImageUrl { get { return imageUrl; } }

				/// <summary>
				/// The ID of the subcategory
				/// </summary>
				public CategoryLabel CategoryLabel { get { return categoryLabel; } }

				// Read data from PRX marshaled buffer
				internal void Read(MemoryBuffer buffer)
				{
					buffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.SubCategoryBegin);

					buffer.ReadString(ref categoryName);
					buffer.ReadString(ref categoryDescription);
					buffer.ReadString(ref imageUrl);

					categoryLabel.Read(buffer);

					buffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.SubCategoryEnd);
				}
			}

			/// <summary>
			/// The category details that were returned from the PlayStation Store
			/// </summary>
			public class Category
			{
				internal SubCategory[] subCategories;
				internal UInt64 countOfProducts;
				internal string categoryName;
				internal string categoryDescription;
				internal string imageUrl;
				internal CategoryLabel categoryLabel;

				/// <summary>
				/// A list of subcategories in this category
				/// </summary>
				public SubCategory[] SubCategories
				{
					get { return subCategories; }
				}

				/// <summary>
				/// The number of products in the category
				/// </summary>
				public UInt64 CountOfProducts { get { return countOfProducts; } }

				/// <summary>
				/// The name of the category
				/// </summary>
				public string CategoryName { get { return categoryName; } }

				/// <summary>
				/// The detailed description of the category
				/// </summary>
				public string CategoryDescription { get { return categoryDescription; } }

				/// <summary>
				/// The image URL of the category
				/// </summary>
				public string ImageUrl { get { return imageUrl; } }

				/// <summary>
				/// The label of the category.
				/// </summary>
				public CategoryLabel CategoryLabel { get { return categoryLabel; } }

				// Read data from PRX marshaled buffer
				internal void Read(MemoryBuffer buffer)
				{
					buffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.CategoryBegin);

					UInt64 numSubCategories = buffer.ReadUInt64();

					if (numSubCategories > 0)
					{
						subCategories = new SubCategory[numSubCategories];

						for (UInt64 i = 0; i < numSubCategories; i++)
						{
							subCategories[i] = new SubCategory();

							subCategories[i].Read(buffer);
						}
					}

					countOfProducts = buffer.ReadUInt64();

					buffer.ReadString(ref categoryName);
					buffer.ReadString(ref categoryDescription);
					buffer.ReadString(ref imageUrl);

					categoryLabel.Read(buffer);

					buffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.CategoryEnd);
				}
			}

			/// <summary>
			/// Information about a product on the PlayStation Store
			/// </summary>
			public class Product
			{
				internal ProductLabel productLabel;
				internal string productName;
				internal string imageUrl;
				internal bool hasDetails;
				internal ProductDetails details;

				/// <summary>
				/// The product label to identify the product on the title.
				/// </summary>
				public ProductLabel ProductLabel { get { return productLabel; } }

				/// <summary>
				/// The name of the product
				/// </summary>
				public string ProductName { get { return productName; } }

				/// <summary>
				/// The product image URL
				/// </summary>
				public string ImageUrl { get { return imageUrl; } }

				/// <summary>
				/// Specifies whether <see cref="Details"/> is set or not
				/// </summary>
				public bool HasDetails { get { return hasDetails; } }

				/// <summary>
				/// Additional details that are only set when specific products are requested with <see cref="GetProducts"/>
				/// </summary>
				public ProductDetails Details { get { return details; } }

				// Read data from PRX marshaled buffer
				internal void Read(MemoryBuffer buffer)
				{
					buffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.ProductBegin);

					productLabel.Read(buffer);
					buffer.ReadString(ref productName);
					buffer.ReadString(ref imageUrl);
					hasDetails = buffer.ReadBool();

					if (hasDetails == true)
					{
						details = new ProductDetails();
						details.Read(buffer);
					}

					buffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.ProductEnd);
				}
			}

			/// <summary>
			/// The purchasability status
			/// </summary>
			public enum PurchasabilityStatus
			{
				/// <summary> Specifies that the user has not purchased this product or SKU </summary>
				NotPurchased,
				/// <summary> Specifies that a product has already been purchased and can be purchased again (consumable or time limited entitlements) </summary>
				PurchasedCanPurchaseAgain,	
				/// <summary> Specifies that a product has already been purchased and cannot be purchased again </summary>
				PurchasedCannotPurchaseAgain
			};

			/// <summary>
			/// The rating descriptors associated with a product on the PlayStation Store
			/// </summary>
			public class RatingDescriptor
			{
				internal string name;
				internal string imageUrl;

				/// <summary>
				/// The name rating descriptor, for example "Strong Language" or "Use of Alcohol"
				/// </summary>
				public string Name { get { return name; } }

				/// <summary>
				/// The URL of the rating descriptor image
				/// </summary>
				public string ImageUrl { get { return imageUrl; } }

				internal void Read(MemoryBuffer buffer)
				{
					buffer.ReadString(ref name);
					buffer.ReadString(ref imageUrl);
				}
			}

			/// <summary>
			/// Detailed information about a product on the PlayStation Store
			/// </summary>
			/// <remarks>
			/// Detailed information about a product on the PlayStation Store. If specific products are requested using <see cref="GetProducts"/>,
			/// the property <see cref="Product.Details"/> will contain additional product and SKU details. The product
			/// details here can be used for 'in-game browsing', with the SKU ID being used in conjunction with "displayCheckoutDialog()".
			/// </remarks>
			public class ProductDetails
			{
				internal DateTime releaseDate;
				internal string longDescription;
				internal string spName;
				internal string ratingSystemId;
				internal string ratingImageUrl;

				internal RatingDescriptor[] ratingDescriptors;
				internal SkuInfo[] skuinfo;

				internal PurchasabilityStatus purchasabilityStatus;
				internal UInt32 starRatingsTotal;
				internal double starRatingScore;

				/// <summary>
				/// The product release date
				/// </summary>
				public DateTime ReleaseDate { get { return releaseDate; } }

				/// <summary>
				/// A long description of the product
				/// </summary>
				public string LongDescription { get { return longDescription; } }

				/// <summary>
				/// The service provider (publisher) name
				/// </summary>
				public string SpName { get { return spName; } }

				/// <summary>
				/// The ID of the rating system (for example: PEGI, ESRB)
				/// </summary>
				public string RatingSystemId { get { return ratingSystemId; } }

				/// <summary>
				/// The URL of the rating icon image
				/// </summary>
				public string RatingImageUrl { get { return ratingImageUrl; } }

				/// <summary>
				/// Rating descriptors
				/// </summary>
				public RatingDescriptor[] RatingDescriptors { get { return ratingDescriptors; } }

				/// <summary>
				/// SKU information
				/// </summary>
				public SkuInfo[] Skuinfo { get { return skuinfo; } }

				/// <summary>
				/// Purchasability status
				/// </summary>
				public PurchasabilityStatus PurchasabilityStatus { get { return purchasabilityStatus; } }

				/// <summary>
				/// The total number of star ratings that users have given this product
				/// </summary>
				public UInt32 StarRatingsTotal { get { return starRatingsTotal; } }

				/// <summary>
				/// The average star rating given by users
				/// </summary>
				public double StarRatingScore { get { return starRatingScore; } }

				internal void Read(MemoryBuffer buffer)
				{
					buffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.ProductDetailsBegin);

					releaseDate = Core.ReadRtcTick(buffer);

					buffer.ReadString(ref longDescription);
					buffer.ReadString(ref spName);
					buffer.ReadString(ref ratingSystemId);
					buffer.ReadString(ref ratingImageUrl);

					UInt64 numDescriptors = buffer.ReadUInt64();

					if (numDescriptors > 0)
					{
						ratingDescriptors = new RatingDescriptor[numDescriptors];
						for (int i = 0; i < (int)numDescriptors; i++)
						{
							ratingDescriptors[i] = new RatingDescriptor();
							ratingDescriptors[i].Read(buffer);
						}
					}
					else
					{
						ratingDescriptors = null;
					}

					UInt64 numSkus = buffer.ReadUInt64();

					if (numSkus > 0)
					{
						skuinfo = new SkuInfo[numSkus];
						for (int i = 0; i < (int)numSkus; i++)
						{
							skuinfo[i] = new SkuInfo();
							skuinfo[i].Read(buffer);
						}
					}
					else
					{
						skuinfo = null;
					}

					purchasabilityStatus = (PurchasabilityStatus)buffer.ReadUInt32();
					starRatingsTotal = buffer.ReadUInt32();

					starRatingScore = buffer.ReadDouble();

					buffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.ProductDetailsEnd);
				}
			}

			/// <summary>
			/// The type of SKU that was returned
			/// </summary>
			public enum SkuType
			{
				/// <summary> Value not set </summary>
				Invalid,
				/// <summary> A standard SKU </summary>
				Standard,
				/// <summary> A pre-order SKU </summary>
				Preorder
			};

			/// <summary>
			/// SKU information for a product on the PlayStation Store
			/// </summary>
			public class SkuInfo
			{
				internal SkuType type;
				internal PurchasabilityStatus purchasabilityStatus;

				internal SkuLabel label;
				internal string name;
				internal string price;

				internal UInt64 intPrice;
				internal UInt32 consumableUseCount;

				/// <summary>
				/// The type of SKU
				/// </summary>
				public SkuType Type { get { return type; } }

				/// <summary>
				/// Purchasability status
				/// </summary>
				public PurchasabilityStatus PurchasabilityStatus { get { return purchasabilityStatus; } }

				/// <summary>
				/// The label of the SKU.
				/// </summary>
				public SkuLabel Label { get { return label; } }

				/// <summary>
				/// The name of the SKU. Only use this in conjunction with the product name if there are more than one SKUs
				/// </summary>
				public string Name { get { return name; } }

				/// <summary>
				/// The price of the SKU. This is formatted to include the currency code or currency symbol
				/// </summary>
				public string Price { get { return price; } }

				/// <summary>
				/// Integer representation of the price. Not for user display
				/// </summary>
				public UInt64 IntPrice { get { return intPrice; } }

				/// <summary>
				/// The number of uses that will be given if the user purchases the SKU (provided that the product entitles the user to a consumable service entitlement)
				/// </summary>
				public UInt32 ConsumableUseCount { get { return consumableUseCount; } }

				internal void Read(MemoryBuffer buffer)
				{
					buffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.SkuInfoBegin);

					type = (SkuType)buffer.ReadUInt32();
					purchasabilityStatus = (PurchasabilityStatus)buffer.ReadUInt32();

					label.Read(buffer);
					buffer.ReadString(ref name);
					buffer.ReadString(ref price);

					intPrice = buffer.ReadUInt64();
					consumableUseCount = buffer.ReadUInt32();

					buffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.SkuInfoEnd);
				}
			}

			/// <summary>
			/// The type of entitlement
			/// </summary>
			public enum EntitlementType
			{
				/// <summary> Invalid entitlement type </summary>
				Invalid,
				/// <summary> Service entitlement </summary>
				Service,
				/// <summary> Consumable service entitlement </summary>
				ServiceConsumable,
				/// <summary> Unified entitlement </summary>
				Unified
			};

			/// <summary>
			/// Represents a service entitlement
			/// </summary>
			public class ServiceEntitlement
			{
				internal ServiceEntitlementLabel entitlementLabel;
				internal DateTime createdDate;
				internal DateTime expireDate;
				internal Int64 remainingCount;
				internal UInt32 consumedCount;
				internal EntitlementType type;

				/// <summary>
				/// The service entitlement ID
				/// </summary>
				public ServiceEntitlementLabel EntitlementLabel { get { return entitlementLabel; } }

				/// <summary>
				/// The date when the user initially got the entitlement
				/// </summary>
				public DateTime CreatedDate { get { return createdDate; } }

				/// <summary>
				/// The date when the entitlement expires
				/// </summary>
				public DateTime ExpireDate { get { return expireDate; } }

				/// <summary>
				/// The remaining uses for a consumable service entitlement. This may be a negative value
				/// </summary>
				public Int64 RemainingCount { get { return remainingCount; } }

				/// <summary>
				/// The amount of times a consumable service entitlement has been consumed
				/// </summary>
				public UInt32 ConsumedCount { get { return consumedCount; } }

				/// <summary>
				/// The type of entitlement
				/// </summary>
				public EntitlementType Type { get { return type; } }

				internal void Read(MemoryBuffer buffer)
				{
					buffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.ServiceEntitlementBegin);

					entitlementLabel.Read(buffer);

					createdDate = Core.ReadRtcTick(buffer);
					expireDate = Core.ReadRtcTick(buffer);

					remainingCount = buffer.ReadInt64();

					consumedCount = buffer.ReadUInt32();

					type = (EntitlementType)buffer.ReadUInt32();

					buffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.ServiceEntitlementEnd);
				}
			}

			#endregion

			#region Requests

			/// <summary>
			/// Parameters required to retrieve category information from the PlayStation Store
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
			public class GetCategoriesRequest : RequestBase
			{
				/// <summary>
				/// The maximum number of categories per request
				/// </summary>
				public const int MAX_CATEGORIES = 8;

				internal UInt64 numCategories;

				[MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_CATEGORIES)]
				internal CategoryLabel[] categoryLabels = new CategoryLabel[MAX_CATEGORIES];

				/// <summary>
				/// The labels of the categories to obtain the information about. Set to null to just get information about the root category
				/// </summary>
				public CategoryLabel[] CategoryLabels
				{
					get
					{
						if (numCategories == 0) return null;

						CategoryLabel[] ids = new CategoryLabel[numCategories];

						Array.Copy(categoryLabels, ids, (int)numCategories);

						return ids;
					}
					set
					{
						if (value != null)
						{
							if (value.Length > MAX_CATEGORIES)
							{
								throw new NpToolkitException("The size of the array is more than " + MAX_CATEGORIES);
							}

							value.CopyTo(categoryLabels, 0);
							numCategories = (UInt32)value.Length;
						}
						else
						{
							numCategories = 0;
						}
					}
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="GetCategoriesRequest"/> class.
				/// </summary>
				public GetCategoriesRequest()
					: base(ServiceTypes.Commerce, FunctionTypes.CommerceGetCategories)
				{

				}
			}

			/// <summary>
			/// The sorting order of products that are returned from the PlayStation Store
			/// </summary>
			public enum ProductSortOrders
			{
				/// <summary> Products will be returned in default order </summary>
				DefaultOrder,
				/// <summary> Products will be returned in alphabetical order </summary>
				Name,
				/// <summary> Products will be returned ordered by price </summary>
				Price,
				/// <summary> Products will be returned ordered by release date </summary>
				ReleaseDate	
			}

			/// <summary>
			/// The sorting direction of products that are returned from the PlayStation Store
			/// </summary>
			public enum ProductSortDirections
			{
				/// <summary> Products returned in ascending order </summary>
				Ascending,
				/// <summary> Products returned in descending order </summary>
				Descending	
			}

			/// <summary>
			/// Parameters required to retrieve category information from the PlayStation Store
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public class GetProductsRequest : RequestBase
			{
				/// <summary>
				/// The maximum number of products per request
				/// </summary>
				public const int MAX_PRODUCTS = 32;

				/// <summary>
				/// The maximum number of categories per request
				/// </summary>
				public const int MAX_CATEGORIES = 8;

				/// <summary>
				/// The default page size for product ids
				/// </summary>
				public const int DEFAULT_PAGE_SIZE = 32;

				internal UInt64 numProducts;

				[MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_PRODUCTS)]
				internal ProductLabel[] productLabels = new ProductLabel[MAX_PRODUCTS];

				internal UInt64 numCategories;

				[MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_CATEGORIES)]
				internal CategoryLabel[] categoryLabels = new CategoryLabel[MAX_CATEGORIES];

				internal UInt32 offset;   //  This is the starting index
				internal UInt32 pageSize;   // Maximum number of entitlements to return. Defaults to <c>DEFAULT_PAGE_SIZE</c>
				internal ProductSortOrders sortOrder;
				internal ProductSortDirections sortDirection;

				[MarshalAs(UnmanagedType.I1)]
				internal bool keepHtmlTags;

				[MarshalAs(UnmanagedType.I1)]
				internal bool useCurrencySymbol;  

				/// <summary>
				/// Labels of the specific products we want to retrieve. If null is specified, all products up to pageSize will be retrieved
				/// </summary>
				public ProductLabel[] ProductLabels
				{
					get
					{
						if (numProducts == 0) return null;

						ProductLabel[] ids = new ProductLabel[numProducts];

						Array.Copy(productLabels, ids, (int)numProducts);

						return ids;
					}
					set
					{
						if (value != null)
						{
							if (value.Length > MAX_PRODUCTS)
							{
								throw new NpToolkitException("The size of the array is more than " + MAX_PRODUCTS);
							}

							value.CopyTo(productLabels, 0);
							numProducts = (UInt32)value.Length;
						}
						else
						{
							numProducts = 0;
						}
					}
				}

				/// <summary>
				/// The labels of the categories to obtain the information about. Set to null to just get information about the root category
				/// </summary>
				public CategoryLabel[] CategoryLabels
				{
					get
					{
						if (numCategories == 0) return null;

						CategoryLabel[] ids = new CategoryLabel[numCategories];

						Array.Copy(categoryLabels, ids, (int)numCategories);

						return ids;
					}
					set
					{
						if (value != null)
						{
							if (value.Length > MAX_CATEGORIES)
							{
								throw new NpToolkitException("The size of the array is more than " + MAX_CATEGORIES);
							}

							value.CopyTo(categoryLabels, 0);
							numCategories = (UInt32)value.Length;
						}
						else
						{
							numCategories = 0;
						}
					}
				}

				/// <summary>
				/// If many products exist, paging can be used. This is the starting index
				/// </summary>
				public UInt32 Offset
				{
					get { return offset; }
					set { offset = value; }
				}

				/// <summary>
				/// Maximum number of entitlements to return. Defaults to <see cref="DEFAULT_PAGE_SIZE"/>
				/// </summary>
				public UInt32 PageSize
				{
					get { return pageSize; }
					set { pageSize = value; }
				}

				/// <summary>
				/// Sorting order in which the products are returned
				/// </summary>
				public ProductSortOrders SortOrder
				{
					get { return sortOrder; }
					set { sortOrder = value; }
				}

				/// <summary>
				/// Sorting direction in which the products are returned
				/// </summary>
				public ProductSortDirections SortDirection
				{
					get { return sortDirection; }
					set { sortDirection = value; }
				}

				/// <summary>
				/// Keep HTML tags in the product long description. false by default
				/// </summary>
				public bool KeepHtmlTags
				{
					get { return keepHtmlTags; }
					set { keepHtmlTags = value; }
				}

				/// <summary>
				/// Use currency symbol (example: $), instead currency code (example: USD). false by default
				/// </summary>
				public bool UseCurrencySymbol
				{
					get { return useCurrencySymbol; }
					set { useCurrencySymbol = value; }
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="GetProductsRequest"/> class.
				/// </summary>
				public GetProductsRequest()
					: base(ServiceTypes.Commerce, FunctionTypes.CommerceGetProducts)
				{
					pageSize = DEFAULT_PAGE_SIZE;
				}
			}


			/// <summary>
			/// Parameters required to retrieve a users service entitlements
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public class GetServiceEntitlementsRequest : RequestBase
			{
				/// <summary>
				/// The default page size for product ids
				/// </summary>
				public const int DEFAULT_PAGE_SIZE = 64;

				internal UInt32 offset;   //  This is the starting index
				internal UInt32 pageSize;   // Maximum number of entitlements to return. Defaults to <c>DEFAULT_PAGE_SIZE</c>

				/// <summary>
				/// If many service entitlements exist, paging can be used. This is the starting index
				/// </summary>
				public UInt32 Offset
				{
					get { return offset; }
					set { offset = value; }
				}

				/// <summary>
				/// Maximum number of entitlements to return. Defaults to <see cref="DEFAULT_PAGE_SIZE"/>
				/// </summary>
				public UInt32 PageSize
				{
					get { return pageSize; }
					set { pageSize = value; }
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="GetServiceEntitlementsRequest"/> class.
				/// </summary>
				public GetServiceEntitlementsRequest()
					: base(ServiceTypes.Commerce, FunctionTypes.CommerceGetServiceEntitlements)
				{
					pageSize = DEFAULT_PAGE_SIZE;
				}
			}

			/// <summary>
			/// Input parameters for consuming from a service entitlement
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public class ConsumeServiceEntitlementRequest : RequestBase
			{
				internal ServiceEntitlementLabel entitlementLabel;
				internal UInt32 consumedCount;

				/// <summary>
				/// The service entitlement label
				/// </summary>
				public ServiceEntitlementLabel EntitlementLabel
				{
					get { return entitlementLabel; }
					set { entitlementLabel = value; }
				}

				/// <summary>
				/// The amount to consume
				/// </summary>
				public UInt32 ConsumedCount
				{
					get { return consumedCount; }
					set { consumedCount = value; }
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="ConsumeServiceEntitlementRequest"/> class.
				/// </summary>
				public ConsumeServiceEntitlementRequest()
					: base(ServiceTypes.Commerce, FunctionTypes.CommerceConsumeServiceEntitlement)
				{
					
				}
			}

			/// <summary>
			/// Input parameters required to display a category on the PlayStation Store
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public class DisplayCategoryBrowseDialogRequest : RequestBase
			{
				internal CategoryLabel categoryLabel;

				/// <summary>
				/// The label of the category to display. To open the root category, don't set this value.
				/// </summary>
				public CategoryLabel CategoryLabel
				{
					get { return categoryLabel; }
					set { categoryLabel = value; }
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="DisplayCategoryBrowseDialogRequest"/> class.
				/// </summary>
				public DisplayCategoryBrowseDialogRequest()
					: base(ServiceTypes.Commerce, FunctionTypes.CommerceDisplayCategoryBrowseDialog)
				{

				}
			}

			/// <summary>
			/// Parameters required to display a product browse dialog
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public class DisplayProductBrowseDialogRequest : RequestBase
			{
				internal ProductLabel productLabel;

				/// <summary>
				/// he label of the product to display.
				/// </summary>
				public ProductLabel ProductLabel
				{
					get { return productLabel; }
					set { productLabel = value; }
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="DisplayProductBrowseDialogRequest"/> class.
				/// </summary>
				public DisplayProductBrowseDialogRequest()
					: base(ServiceTypes.Commerce, FunctionTypes.CommerceDisplayProductBrowseDialog)
				{

				}
			}

			/// <summary>
			/// Parameters required to display the voucher input dialog
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public class DisplayVoucherCodeInputDialogRequest : RequestBase
			{
				/// <summary>
				/// The default page size for product ids
				/// </summary>
				public const int VOUCHER_CODE_LEN = 63;

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = VOUCHER_CODE_LEN + 1)]
				internal string voucherCode;

				/// <summary>
				/// Voucher code can be pre-filled. If not set, user can input voucher code manually
				/// </summary>
				/// <exception cref="NpToolkitException">Will throw an exception if the voucher code is more than <see cref="VOUCHER_CODE_LEN"/> characters.</exception>
				public string VoucherCode
				{
					get { return voucherCode; }
					set
					{
						if (value.Length > VOUCHER_CODE_LEN)
						{
							throw new NpToolkitException("The size of the voucher code is more than " + VOUCHER_CODE_LEN + " characters.");
						}
						voucherCode = value;
					}
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="DisplayVoucherCodeInputDialogRequest"/> class.
				/// </summary>
				public DisplayVoucherCodeInputDialogRequest()
					: base(ServiceTypes.Commerce, FunctionTypes.CommerceDisplayVoucherCodeInputDialog)
				{

				}
			}

			/// <summary>
			/// Parameters required to display the checkout dialog
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public class DisplayCheckoutDialogRequest : RequestBase
			{
				/// <summary>
				/// The maximum amount of targets that can be set.
				/// </summary>
				public const int MAX_TARGETS = 10;

				internal UInt64 numTargets;

				[MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_TARGETS)]
				internal CheckoutTarget[] targets = new CheckoutTarget[MAX_TARGETS];

				/// <summary>
				/// The targets to display in the checkout.
				/// </summary>
				/// <remarks>
				/// CheckoutTarget Sku Ids must not be hard coded as they can be changed any time after release. 
				/// CheckoutTarget Sku Ids must be retrieved programactically using APIs such as <see cref="GetProducts"/>.
				/// </remarks>
				public CheckoutTarget[] Targets
				{
					get
					{
						if (numTargets == 0) return null;

						CheckoutTarget[] copyTargets = new CheckoutTarget[numTargets];

						Array.Copy(targets, copyTargets, (int)numTargets);

						return copyTargets;
					}
					set
					{
						if (value != null)
						{
							if (value.Length > MAX_TARGETS)
							{
								throw new NpToolkitException("The size of the array is more than " + MAX_TARGETS);
							}

							value.CopyTo(targets, 0);
							numTargets = (UInt64)value.Length;
						}
						else
						{
							numTargets = 0;
						}
					}
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="DisplayCheckoutDialogRequest"/> class.
				/// </summary>
				public DisplayCheckoutDialogRequest()
					: base(ServiceTypes.Commerce, FunctionTypes.CommerceDisplayCheckoutDialog)
				{

				}
			}

			/// <summary>
			/// Parameters required to display a users download list
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public class DisplayDownloadListDialogRequest : RequestBase
			{
				/// <summary>
				/// The maximum number of targets that can be set.
				/// </summary>
				public const int MAX_TARGETS = 10;

				internal UInt64 numTargets;

				[MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_TARGETS)]
				internal DownloadListTarget[] targets = new DownloadListTarget[MAX_TARGETS];

				/// <summary>
				/// Specific targets to display in the download list. Specify null to display all available products to the user
				/// </summary>
				public DownloadListTarget[] Targets
				{
					get
					{
						if (numTargets == 0) return null;

						DownloadListTarget[] copyTargets = new DownloadListTarget[numTargets];

						Array.Copy(targets, copyTargets, (int)numTargets);

						return copyTargets;
					}
					set
					{
						if (value != null)
						{
							if (value.Length > MAX_TARGETS)
							{
								throw new NpToolkitException("The size of the array is more than " + MAX_TARGETS);
							}

							value.CopyTo(targets, 0);
							numTargets = (UInt64)value.Length;
						}
						else
						{
							numTargets = 0;
						}
					}
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="DisplayDownloadListDialogRequest"/> class.
				/// </summary>
				public DisplayDownloadListDialogRequest()
					: base(ServiceTypes.Commerce, FunctionTypes.CommerceDisplayDownloadListDialog)
				{

				}
			}

			/// <summary>
			/// Parameters required to display a screen where the user can join PlayStation Plus
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public class DisplayJoinPlusDialogRequest : RequestBase
			{
				// Default to SCE_NP_PLUS_FEATURE_REALTIME_MULTIPLAY. This is also the only flag so it is hard coded at the moment.
				// This is provide for completeness as additional flags maybe added in the future to the SDK.
				internal UInt64 features; 

				/// <summary>
				/// Initializes a new instance of the <see cref="DisplayJoinPlusDialogRequest"/> class.
				/// </summary>
				public DisplayJoinPlusDialogRequest()
					: base(ServiceTypes.Commerce, FunctionTypes.CommerceDisplayJoinPlusDialog)
				{
					features = 0x1;
				}
			}

			/// <summary>
			/// PS Store Icon Position
			/// </summary>
			public enum PsStoreIconPos
			{
				/// <summary> Screen lower center </summary>
				Center = 0,
				/// <summary> Screen lower left </summary>
				Left,
				/// <summary> Screen lower right </summary>
				Right,
			}

			/// <summary>
			/// Parameters required to show or hide the PlayStation Store icon as an overlay
			/// </summary>
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public class SetPsStoreIconDisplayStateRequest : RequestBase
			{
				internal PsStoreIconPos iconPosition;

				[MarshalAs(UnmanagedType.I1)]
				internal bool showIcon;

				/// <summary>
				/// The position of the icon on the screen
				/// </summary>
				public PsStoreIconPos IconPosition
				{
					get { return iconPosition; }
					set { iconPosition = value; }
				}

				/// <summary>
				/// Set to true to display the Store icon, and false to remove it
				/// </summary>
				public bool ShowIcon
				{
					get { return showIcon; }
					set { showIcon = value; }
				}

				/// <summary>
				/// Initializes a new instance of the <see cref="SetPsStoreIconDisplayStateRequest"/> class.
				/// </summary>
				public SetPsStoreIconDisplayStateRequest()
					: base(ServiceTypes.Commerce, FunctionTypes.CommerceSetPsStoreIconDisplayState)
				{

				}
			}

			#endregion

			#region Get Categories

			/// <summary>
			/// The category details that were returned from the PlayStation Store
			/// </summary>
			public class CategoriesResponse : ResponseBase
			{
				internal Category[] categories;
				
				/// <summary>
				/// The categories that were retrieved from the Store
				/// </summary>
				public Category[] Categories
				{
					get { return categories; }
				}

				/// <summary>
				/// Read the response data from the plug-in
				/// </summary>
				/// <param name="id">The request id.</param>
				/// <param name="apiCalled">The API called.</param>
				/// <param name="request">The Request object.</param>
				protected internal override void ReadResult(UInt32 id, FunctionTypes apiCalled, RequestBase request)
				{
					base.ReadResult(id, apiCalled, request);

					APIResult result;

					MemoryBuffer readBuffer = BeginReadResponseBuffer(id, apiCalled, out result);

					if (result.RaiseException == true) throw new NpToolkitException(result);

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.CategoriesBegin);

					UInt64 numCategories = readBuffer.ReadUInt64();

					if (numCategories > 0)
					{
						categories = new Category[numCategories];

						for (UInt64 i = 0; i < numCategories; i++)
						{
							categories[i] = new Category();

							categories[i].Read(readBuffer);
						}
					}
			
					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.CategoriesEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			/// <summary>
			/// Gets information about in-game categories on the PlayStation®Store
			/// </summary>
			/// <param name="request">Parameters needed to retrieve category information. To retrieve the root category, don't specify any category IDs</param>
			/// <param name="response">This response will contain the category information upon successful completion</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int GetCategories(GetCategoriesRequest request, CategoriesResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxGetCategories(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Get Products

			
			/// <summary>
			/// A list of products that were retrieved from the PlayStation Store
			/// </summary>
			public class ProductsResponse : ResponseBase
			{
				internal Product[] products;

				/// <summary>
				/// The categories that were retrieved from the Store
				/// </summary>
				public Product[] Products
				{
					get { return products; }
				}
				
				/// <summary>
				/// Read the response data from the plug-in
				/// </summary>
				/// <param name="id">The request id.</param>
				/// <param name="apiCalled">The API called.</param>
				/// <param name="request">The Request object.</param>
				protected internal override void ReadResult(UInt32 id, FunctionTypes apiCalled, RequestBase request)
				{
					base.ReadResult(id, apiCalled, request);

					APIResult result;

					MemoryBuffer readBuffer = BeginReadResponseBuffer(id, apiCalled, out result);

					if (result.RaiseException == true) throw new NpToolkitException(result);

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.ProductsBegin);

					UInt64 numProducts = readBuffer.ReadUInt64();

					if (numProducts > 0)
					{
						products = new Product[numProducts];

						for (UInt64 i = 0; i < numProducts; i++)
						{
							products[i] = new Product();

							products[i].Read(readBuffer);
						}
					}
				
					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.ProductsEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			/// <summary>
			/// Gets products that are available on the PlayStation®Store
			/// </summary>
			/// <param name="request">Parameters required for retrieving products for a user. This can contain specific category or product IDs</param>
			/// <param name="response">This response will contain a list of products upon successful completion</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int GetProducts(GetProductsRequest request, ProductsResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxGetProducts(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion


			#region Get Service Entitlements

			/// <summary>
			/// A list of service entitlements for a user that were retrieved from the PlayStation Network
			/// </summary>
			public class ServiceEntitlementsResponse : ResponseBase
			{
				internal ServiceEntitlement[] entitlements;
				internal UInt64 totalEntitlementsAvailable;

				/// <summary>
				/// The service entitlements that were returned
				/// </summary>
				public ServiceEntitlement[] Entitlements
				{
					get { return entitlements; }
				}

				/// <summary>
				/// Used for paging
				/// </summary>
				public UInt64 TotalEntitlementsAvailable
				{
					get { return totalEntitlementsAvailable; }
				}

				/// <summary>
				/// Read the response data from the plug-in
				/// </summary>
				/// <param name="id">The request id.</param>
				/// <param name="apiCalled">The API called.</param>
				/// <param name="request">The Request object.</param>
				protected internal override void ReadResult(UInt32 id, FunctionTypes apiCalled, RequestBase request)
				{
					base.ReadResult(id, apiCalled, request);

					APIResult result;

					MemoryBuffer readBuffer = BeginReadResponseBuffer(id, apiCalled, out result);

					if (result.RaiseException == true) throw new NpToolkitException(result);

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.ServiceEntitlementsBegin);

					totalEntitlementsAvailable = readBuffer.ReadUInt64();
					UInt64 numEntitlements = readBuffer.ReadUInt64();

					if (numEntitlements > 0)
					{
						entitlements = new ServiceEntitlement[numEntitlements];

						for (int i = 0; i < (int)numEntitlements; i++)
						{
							entitlements[i] = new ServiceEntitlement();
							entitlements[i].Read(readBuffer);
						}
					}
					else
					{
						entitlements = null;
					}

					readBuffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.ServiceEntitlementsEnd);

					EndReadResponseBuffer(readBuffer);
				}
			}

			/// <summary>
			/// Get a list of Service Entitlements for a given local user
			/// </summary>
			/// <param name="request"> Required parameters to retrieve a users Service Entitlements. If a user has many service entitlements, the request can be paged</param>
			/// <param name="response">This response will contain the users Service Entitlements on successful completion</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int GetServiceEntitlements(GetServiceEntitlementsRequest request, ServiceEntitlementsResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxGetServiceEntitlements(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Consume Service Entitlement

			/// <summary>
			/// Consume a Service Entitlement
			/// </summary>
			/// <param name="request"> The parameters required to consume from a service entitlement. Includes the entitlement ID and a number to consume</param>
			/// <param name="response"> This response does not have data, only return code</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int ConsumeServiceEntitlement(ConsumeServiceEntitlementRequest request, Core.EmptyResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxConsumeServiceEntitlement(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Display Category Browse Dialog

			/// <summary>
			/// This function opens the commerce dialog to a specified category on the PlayStation®Store
			/// </summary>
			/// <param name="request"> If no category ID is specified, the root category will be shown </param>
			/// <param name="response"> This response does not have data, only return code</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int DisplayCategoryBrowseDialog(DisplayCategoryBrowseDialogRequest request, Core.EmptyResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxDisplayCategoryBrowseDialog(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Display Product Browse Dialog

			/// <summary>
			/// This function opens the commerce dialog to a specified product on the PlayStation®Store
			/// </summary>
			/// <param name="request"> The parameters required to open the dialog to a specified product </param>
			/// <param name="response"> This response does not have data, only return code</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int DisplayProductBrowseDialog(DisplayProductBrowseDialogRequest request, Core.EmptyResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxDisplayProductBrowseDialog(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Display Voucher Code Input Dialog

			/// <summary>
			/// Display a dialog where the user can redeem a voucher code
			/// </summary>
			/// <param name="request"> The parameters required to open the voucher redemption dialog </param>
			/// <param name="response"> This response does not have data, only return code</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int DisplayVoucherCodeInputDialog(DisplayVoucherCodeInputDialogRequest request, Core.EmptyResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxDisplayVoucherCodeInputDialog(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Display Checkout Dialog

			/// <summary>
			/// This function opens the commerce dialog to a specified SKU ID on the PlayStation®Store
			/// </summary>
			/// <param name="request"> The SKU IDs that the user would like to purchase </param>
			/// <param name="response"> This response does not have data, only return code</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int DisplayCheckoutDialog(DisplayCheckoutDialogRequest request, Core.EmptyResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxDisplayCheckoutDialog(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Display Download List Dialog

			/// <summary>
			/// This function opens the commerce dialog to a users download list
			/// </summary>
			/// <param name="request"> Parameters required for opening the download list dialog</param>
			/// <param name="response"> This response does not have data, only return code</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int DisplayDownloadListDialog(DisplayDownloadListDialogRequest request, Core.EmptyResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxDisplayDownloadListDialog(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Display Join Plus Dialog

			/// <summary>
			/// Displays a screen to purchase entitlement to join PlayStation®Plus
			/// </summary>
			/// <param name="request"> Parameters required to show the join plus dialog </param>
			/// <param name="response"> This response does not have data, only return code</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int DisplayJoinPlusDialog(DisplayJoinPlusDialogRequest request, Core.EmptyResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxDisplayJoinPlusDialog(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion

			#region Set Ps-Store Icon Display State

			/// <summary>
			/// Show or hide the PlayStation®Store Icon
			/// </summary>
			/// <param name="request">  The parameters required to show/hide the icon, along with the position on the screen </param>
			/// <param name="response"> This response does not have data, only return code</param>
			/// <returns>If the operation is asynchronous, the function provides the request Id.</returns>
			/// <exception cref="NpToolkitException">Will throw an exception either when the request data is invalid, or an internal error has occured inside the NpToolkit plug-in.</exception>
			public static int SetPsStoreIconDisplayState(SetPsStoreIconDisplayStateRequest request, Core.EmptyResponse response)
			{
				APIResult result;

				if (response.locked == true)
				{
					throw new NpToolkitException("Response object is already locked");
				}

				int ret = PrxSetPsStoreIconDisplayState(request, out result);

				if (result.RaiseException == true) throw new NpToolkitException(result);

				RequestBase.FinaliseRequest(request, response, ret);

				return ret;
			}

			#endregion
		}
	}
}

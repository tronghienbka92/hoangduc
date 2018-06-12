using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Messages;
using Nop.Core.Domain.Orders;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Seo;
using Nop.Services.Stores;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Nop.Core.Domain.NhaXes;

namespace Nop.Services.ExportImport
{
    /// <summary>
    /// Export manager
    /// </summary>
    public partial class ExportManager : IExportManager
    {
        #region Fields

        private readonly ICategoryService _categoryService;
        private readonly IManufacturerService _manufacturerService;
        private readonly IProductAttributeService _productAttributeService;
        private readonly IPictureService _pictureService;
        private readonly INewsLetterSubscriptionService _newsLetterSubscriptionService;
        private readonly IStoreService _storeService;

        #endregion

        #region Ctor

        public ExportManager(ICategoryService categoryService,
            IManufacturerService manufacturerService,
            IProductAttributeService productAttributeService,
            IPictureService pictureService,
            INewsLetterSubscriptionService newsLetterSubscriptionService,
            IStoreService storeService)
        {
            this._categoryService = categoryService;
            this._manufacturerService = manufacturerService;
            this._productAttributeService = productAttributeService;
            this._pictureService = pictureService;
            this._newsLetterSubscriptionService = newsLetterSubscriptionService;
            this._storeService = storeService;
        }

        #endregion

        #region Utilities

        protected virtual void WriteCategories(XmlWriter xmlWriter, int parentCategoryId)
        {
            var categories = _categoryService.GetAllCategoriesByParentCategoryId(parentCategoryId, true);
            if (categories != null && categories.Count > 0)
            {
                foreach (var category in categories)
                {
                    xmlWriter.WriteStartElement("Category");
                    xmlWriter.WriteElementString("Id", null, category.Id.ToString());
                    xmlWriter.WriteElementString("Name", null, category.Name);
                    xmlWriter.WriteElementString("Description", null, category.Description);
                    xmlWriter.WriteElementString("CategoryTemplateId", null, category.CategoryTemplateId.ToString());
                    xmlWriter.WriteElementString("MetaKeywords", null, category.MetaKeywords);
                    xmlWriter.WriteElementString("MetaDescription", null, category.MetaDescription);
                    xmlWriter.WriteElementString("MetaTitle", null, category.MetaTitle);
                    xmlWriter.WriteElementString("SeName", null, category.GetSeName(0));
                    xmlWriter.WriteElementString("ParentCategoryId", null, category.ParentCategoryId.ToString());
                    xmlWriter.WriteElementString("PictureId", null, category.PictureId.ToString());
                    xmlWriter.WriteElementString("PageSize", null, category.PageSize.ToString());
                    xmlWriter.WriteElementString("AllowCustomersToSelectPageSize", null, category.AllowCustomersToSelectPageSize.ToString());
                    xmlWriter.WriteElementString("PageSizeOptions", null, category.PageSizeOptions);
                    xmlWriter.WriteElementString("PriceRanges", null, category.PriceRanges);
                    xmlWriter.WriteElementString("ShowOnHomePage", null, category.ShowOnHomePage.ToString());
                    xmlWriter.WriteElementString("IncludeInTopMenu", null, category.IncludeInTopMenu.ToString());
                    xmlWriter.WriteElementString("Published", null, category.Published.ToString());
                    xmlWriter.WriteElementString("Deleted", null, category.Deleted.ToString());
                    xmlWriter.WriteElementString("DisplayOrder", null, category.DisplayOrder.ToString());
                    xmlWriter.WriteElementString("CreatedOnUtc", null, category.CreatedOnUtc.ToString());
                    xmlWriter.WriteElementString("UpdatedOnUtc", null, category.UpdatedOnUtc.ToString());


                    xmlWriter.WriteStartElement("Products");
                    var productCategories = _categoryService.GetProductCategoriesByCategoryId(category.Id, 0, int.MaxValue, true);
                    foreach (var productCategory in productCategories)
                    {
                        var product = productCategory.Product;
                        if (product != null && !product.Deleted)
                        {
                            xmlWriter.WriteStartElement("ProductCategory");
                            xmlWriter.WriteElementString("ProductCategoryId", null, productCategory.Id.ToString());
                            xmlWriter.WriteElementString("ProductId", null, productCategory.ProductId.ToString());
                            xmlWriter.WriteElementString("ProductName", null, product.Name);
                            xmlWriter.WriteElementString("IsFeaturedProduct", null, productCategory.IsFeaturedProduct.ToString());
                            xmlWriter.WriteElementString("DisplayOrder", null, productCategory.DisplayOrder.ToString());
                            xmlWriter.WriteEndElement();
                        }
                    }
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("SubCategories");
                    WriteCategories(xmlWriter, category.Id);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Export manufacturer list to xml
        /// </summary>
        /// <param name="manufacturers">Manufacturers</param>
        /// <returns>Result in XML format</returns>
        public virtual string ExportManufacturersToXml(IList<Manufacturer> manufacturers)
        {
            var sb = new StringBuilder();
            var stringWriter = new StringWriter(sb);
            var xmlWriter = new XmlTextWriter(stringWriter);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Manufacturers");
            xmlWriter.WriteAttributeString("Version", NopVersion.CurrentVersion);

            foreach (var manufacturer in manufacturers)
            {
                xmlWriter.WriteStartElement("Manufacturer");

                xmlWriter.WriteElementString("ManufacturerId", null, manufacturer.Id.ToString());
                xmlWriter.WriteElementString("Name", null, manufacturer.Name);
                xmlWriter.WriteElementString("Description", null, manufacturer.Description);
                xmlWriter.WriteElementString("ManufacturerTemplateId", null, manufacturer.ManufacturerTemplateId.ToString());
                xmlWriter.WriteElementString("MetaKeywords", null, manufacturer.MetaKeywords);
                xmlWriter.WriteElementString("MetaDescription", null, manufacturer.MetaDescription);
                xmlWriter.WriteElementString("MetaTitle", null, manufacturer.MetaTitle);
                xmlWriter.WriteElementString("SEName", null, manufacturer.GetSeName(0));
                xmlWriter.WriteElementString("PictureId", null, manufacturer.PictureId.ToString());
                xmlWriter.WriteElementString("PageSize", null, manufacturer.PageSize.ToString());
                xmlWriter.WriteElementString("AllowCustomersToSelectPageSize", null, manufacturer.AllowCustomersToSelectPageSize.ToString());
                xmlWriter.WriteElementString("PageSizeOptions", null, manufacturer.PageSizeOptions);
                xmlWriter.WriteElementString("PriceRanges", null, manufacturer.PriceRanges);
                xmlWriter.WriteElementString("Published", null, manufacturer.Published.ToString());
                xmlWriter.WriteElementString("Deleted", null, manufacturer.Deleted.ToString());
                xmlWriter.WriteElementString("DisplayOrder", null, manufacturer.DisplayOrder.ToString());
                xmlWriter.WriteElementString("CreatedOnUtc", null, manufacturer.CreatedOnUtc.ToString());
                xmlWriter.WriteElementString("UpdatedOnUtc", null, manufacturer.UpdatedOnUtc.ToString());

                xmlWriter.WriteStartElement("Products");
                var productManufacturers = _manufacturerService.GetProductManufacturersByManufacturerId(manufacturer.Id, 0, int.MaxValue, true);
                if (productManufacturers != null)
                {
                    foreach (var productManufacturer in productManufacturers)
                    {
                        var product = productManufacturer.Product;
                        if (product != null && !product.Deleted)
                        {
                            xmlWriter.WriteStartElement("ProductManufacturer");
                            xmlWriter.WriteElementString("ProductManufacturerId", null, productManufacturer.Id.ToString());
                            xmlWriter.WriteElementString("ProductId", null, productManufacturer.ProductId.ToString());
                            xmlWriter.WriteElementString("ProductName", null, product.Name);
                            xmlWriter.WriteElementString("IsFeaturedProduct", null, productManufacturer.IsFeaturedProduct.ToString());
                            xmlWriter.WriteElementString("DisplayOrder", null, productManufacturer.DisplayOrder.ToString());
                            xmlWriter.WriteEndElement();
                        }
                    }
                }
                xmlWriter.WriteEndElement();


                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
            return stringWriter.ToString();
        }

        /// <summary>
        /// Export category list to xml
        /// </summary>
        /// <returns>Result in XML format</returns>
        public virtual string ExportCategoriesToXml()
        {
            var sb = new StringBuilder();
            var stringWriter = new StringWriter(sb);
            var xmlWriter = new XmlTextWriter(stringWriter);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Categories");
            xmlWriter.WriteAttributeString("Version", NopVersion.CurrentVersion);
            WriteCategories(xmlWriter, 0);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
            return stringWriter.ToString();
        }

        /// <summary>
        /// Export product list to xml
        /// </summary>
        /// <param name="products">Products</param>
        /// <returns>Result in XML format</returns>
        public virtual string ExportProductsToXml(IList<Product> products)
        {
            var sb = new StringBuilder();
            var stringWriter = new StringWriter(sb);
            var xmlWriter = new XmlTextWriter(stringWriter);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Products");
            xmlWriter.WriteAttributeString("Version", NopVersion.CurrentVersion);

            foreach (var product in products)
            {
                xmlWriter.WriteStartElement("Product");

                xmlWriter.WriteElementString("ProductId", null, product.Id.ToString());
                xmlWriter.WriteElementString("ProductTypeId", null, product.ProductTypeId.ToString());
                xmlWriter.WriteElementString("ParentGroupedProductId", null, product.ParentGroupedProductId.ToString());
                xmlWriter.WriteElementString("VisibleIndividually", null, product.VisibleIndividually.ToString());
                xmlWriter.WriteElementString("Name", null, product.Name);
                xmlWriter.WriteElementString("ShortDescription", null, product.ShortDescription);
                xmlWriter.WriteElementString("FullDescription", null, product.FullDescription);
                xmlWriter.WriteElementString("AdminComment", null, product.AdminComment);
                xmlWriter.WriteElementString("VendorId", null, product.VendorId.ToString());
                xmlWriter.WriteElementString("ProductTemplateId", null, product.ProductTemplateId.ToString());
                xmlWriter.WriteElementString("ShowOnHomePage", null, product.ShowOnHomePage.ToString());
                xmlWriter.WriteElementString("MetaKeywords", null, product.MetaKeywords);
                xmlWriter.WriteElementString("MetaDescription", null, product.MetaDescription);
                xmlWriter.WriteElementString("MetaTitle", null, product.MetaTitle);
                xmlWriter.WriteElementString("SEName", null, product.GetSeName(0));
                xmlWriter.WriteElementString("AllowCustomerReviews", null, product.AllowCustomerReviews.ToString());
                xmlWriter.WriteElementString("SKU", null, product.Sku);
                xmlWriter.WriteElementString("ManufacturerPartNumber", null, product.ManufacturerPartNumber);
                xmlWriter.WriteElementString("Gtin", null, product.Gtin);
                xmlWriter.WriteElementString("IsGiftCard", null, product.IsGiftCard.ToString());
                xmlWriter.WriteElementString("GiftCardType", null, product.GiftCardType.ToString());
                xmlWriter.WriteElementString("RequireOtherProducts", null, product.RequireOtherProducts.ToString());
                xmlWriter.WriteElementString("RequiredProductIds", null, product.RequiredProductIds);
                xmlWriter.WriteElementString("AutomaticallyAddRequiredProducts", null, product.AutomaticallyAddRequiredProducts.ToString());
                xmlWriter.WriteElementString("IsDownload", null, product.IsDownload.ToString());
                xmlWriter.WriteElementString("DownloadId", null, product.DownloadId.ToString());
                xmlWriter.WriteElementString("UnlimitedDownloads", null, product.UnlimitedDownloads.ToString());
                xmlWriter.WriteElementString("MaxNumberOfDownloads", null, product.MaxNumberOfDownloads.ToString());
                if (product.DownloadExpirationDays.HasValue)
                    xmlWriter.WriteElementString("DownloadExpirationDays", null, product.DownloadExpirationDays.ToString());
                else
                    xmlWriter.WriteElementString("DownloadExpirationDays", null, string.Empty);
                xmlWriter.WriteElementString("DownloadActivationType", null, product.DownloadActivationType.ToString());
                xmlWriter.WriteElementString("HasSampleDownload", null, product.HasSampleDownload.ToString());
                xmlWriter.WriteElementString("SampleDownloadId", null, product.SampleDownloadId.ToString());
                xmlWriter.WriteElementString("HasUserAgreement", null, product.HasUserAgreement.ToString());
                xmlWriter.WriteElementString("UserAgreementText", null, product.UserAgreementText);
                xmlWriter.WriteElementString("IsRecurring", null, product.IsRecurring.ToString());
                xmlWriter.WriteElementString("RecurringCycleLength", null, product.RecurringCycleLength.ToString());
                xmlWriter.WriteElementString("RecurringCyclePeriodId", null, product.RecurringCyclePeriodId.ToString());
                xmlWriter.WriteElementString("RecurringTotalCycles", null, product.RecurringTotalCycles.ToString());
                xmlWriter.WriteElementString("IsRental", null, product.IsRental.ToString());
                xmlWriter.WriteElementString("RentalPriceLength", null, product.RentalPriceLength.ToString());
                xmlWriter.WriteElementString("RentalPricePeriodId", null, product.RentalPricePeriodId.ToString());
                xmlWriter.WriteElementString("IsShipEnabled", null, product.IsShipEnabled.ToString());
                xmlWriter.WriteElementString("IsFreeShipping", null, product.IsFreeShipping.ToString());
                xmlWriter.WriteElementString("ShipSeparately", null, product.ShipSeparately.ToString());
                xmlWriter.WriteElementString("AdditionalShippingCharge", null, product.AdditionalShippingCharge.ToString());
                xmlWriter.WriteElementString("DeliveryDateId", null, product.DeliveryDateId.ToString());
                xmlWriter.WriteElementString("IsTaxExempt", null, product.IsTaxExempt.ToString());
                xmlWriter.WriteElementString("TaxCategoryId", null, product.TaxCategoryId.ToString());
                xmlWriter.WriteElementString("IsTelecommunicationsOrBroadcastingOrElectronicServices", null, product.IsTelecommunicationsOrBroadcastingOrElectronicServices.ToString());
                xmlWriter.WriteElementString("ManageInventoryMethodId", null, product.ManageInventoryMethodId.ToString());
                xmlWriter.WriteElementString("UseMultipleWarehouses", null, product.UseMultipleWarehouses.ToString());
                xmlWriter.WriteElementString("WarehouseId", null, product.WarehouseId.ToString());
                xmlWriter.WriteElementString("StockQuantity", null, product.StockQuantity.ToString());
                xmlWriter.WriteElementString("DisplayStockAvailability", null, product.DisplayStockAvailability.ToString());
                xmlWriter.WriteElementString("DisplayStockQuantity", null, product.DisplayStockQuantity.ToString());
                xmlWriter.WriteElementString("MinStockQuantity", null, product.MinStockQuantity.ToString());
                xmlWriter.WriteElementString("LowStockActivityId", null, product.LowStockActivityId.ToString());
                xmlWriter.WriteElementString("NotifyAdminForQuantityBelow", null, product.NotifyAdminForQuantityBelow.ToString());
                xmlWriter.WriteElementString("BackorderModeId", null, product.BackorderModeId.ToString());
                xmlWriter.WriteElementString("AllowBackInStockSubscriptions", null, product.AllowBackInStockSubscriptions.ToString());
                xmlWriter.WriteElementString("OrderMinimumQuantity", null, product.OrderMinimumQuantity.ToString());
                xmlWriter.WriteElementString("OrderMaximumQuantity", null, product.OrderMaximumQuantity.ToString());
                xmlWriter.WriteElementString("AllowedQuantities", null, product.AllowedQuantities);
                xmlWriter.WriteElementString("AllowAddingOnlyExistingAttributeCombinations", null, product.AllowAddingOnlyExistingAttributeCombinations.ToString());
                xmlWriter.WriteElementString("DisableBuyButton", null, product.DisableBuyButton.ToString());
                xmlWriter.WriteElementString("DisableWishlistButton", null, product.DisableWishlistButton.ToString());
                xmlWriter.WriteElementString("AvailableForPreOrder", null, product.AvailableForPreOrder.ToString());
                xmlWriter.WriteElementString("PreOrderAvailabilityStartDateTimeUtc", null, product.PreOrderAvailabilityStartDateTimeUtc.HasValue ? product.PreOrderAvailabilityStartDateTimeUtc.ToString() : "");
                xmlWriter.WriteElementString("CallForPrice", null, product.CallForPrice.ToString());
                xmlWriter.WriteElementString("Price", null, product.Price.ToString());
                xmlWriter.WriteElementString("OldPrice", null, product.OldPrice.ToString());
                xmlWriter.WriteElementString("ProductCost", null, product.ProductCost.ToString());
                xmlWriter.WriteElementString("SpecialPrice", null, product.SpecialPrice.HasValue ? product.SpecialPrice.ToString() : "");
                xmlWriter.WriteElementString("SpecialPriceStartDateTimeUtc", null, product.SpecialPriceStartDateTimeUtc.HasValue ? product.SpecialPriceStartDateTimeUtc.ToString() : "");
                xmlWriter.WriteElementString("SpecialPriceEndDateTimeUtc", null, product.SpecialPriceEndDateTimeUtc.HasValue ? product.SpecialPriceEndDateTimeUtc.ToString() : "");
                xmlWriter.WriteElementString("CustomerEntersPrice", null, product.CustomerEntersPrice.ToString());
                xmlWriter.WriteElementString("MinimumCustomerEnteredPrice", null, product.MinimumCustomerEnteredPrice.ToString());
                xmlWriter.WriteElementString("MaximumCustomerEnteredPrice", null, product.MaximumCustomerEnteredPrice.ToString());
                xmlWriter.WriteElementString("Weight", null, product.Weight.ToString());
                xmlWriter.WriteElementString("Length", null, product.Length.ToString());
                xmlWriter.WriteElementString("Width", null, product.Width.ToString());
                xmlWriter.WriteElementString("Height", null, product.Height.ToString());
                xmlWriter.WriteElementString("Published", null, product.Published.ToString());
                xmlWriter.WriteElementString("CreatedOnUtc", null, product.CreatedOnUtc.ToString());
                xmlWriter.WriteElementString("UpdatedOnUtc", null, product.UpdatedOnUtc.ToString());



                xmlWriter.WriteStartElement("ProductDiscounts");
                var discounts = product.AppliedDiscounts;
                foreach (var discount in discounts)
                {
                    xmlWriter.WriteStartElement("Discount");
                    xmlWriter.WriteElementString("DiscountId", null, discount.Id.ToString());
                    xmlWriter.WriteElementString("Name", null, discount.Name);
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();


                xmlWriter.WriteStartElement("TierPrices");
                var tierPrices = product.TierPrices;
                foreach (var tierPrice in tierPrices)
                {
                    xmlWriter.WriteStartElement("TierPrice");
                    xmlWriter.WriteElementString("TierPriceId", null, tierPrice.Id.ToString());
                    xmlWriter.WriteElementString("StoreId", null, tierPrice.StoreId.ToString());
                    xmlWriter.WriteElementString("CustomerRoleId", null, tierPrice.CustomerRoleId.HasValue ? tierPrice.CustomerRoleId.ToString() : "0");
                    xmlWriter.WriteElementString("Quantity", null, tierPrice.Quantity.ToString());
                    xmlWriter.WriteElementString("Price", null, tierPrice.Price.ToString());
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("ProductAttributes");
                var productAttributMappings = _productAttributeService.GetProductAttributeMappingsByProductId(product.Id);
                foreach (var productAttributeMapping in productAttributMappings)
                {
                    xmlWriter.WriteStartElement("ProductAttributeMapping");
                    xmlWriter.WriteElementString("ProductAttributeMappingId", null, productAttributeMapping.Id.ToString());
                    xmlWriter.WriteElementString("ProductAttributeId", null, productAttributeMapping.ProductAttributeId.ToString());
                    xmlWriter.WriteElementString("ProductAttributeName", null, productAttributeMapping.ProductAttribute.Name);
                    xmlWriter.WriteElementString("TextPrompt", null, productAttributeMapping.TextPrompt);
                    xmlWriter.WriteElementString("IsRequired", null, productAttributeMapping.IsRequired.ToString());
                    xmlWriter.WriteElementString("AttributeControlTypeId", null, productAttributeMapping.AttributeControlTypeId.ToString());
                    xmlWriter.WriteElementString("DisplayOrder", null, productAttributeMapping.DisplayOrder.ToString());
                    //validation rules
                    if (productAttributeMapping.ValidationRulesAllowed())
                    {
                        if (productAttributeMapping.ValidationMinLength.HasValue)
                        {
                            xmlWriter.WriteElementString("ValidationMinLength", null, productAttributeMapping.ValidationMinLength.Value.ToString());
                        }
                        if (productAttributeMapping.ValidationMaxLength.HasValue)
                        {
                            xmlWriter.WriteElementString("ValidationMaxLength", null, productAttributeMapping.ValidationMaxLength.Value.ToString());
                        }
                        if (String.IsNullOrEmpty(productAttributeMapping.ValidationFileAllowedExtensions))
                        {
                            xmlWriter.WriteElementString("ValidationFileAllowedExtensions", null, productAttributeMapping.ValidationFileAllowedExtensions);
                        }
                        if (productAttributeMapping.ValidationFileMaximumSize.HasValue)
                        {
                            xmlWriter.WriteElementString("ValidationFileMaximumSize", null, productAttributeMapping.ValidationFileMaximumSize.Value.ToString());
                        }
                        xmlWriter.WriteElementString("DefaultValue", null, productAttributeMapping.DefaultValue);
                    }


                    xmlWriter.WriteStartElement("ProductAttributeValues");
                    var productAttributeValues = productAttributeMapping.ProductAttributeValues;
                    foreach (var productAttributeValue in productAttributeValues)
                    {
                        xmlWriter.WriteStartElement("ProductAttributeValue");
                        xmlWriter.WriteElementString("ProductAttributeValueId", null, productAttributeValue.Id.ToString());
                        xmlWriter.WriteElementString("Name", null, productAttributeValue.Name);
                        xmlWriter.WriteElementString("AttributeValueTypeId", null, productAttributeValue.AttributeValueTypeId.ToString());
                        xmlWriter.WriteElementString("AssociatedProductId", null, productAttributeValue.AssociatedProductId.ToString());
                        xmlWriter.WriteElementString("ColorSquaresRgb", null, productAttributeValue.ColorSquaresRgb);
                        xmlWriter.WriteElementString("PriceAdjustment", null, productAttributeValue.PriceAdjustment.ToString());
                        xmlWriter.WriteElementString("WeightAdjustment", null, productAttributeValue.WeightAdjustment.ToString());
                        xmlWriter.WriteElementString("Cost", null, productAttributeValue.Cost.ToString());
                        xmlWriter.WriteElementString("Quantity", null, productAttributeValue.Quantity.ToString());
                        xmlWriter.WriteElementString("IsPreSelected", null, productAttributeValue.IsPreSelected.ToString());
                        xmlWriter.WriteElementString("DisplayOrder", null, productAttributeValue.DisplayOrder.ToString());
                        xmlWriter.WriteElementString("PictureId", null, productAttributeValue.PictureId.ToString());
                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteEndElement();


                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();









                xmlWriter.WriteStartElement("ProductPictures");
                var productPictures = product.ProductPictures;
                foreach (var productPicture in productPictures)
                {
                    xmlWriter.WriteStartElement("ProductPicture");
                    xmlWriter.WriteElementString("ProductPictureId", null, productPicture.Id.ToString());
                    xmlWriter.WriteElementString("PictureId", null, productPicture.PictureId.ToString());
                    xmlWriter.WriteElementString("DisplayOrder", null, productPicture.DisplayOrder.ToString());
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("ProductCategories");
                var productCategories = _categoryService.GetProductCategoriesByProductId(product.Id);
                if (productCategories != null)
                {
                    foreach (var productCategory in productCategories)
                    {
                        xmlWriter.WriteStartElement("ProductCategory");
                        xmlWriter.WriteElementString("ProductCategoryId", null, productCategory.Id.ToString());
                        xmlWriter.WriteElementString("CategoryId", null, productCategory.CategoryId.ToString());
                        xmlWriter.WriteElementString("IsFeaturedProduct", null, productCategory.IsFeaturedProduct.ToString());
                        xmlWriter.WriteElementString("DisplayOrder", null, productCategory.DisplayOrder.ToString());
                        xmlWriter.WriteEndElement();
                    }
                }
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("ProductManufacturers");
                var productManufacturers = _manufacturerService.GetProductManufacturersByProductId(product.Id);
                if (productManufacturers != null)
                {
                    foreach (var productManufacturer in productManufacturers)
                    {
                        xmlWriter.WriteStartElement("ProductManufacturer");
                        xmlWriter.WriteElementString("ProductManufacturerId", null, productManufacturer.Id.ToString());
                        xmlWriter.WriteElementString("ManufacturerId", null, productManufacturer.ManufacturerId.ToString());
                        xmlWriter.WriteElementString("IsFeaturedProduct", null, productManufacturer.IsFeaturedProduct.ToString());
                        xmlWriter.WriteElementString("DisplayOrder", null, productManufacturer.DisplayOrder.ToString());
                        xmlWriter.WriteEndElement();
                    }
                }
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("ProductSpecificationAttributes");
                var productSpecificationAttributes = product.ProductSpecificationAttributes;
                foreach (var productSpecificationAttribute in productSpecificationAttributes)
                {
                    xmlWriter.WriteStartElement("ProductSpecificationAttribute");
                    xmlWriter.WriteElementString("ProductSpecificationAttributeId", null, productSpecificationAttribute.Id.ToString());
                    xmlWriter.WriteElementString("SpecificationAttributeOptionId", null, productSpecificationAttribute.SpecificationAttributeOptionId.ToString());
                    xmlWriter.WriteElementString("CustomValue", null, productSpecificationAttribute.CustomValue);
                    xmlWriter.WriteElementString("AllowFiltering", null, productSpecificationAttribute.AllowFiltering.ToString());
                    xmlWriter.WriteElementString("ShowOnProductPage", null, productSpecificationAttribute.ShowOnProductPage.ToString());
                    xmlWriter.WriteElementString("DisplayOrder", null, productSpecificationAttribute.DisplayOrder.ToString());
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
            return stringWriter.ToString();
        }

        /// <summary>
        /// Export products to XLSX
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="products">Products</param>
        public virtual void ExportProductsToXlsx(Stream stream, IList<Product> products)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            // ok, we can run the real code of the sample now
            using (var xlPackage = new ExcelPackage(stream))
            {
                // uncomment this line if you want the XML written out to the outputDir
                //xlPackage.DebugMode = true; 

                // get handle to the existing worksheet
                var worksheet = xlPackage.Workbook.Worksheets.Add("Products");
                //Create Headers and format them 
                var properties = new[]
                {
                    "ProductTypeId",
                    "ParentGroupedProductId",
                    "VisibleIndividually",
                    "Name",
                    "ShortDescription",
                    "FullDescription",
                    "VendorId",
                    "ProductTemplateId",
                    "ShowOnHomePage",
                    "MetaKeywords",
                    "MetaDescription",
                    "MetaTitle",
                    "SeName",
                    "AllowCustomerReviews",
                    "Published",
                    "SKU",
                    "ManufacturerPartNumber",
                    "Gtin",
                    "IsGiftCard",
                    "GiftCardTypeId",
                    "RequireOtherProducts",
                    "RequiredProductIds",
                    "AutomaticallyAddRequiredProducts",
                    "IsDownload",
                    "DownloadId",
                    "UnlimitedDownloads",
                    "MaxNumberOfDownloads",
                    "DownloadActivationTypeId",
                    "HasSampleDownload",
                    "SampleDownloadId",
                    "HasUserAgreement",
                    "UserAgreementText",
                    "IsRecurring",
                    "RecurringCycleLength",
                    "RecurringCyclePeriodId",
                    "RecurringTotalCycles",
                    "IsRental",
                    "RentalPriceLength",
                    "RentalPricePeriodId",
                    "IsShipEnabled",
                    "IsFreeShipping",
                    "ShipSeparately",
                    "AdditionalShippingCharge",
                    "DeliveryDateId",
                    "IsTaxExempt",
                    "TaxCategoryId",
                    "IsTelecommunicationsOrBroadcastingOrElectronicServices",
                    "ManageInventoryMethodId",
                    "UseMultipleWarehouses",
                    "WarehouseId",
                    "StockQuantity",
                    "DisplayStockAvailability",
                    "DisplayStockQuantity",
                    "MinStockQuantity",
                    "LowStockActivityId",
                    "NotifyAdminForQuantityBelow",
                    "BackorderModeId",
                    "AllowBackInStockSubscriptions",
                    "OrderMinimumQuantity",
                    "OrderMaximumQuantity",
                    "AllowedQuantities",
                    "AllowAddingOnlyExistingAttributeCombinations",
                    "DisableBuyButton",
                    "DisableWishlistButton",
                    "AvailableForPreOrder",
                    "PreOrderAvailabilityStartDateTimeUtc",
                    "CallForPrice",
                    "Price",
                    "OldPrice",
                    "ProductCost",
                    "SpecialPrice",
                    "SpecialPriceStartDateTimeUtc",
                    "SpecialPriceEndDateTimeUtc",
                    "CustomerEntersPrice",
                    "MinimumCustomerEnteredPrice",
                    "MaximumCustomerEnteredPrice",
                    "Weight",
                    "Length",
                    "Width",
                    "Height",
                    "CreatedOnUtc",
                    "CategoryIds",
                    "ManufacturerIds",
                    "Picture1",
                    "Picture2",
                    "Picture3"
                };
                for (int i = 0; i < properties.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = properties[i];
                    worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                }


                int row = 2;
                foreach (var p in products)
                {
                    int col = 1;

                    worksheet.Cells[row, col].Value = p.ProductTypeId;
                    col++;

                    worksheet.Cells[row, col].Value = p.ParentGroupedProductId;
                    col++;

                    worksheet.Cells[row, col].Value = p.VisibleIndividually;
                    col++;

                    worksheet.Cells[row, col].Value = p.Name;
                    col++;

                    worksheet.Cells[row, col].Value = p.ShortDescription;
                    col++;

                    worksheet.Cells[row, col].Value = p.FullDescription;
                    col++;

                    worksheet.Cells[row, col].Value = p.VendorId;
                    col++;

                    worksheet.Cells[row, col].Value = p.ProductTemplateId;
                    col++;

                    worksheet.Cells[row, col].Value = p.ShowOnHomePage;
                    col++;

                    worksheet.Cells[row, col].Value = p.MetaKeywords;
                    col++;

                    worksheet.Cells[row, col].Value = p.MetaDescription;
                    col++;

                    worksheet.Cells[row, col].Value = p.MetaTitle;
                    col++;

                    worksheet.Cells[row, col].Value = p.GetSeName(0);
                    col++;

                    worksheet.Cells[row, col].Value = p.AllowCustomerReviews;
                    col++;

                    worksheet.Cells[row, col].Value = p.Published;
                    col++;

                    worksheet.Cells[row, col].Value = p.Sku;
                    col++;

                    worksheet.Cells[row, col].Value = p.ManufacturerPartNumber;
                    col++;

                    worksheet.Cells[row, col].Value = p.Gtin;
                    col++;

                    worksheet.Cells[row, col].Value = p.IsGiftCard;
                    col++;

                    worksheet.Cells[row, col].Value = p.GiftCardTypeId;
                    col++;

                    worksheet.Cells[row, col].Value = p.RequireOtherProducts;
                    col++;

                    worksheet.Cells[row, col].Value = p.RequiredProductIds;
                    col++;

                    worksheet.Cells[row, col].Value = p.AutomaticallyAddRequiredProducts;
                    col++;

                    worksheet.Cells[row, col].Value = p.IsDownload;
                    col++;

                    worksheet.Cells[row, col].Value = p.DownloadId;
                    col++;

                    worksheet.Cells[row, col].Value = p.UnlimitedDownloads;
                    col++;

                    worksheet.Cells[row, col].Value = p.MaxNumberOfDownloads;
                    col++;

                    worksheet.Cells[row, col].Value = p.DownloadActivationTypeId;
                    col++;

                    worksheet.Cells[row, col].Value = p.HasSampleDownload;
                    col++;

                    worksheet.Cells[row, col].Value = p.SampleDownloadId;
                    col++;

                    worksheet.Cells[row, col].Value = p.HasUserAgreement;
                    col++;

                    worksheet.Cells[row, col].Value = p.UserAgreementText;
                    col++;

                    worksheet.Cells[row, col].Value = p.IsRecurring;
                    col++;

                    worksheet.Cells[row, col].Value = p.RecurringCycleLength;
                    col++;

                    worksheet.Cells[row, col].Value = p.RecurringCyclePeriodId;
                    col++;

                    worksheet.Cells[row, col].Value = p.RecurringTotalCycles;
                    col++;

                    worksheet.Cells[row, col].Value = p.IsRental;
                    col++;

                    worksheet.Cells[row, col].Value = p.RentalPriceLength;
                    col++;

                    worksheet.Cells[row, col].Value = p.RentalPricePeriodId;
                    col++;

                    worksheet.Cells[row, col].Value = p.IsShipEnabled;
                    col++;

                    worksheet.Cells[row, col].Value = p.IsFreeShipping;
                    col++;

                    worksheet.Cells[row, col].Value = p.ShipSeparately;
                    col++;

                    worksheet.Cells[row, col].Value = p.AdditionalShippingCharge;
                    col++;

                    worksheet.Cells[row, col].Value = p.DeliveryDateId;
                    col++;

                    worksheet.Cells[row, col].Value = p.IsTaxExempt;
                    col++;

                    worksheet.Cells[row, col].Value = p.TaxCategoryId;
                    col++;

                    worksheet.Cells[row, col].Value = p.IsTelecommunicationsOrBroadcastingOrElectronicServices;
                    col++;

                    worksheet.Cells[row, col].Value = p.ManageInventoryMethodId;
                    col++;

                    worksheet.Cells[row, col].Value = p.UseMultipleWarehouses;
                    col++;

                    worksheet.Cells[row, col].Value = p.WarehouseId;
                    col++;

                    worksheet.Cells[row, col].Value = p.StockQuantity;
                    col++;

                    worksheet.Cells[row, col].Value = p.DisplayStockAvailability;
                    col++;

                    worksheet.Cells[row, col].Value = p.DisplayStockQuantity;
                    col++;

                    worksheet.Cells[row, col].Value = p.MinStockQuantity;
                    col++;

                    worksheet.Cells[row, col].Value = p.LowStockActivityId;
                    col++;

                    worksheet.Cells[row, col].Value = p.NotifyAdminForQuantityBelow;
                    col++;

                    worksheet.Cells[row, col].Value = p.BackorderModeId;
                    col++;

                    worksheet.Cells[row, col].Value = p.AllowBackInStockSubscriptions;
                    col++;

                    worksheet.Cells[row, col].Value = p.OrderMinimumQuantity;
                    col++;

                    worksheet.Cells[row, col].Value = p.OrderMaximumQuantity;
                    col++;

                    worksheet.Cells[row, col].Value = p.AllowedQuantities;
                    col++;

                    worksheet.Cells[row, col].Value = p.AllowAddingOnlyExistingAttributeCombinations;
                    col++;

                    worksheet.Cells[row, col].Value = p.DisableBuyButton;
                    col++;

                    worksheet.Cells[row, col].Value = p.DisableWishlistButton;
                    col++;

                    worksheet.Cells[row, col].Value = p.AvailableForPreOrder;
                    col++;

                    worksheet.Cells[row, col].Value = p.PreOrderAvailabilityStartDateTimeUtc;
                    col++;

                    worksheet.Cells[row, col].Value = p.CallForPrice;
                    col++;

                    worksheet.Cells[row, col].Value = p.Price;
                    col++;

                    worksheet.Cells[row, col].Value = p.OldPrice;
                    col++;

                    worksheet.Cells[row, col].Value = p.ProductCost;
                    col++;

                    worksheet.Cells[row, col].Value = p.SpecialPrice;
                    col++;

                    worksheet.Cells[row, col].Value = p.SpecialPriceStartDateTimeUtc;
                    col++;

                    worksheet.Cells[row, col].Value = p.SpecialPriceEndDateTimeUtc;
                    col++;

                    worksheet.Cells[row, col].Value = p.CustomerEntersPrice;
                    col++;

                    worksheet.Cells[row, col].Value = p.MinimumCustomerEnteredPrice;
                    col++;

                    worksheet.Cells[row, col].Value = p.MaximumCustomerEnteredPrice;
                    col++;

                    worksheet.Cells[row, col].Value = p.Weight;
                    col++;

                    worksheet.Cells[row, col].Value = p.Length;
                    col++;

                    worksheet.Cells[row, col].Value = p.Width;
                    col++;

                    worksheet.Cells[row, col].Value = p.Height;
                    col++;

                    worksheet.Cells[row, col].Value = p.CreatedOnUtc.ToOADate();
                    col++;

                    //category identifiers
                    string categoryIds = null;
                    foreach (var pc in _categoryService.GetProductCategoriesByProductId(p.Id))
                    {
                        categoryIds += pc.CategoryId;
                        categoryIds += ";";
                    }
                    worksheet.Cells[row, col].Value = categoryIds;
                    col++;

                    //manufacturer identifiers
                    string manufacturerIds = null;
                    foreach (var pm in _manufacturerService.GetProductManufacturersByProductId(p.Id))
                    {
                        manufacturerIds += pm.ManufacturerId;
                        manufacturerIds += ";";
                    }
                    worksheet.Cells[row, col].Value = manufacturerIds;
                    col++;

                    //pictures (up to 3 pictures)
                    string picture1 = null;
                    string picture2 = null;
                    string picture3 = null;
                    var pictures = _pictureService.GetPicturesByProductId(p.Id, 3);
                    for (int i = 0; i < pictures.Count; i++)
                    {
                        string pictureLocalPath = _pictureService.GetThumbLocalPath(pictures[i]);
                        switch (i)
                        {
                            case 0:
                                picture1 = pictureLocalPath;
                                break;
                            case 1:
                                picture2 = pictureLocalPath;
                                break;
                            case 2:
                                picture3 = pictureLocalPath;
                                break;
                        }
                    }
                    worksheet.Cells[row, col].Value = picture1;
                    col++;
                    worksheet.Cells[row, col].Value = picture2;
                    col++;
                    worksheet.Cells[row, col].Value = picture3;
                    col++;

                    row++;
                }








                // we had better add some document properties to the spreadsheet 

                // set some core property values
                //var storeName = _storeInformationSettings.StoreName;
                //var storeUrl = _storeInformationSettings.StoreUrl;
                //xlPackage.Workbook.Properties.Title = string.Format("{0} products", storeName);
                //xlPackage.Workbook.Properties.Author = storeName;
                //xlPackage.Workbook.Properties.Subject = string.Format("{0} products", storeName);
                //xlPackage.Workbook.Properties.Keywords = string.Format("{0} products", storeName);
                //xlPackage.Workbook.Properties.Category = "Products";
                //xlPackage.Workbook.Properties.Comments = string.Format("{0} products", storeName);

                // set some extended property values
                //xlPackage.Workbook.Properties.Company = storeName;
                //xlPackage.Workbook.Properties.HyperlinkBase = new Uri(storeUrl);

                // save the new spreadsheet
                xlPackage.Save();
            }
        }

        /// <summary>
        /// Export order list to xml
        /// </summary>
        /// <param name="orders">Orders</param>
        /// <returns>Result in XML format</returns>
        public virtual string ExportOrdersToXml(IList<Order> orders)
        {
            var sb = new StringBuilder();
            var stringWriter = new StringWriter(sb);
            var xmlWriter = new XmlTextWriter(stringWriter);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Orders");
            xmlWriter.WriteAttributeString("Version", NopVersion.CurrentVersion);


            foreach (var order in orders)
            {
                xmlWriter.WriteStartElement("Order");

                xmlWriter.WriteElementString("OrderId", null, order.Id.ToString());
                xmlWriter.WriteElementString("OrderGuid", null, order.OrderGuid.ToString());
                xmlWriter.WriteElementString("StoreId", null, order.StoreId.ToString());
                xmlWriter.WriteElementString("CustomerId", null, order.CustomerId.ToString());
                xmlWriter.WriteElementString("OrderStatusId", null, order.OrderStatusId.ToString());
                xmlWriter.WriteElementString("PaymentStatusId", null, order.PaymentStatusId.ToString());
                xmlWriter.WriteElementString("ShippingStatusId", null, order.ShippingStatusId.ToString());
                xmlWriter.WriteElementString("CustomerLanguageId", null, order.CustomerLanguageId.ToString());
                xmlWriter.WriteElementString("CustomerTaxDisplayTypeId", null, order.CustomerTaxDisplayTypeId.ToString());
                xmlWriter.WriteElementString("CustomerIp", null, order.CustomerIp);
                xmlWriter.WriteElementString("OrderSubtotalInclTax", null, order.OrderSubtotalInclTax.ToString());
                xmlWriter.WriteElementString("OrderSubtotalExclTax", null, order.OrderSubtotalExclTax.ToString());
                xmlWriter.WriteElementString("OrderSubTotalDiscountInclTax", null, order.OrderSubTotalDiscountInclTax.ToString());
                xmlWriter.WriteElementString("OrderSubTotalDiscountExclTax", null, order.OrderSubTotalDiscountExclTax.ToString());
                xmlWriter.WriteElementString("OrderShippingInclTax", null, order.OrderShippingInclTax.ToString());
                xmlWriter.WriteElementString("OrderShippingExclTax", null, order.OrderShippingExclTax.ToString());
                xmlWriter.WriteElementString("PaymentMethodAdditionalFeeInclTax", null, order.PaymentMethodAdditionalFeeInclTax.ToString());
                xmlWriter.WriteElementString("PaymentMethodAdditionalFeeExclTax", null, order.PaymentMethodAdditionalFeeExclTax.ToString());
                xmlWriter.WriteElementString("TaxRates", null, order.TaxRates);
                xmlWriter.WriteElementString("OrderTax", null, order.OrderTax.ToString());
                xmlWriter.WriteElementString("OrderTotal", null, order.OrderTotal.ToString());
                xmlWriter.WriteElementString("RefundedAmount", null, order.RefundedAmount.ToString());
                xmlWriter.WriteElementString("OrderDiscount", null, order.OrderDiscount.ToString());
                xmlWriter.WriteElementString("CurrencyRate", null, order.CurrencyRate.ToString());
                xmlWriter.WriteElementString("CustomerCurrencyCode", null, order.CustomerCurrencyCode);
                xmlWriter.WriteElementString("AffiliateId", null, order.AffiliateId.ToString());
                xmlWriter.WriteElementString("AllowStoringCreditCardNumber", null, order.AllowStoringCreditCardNumber.ToString());
                xmlWriter.WriteElementString("CardType", null, order.CardType);
                xmlWriter.WriteElementString("CardName", null, order.CardName);
                xmlWriter.WriteElementString("CardNumber", null, order.CardNumber);
                xmlWriter.WriteElementString("MaskedCreditCardNumber", null, order.MaskedCreditCardNumber);
                xmlWriter.WriteElementString("CardCvv2", null, order.CardCvv2);
                xmlWriter.WriteElementString("CardExpirationMonth", null, order.CardExpirationMonth);
                xmlWriter.WriteElementString("CardExpirationYear", null, order.CardExpirationYear);
                xmlWriter.WriteElementString("PaymentMethodSystemName", null, order.PaymentMethodSystemName);
                xmlWriter.WriteElementString("AuthorizationTransactionId", null, order.AuthorizationTransactionId);
                xmlWriter.WriteElementString("AuthorizationTransactionCode", null, order.AuthorizationTransactionCode);
                xmlWriter.WriteElementString("AuthorizationTransactionResult", null, order.AuthorizationTransactionResult);
                xmlWriter.WriteElementString("CaptureTransactionId", null, order.CaptureTransactionId);
                xmlWriter.WriteElementString("CaptureTransactionResult", null, order.CaptureTransactionResult);
                xmlWriter.WriteElementString("SubscriptionTransactionId", null, order.SubscriptionTransactionId);
                xmlWriter.WriteElementString("PaidDateUtc", null, (order.PaidDateUtc == null) ? string.Empty : order.PaidDateUtc.Value.ToString());
                xmlWriter.WriteElementString("ShippingMethod", null, order.ShippingMethod);
                xmlWriter.WriteElementString("ShippingRateComputationMethodSystemName", null, order.ShippingRateComputationMethodSystemName);
                xmlWriter.WriteElementString("CustomValuesXml", null, order.CustomValuesXml);
                xmlWriter.WriteElementString("VatNumber", null, order.VatNumber);
                xmlWriter.WriteElementString("Deleted", null, order.Deleted.ToString());
                xmlWriter.WriteElementString("CreatedOnUtc", null, order.CreatedOnUtc.ToString());

                //products
                var orderItems = order.OrderItems;
                if (orderItems.Count > 0)
                {
                    xmlWriter.WriteStartElement("OrderItems");
                    foreach (var orderItem in orderItems)
                    {
                        xmlWriter.WriteStartElement("OrderItem");
                        xmlWriter.WriteElementString("Id", null, orderItem.Id.ToString());
                        xmlWriter.WriteElementString("OrderItemGuid", null, orderItem.OrderItemGuid.ToString());
                        xmlWriter.WriteElementString("ProductId", null, orderItem.ProductId.ToString());

                        var product = orderItem.Product;
                        xmlWriter.WriteElementString("ProductName", null, product.Name);
                        xmlWriter.WriteElementString("UnitPriceInclTax", null, orderItem.UnitPriceInclTax.ToString());
                        xmlWriter.WriteElementString("UnitPriceExclTax", null, orderItem.UnitPriceExclTax.ToString());
                        xmlWriter.WriteElementString("PriceInclTax", null, orderItem.PriceInclTax.ToString());
                        xmlWriter.WriteElementString("PriceExclTax", null, orderItem.PriceExclTax.ToString());
                        xmlWriter.WriteElementString("DiscountAmountInclTax", null, orderItem.DiscountAmountInclTax.ToString());
                        xmlWriter.WriteElementString("DiscountAmountExclTax", null, orderItem.DiscountAmountExclTax.ToString());
                        xmlWriter.WriteElementString("OriginalProductCost", null, orderItem.OriginalProductCost.ToString());
                        xmlWriter.WriteElementString("AttributeDescription", null, orderItem.AttributeDescription);
                        xmlWriter.WriteElementString("AttributesXml", null, orderItem.AttributesXml);
                        xmlWriter.WriteElementString("Quantity", null, orderItem.Quantity.ToString());
                        xmlWriter.WriteElementString("DownloadCount", null, orderItem.DownloadCount.ToString());
                        xmlWriter.WriteElementString("IsDownloadActivated", null, orderItem.IsDownloadActivated.ToString());
                        xmlWriter.WriteElementString("LicenseDownloadId", null, orderItem.LicenseDownloadId.ToString());
                        var rentalStartDate = orderItem.RentalStartDateUtc.HasValue ? orderItem.Product.FormatRentalDate(orderItem.RentalStartDateUtc.Value) : "";
                        xmlWriter.WriteElementString("RentalStartDateUtc", null, rentalStartDate);
                        var rentalEndDate = orderItem.RentalEndDateUtc.HasValue ? orderItem.Product.FormatRentalDate(orderItem.RentalEndDateUtc.Value) : "";
                        xmlWriter.WriteElementString("RentalEndDateUtc", null, rentalEndDate);
                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteEndElement();
                }

                //shipments
                var shipments = order.Shipments.OrderBy(x => x.CreatedOnUtc).ToList();
                if (shipments.Count > 0)
                {
                    xmlWriter.WriteStartElement("Shipments");
                    foreach (var shipment in shipments)
                    {
                        xmlWriter.WriteStartElement("Shipment");
                        xmlWriter.WriteElementString("ShipmentId", null, shipment.Id.ToString());
                        xmlWriter.WriteElementString("TrackingNumber", null, shipment.TrackingNumber);
                        xmlWriter.WriteElementString("TotalWeight", null, shipment.TotalWeight.HasValue ? shipment.TotalWeight.Value.ToString() : "");

                        xmlWriter.WriteElementString("ShippedDateUtc", null, shipment.ShippedDateUtc.HasValue ?
                            shipment.ShippedDateUtc.ToString() : "");
                        xmlWriter.WriteElementString("DeliveryDateUtc", null, shipment.DeliveryDateUtc.HasValue ?
                            shipment.DeliveryDateUtc.Value.ToString() : "");
                        xmlWriter.WriteElementString("CreatedOnUtc", null, shipment.CreatedOnUtc.ToString());
                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
            return stringWriter.ToString();
        }

        /// <summary>
        /// Export orders to XLSX
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="orders">Orders</param>
        public virtual void ExportOrdersToXlsx(Stream stream, IList<Order> orders)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            // ok, we can run the real code of the sample now
            using (var xlPackage = new ExcelPackage(stream))
            {
                // uncomment this line if you want the XML written out to the outputDir
                //xlPackage.DebugMode = true; 

                // get handle to the existing worksheet
                var worksheet = xlPackage.Workbook.Worksheets.Add("Orders");
                //Create Headers and format them
                var properties = new[]
                    {
                        //order properties
                        "OrderId",
                        "StoreId",
                        "OrderGuid",
                        "CustomerId",
                        "OrderStatusId",
                        "PaymentStatusId",
                        "ShippingStatusId",
                        "OrderSubtotalInclTax",
                        "OrderSubtotalExclTax",
                        "OrderSubTotalDiscountInclTax",
                        "OrderSubTotalDiscountExclTax",
                        "OrderShippingInclTax",
                        "OrderShippingExclTax",
                        "PaymentMethodAdditionalFeeInclTax",
                        "PaymentMethodAdditionalFeeExclTax",
                        "TaxRates",
                        "OrderTax",
                        "OrderTotal",
                        "RefundedAmount",
                        "OrderDiscount",
                        "CurrencyRate",
                        "CustomerCurrencyCode",
                        "AffiliateId",
                        "PaymentMethodSystemName",
                        "ShippingPickUpInStore",
                        "ShippingMethod",
                        "ShippingRateComputationMethodSystemName",
                        "CustomValuesXml",
                        "VatNumber",
                        "CreatedOnUtc",
                        //billing address
                        "BillingFirstName",
                        "BillingLastName",
                        "BillingEmail",
                        "BillingCompany",
                        "BillingCountry",
                        "BillingStateProvince",
                        "BillingCity",
                        "BillingAddress1",
                        "BillingAddress2",
                        "BillingZipPostalCode",
                        "BillingPhoneNumber",
                        "BillingFaxNumber",
                        //shipping address
                        "ShippingFirstName",
                        "ShippingLastName",
                        "ShippingEmail",
                        "ShippingCompany",
                        "ShippingCountry",
                        "ShippingStateProvince",
                        "ShippingCity",
                        "ShippingAddress1",
                        "ShippingAddress2",
                        "ShippingZipPostalCode",
                        "ShippingPhoneNumber",
                        "ShippingFaxNumber"
                    };
                for (int i = 0; i < properties.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = properties[i];
                    worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                }


                int row = 2;
                foreach (var order in orders)
                {
                    int col = 1;

                    //order properties
                    worksheet.Cells[row, col].Value = order.Id;
                    col++;

                    worksheet.Cells[row, col].Value = order.StoreId;
                    col++;

                    worksheet.Cells[row, col].Value = order.OrderGuid;
                    col++;

                    worksheet.Cells[row, col].Value = order.CustomerId;
                    col++;

                    worksheet.Cells[row, col].Value = order.OrderStatusId;
                    col++;

                    worksheet.Cells[row, col].Value = order.PaymentStatusId;
                    col++;

                    worksheet.Cells[row, col].Value = order.ShippingStatusId;
                    col++;

                    worksheet.Cells[row, col].Value = order.OrderSubtotalInclTax;
                    col++;

                    worksheet.Cells[row, col].Value = order.OrderSubtotalExclTax;
                    col++;

                    worksheet.Cells[row, col].Value = order.OrderSubTotalDiscountInclTax;
                    col++;

                    worksheet.Cells[row, col].Value = order.OrderSubTotalDiscountExclTax;
                    col++;

                    worksheet.Cells[row, col].Value = order.OrderShippingInclTax;
                    col++;

                    worksheet.Cells[row, col].Value = order.OrderShippingExclTax;
                    col++;

                    worksheet.Cells[row, col].Value = order.PaymentMethodAdditionalFeeInclTax;
                    col++;

                    worksheet.Cells[row, col].Value = order.PaymentMethodAdditionalFeeExclTax;
                    col++;

                    worksheet.Cells[row, col].Value = order.TaxRates;
                    col++;

                    worksheet.Cells[row, col].Value = order.OrderTax;
                    col++;

                    worksheet.Cells[row, col].Value = order.OrderTotal;
                    col++;

                    worksheet.Cells[row, col].Value = order.RefundedAmount;
                    col++;

                    worksheet.Cells[row, col].Value = order.OrderDiscount;
                    col++;

                    worksheet.Cells[row, col].Value = order.CurrencyRate;
                    col++;

                    worksheet.Cells[row, col].Value = order.CustomerCurrencyCode;
                    col++;

                    worksheet.Cells[row, col].Value = order.AffiliateId;
                    col++;

                    worksheet.Cells[row, col].Value = order.PaymentMethodSystemName;
                    col++;

                    worksheet.Cells[row, col].Value = order.PickUpInStore;
                    col++;

                    worksheet.Cells[row, col].Value = order.ShippingMethod;
                    col++;

                    worksheet.Cells[row, col].Value = order.ShippingRateComputationMethodSystemName;
                    col++;

                    worksheet.Cells[row, col].Value = order.CustomValuesXml;
                    col++;

                    worksheet.Cells[row, col].Value = order.VatNumber;
                    col++;

                    worksheet.Cells[row, col].Value = order.CreatedOnUtc.ToOADate();
                    col++;


                    //billing address
                    worksheet.Cells[row, col].Value = order.BillingAddress != null ? order.BillingAddress.FirstName : "";
                    col++;

                    worksheet.Cells[row, col].Value = order.BillingAddress != null ? order.BillingAddress.LastName : "";
                    col++;

                    worksheet.Cells[row, col].Value = order.BillingAddress != null ? order.BillingAddress.Email : "";
                    col++;

                    worksheet.Cells[row, col].Value = order.BillingAddress != null ? order.BillingAddress.Company : "";
                    col++;

                    worksheet.Cells[row, col].Value = order.BillingAddress != null && order.BillingAddress.Country != null ? order.BillingAddress.Country.Name : "";
                    col++;

                    worksheet.Cells[row, col].Value = order.BillingAddress != null && order.BillingAddress.StateProvince != null ? order.BillingAddress.StateProvince.Name : "";
                    col++;

                    worksheet.Cells[row, col].Value = order.BillingAddress != null ? order.BillingAddress.City : "";
                    col++;

                    worksheet.Cells[row, col].Value = order.BillingAddress != null ? order.BillingAddress.Address1 : "";
                    col++;

                    worksheet.Cells[row, col].Value = order.BillingAddress != null ? order.BillingAddress.Address2 : "";
                    col++;

                    worksheet.Cells[row, col].Value = order.BillingAddress != null ? order.BillingAddress.ZipPostalCode : "";
                    col++;

                    worksheet.Cells[row, col].Value = order.BillingAddress != null ? order.BillingAddress.PhoneNumber : "";
                    col++;

                    worksheet.Cells[row, col].Value = order.BillingAddress != null ? order.BillingAddress.FaxNumber : "";
                    col++;

                    //shipping address
                    worksheet.Cells[row, col].Value = order.ShippingAddress != null ? order.ShippingAddress.FirstName : "";
                    col++;

                    worksheet.Cells[row, col].Value = order.ShippingAddress != null ? order.ShippingAddress.LastName : "";
                    col++;

                    worksheet.Cells[row, col].Value = order.ShippingAddress != null ? order.ShippingAddress.Email : "";
                    col++;

                    worksheet.Cells[row, col].Value = order.ShippingAddress != null ? order.ShippingAddress.Company : "";
                    col++;

                    worksheet.Cells[row, col].Value = order.ShippingAddress != null && order.ShippingAddress.Country != null ? order.ShippingAddress.Country.Name : "";
                    col++;

                    worksheet.Cells[row, col].Value = order.ShippingAddress != null && order.ShippingAddress.StateProvince != null ? order.ShippingAddress.StateProvince.Name : "";
                    col++;

                    worksheet.Cells[row, col].Value = order.ShippingAddress != null ? order.ShippingAddress.City : "";
                    col++;

                    worksheet.Cells[row, col].Value = order.ShippingAddress != null ? order.ShippingAddress.Address1 : "";
                    col++;

                    worksheet.Cells[row, col].Value = order.ShippingAddress != null ? order.ShippingAddress.Address2 : "";
                    col++;

                    worksheet.Cells[row, col].Value = order.ShippingAddress != null ? order.ShippingAddress.ZipPostalCode : "";
                    col++;

                    worksheet.Cells[row, col].Value = order.ShippingAddress != null ? order.ShippingAddress.PhoneNumber : "";
                    col++;

                    worksheet.Cells[row, col].Value = order.ShippingAddress != null ? order.ShippingAddress.FaxNumber : "";
                    col++;

                    //next row
                    row++;
                }








                // we had better add some document properties to the spreadsheet 

                // set some core property values
                //var storeName = _storeInformationSettings.StoreName;
                //var storeUrl = _storeInformationSettings.StoreUrl;
                //xlPackage.Workbook.Properties.Title = string.Format("{0} orders", storeName);
                //xlPackage.Workbook.Properties.Author = storeName;
                //xlPackage.Workbook.Properties.Subject = string.Format("{0} orders", storeName);
                //xlPackage.Workbook.Properties.Keywords = string.Format("{0} orders", storeName);
                //xlPackage.Workbook.Properties.Category = "Orders";
                //xlPackage.Workbook.Properties.Comments = string.Format("{0} orders", storeName);

                // set some extended property values
                //xlPackage.Workbook.Properties.Company = storeName;
                //xlPackage.Workbook.Properties.HyperlinkBase = new Uri(storeUrl);

                // save the new spreadsheet
                xlPackage.Save();
            }
        }

        /// <summary>
        /// Export customer list to XLSX
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="customers">Customers</param>
        public virtual void ExportCustomersToXlsx(Stream stream, IList<Customer> customers)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            // ok, we can run the real code of the sample now
            using (var xlPackage = new ExcelPackage(stream))
            {
                // uncomment this line if you want the XML written out to the outputDir
                //xlPackage.DebugMode = true; 

                // get handle to the existing worksheet
                var worksheet = xlPackage.Workbook.Worksheets.Add("Customers");
                //Create Headers and format them
                var properties = new[]
                    {
                        "CustomerId",
                        "CustomerGuid",
                        "Email",
                        "Username",
                        "PasswordStr",//why can't we use 'Password' name?
                        "PasswordFormatId",
                        "PasswordSalt",
                        "IsTaxExempt",
                        "AffiliateId",
                        "VendorId",
                        "Active",
                        "IsGuest",
                        "IsRegistered",
                        "IsAdministrator",
                        "IsForumModerator",
                        "FirstName",
                        "LastName",
                        "Gender",
                        "Company",
                        "StreetAddress",
                        "StreetAddress2",
                        "ZipPostalCode",
                        "City",
                        "CountryId",
                        "StateProvinceId",
                        "Phone",
                        "Fax",
                        "VatNumber",
                        "VatNumberStatusId",
                        "TimeZoneId",
                        "AvatarPictureId",
                        "ForumPostCount",
                        "Signature"
                    };
                for (int i = 0; i < properties.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = properties[i];
                    worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                }


                int row = 2;
                foreach (var customer in customers)
                {
                    int col = 1;

                    worksheet.Cells[row, col].Value = customer.Id;
                    col++;

                    worksheet.Cells[row, col].Value = customer.CustomerGuid;
                    col++;

                    worksheet.Cells[row, col].Value = customer.Email;
                    col++;

                    worksheet.Cells[row, col].Value = customer.Username;
                    col++;

                    worksheet.Cells[row, col].Value = customer.Password;
                    col++;

                    worksheet.Cells[row, col].Value = customer.PasswordFormatId;
                    col++;

                    worksheet.Cells[row, col].Value = customer.PasswordSalt;
                    col++;

                    worksheet.Cells[row, col].Value = customer.IsTaxExempt;
                    col++;

                    worksheet.Cells[row, col].Value = customer.AffiliateId;
                    col++;

                    worksheet.Cells[row, col].Value = customer.VendorId;
                    col++;

                    worksheet.Cells[row, col].Value = customer.Active;
                    col++;

                    //roles
                    worksheet.Cells[row, col].Value = customer.IsGuest();
                    col++;

                    worksheet.Cells[row, col].Value = customer.IsRegistered();
                    col++;

                    worksheet.Cells[row, col].Value = customer.IsAdmin();
                    col++;

                    worksheet.Cells[row, col].Value = customer.IsForumModerator();
                    col++;

                    //attributes
                    var firstName = customer.GetAttribute<string>(SystemCustomerAttributeNames.FirstName);
                    var lastName = customer.GetAttribute<string>(SystemCustomerAttributeNames.LastName);
                    var gender = customer.GetAttribute<string>(SystemCustomerAttributeNames.Gender);
                    var company = customer.GetAttribute<string>(SystemCustomerAttributeNames.Company);
                    var streetAddress = customer.GetAttribute<string>(SystemCustomerAttributeNames.StreetAddress);
                    var streetAddress2 = customer.GetAttribute<string>(SystemCustomerAttributeNames.StreetAddress2);
                    var zipPostalCode = customer.GetAttribute<string>(SystemCustomerAttributeNames.ZipPostalCode);
                    var city = customer.GetAttribute<string>(SystemCustomerAttributeNames.City);
                    var countryId = customer.GetAttribute<int>(SystemCustomerAttributeNames.CountryId);
                    var stateProvinceId = customer.GetAttribute<int>(SystemCustomerAttributeNames.StateProvinceId);
                    var phone = customer.GetAttribute<string>(SystemCustomerAttributeNames.Phone);
                    var fax = customer.GetAttribute<string>(SystemCustomerAttributeNames.Fax);
                    var vatNumber = customer.GetAttribute<string>(SystemCustomerAttributeNames.VatNumber);
                    var vatNumberStatusId = customer.GetAttribute<int>(SystemCustomerAttributeNames.VatNumberStatusId);
                    var timeZoneId = customer.GetAttribute<string>(SystemCustomerAttributeNames.TimeZoneId);

                    var avatarPictureId = customer.GetAttribute<int>(SystemCustomerAttributeNames.AvatarPictureId);
                    var forumPostCount = customer.GetAttribute<int>(SystemCustomerAttributeNames.ForumPostCount);
                    var signature = customer.GetAttribute<string>(SystemCustomerAttributeNames.Signature);

                    worksheet.Cells[row, col].Value = firstName;
                    col++;

                    worksheet.Cells[row, col].Value = lastName;
                    col++;

                    worksheet.Cells[row, col].Value = gender;
                    col++;

                    worksheet.Cells[row, col].Value = company;
                    col++;

                    worksheet.Cells[row, col].Value = streetAddress;
                    col++;

                    worksheet.Cells[row, col].Value = streetAddress2;
                    col++;

                    worksheet.Cells[row, col].Value = zipPostalCode;
                    col++;

                    worksheet.Cells[row, col].Value = city;
                    col++;

                    worksheet.Cells[row, col].Value = countryId;
                    col++;

                    worksheet.Cells[row, col].Value = stateProvinceId;
                    col++;

                    worksheet.Cells[row, col].Value = phone;
                    col++;

                    worksheet.Cells[row, col].Value = fax;
                    col++;

                    worksheet.Cells[row, col].Value = vatNumber;
                    col++;

                    worksheet.Cells[row, col].Value = vatNumberStatusId;
                    col++;

                    worksheet.Cells[row, col].Value = timeZoneId;
                    col++;

                    worksheet.Cells[row, col].Value = avatarPictureId;
                    col++;

                    worksheet.Cells[row, col].Value = forumPostCount;
                    col++;

                    worksheet.Cells[row, col].Value = signature;
                    col++;

                    row++;
                }


                // we had better add some document properties to the spreadsheet 

                // set some core property values
                //var storeName = _storeInformationSettings.StoreName;
                //var storeUrl = _storeInformationSettings.StoreUrl;
                //xlPackage.Workbook.Properties.Title = string.Format("{0} customers", storeName);
                //xlPackage.Workbook.Properties.Author = storeName;
                //xlPackage.Workbook.Properties.Subject = string.Format("{0} customers", storeName);
                //xlPackage.Workbook.Properties.Keywords = string.Format("{0} customers", storeName);
                //xlPackage.Workbook.Properties.Category = "Customers";
                //xlPackage.Workbook.Properties.Comments = string.Format("{0} customers", storeName);

                // set some extended property values
                //xlPackage.Workbook.Properties.Company = storeName;
                //xlPackage.Workbook.Properties.HyperlinkBase = new Uri(storeUrl);

                // save the new spreadsheet
                xlPackage.Save();
            }
        }

        /// <summary>
        /// Export customer list to xml
        /// </summary>
        /// <param name="customers">Customers</param>
        /// <returns>Result in XML format</returns>
        public virtual string ExportCustomersToXml(IList<Customer> customers)
        {
            var sb = new StringBuilder();
            var stringWriter = new StringWriter(sb);
            var xmlWriter = new XmlTextWriter(stringWriter);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Customers");
            xmlWriter.WriteAttributeString("Version", NopVersion.CurrentVersion);

            foreach (var customer in customers)
            {
                xmlWriter.WriteStartElement("Customer");
                xmlWriter.WriteElementString("CustomerId", null, customer.Id.ToString());
                xmlWriter.WriteElementString("CustomerGuid", null, customer.CustomerGuid.ToString());
                xmlWriter.WriteElementString("Email", null, customer.Email);
                xmlWriter.WriteElementString("Username", null, customer.Username);
                xmlWriter.WriteElementString("Password", null, customer.Password);
                xmlWriter.WriteElementString("PasswordFormatId", null, customer.PasswordFormatId.ToString());
                xmlWriter.WriteElementString("PasswordSalt", null, customer.PasswordSalt);
                xmlWriter.WriteElementString("IsTaxExempt", null, customer.IsTaxExempt.ToString());
                xmlWriter.WriteElementString("AffiliateId", null, customer.AffiliateId.ToString());
                xmlWriter.WriteElementString("VendorId", null, customer.VendorId.ToString());
                xmlWriter.WriteElementString("Active", null, customer.Active.ToString());


                xmlWriter.WriteElementString("IsGuest", null, customer.IsGuest().ToString());
                xmlWriter.WriteElementString("IsRegistered", null, customer.IsRegistered().ToString());
                xmlWriter.WriteElementString("IsAdministrator", null, customer.IsAdmin().ToString());
                xmlWriter.WriteElementString("IsForumModerator", null, customer.IsForumModerator().ToString());

                xmlWriter.WriteElementString("FirstName", null, customer.GetAttribute<string>(SystemCustomerAttributeNames.FirstName));
                xmlWriter.WriteElementString("LastName", null, customer.GetAttribute<string>(SystemCustomerAttributeNames.LastName));
                xmlWriter.WriteElementString("Gender", null, customer.GetAttribute<string>(SystemCustomerAttributeNames.Gender));
                xmlWriter.WriteElementString("Company", null, customer.GetAttribute<string>(SystemCustomerAttributeNames.Company));

                xmlWriter.WriteElementString("CountryId", null, customer.GetAttribute<int>(SystemCustomerAttributeNames.CountryId).ToString());
                xmlWriter.WriteElementString("StreetAddress", null, customer.GetAttribute<string>(SystemCustomerAttributeNames.StreetAddress));
                xmlWriter.WriteElementString("StreetAddress2", null, customer.GetAttribute<string>(SystemCustomerAttributeNames.StreetAddress2));
                xmlWriter.WriteElementString("ZipPostalCode", null, customer.GetAttribute<string>(SystemCustomerAttributeNames.ZipPostalCode));
                xmlWriter.WriteElementString("City", null, customer.GetAttribute<string>(SystemCustomerAttributeNames.City));
                xmlWriter.WriteElementString("CountryId", null, customer.GetAttribute<int>(SystemCustomerAttributeNames.CountryId).ToString());
                xmlWriter.WriteElementString("StateProvinceId", null, customer.GetAttribute<int>(SystemCustomerAttributeNames.StateProvinceId).ToString());
                xmlWriter.WriteElementString("Phone", null, customer.GetAttribute<string>(SystemCustomerAttributeNames.Phone));
                xmlWriter.WriteElementString("Fax", null, customer.GetAttribute<string>(SystemCustomerAttributeNames.Fax));
                xmlWriter.WriteElementString("VatNumber", null, customer.GetAttribute<string>(SystemCustomerAttributeNames.VatNumber));
                xmlWriter.WriteElementString("VatNumberStatusId", null, customer.GetAttribute<int>(SystemCustomerAttributeNames.VatNumberStatusId).ToString());
                xmlWriter.WriteElementString("TimeZoneId", null, customer.GetAttribute<string>(SystemCustomerAttributeNames.TimeZoneId));

                foreach (var store in _storeService.GetAllStores())
                {
                    var newsletter = _newsLetterSubscriptionService.GetNewsLetterSubscriptionByEmailAndStoreId(customer.Email, store.Id);
                    bool subscribedToNewsletters = newsletter != null && newsletter.Active;
                    xmlWriter.WriteElementString(string.Format("Newsletter-in-store-{0}", store.Id), null, subscribedToNewsletters.ToString());
                }

                xmlWriter.WriteElementString("AvatarPictureId", null, customer.GetAttribute<int>(SystemCustomerAttributeNames.AvatarPictureId).ToString());
                xmlWriter.WriteElementString("ForumPostCount", null, customer.GetAttribute<int>(SystemCustomerAttributeNames.ForumPostCount).ToString());
                xmlWriter.WriteElementString("Signature", null, customer.GetAttribute<string>(SystemCustomerAttributeNames.Signature));

                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
            return stringWriter.ToString();
        }

        /// <summary>
        /// Export newsletter subscribers to TXT
        /// </summary>
        /// <param name="subscriptions">Subscriptions</param>
        /// <returns>Result in TXT (string) format</returns>
        public virtual string ExportNewsletterSubscribersToTxt(IList<NewsLetterSubscription> subscriptions)
        {
            if (subscriptions == null)
                throw new ArgumentNullException("subscriptions");

            var sb = new StringBuilder();
            foreach (var subscription in subscriptions)
            {
                sb.Append(subscription.Email);
                sb.Append(",");
                sb.Append(subscription.Active);
                sb.Append(",");
                sb.Append(subscription.StoreId);
                sb.Append(Environment.NewLine);  //new line
            }
            return sb.ToString();
        }

        /// <summary>
        /// Export states to TXT
        /// </summary>
        /// <param name="states">States</param>
        /// <returns>Result in TXT (string) format</returns>
        public virtual string ExportStatesToTxt(IList<StateProvince> states)
        {
            if (states == null)
                throw new ArgumentNullException("states");

            var sb = new StringBuilder();
            foreach (var state in states)
            {
                sb.Append(state.Country.TwoLetterIsoCode);
                sb.Append(",");
                sb.Append(state.Name);
                sb.Append(",");
                sb.Append(state.Abbreviation);
                sb.Append(",");
                sb.Append(state.Published);
                sb.Append(",");
                sb.Append(state.DisplayOrder);
                sb.Append(Environment.NewLine);  //new line
            }
            return sb.ToString();
        }

        #endregion
        public virtual void ExportDoanhThuXeToXlsx(Stream stream, List<DoanhThuNhanVienTheoXe> DoanhThuXes, DateTime TuNgay, DateTime DenNgay)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            // ok, we can run the real code of the sample now
            using (var xlPackage = new ExcelPackage(stream))
            {
                // uncomment this line if you want the XML written out to the outputDir
                //xlPackage.DebugMode = true; 

                // get handle to the existing worksheet
                var worksheet = xlPackage.Workbook.Worksheets.Add("DoanhThuXe");
                //Create Headers and format them
                worksheet.DefaultRowHeight = 20;
                worksheet.Cells["A1:G1"].Merge = true;

                worksheet.Cells[1, 1].Value = "Bảng kê chi tiết doanh thu bán vé xe khách";
                worksheet.Cells["B2"].Value = "Tuyến đường : ";


                worksheet.Cells[1, 1].Style.Font.Size = 14;
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //ngay thang nam

                worksheet.Cells["B2:D2"].Merge = true;

                worksheet.Cells["B2"].Style.Font.Bold = true;

                worksheet.Cells["E2:H2"].Merge = true;
                worksheet.Cells["E2"].Value = "Từ ngày " + TuNgay.ToString("dd/MM/yyyy") + " Đến ngày " + DenNgay.ToString("dd/MM/yyyy");
                worksheet.Cells["E2"].Style.Font.Bold = true;

                worksheet.Cells["A4:A5"].Merge = true;
                worksheet.Cells["A4"].Value = "STT";

                worksheet.Cells["B4:B5"].Merge = true;
                worksheet.Cells["B4"].Value = "Biển số";

                worksheet.Cells["C4:C5"].Merge = true;
                worksheet.Cells["C4"].Value = "Tên lái xe";

                worksheet.Cells["D4:D5"].Merge = true;
                worksheet.Cells["D4"].Value = "Tổng lượt";

                worksheet.Cells["E4:E5"].Merge = true;
                worksheet.Cells["E4"].Value = "Số khách";

                worksheet.Cells["F4:F5"].Merge = true;
                worksheet.Cells["F4"].Value = "Thành tiền";





                int row = 6;
                int stt = 1;
                foreach (var item in DoanhThuXes)
                {
                    int col = 1;
                    worksheet.Cells[row, col].Value = stt;
                    col++;
                    worksheet.Cells[row, col].Value = item.BienSoXe;
                    col++;

                    worksheet.Cells[row, col].Value = item.TenLaiXe;
                    col++;

                    worksheet.Cells[row, col].Value = item.TongLuot;
                    col++;


                    worksheet.Cells[row, col].Value = item.SoKhach;
                    col++;


                    worksheet.Cells[row, col].Value = item.ThanhTien.ToString("###,###");
                    col++;

                    row++;
                    stt++;
                }
                //style header
                string modelRangeheader = "A4:G5";
                var modelTableheader = worksheet.Cells[modelRangeheader];
                modelTableheader.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                modelTableheader.Style.Font.Bold = true;
                //style body
                var modelRows = row;
                string modelRange = "A6:G" + modelRows.ToString();
                var modelTable = worksheet.Cells[modelRange];

                // Assign borders
                modelTable.Style.Border.Top.Style = ExcelBorderStyle.Dashed;
                modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.DashDot;
                modelTable.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                // Fill worksheet with data to export

                modelTable.AutoFitColumns();
                // we had better add some document properties to the spreadsheet 

                // set some core property values
                //var storeName = _storeInformationSettings.StoreName;
                //var storeUrl = _storeInformationSettings.StoreUrl;
                //xlPackage.Workbook.Properties.Title = string.Format("{0} customers", storeName);
                //xlPackage.Workbook.Properties.Author = storeName;
                //xlPackage.Workbook.Properties.Subject = string.Format("{0} customers", storeName);
                //xlPackage.Workbook.Properties.Keywords = string.Format("{0} customers", storeName);
                //xlPackage.Workbook.Properties.Category = "Customers";
                //xlPackage.Workbook.Properties.Comments = string.Format("{0} customers", storeName);

                // set some extended property values
                //xlPackage.Workbook.Properties.Company = storeName;
                //xlPackage.Workbook.Properties.HyperlinkBase = new Uri(storeUrl);

                // save the new spreadsheet
                xlPackage.Save();
            }
        }

        public virtual void ExportDoanhThuTheoNgayToXlsx(Stream stream,List<DoanhThuTheoNgay> DoanhThuNgays,DateTime TuNgay,DateTime DenNgay)
        {
            if (stream == null)
                throw new ArgumentException("stream");
            // ok, we can run the real code of the sample now
            using (var xlPackage = new ExcelPackage(stream))
            {
                // uncomment this line if you want the XML written out to the outputDir
                //xlPackage.DebugMode = true; 

                // get handle to the existing worksheet
                var worksheet = xlPackage.Workbook.Worksheets.Add("DoanhThuNgay");
                //Create Headers and format them
                worksheet.DefaultRowHeight = 20;
                worksheet.Cells["A1:G1"].Merge = true;

                worksheet.Cells[1, 1].Value = "Bảng kê chi tiết doanh thu bán vé xe khách theo tháng";
                worksheet.Cells["B2"].Value = "Tuyến đường : ";


                worksheet.Cells[1, 1].Style.Font.Size = 14;
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //ngay thang nam

                worksheet.Cells["B2:D2"].Merge = true;

                worksheet.Cells["B2"].Style.Font.Bold = true;

                worksheet.Cells["E2:H2"].Merge = true;
                worksheet.Cells["E2"].Value = "Từ ngày " + TuNgay.ToString("dd/MM/yyyy") + " Đến ngày " + DenNgay.ToString("dd/MM/yyyy");
                worksheet.Cells["E2"].Style.Font.Bold = true;

                worksheet.Cells["A4:A5"].Merge = true;
                worksheet.Cells["A4"].Value = "STT";

                worksheet.Cells["B4:C5"].Merge = true;
                worksheet.Cells["B4"].Value = "Ngày bán";

                worksheet.Cells["D4:D5"].Merge = true;
                worksheet.Cells["D4"].Value = "Số lượng đặt";

                worksheet.Cells["E4:E5"].Merge = true;
                worksheet.Cells["E4"].Value = "Số lượng chuyển";

                worksheet.Cells["F4:F5"].Merge = true;
                worksheet.Cells["F4"].Value = "Số lượng hủy";

                worksheet.Cells["G4:G5"].Merge = true;
                worksheet.Cells["G4"].Value = "Thành tiền";

                int row = 6;
                int stt = 1;
                foreach (var item in DoanhThuNgays)
                {
                    int col = 1;

                    worksheet.Cells[row, col].Value = stt;
                    col++;

                    worksheet.Cells["B" + row + ":C" + row].Merge = true;
                    worksheet.Cells[row, col].Value = item.NgayBan;
                    col +=2;

                    worksheet.Cells[row, col].Value = item.SoLuongDat;
                    col ++;

                    worksheet.Cells[row, col].Value = item.SoLuongChuyen;
                    col++;

                    worksheet.Cells[row, col].Value = item.SoLuongHuy;
                    col++;

                    worksheet.Cells[row, col].Value = item.ThanhTien.ToString("###,###");
                    col++;

                    row++;
                    stt++;
                }
                //style header
                string modelRangeheader = "A4:G5";
                var modelTableheader = worksheet.Cells[modelRangeheader];
                modelTableheader.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                modelTableheader.Style.Font.Bold = true;
                //style body
                var modelRows = row;
                string modelRange = "A6:G" + modelRows.ToString();
                var modelTable = worksheet.Cells[modelRange];

                // Assign borders
                modelTable.Style.Border.Top.Style = ExcelBorderStyle.Dashed;
                modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.DashDot;
                modelTable.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                // Fill worksheet with data to export

                modelTable.AutoFitColumns();
                // we had better add some document properties to the spreadsheet 

                // set some core property values
                //var storeName = _storeInformationSettings.StoreName;
                //var storeUrl = _storeInformationSettings.StoreUrl;
                //xlPackage.Workbook.Properties.Title = string.Format("{0} customers", storeName);
                //xlPackage.Workbook.Properties.Author = storeName;
                //xlPackage.Workbook.Properties.Subject = string.Format("{0} customers", storeName);
                //xlPackage.Workbook.Properties.Keywords = string.Format("{0} customers", storeName);
                //xlPackage.Workbook.Properties.Category = "Customers";
                //xlPackage.Workbook.Properties.Comments = string.Format("{0} customers", storeName);

                // set some extended property values
                //xlPackage.Workbook.Properties.Company = storeName;
                //xlPackage.Workbook.Properties.HyperlinkBase = new Uri(storeUrl);

                // save the new spreadsheet
                xlPackage.Save();
            }
        }

        public virtual void ExportLuotTrongNgayToXlsx(Stream stream, List<XeDiTrongNgay> DoanhThuXes, DateTime TuNgay,DateTime DenNgay)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            // ok, we can run the real code of the sample now
            using (var xlPackage = new ExcelPackage(stream))
            {
                // uncomment this line if you want the XML written out to the outputDir
                //xlPackage.DebugMode = true; 

                // get handle to the existing worksheet
                var worksheet = xlPackage.Workbook.Worksheets.Add("DoanhThuXe");
                //Create Headers and format them
                worksheet.DefaultRowHeight = 20;
                worksheet.Cells["A1:I1"].Merge = true;

                worksheet.Cells[1, 1].Value = "Bảng kê chi tiết chuyến theo ngày";
                worksheet.Cells["B2"].Value = "Tuyến đường : ";


                worksheet.Cells[1, 1].Style.Font.Size = 14;
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //ngay thang nam

                worksheet.Cells["B2:D2"].Merge = true;

                worksheet.Cells["B2"].Style.Font.Bold = true;

                worksheet.Cells["E2:H2"].Merge = true;
                worksheet.Cells["E2"].Value = "Từ ngày " + TuNgay.ToString("dd/MM/yyyy") + " đến ngày " + DenNgay.ToString("dd/MM/yyyy");
                worksheet.Cells["E2"].Style.Font.Bold = true;

                worksheet.Cells["A4:A5"].Merge = true;
                worksheet.Cells["A4"].Value = "STT";
                //worksheet.Cells["A4"].Style.

                worksheet.Cells["B4:B5"].Merge = true;
                worksheet.Cells["B4"].Value = "Biển số";

                worksheet.Cells["C4:C5"].Merge = true;
                worksheet.Cells["C4"].Value = "Bắt đầu";

                worksheet.Cells["D4:D5"].Merge = true;
                worksheet.Cells["D4"].Value = "Kết thúc";

                worksheet.Cells["E4:E5"].Merge = true;
                worksheet.Cells["E4"].Value = "Số lượt";

                worksheet.Cells["F4:F5"].Merge = true;
                worksheet.Cells["F4"].Value = "Số khách";

                worksheet.Cells["G4:G5"].Merge = true;
                worksheet.Cells["G4"].Value = "Số tiền";

                worksheet.Cells["H4:H5"].Merge = true;
                worksheet.Cells["H4"].Value = "Hành trình";


                worksheet.Cells["I4:I5"].Merge = true;
                worksheet.Cells["I4"].Value = "Lái xe";


                int row = 6;
                int stt = 1;
                foreach (var item in DoanhThuXes)
                {
                    foreach (var chuyen in item.doanhthus)
                    {
                        int col = 1;
                        worksheet.Cells[row, col].Value = stt;
                        col++;
                        worksheet.Cells[row, col].Value = chuyen.BienSoXe;
                        col++;

                        worksheet.Cells[row, col].Value = chuyen.BatDau;
                        col++;

                        worksheet.Cells[row, col].Value = chuyen.KetThuc;
                        col++;

                        worksheet.Cells[row, col].Value = chuyen.SoLuot;
                        col++;

                        worksheet.Cells[row, col].Value = chuyen.SoKhach;
                        col++;


                        worksheet.Cells[row, col].Value = chuyen.SoTien.ToString("###,###");
                        col++;
                        worksheet.Cells[row, col].Value = chuyen.DiemDau;
                        col++;
                        worksheet.Cells[row, col].Value = chuyen.TenLai;
                        col++;

                        row++;
                        stt++;
                    }

                    int _col = 1;
                    worksheet.Cells[row, _col].Value = "";
                    _col++;

                    worksheet.Cells[row, _col].Value = "Tổng";
                    _col++;

                    worksheet.Cells[row, _col].Value = "";
                    _col++;

                    worksheet.Cells[row, _col].Value = "";
                    _col++;

                    worksheet.Cells[row, _col].Value = item.TongLuot;
                    _col++;

                    worksheet.Cells[row, _col].Value = item.TongKhach;
                    _col++;


                    worksheet.Cells[row, _col].Value = item.TongTien.ToString("###,###");
                    _col++;
                    worksheet.Cells[row, _col].Value = "";
                    _col++;
                    worksheet.Cells[row, _col].Value = "";
                    _col++;

                    row++;
                }
                //style header
                string modelRangeheader = "A4:I5";
                var modelTableheader = worksheet.Cells[modelRangeheader];
                modelTableheader.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                modelTableheader.Style.Font.Bold = true;
                //style body
                var modelRows = row;
                string modelRange = "A6:I" + modelRows.ToString();
                var modelTable = worksheet.Cells[modelRange];

                // Assign borders
                modelTable.Style.Border.Top.Style = ExcelBorderStyle.Dashed;
                modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.DashDot;
                modelTable.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                // Fill worksheet with data to export

                modelTable.AutoFitColumns();
                // we had better add some document properties to the spreadsheet 

                // set some core property values
                //var storeName = _storeInformationSettings.StoreName;
                //var storeUrl = _storeInformationSettings.StoreUrl;
                //xlPackage.Workbook.Properties.Title = string.Format("{0} customers", storeName);
                //xlPackage.Workbook.Properties.Author = storeName;
                //xlPackage.Workbook.Properties.Subject = string.Format("{0} customers", storeName);
                //xlPackage.Workbook.Properties.Keywords = string.Format("{0} customers", storeName);
                //xlPackage.Workbook.Properties.Category = "Customers";
                //xlPackage.Workbook.Properties.Comments = string.Format("{0} customers", storeName);

                // set some extended property values
                //xlPackage.Workbook.Properties.Company = storeName;
                //xlPackage.Workbook.Properties.HyperlinkBase = new Uri(storeUrl);

                // save the new spreadsheet
                xlPackage.Save();
            }
        }
        public virtual void ExportLenhPhuToXlsx(Stream stream, List<DatVe> hanhkhachs, DateTime NgayDi, string BienSo)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            // ok, we can run the real code of the sample now
            using (var xlPackage = new ExcelPackage(stream))
            {
                // uncomment this line if you want the XML written out to the outputDir
                //xlPackage.DebugMode = true; 

                // get handle to the existing worksheet
                var worksheet = xlPackage.Workbook.Worksheets.Add("Lenh Phu");
                //Create Headers and format them
                worksheet.DefaultRowHeight = 20;
                worksheet.Cells["A1:D1"].Merge = true;

                worksheet.Cells[1, 1].Value = "Lệnh phụ đi đường";

                worksheet.Cells["B2"].Value = "Biển số : " + BienSo;


                worksheet.Cells[1, 1].Style.Font.Size = 14;
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //ngay thang nam



                worksheet.Cells["B2"].Style.Font.Bold = true;


                worksheet.Cells["D2"].Value = "Ngày " + NgayDi.ToString("dd/MM/yyyy HH:mm");
                worksheet.Cells["D2"].Style.Font.Bold = true;

                worksheet.Cells["A4:A5"].Merge = true;
                worksheet.Cells["A4"].Value = "STT";

                worksheet.Cells["B4:B5"].Merge = true;
                worksheet.Cells["B4"].Value = "Họ tên hành khách";

                worksheet.Cells["C4:C5"].Merge = true;
                worksheet.Cells["C4"].Value = "Điểm đón-trả khách";

                worksheet.Cells["D4:D5"].Merge = true;
                worksheet.Cells["D4"].Value = "Ghi chú";


                int row = 6;
                int stt = 1;
                foreach (var item in hanhkhachs)
                {
                    int col = 1;
                    worksheet.Cells[row, col].Value = stt;
                    col++;
                    string tenkh = string.IsNullOrEmpty(item.TenKhachHangDiKem) ? item.khachhang.Ten : item.TenKhachHangDiKem;
                    worksheet.Cells[row, col].Value = tenkh;
                    col++;
                    if (item.isNoiBai)
                        worksheet.Cells[row, col].Value = item.diemdon.TenDiemDon + "/Nội Bài";
                    else
                        worksheet.Cells[row, col].Value = item.diemdon.TenDiemDon;

                    col++;
                    string _dienthoai = item.khachhang.DienThoai;
                    if (!string.IsNullOrEmpty(item.GhiChu))
                        _dienthoai = _dienthoai + "(" + item.GhiChu + ")";
                    worksheet.Cells[row, col].Value = _dienthoai;
                    col++;


                    row++;
                    stt++;
                }
                //style header
                string modelRangeheader = "A4:D5";
                var modelTableheader = worksheet.Cells[modelRangeheader];
                modelTableheader.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                modelTableheader.Style.Font.Bold = true;
                //style body
                var modelRows = row;
                string modelRange = "A6:D" + modelRows.ToString();
                var modelTable = worksheet.Cells[modelRange];
                // Assign borders
                modelTable.Style.Border.Top.Style = ExcelBorderStyle.Dashed;
                modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.DashDot;
                modelTable.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                string modelRangeInfo = "B6:D" + modelRows.ToString();
                var infotable = worksheet.Cells[modelRangeInfo];
                infotable.AutoFitColumns(30, 100);

                xlPackage.Save();
            }
        }

        public virtual void ExportKhachHangToXlsx(Stream stream, List<DatVe> DoanhThuXes, DateTime TuNgay, DateTime DenNgay)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            // ok, we can run the real code of the sample now
            using (var xlPackage = new ExcelPackage(stream))
            {
                // uncomment this line if you want the XML written out to the outputDir
                //xlPackage.DebugMode = true; 

                // get handle to the existing worksheet
                var worksheet = xlPackage.Workbook.Worksheets.Add("ThongKeLuotKH");
                //Create Headers and format them
                worksheet.DefaultRowHeight = 20;
                worksheet.Cells["A1:G1"].Merge = true;
                var tenkhach = "";
                worksheet.Cells[1, 1].Value = "Thống kê lượt khách";
                worksheet.Cells["B2"].Value = "Khách hàng : " + tenkhach;


                worksheet.Cells[1, 1].Style.Font.Size = 14;
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //ngay thang nam

                worksheet.Cells["B2:D2"].Merge = true;

                worksheet.Cells["B2"].Style.Font.Bold = true;

                worksheet.Cells["E2:H2"].Merge = true;
                worksheet.Cells["E2"].Value = "Từ ngày " + TuNgay.ToString("dd/MM/yyyy") + " Đến ngày " + DenNgay.ToString("dd/MM/yyyy");
                worksheet.Cells["E2"].Style.Font.Bold = true;

                worksheet.Cells["A4:A5"].Merge = true;
                worksheet.Cells["A4"].Value = "STT";

                worksheet.Cells["B4:B5"].Merge = true;
                worksheet.Cells["B4"].Value = "Hành trình";

                worksheet.Cells["C4:C5"].Merge = true;
                worksheet.Cells["C4"].Value = "Điện thoại";

                worksheet.Cells["D4:D5"].Merge = true;
                worksheet.Cells["D4"].Value = "Ngày đi";

                worksheet.Cells["E4:E5"].Merge = true;
                worksheet.Cells["E4"].Value = "Biến số xe";

                worksheet.Cells["F4:F5"].Merge = true;
                worksheet.Cells["F4"].Value = "Thành tiền";


                int row = 6;
                int stt = 1;
                foreach (var item in DoanhThuXes)
                {
                    int col = 1;
                    tenkhach = item.khachhang.Ten;
                    worksheet.Cells[row, col].Value = stt;
                    col++;
                    worksheet.Cells[row, col].Value = item.hanhtrinh.MoTa;
                    col++;

                    worksheet.Cells[row, col].Value = item.khachhang.DienThoai;
                    col++;

                    worksheet.Cells[row, col].Value = item.chuyendi.NgayDiThuc.ToString("dd/MM/yyyy HH:mm");
                    col++;

                    if (item.chuyendi.xevanchuyen == null)
                    {
                        worksheet.Cells[row, col].Value = "";
                    }
                    else
                    {
                        worksheet.Cells[row, col].Value = item.chuyendi.xevanchuyen.BienSo;
                    }
                    col++;


                    worksheet.Cells[row, col].Value = item.GiaTien.ToString("###,###");
                    col++;

                    row++;
                    stt++;
                }
                int coltong = 1;
                worksheet.Cells[row, coltong].Value = stt;
                coltong++;
                worksheet.Cells[row, coltong].Value = "Tổng";
                coltong = coltong + 2;

                worksheet.Cells[row, coltong].Value = DoanhThuXes.Count();

                coltong = coltong + 2;

                worksheet.Cells[row, coltong].Value = DoanhThuXes.Sum(c => c.GiaTien).ToString("###,###");
                coltong++;
                //style header
                string modelRangeheader = "A4:G5";
                var modelTableheader = worksheet.Cells[modelRangeheader];
                modelTableheader.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                modelTableheader.Style.Font.Bold = true;
                //style body
                var modelRows = row;
                string modelRange = "A6:G" + modelRows.ToString();
                var modelTable = worksheet.Cells[modelRange];

                // Assign borders
                modelTable.Style.Border.Top.Style = ExcelBorderStyle.Dashed;
                modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.DashDot;
                modelTable.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                // Fill worksheet with data to export

                modelTable.AutoFitColumns();
                // we had better add some document properties to the spreadsheet 

                // set some core property values
                //var storeName = _storeInformationSettings.StoreName;
                //var storeUrl = _storeInformationSettings.StoreUrl;
                //xlPackage.Workbook.Properties.Title = string.Format("{0} customers", storeName);
                //xlPackage.Workbook.Properties.Author = storeName;
                //xlPackage.Workbook.Properties.Subject = string.Format("{0} customers", storeName);
                //xlPackage.Workbook.Properties.Keywords = string.Format("{0} customers", storeName);
                //xlPackage.Workbook.Properties.Category = "Customers";
                //xlPackage.Workbook.Properties.Comments = string.Format("{0} customers", storeName);

                // set some extended property values
                //xlPackage.Workbook.Properties.Company = storeName;
                //xlPackage.Workbook.Properties.HyperlinkBase = new Uri(storeUrl);

                // save the new spreadsheet
                xlPackage.Save();
            }
        }
        /// <summary>
        /// xuat excel hop dong chuyen
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="DoanhThuXes"></param>
        /// <param name="TuNgay"></param>
        /// <param name="DenNgay"></param>
        public virtual void ExportHopDongChuyenToXlsx(Stream stream, List<HopDongChuyen> HopDongChuyens, DateTime TuNgay, DateTime DenNgay)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            // ok, we can run the real code of the sample now
            using (var xlPackage = new ExcelPackage(stream))
            {
                // uncomment this line if you want the XML written out to the outputDir
                //xlPackage.DebugMode = true; 

                // get handle to the existing worksheet
                var worksheet = xlPackage.Workbook.Worksheets.Add("HopDongChuyens");
                //Create Headers and format them
                worksheet.DefaultRowHeight = 20;
                worksheet.Cells["A1:L1"].Merge = true;
                var tenkhach = "";
                worksheet.Cells[1, 1].Value = "Danh sách hợp đồng chuyến";
                worksheet.Cells["B2"].Value = " " ;


                worksheet.Cells[1, 1].Style.Font.Size = 14;
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //ngay thang nam

                worksheet.Cells["B2:D2"].Merge = true;

                worksheet.Cells["B2"].Style.Font.Bold = true;

                worksheet.Cells["E2:H2"].Merge = true;
                worksheet.Cells["E2"].Value = "Từ ngày " + TuNgay.ToString("dd/MM/yyyy") + " Đến ngày " + DenNgay.ToString("dd/MM/yyyy");
                worksheet.Cells["E2"].Style.Font.Bold = true;

                worksheet.Cells["A4:A5"].Merge = true;
                worksheet.Cells["A4"].Value = "STT";

                worksheet.Cells["B4:B5"].Merge = true;
                worksheet.Cells["B4"].Value = "Số hợp đồng";

                worksheet.Cells["C4:C5"].Merge = true;
                worksheet.Cells["C4"].Value = "Ngày đi";

                worksheet.Cells["D4:D5"].Merge = true;
                worksheet.Cells["D4"].Value = "Biến số xe";

                worksheet.Cells["E4:E5"].Merge = true;
                worksheet.Cells["E4"].Value = "Tên lái xe";

                worksheet.Cells["F4:F5"].Merge = true;
                worksheet.Cells["F4"].Value = "Hành trình";

                worksheet.Cells["G4:G5"].Merge = true;
                worksheet.Cells["G4"].Value = "Km xuất";

                worksheet.Cells["H4:H5"].Merge = true;
                worksheet.Cells["H4"].Value = "Giá trị HD";
                int row = 6;
                int stt = 1;
                foreach (var item in HopDongChuyens)
                {
                    int col = 1;
                    string ngaydi = "";
                    string bienso = "";
                    string tenlaixe = "";
                   
                    worksheet.Cells[row, col].Value = stt;
                    col++;
                    worksheet.Cells[row, col].Value = item.SoHopDong;
                    col++;
                    if (item.ThoiGianDonKhach.HasValue)
                    {
                        ngaydi = item.ThoiGianDonKhach.Value.ToString("dd/MM/yyyy");
                    }

                    worksheet.Cells[row, col].Value = ngaydi; 
                    col++;
                    if(item.XeInfo!=null)
                        bienso = item.XeInfo.BienSo;
                    worksheet.Cells[row, col].Value = bienso; 
                    col++;
                    if(item.laixe!=null)
                       tenlaixe = item.laixe.HoVaTen;
                    worksheet.Cells[row, col].Value = tenlaixe;
                    col++;
                    worksheet.Cells[row, col].Value = item.LoTrinh;
                    col++;

                    worksheet.Cells[row, col].Value = item.KmXuat;
                    col++;
                    worksheet.Cells[row, col].Value = item.GiaTri.ToString("###,###");
                    col++;
                    row++;
                    stt++;
                }
                int coltong = 1;
                worksheet.Cells[row, coltong].Value = stt;
                coltong++;
                worksheet.Cells[row, coltong].Value = "Tổng";
                coltong = coltong + 6;

                worksheet.Cells[row, coltong].Value = HopDongChuyens.Sum(c => c.GiaTri).ToString("###,###");
                coltong++;
                //style header
                string modelRangeheader = "A4:H5";
                var modelTableheader = worksheet.Cells[modelRangeheader];
                modelTableheader.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                modelTableheader.Style.Font.Bold = true;
                //style body
                var modelRows = row;
                string modelRange = "A6:H" + modelRows.ToString();
                var modelTable = worksheet.Cells[modelRange];

                // Assign borders
                modelTable.Style.Border.Top.Style = ExcelBorderStyle.Dashed;
                modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.DashDot;
                modelTable.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                // Fill worksheet with data to export

                modelTable.AutoFitColumns();
                // we had better add some document properties to the spreadsheet 

                // set some core property values
                //var storeName = _storeInformationSettings.StoreName;
                //var storeUrl = _storeInformationSettings.StoreUrl;
                //xlPackage.Workbook.Properties.Title = string.Format("{0} customers", storeName);
                //xlPackage.Workbook.Properties.Author = storeName;
                //xlPackage.Workbook.Properties.Subject = string.Format("{0} customers", storeName);
                //xlPackage.Workbook.Properties.Keywords = string.Format("{0} customers", storeName);
                //xlPackage.Workbook.Properties.Category = "Customers";
                //xlPackage.Workbook.Properties.Comments = string.Format("{0} customers", storeName);

                // set some extended property values
                //xlPackage.Workbook.Properties.Company = storeName;
                //xlPackage.Workbook.Properties.HyperlinkBase = new Uri(storeUrl);

                // save the new spreadsheet
                xlPackage.Save();
            }
        }
        /// <summary>
        /// doanh thu tong hop
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="HopDongChuyens"></param>
        /// <param name="TuNgay"></param>
        /// <param name="DenNgay"></param>
        public virtual void ExportDTTongHopToXlsx(Stream stream, List<ListDoanhThuTongHop> DTTongHop, DateTime TuNgay, DateTime DenNgay)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            // ok, we can run the real code of the sample now
            using (var xlPackage = new ExcelPackage(stream))
            {
                // uncomment this line if you want the XML written out to the outputDir
                //xlPackage.DebugMode = true; 

                // get handle to the existing worksheet
                var worksheet = xlPackage.Workbook.Worksheets.Add("DTTongHop");
                //Create Headers and format them
                worksheet.DefaultRowHeight = 20;
                worksheet.Cells["A1:G1"].Merge = true;
                var tenkhach = "";
                worksheet.Cells[1, 1].Value = "Doanh thu tổng hợp";
                worksheet.Cells["B2"].Value = " ";


                worksheet.Cells[1, 1].Style.Font.Size = 14;
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //ngay thang nam

                worksheet.Cells["B2:D2"].Merge = true;

                worksheet.Cells["B2"].Style.Font.Bold = true;

                worksheet.Cells["E2:H2"].Merge = true;
                worksheet.Cells["E2"].Value = "Từ ngày " + TuNgay.ToString("dd/MM/yyyy") + " Đến ngày " + DenNgay.ToString("dd/MM/yyyy");
                worksheet.Cells["E2"].Style.Font.Bold = true;

                worksheet.Cells["A4:A5"].Merge = true;
                worksheet.Cells["A4"].Value = "Ngày đi";

                worksheet.Cells["B4:D4"].Merge = true;
                worksheet.Cells["B4"].Value = "Hà Nội ";


                worksheet.Cells["B5"].Value = "Số lượt";
                worksheet.Cells["C5"].Value = "Số lượng";
                worksheet.Cells["D5"].Value = "Tổng doanh thu";


                worksheet.Cells["K4:K5"].Merge = true;
                worksheet.Cells["K4"].Value = "Tổng";

                worksheet.Cells["L4:L5"].Merge = true;
                worksheet.Cells["L4"].Value = "DT hợp đồng chuyến";

                int row = 6;
                int stt = 1;
                decimal TongTien = 0;
                foreach (var item in DTTongHop)
                {
                    int col = 1;



                    worksheet.Cells[row, col].Value = item.NgayDi.ToString("dd/MM/yyyy");
                    col++;

                    worksheet.Cells[row, col].Value = item.DT_HN_ND.TongLuot;
                    col++;
                    worksheet.Cells[row, col].Value = item.DT_HN_ND.SoKhach;
                    col++;
                    worksheet.Cells[row, col].Value = item.DT_HN_ND.ThanhTien.ToString("###,###");
                    col++;






                    decimal Tong = item.DT_HN_ND.ThanhTien;
                    TongTien = TongTien + Tong;
                    worksheet.Cells[row, col].Value = Tong.ToString("###,###");
                    col++;
                    worksheet.Cells[row, col].Value = item.DTHopDongChuyen.ToString("###,###");
                    col++;
                    row++;
                    stt++;
                }
                int coltong = 1;

                worksheet.Cells[row, coltong].Value = "Tổng";
                coltong++;

                worksheet.Cells[row, coltong].Value = DTTongHop.Sum(c => c.DT_HN_ND.TongLuot);
                coltong++;
                worksheet.Cells[row, coltong].Value = DTTongHop.Sum(c => c.DT_HN_ND.SoKhach);
                coltong++;
                worksheet.Cells[row, coltong].Value = DTTongHop.Sum(c => c.DT_HN_ND.ThanhTien).ToString("###,###");
                coltong++;



                worksheet.Cells[row, coltong].Value = TongTien.ToString("###,###");
                coltong++;
                worksheet.Cells[row, coltong].Value = DTTongHop.Sum(c => c.DTHopDongChuyen).ToString("###,###");
                coltong++;
                //style header
                string modelRangeheader = "A4:L5";
                var modelTableheader = worksheet.Cells[modelRangeheader];
                modelTableheader.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                modelTableheader.Style.Font.Bold = true;
                //style body
                var modelRows = row;
                string modelRange = "A6:L" + modelRows.ToString();
                var modelTable = worksheet.Cells[modelRange];

                // Assign borders
                modelTable.Style.Border.Top.Style = ExcelBorderStyle.Dashed;
                modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.DashDot;
                modelTable.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                // Fill worksheet with data to export
                string _modelRange = "A4:L" + modelRows.ToString();
                var _modelTable = worksheet.Cells[_modelRange];

                _modelTable.AutoFitColumns();
                // we had better add some document properties to the spreadsheet 

                // set some core property values
                //var storeName = _storeInformationSettings.StoreName;
                //var storeUrl = _storeInformationSettings.StoreUrl;
                //xlPackage.Workbook.Properties.Title = string.Format("{0} customers", storeName);
                //xlPackage.Workbook.Properties.Author = storeName;
                //xlPackage.Workbook.Properties.Subject = string.Format("{0} customers", storeName);
                //xlPackage.Workbook.Properties.Keywords = string.Format("{0} customers", storeName);
                //xlPackage.Workbook.Properties.Category = "Customers";
                //xlPackage.Workbook.Properties.Comments = string.Format("{0} customers", storeName);

                // set some extended property values
                //xlPackage.Workbook.Properties.Company = storeName;
                //xlPackage.Workbook.Properties.HyperlinkBase = new Uri(storeUrl);

                // save the new spreadsheet
                xlPackage.Save();
            }
        }
        /// <summary>
        /// Export customer list to XLSX
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="customers">Customers</param>
        public virtual void ExportKhachHangToXlsx(Stream stream, List<KhachHangThongKe> dt)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            // ok, we can run the real code of the sample now
            using (var xlPackage = new ExcelPackage(stream))
            {
                // uncomment this line if you want the XML written out to the outputDir
                //xlPackage.DebugMode = true; 

                // get handle to the existing worksheet
                var worksheet = xlPackage.Workbook.Worksheets.Add("khachhang");
                //Create Headers and format them
                var properties = new[]
                    {
                           "STT",
                        "Khách Hàng",
                        "Số Điện Thoại",
                        "Địa Chỉ",
                        "SL Vé đặt",
                        "SL Vé hủy"
                    };
                for (int i = 0; i < properties.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = properties[i];
                    worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                }


                int row = 2;
                foreach (var customer in dt)
                {
                    int col = 1;
                    worksheet.Cells[row, col].Value = row-1;
                    col++;

                    worksheet.Cells[row, col].Value = customer.Ten;
                    col++;

                    worksheet.Cells[row, col].Value = customer.DienThoai;
                    col++;

                    worksheet.Cells[row, col].Value = customer.DiaChi;
                    col++;

                    worksheet.Cells[row, col].Value = customer.SoLuongDat;
                    col++;

                    worksheet.Cells[row, col].Value = customer.SoLuongHuy;
                    col++;

                    row++;
                }


                //style body
                var modelRows = row;
                string modelRange = "A1:F" + modelRows.ToString();
                var modelTable = worksheet.Cells[modelRange];

                // Assign borders
                modelTable.Style.Border.Top.Style = ExcelBorderStyle.Dashed;
                modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.DashDot;
                modelTable.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                // Fill worksheet with data to export

                modelTable.AutoFitColumns();

                // save the new spreadsheet
                xlPackage.Save();
            }
        }
    }
}

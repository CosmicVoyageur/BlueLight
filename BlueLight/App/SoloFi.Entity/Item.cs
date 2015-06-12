using System;
using System.Collections.Generic;
using XamlingCore.Portable.Model.Contract;

namespace SoloFi.Entity
{
    public class Item : IEntity
    {
        //public string TagId { get; set; }
        //public string Name { get; set; }
        //public string Description { get; set; }
        //public string PurchaseDate { get; set; }
        //[JsonProperty(PropertyName = "LocationReference")]
        //public string PlaceReference { get; set; }
        //[JsonProperty(PropertyName = "Guid")]
        //public Guid Id { get; set; }
        //public string ImageUrl { get; set; }

        #region Legacy API Item DTO

        public Guid Id { get; set; }
        // this references the Guid field in the DB, not the ID_Asset field (must add Guid field to DB)

        public string ParentItemId { get; set; }
        // Determine the parent item that this item is linked to (must add Guid field to DB)
        public string ParentItemName { get; set; }

        public string TypeId { get; set; } // This represents the Sku category for the item (must add Guid field to DB)
        public string TypeName { get; set; }

        public Guid PlaceId { get; set; } // id of location this asset is in (must add Guid field to DB)
        public string PlaceName { get; set; }

        public string PersonId { get; set; }
        // id of user/employee that is associated with this item (must add Guid field to DB)
        public string PersonName { get; set; }

        public string Name { get; set; } // Name property for the item

        public string Description { get; set; } // Description property for the item

        public string TagId { get; set; }
        // Determine the tag that this item is linked to (not implemented in legacy DB)
        public string TagNumber { get; set; } // tag EPC
        public string TagTypeName { get; set; } // display name for the type of tag

        public List<string> ImageURIs { set; get; } // array of images that belong to this object

        //        public List<ExtendedProperty> ExtendedProperties { get; set; } // not implemented in legacy DB

        public DateTime DateCreated { set; get; } // date item was created
        public string CreatedByUserId { set; get; } // id of the user who created it
        public DateTime DateLastModified { set; get; } // datetime fo creation
        public string LastModifiedByUserId { set; get; } // id of last user who modified it

        public bool? IsDeleted { get; set; } // if item is deleted
        public bool? IsActive { get; set; } // if item is active

        #endregion



        public override bool Equals(object obj)
        {
            var item = obj as Item;
            if (item==null) return base.Equals(obj);

            return item.Id == this.Id;
        }

        public static Item NewItem()
        {
            return new Item
            {
                Id = Guid.NewGuid(),
                ImageURIs = new List<string>(),
                DateCreated = DateTime.Now,
                DateLastModified = DateTime.Now,
                IsActive = true,
                IsDeleted = false
            };
        }
    }
}

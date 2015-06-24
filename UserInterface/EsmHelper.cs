using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VanLeeuwen.Framework.Linq;
using VanLeeuwen.Projects.WebPortal.DataAccess.Database;

namespace VanLeeuwen.Projects.WebPortal.UserInterface
{
  public class EsmHelper
  {
    public static IQueryable<vw_item> GetData(DALPortalDataContext portalDc, ItemFilter itemFilter)
    {
      var predicate = PredicateExtensions.True<vw_item>();

      if (!String.IsNullOrEmpty(itemFilter.size))
        predicate = predicate.And(c => c.size.Equals(itemFilter.size));
      if (!String.IsNullOrEmpty(itemFilter.companyName))
        predicate = predicate.And(c => c.companyName.Equals(itemFilter.companyName));
      if (!String.IsNullOrEmpty(itemFilter.description))
        predicate = predicate.And(c => c.description.Equals(itemFilter.description));
      if (!String.IsNullOrEmpty(itemFilter.itemGroupName))
        predicate = predicate.And(c => c.itemGroupName.Equals(itemFilter.itemGroupName));

      //

      if (!String.IsNullOrEmpty(itemFilter.outsideDiameter1))
        predicate = predicate.And(c => c.outsideDiameter1.Equals(itemFilter.outsideDiameter1));
      if (!String.IsNullOrEmpty(itemFilter.treatmentHeat))
        predicate = predicate.And(c => c.treatmentHeat.Equals(itemFilter.treatmentHeat));
      if (!String.IsNullOrEmpty(itemFilter.outsideDiameter2))
        predicate = predicate.And(c => c.outsideDiameter2.Equals(itemFilter.outsideDiameter2));
      if (!String.IsNullOrEmpty(itemFilter.treatmentSurface))
        predicate = predicate.And(c => c.treatmentSurface.Equals(itemFilter.treatmentSurface));
      if (!String.IsNullOrEmpty(itemFilter.outsideDiameter3))
        predicate = predicate.And(c => c.outsideDiameter3.Equals(itemFilter.outsideDiameter3));
      if (!String.IsNullOrEmpty(itemFilter.ends))
        predicate = predicate.And(c => c.ends.Equals(itemFilter.ends));
      if (!String.IsNullOrEmpty(itemFilter.wallThickness1))
        predicate = predicate.And(c => c.wallThickness1.Equals(itemFilter.wallThickness1));
      if (!String.IsNullOrEmpty(itemFilter.cdi))
        predicate = predicate.And(c => c.cdi.Equals(itemFilter.cdi));
      if (!String.IsNullOrEmpty(itemFilter.wallThickness2))
        predicate = predicate.And(c => c.wallThickness2.Equals(itemFilter.wallThickness2));
      if (!String.IsNullOrEmpty(itemFilter.supplier))
        predicate = predicate.And(c => c.supplier.Equals(itemFilter.supplier));
      if (!String.IsNullOrEmpty(itemFilter.type))
        predicate = predicate.And(c => c.type.Equals(itemFilter.type));
      if (!String.IsNullOrEmpty(itemFilter.other))
        predicate = predicate.And(c => c.other.Equals(itemFilter.other));
      if (!String.IsNullOrEmpty(itemFilter.specification))
        predicate = predicate.And(c => c.specification.Equals(itemFilter.specification));
      if (!String.IsNullOrEmpty(itemFilter.certificates))
        predicate = predicate.And(c => c.certificates.Equals(itemFilter.certificates));
      if (!String.IsNullOrEmpty(itemFilter.length))
        predicate = predicate.And(c => c.length.Equals(itemFilter.length));

      return portalDc.vw_items.Where(predicate);
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shippment.Models;
using System.Data;
using System.Data.Entity;
using Shippment.Common;

namespace Shippment.BLL
{
    public class PackageDefineBLL
    {
        public ErrorInfo Add(PackageDefine packageDefine)
        {
            ShippmentEntities db = new ShippmentEntities();
            
            db.PackageDefines.Add(packageDefine);
            if (db.SaveChanges() > 0) return ErrorInfo.Succeed;
            else return ErrorInfo.Err_Internal_Error;
        }
        public IQueryable<PackageDefine> GetAll()
        {
            ShippmentEntities db = new ShippmentEntities();
            IQueryable<PackageDefine> list = from p in db.PackageDefines
                                             where !p.IsDeleted
                                           select p;
            return list;
        }
        public PackageDefine Get(int id)
        {
            ShippmentEntities db = new ShippmentEntities();
            PackageDefine pack = (from p in db.PackageDefines
                                 where p.Id == id &&!p.IsDeleted
                                 select p).FirstOrDefault();
            return pack;
        }
        public IQueryable<PackageDefine> Get(string partNo)
        {
            IQueryable<PackageDefine> packageDefines = null;
            ShippmentEntities db = new ShippmentEntities();
           
            if (!string.IsNullOrEmpty(partNo))
            {
                packageDefines = from p in db.PackageDefines
                                 where p.PartNo == partNo && !p.IsDeleted
                                 select p;
            }
            return packageDefines;
        }
        public ErrorInfo Edit(int id, PackageDefine packageDefine)
        {

            if (id != packageDefine.Id)
            {
                return ErrorInfo.Err_Bad_Request_Information;
            }
            ShippmentEntities db = new ShippmentEntities();
            PackageDefine pack = db.PackageDefines.FirstOrDefault(p => p.Id == id && !p.IsDeleted);
            if (pack != null)
            {
                pack.Material = packageDefine.Material;
                pack.Length_mm = packageDefine.Length_mm;
                pack.Width_mm = packageDefine.Width_mm;
                pack.Height_mm = packageDefine.Height_mm;
                pack.NetWeight_kg = packageDefine.NetWeight_kg;
                pack.GrossWeight_kg = packageDefine.GrossWeight_kg;
                pack.Note = packageDefine.Note;
                pack.IsDeleted = packageDefine.IsDeleted;
                pack.PartNo = packageDefine.PartNo;
                pack.Unit = packageDefine.Unit;
                pack.Qty = packageDefine.Qty;
                db.Entry(pack).State = EntityState.Modified;
                if (db.SaveChanges() > 0) return ErrorInfo.Succeed;
                else return ErrorInfo.Err_Internal_Error;
            }
            else
            {
                return ErrorInfo.Err_Resource_NotExist;
            }
            
        }
        public ErrorInfo Delete(int id)
        {
            ShippmentEntities db = new ShippmentEntities();
            PackageDefine pack = db.PackageDefines.FirstOrDefault(p => p.Id == id);
            if (pack != null)
            {
                pack.IsDeleted = true;
                db.Entry(pack).State = EntityState.Modified;
                if (db.SaveChanges() > 0) return ErrorInfo.Succeed;
                else return ErrorInfo.Err_Internal_Error;
            }
            else
                return ErrorInfo.Err_Resource_NotExist;
        }
        #region Application
        public IQueryable< PackageDefine> GetPackageDefinesByCustItem(string custItemCode)
        {
            IQueryable<PackageDefine> packageDefines = null;
            ShippmentEntities db = new ShippmentEntities();
            string partNo = db.CustProducts.FirstOrDefault(p => p.CusItemCode == custItemCode && !p.IsDeleted).PartNo;
            if(!string.IsNullOrEmpty(partNo))
            {
                packageDefines = from p in db.PackageDefines
                                 where p.PartNo == partNo && !p.IsDeleted
                                 select p;
            }
            return packageDefines;
        }
        public List<FullPackagePlan> GetLoadedPackageByPallet(int palletId)
        {
            ShippmentEntities db = new ShippmentEntities();
            List<FullPackagePlan> list = new List<FullPackagePlan>();
            PalletDefine pallet = db.PalletDefines.FirstOrDefault(p => p.Id == palletId && !p.IsDeleted);
            if(pallet!=null)
            {
                
                foreach (var pp in pallet.PackagePlans)
                {
                    FullPackagePlan plan = new FullPackagePlan();
                    plan.Id = pp.PackageDefineId;
                    plan.pkgQty = pp.Qty;
                    plan.palletDefineId = pp.PalletDefineId;
                    plan.Material = pp.PackageDefine.Material;
                    plan.Length_mm = pp.PackageDefine.Length_mm;
                    plan.Width_mm = pp.PackageDefine.Width_mm;
                    plan.Height_mm = pp.PackageDefine.Height_mm;
                    plan.NetWeight_kg = pp.PackageDefine.NetWeight_kg;
                    plan.GrossWeight_kg = pp.PackageDefine.GrossWeight_kg;
                    plan.Note = pp.PackageDefine.Note;
                    plan.PartNo = pp.PackageDefine.PartNo;
                    plan.Qty = pp.PackageDefine.Qty;
                    plan.Unit = pp.PackageDefine.Unit;
                    list.Add(plan);
                }
            }
            return list;
        }
        #endregion
    }
}

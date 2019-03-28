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
    public class PackagingBLL
    {

        #region 标准CRUD
        public ErrorInfo Add(Package package)
        {
            ShippmentEntities db = new ShippmentEntities();
            db.Packages.Add(package);
            if (db.SaveChanges() > 0) return ErrorInfo.Succeed;
            else return ErrorInfo.Err_Internal_Error;
        }
        public IQueryable<Package> GetAll()
        {
            ShippmentEntities db = new ShippmentEntities();
            IQueryable<Package> list = from p in db.Packages
                                       select p;
            return list;
        }
        public Package Get(int id)
        {
            ShippmentEntities db = new ShippmentEntities();
            Package truck = (from p in db.Packages
                             where p.Id == id
                             select p).FirstOrDefault();
            return truck;
        }
        public ErrorInfo Edit(int id, Package package)
        {

            if (id != package.Id)
            {
                return ErrorInfo.Err_Bad_Request_Information;
            }
            ShippmentEntities db = new ShippmentEntities();
            Package truck = db.Packages.FirstOrDefault(p => p.Id == id && !p.IsDeleted);
            if (truck != null)
            {
                truck.GWeightReal = package.GWeightReal;
                truck.Status = package.Status;
                truck.IsDeleted = package.IsDeleted;
                truck.Boxname = package.Boxname;
                truck.SealedTime = package.SealedTime;
                truck.PackNo = package.PackNo;
                truck.Packer = package.Packer;
                truck.PackTime = package.PackTime;
                truck.PartNo = package.PartNo;
                truck.LotNo = package.LotNo;
                db.Entry(truck).State = EntityState.Modified;
                if (db.SaveChanges() > 0) return ErrorInfo.Succeed;
            }
            return ErrorInfo.Err_Internal_Error;
        }
        public ErrorInfo Delete(int id)
        {
            ShippmentEntities db = new ShippmentEntities();
            Package package = db.Packages.FirstOrDefault(p => p.Id == id);
            if (package != null)
            {
                db.Entry(package).State = EntityState.Deleted;
                if (db.SaveChanges() > 0) return ErrorInfo.Succeed;
            }
            return ErrorInfo.Err_Internal_Error;
        }
        #endregion
        #region 功能函数
        
        #endregion
    }
}

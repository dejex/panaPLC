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
    public class TruckDefineBLL
    {
        public ErrorInfo Add(TruckDefine truckDefine)
        {
            ShippmentEntities db = new ShippmentEntities();
            var trucks = (from m in db.TruckDefines
                            where m.Name == truckDefine.Name
                            select m).ToList();
            if (trucks.Count > 0) return ErrorInfo.Err_Resource_Exist;
            db.TruckDefines.Add(truckDefine);
            if (db.SaveChanges() > 0) return ErrorInfo.Succeed;
            else return ErrorInfo.Err_Internal_Error;
        }
        public IQueryable<TruckDefine> GetAll()
        {
            ShippmentEntities db = new ShippmentEntities();
            IQueryable<TruckDefine> list = from p in db.TruckDefines
                                              select p;
            return list;
        }
        public TruckDefine Get(int id)
        {
            ShippmentEntities db = new ShippmentEntities();
            TruckDefine truck = (from p in db.TruckDefines
                                  where p.Id == id
                                  select p).FirstOrDefault();
            return truck;
        }
        public ErrorInfo Edit(int id, TruckDefine truckDefine)
        {

            if (id != truckDefine.Id)
            {
                return ErrorInfo.Err_Bad_Request_Information;
            }
            ShippmentEntities db = new ShippmentEntities();
            TruckDefine truck = db.TruckDefines.FirstOrDefault(p => p.Id == id);
            if (truck != null)
            {
                truck.Name = truckDefine.Name;
                truck.Length_mm = truckDefine.Length_mm;
                truck.Width_mm = truckDefine.Width_mm;
                truck.Height_mm = truckDefine.Height_mm;
                truck.MaxLoad_T = truckDefine.MaxLoad_T;
                truck.CarriagePlans = truckDefine.CarriagePlans;
                db.Entry(truck).State = EntityState.Modified;
                if (db.SaveChanges() > 0) return ErrorInfo.Succeed;
            }
            return ErrorInfo.Err_Internal_Error;
        }
        public ErrorInfo Delete(int id)
        {
            ShippmentEntities db = new ShippmentEntities();
            TruckDefine truck = db.TruckDefines.FirstOrDefault(p => p.Id == id);
            if (truck != null)
            {
                db.Entry(truck).State = EntityState.Deleted;
                if (db.SaveChanges() > 0) return ErrorInfo.Succeed;
            }
            return ErrorInfo.Err_Internal_Error;
        }
    }
}

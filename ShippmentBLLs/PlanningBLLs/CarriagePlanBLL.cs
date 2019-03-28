using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;
using Shippment.Models;
using Shippment.Common;

namespace Shippment.BLL
{
    public class CarriagePlanBLL
    {
        public ErrorInfo Add(CarriagePlan carriagePlan)
        {
            ShippmentEntities db = new ShippmentEntities();
            db.CarriagePlans.Add(carriagePlan);
            if (db.SaveChanges() > 0) return ErrorInfo.Succeed;
            else return ErrorInfo.Err_Internal_Error;
        }
        public IQueryable<CarriagePlan> GetAll()
        {
            ShippmentEntities db = new ShippmentEntities();
            IQueryable<CarriagePlan> list = from p in db.CarriagePlans
                                          select p;
            return list;
        }
        public CarriagePlan Get(int id)
        {
            ShippmentEntities db = new ShippmentEntities();
            CarriagePlan carriage = (from p in db.CarriagePlans
                                 where p.Id == id
                                 select p).FirstOrDefault();
            return carriage;
        }
        public ErrorInfo Edit(int id, CarriagePlan carriagePlan)
        {

            if (id != carriagePlan.Id)
            {
                return ErrorInfo.Err_Bad_Request_Information;
            }
            ShippmentEntities db = new ShippmentEntities();
            CarriagePlan carriage = db.CarriagePlans.FirstOrDefault(p => p.Id == id );
            if (carriage != null)
            {
               
                //carriage.HwShipOrder = carriagePlan.HwShipOrder;//常规修改不该修改导航属性吧？
                //carriage.HwShipOrderId = carriagePlan.HwShipOrderId;//常规修改不该修改导航属性吧？
               
                carriage.TruckDefine = carriagePlan.TruckDefine;
               
                db.Entry(carriage).State = System.Data.Entity.EntityState.Modified;
                if (db.SaveChanges() > 0) return ErrorInfo.Succeed;
            }
            return ErrorInfo.Err_Internal_Error;
        }
        public ErrorInfo Delete(int id)
        {
            ShippmentEntities db = new ShippmentEntities();
            CarriagePlan carriage = db.CarriagePlans.FirstOrDefault(p => p.Id == id );
            if (carriage != null)
            {

                db.Entry(carriage).State = System.Data.Entity.EntityState.Deleted;
                if (db.SaveChanges() > 0) return ErrorInfo.Succeed;
            }
            return ErrorInfo.Err_Internal_Error;
        }
    }
}

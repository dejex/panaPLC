using Shippment.Common;
using Shippment.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shippment.BLL
{
    public class HwProductPlanBLL
    {
        public static bool Exist(HwProductPlan prodPlan)
        {
            ShippmentEntities db = new ShippmentEntities();
            var plans = (from m in db.HwProductPlans
                         where m.HwShipOrderId == prodPlan.HwShipOrderId &&
                         m.CustProduct.Id == prodPlan.CustProduct.Id
                         select m).ToList();
            return (plans.Count > 0);
        }
       
        public static ErrorInfo Add(HwProductPlan prodPlan)
        {
            if (Exist(prodPlan)) return ErrorInfo.Err_Resource_Exist;
            ShippmentEntities db = new ShippmentEntities();
            db.HwProductPlans.Add(prodPlan);
            if (db.SaveChanges() > 0) return ErrorInfo.Succeed;
            else return ErrorInfo.Err_Internal_Error;
        }
        public static IQueryable<HwProductPlan> GetAll()
        {
            ShippmentEntities db = new ShippmentEntities();
            IQueryable<HwProductPlan> list = from p in db.HwProductPlans
                                           select p;
            return list;
        }
        /// <summary>
        /// 根据HW发货订单号查找发货产品计划
        /// </summary>
        /// <param name="hwShipOrderId"></param>
        /// <returns></returns>
        public static IQueryable< HwProductPlan> Get(int hwShipOrderId)
        {
            ShippmentEntities db = new ShippmentEntities();
            var plan = from p in db.HwProductPlans
                       where p.HwShipOrderId == hwShipOrderId
                       select p;
            return plan;
        }
        public static HwProductPlan Get(int hwShipOrderId, int custProductId)
        {
            ShippmentEntities db = new ShippmentEntities();
            HwProductPlan prd = (from p in db.HwProductPlans
                                 where p.CustProductId == custProductId && p.HwShipOrderId == hwShipOrderId
                                 select p).FirstOrDefault();
            return prd;
        }
        public static ErrorInfo Edit(int id, HwProductPlan prodPlan)
        {

            if (id != prodPlan.Id)
            {
                return ErrorInfo.Err_Bad_Request_Information;
            }
            ShippmentEntities db = new ShippmentEntities();
            HwProductPlan plan = db.HwProductPlans.FirstOrDefault(p => p.Id == id);
            if (plan != null)
            {
                plan.Qty = prodPlan.Qty;
                plan.HwShipOrderId = prodPlan.HwShipOrderId;
                plan.CustProductId = prodPlan.CustProductId;
                db.Entry(plan).State = EntityState.Modified;
                if (db.SaveChanges() > 0) return ErrorInfo.Succeed;
            }
            return ErrorInfo.Err_Internal_Error;
        }
        public static ErrorInfo Delete(int id)
        {
            ShippmentEntities db = new ShippmentEntities();
            HwProductPlan plan = db.HwProductPlans.FirstOrDefault(p => p.Id == id);
            if (plan != null)
            {
                db.Entry(plan).State = EntityState.Deleted;
                if (db.SaveChanges() > 0) return ErrorInfo.Succeed;
            }
            return ErrorInfo.Err_Internal_Error;
        }
    }
}

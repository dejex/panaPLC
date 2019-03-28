using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;
using Shippment.Common;
using Shippment.Models;

namespace Shippment.BLL
{
    public class CustProductsBLL
    {
        public ErrorInfo Add(CustProduct custProduct)
        {
            
            ShippmentEntities db = new ShippmentEntities();
            db.CustProducts.Add(custProduct);
            if (db.SaveChanges() > 0) return ErrorInfo.Succeed;
            else return ErrorInfo.Err_Internal_Error;
        }
        public IQueryable<CustProduct> GetAll()
        {
            ShippmentEntities db = new ShippmentEntities();
            IQueryable<CustProduct> list = from p in db.CustProducts
                                           where !p.IsDeleted
                                           select p;
            return list;
        }
        
        public CustProduct Get(int id)
        {
            if (id == 0) return new CustProduct();
            ShippmentEntities db = new ShippmentEntities();
            CustProduct prd = (from p in db.CustProducts
                                where p.Id == id && !p.IsDeleted
                                select p).FirstOrDefault();
            return prd;
        }
        public ErrorInfo Edit(int id, CustProduct custProduct)
        {

            if (id != custProduct.Id)
            {
                return ErrorInfo.Err_Bad_Request_Information;
            }            
            ShippmentEntities db = new ShippmentEntities();           
            CustProduct prd = db.CustProducts.FirstOrDefault(p => p.Id == id && !p.IsDeleted);
            if (prd == null)
            {
                return ErrorInfo.Err_Resource_NotExist;
            }
            else
            {
                prd.CustCode = custProduct.CustCode;
                prd.Original = custProduct.Original;
                prd.CusItemCode = custProduct.CusItemCode;
                prd.CusVersion = custProduct.CusVersion;
                prd.CusModelNo = custProduct.CusModelNo;
                prd.CusDescription = custProduct.CusDescription;
                prd.IsDeleted = custProduct.IsDeleted;
                prd.PartNo = custProduct.PartNo;
                db.Entry(prd).State = EntityState.Modified;
                if (db.SaveChanges() > 0) return ErrorInfo.Succeed;
                else return ErrorInfo.Err_Internal_Error;
            }
            
        }
        public ErrorInfo Delete(int id)
        {
            ShippmentEntities db = new ShippmentEntities();
            CustProduct prd = db.CustProducts.FirstOrDefault(p => p.Id == id && !p.IsDeleted);
            if (prd == null) { return ErrorInfo.Err_Resource_NotExist; }
            prd.IsDeleted = true;
            return Edit(id, prd);
        }
        #region Application
        public string GetPartNo(string custItemCode)
        {
            ShippmentEntities db = new ShippmentEntities();
            string partno= db.CustProducts.FirstOrDefault(p => p.CusItemCode == custItemCode && !p.IsDeleted).PartNo;
            if (string.IsNullOrEmpty(partno)) partno = custItemCode;
            return partno;
        }
        public static int GetCustproductId(string custCode,string custItemCode)
        {
            ShippmentEntities db = new ShippmentEntities();
            CustProduct prd = db.CustProducts.FirstOrDefault(p => p.CustCode == custCode && p.CusItemCode == custItemCode && !p.IsDeleted);
            return prd.Id;
        }
        #endregion
    }
}

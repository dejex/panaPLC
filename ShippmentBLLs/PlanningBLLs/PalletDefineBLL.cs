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
    
    public class PalletDefineBLL
    {
        #region CRUD
        public ErrorInfo Add(PalletDefine palletDefine)
        {
            ShippmentEntities db = new ShippmentEntities();

            db.PalletDefines.Add(palletDefine);
            if (db.SaveChanges() > 0) return ErrorInfo.Succeed;
            else return ErrorInfo.Err_Internal_Error;
        }
        public IQueryable<PalletDefine> GetAll()
        {
            ShippmentEntities db = new ShippmentEntities();
            IQueryable<PalletDefine> list = from p in db.PalletDefines
                                            where !p.IsDeleted
                                            select p;
            return list;
        }
        public PalletDefine Get(int id)
        {
            ShippmentEntities db = new ShippmentEntities();
            PalletDefine pallet = (from p in db.PalletDefines
                                   where p.Id == id && !p.IsDeleted
                                   select p).FirstOrDefault();
            return pallet;
        }
        public ErrorInfo Edit(int id, PalletDefine palletDefine)
        {

            if (id != palletDefine.Id)
            {
                return ErrorInfo.Err_Bad_Request_Information;
            }
            ShippmentEntities db = new ShippmentEntities();
            PalletDefine pallet = db.PalletDefines.FirstOrDefault(p => p.Id == id && !p.IsDeleted);
            if (pallet != null)
            {
                pallet.Material = palletDefine.Material;
                pallet.Length_mm = palletDefine.Length_mm;
                pallet.Width_mm = palletDefine.Width_mm;
                pallet.Height_mm = palletDefine.Height_mm;
                pallet.NetWeight_kg = palletDefine.NetWeight_kg;
                pallet.GrossWeight_kg = palletDefine.GrossWeight_kg;
                pallet.Note = palletDefine.Note;
                pallet.IsDeleted = palletDefine.IsDeleted;
                db.Entry(pallet).State = System.Data.Entity.EntityState.Modified;
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
            PalletDefine pallet = db.PalletDefines.FirstOrDefault(p => p.Id == id);
            if (pallet != null)
            {
                pallet.IsDeleted = true;
                db.Entry(pallet).State = System.Data.Entity.EntityState.Modified;
                if (db.SaveChanges() > 0) return ErrorInfo.Succeed;
                else return ErrorInfo.Err_Internal_Error;
            }
            else
                return ErrorInfo.Err_Resource_NotExist;
        }
        #endregion
        #region Application

        /// <summary>
        /// 根据物料代码找出所有定义过的托板定义
        /// </summary>
        /// <param name="partno">内部物料代码</param>
        /// <returns></returns>
        public IQueryable< PalletDefine> GetAll(string partno)
        {
            ShippmentEntities db = new ShippmentEntities();
            var pallets = from p in db.R_PalletPackage
                          where p.PackageDefine.PartNo == partno
                          select p.PalletDefine;
            return pallets;
        }
        
        public IQueryable<PalletDefine> GetPalletDefinesByCustItem(string custItemCode)
        {
            ShippmentEntities db = new ShippmentEntities();
            string partNo = db.CustProducts.FirstOrDefault(p => p.CusItemCode == custItemCode && !p.IsDeleted).PartNo;
            return GetAll(partNo);
        }
        
        #endregion
    }
}

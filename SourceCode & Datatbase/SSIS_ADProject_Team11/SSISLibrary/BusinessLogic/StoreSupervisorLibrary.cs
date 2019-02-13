using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSISLibrary
{
    public class StoreSupervisorLibrary
    {
        private SSISDbModelContext ctx = new SSISDbModelContext();
        CommonFunctionLibrary cfl = new CommonFunctionLibrary();

        // -------------------- ADJUSTMENT VOUCHER

        public List<AdjustmentVoucher> GetAllAdjustmentVoucher()
        {
            return ctx.AdjustmentVoucher.ToList<AdjustmentVoucher>();
        }

        public AdjustmentVoucher GetAdjustmentVoucher(int id)
        {
            return ctx.AdjustmentVoucher.Where(p => p.AdjustmentVoucherID == id).FirstOrDefault();
        }

        public AdjustmentVoucher CreateAdjustmentVoucher(int discrepancyID)
        {
            if (ctx.AdjustmentVoucher.Where(p => p.DiscrepancyID == discrepancyID).FirstOrDefault() == null)
            {
                AdjustmentVoucher newAdjustmentVoucher = new AdjustmentVoucher();
                newAdjustmentVoucher.AdjustmentVoucherID = ctx.AdjustmentVoucher.OrderByDescending(p => p.AdjustmentVoucherID).FirstOrDefault().AdjustmentVoucherID + 1;
                newAdjustmentVoucher.DiscrepancyID = discrepancyID;
                newAdjustmentVoucher.AdjustmentVoucherDate = DateTime.Now;
                ctx.AdjustmentVoucher.Add(newAdjustmentVoucher);
                ctx.SaveChanges();
                return newAdjustmentVoucher;
            }
            else return null;
        }

        public List<Discrepancy> GetAdjustedDiscrepancy()
        {
            return ctx.Discrepancy.Where(p => p.DiscrepancyStatus == "Adjusted").ToList<Discrepancy>();
        }

        // -------------------- ADJUSTMENT VOUCHER DETAIL

        public List<AdjustmentVoucherDetail> GetAllAdjustmentVoucherDetails()
        {
            return ctx.AdjustmentVoucherDetail.ToList<AdjustmentVoucherDetail>();
        }

        public List<AdjustmentVoucherDetail> GetAdjustmentVoucherDetail(int id)
        {
            return ctx.AdjustmentVoucherDetail.Where(p => p.AdjustmentVoucherID == id).ToList<AdjustmentVoucherDetail>();
        }

        public List<AdjustmentVoucherDetail> CreateAdjustmentVoucherDetail(Discrepancy discrepancy) // 
        {
            List<DiscrepancyDetail> listDiscrepancyDetail = GetDiscrepancyDetail(discrepancy.DiscrepancyID);
            List<AdjustmentVoucherDetail> listAdjustmentVoucherDetail = new List<AdjustmentVoucherDetail>();
            foreach (var item in listDiscrepancyDetail)
            {
                AdjustmentVoucherDetail avDetail = new AdjustmentVoucherDetail();
                avDetail.AdjustmentVoucherID = ctx.AdjustmentVoucher.OrderByDescending(p => p.AdjustmentVoucherID).FirstOrDefault().AdjustmentVoucherID;
                avDetail.AdjustmentQuantity = (int)item.DiscrepancyQuantity;
                avDetail.InventoryID = item.InventoryID;
                listAdjustmentVoucherDetail.Add(avDetail);
            }
            return listAdjustmentVoucherDetail;
        }

        public List<AdjustmentVoucherDetail> CreateAdjustmentVoucherDetail(List<DiscrepancyDetail> listDiscrepancyDetail)
        {
            List<AdjustmentVoucherDetail> listAdjustmentVoucherDetail = new List<AdjustmentVoucherDetail>();
            if (listDiscrepancyDetail != null)
            {
                foreach (var item in listDiscrepancyDetail)
                {
                    AdjustmentVoucherDetail avDetail = new AdjustmentVoucherDetail();
                    avDetail.AdjustmentVoucherID = ctx.AdjustmentVoucher.OrderByDescending(p => p.AdjustmentVoucherID).FirstOrDefault().AdjustmentVoucherID + 1;
                    avDetail.AdjustmentQuantity = (int)item.DiscrepancyQuantity;
                    avDetail.InventoryID = item.InventoryID;
                    listAdjustmentVoucherDetail.Add(avDetail);
                }
                return listAdjustmentVoucherDetail;
            }
            else return null;
        }

        public void AdjustInventory(int id, int? adjustmentQuantity)
        {
            Inventory inventory = ctx.Inventory.Where(p => p.InventoryID == id).FirstOrDefault();
            inventory.Quantity = inventory.Quantity + adjustmentQuantity;
        }

        public void SaveAdjustmentVoucherDetail(List<AdjustmentVoucherDetail> listAdjustmentVoucherDetail, string currentUser)
        {
            AdjustmentVoucherDetail adjustmentVoucherDetail = listAdjustmentVoucherDetail.First();
            AdjustmentVoucher adjustmentVoucher = GetAdjustmentVoucher(adjustmentVoucherDetail.AdjustmentVoucherID);
            Discrepancy discrepancy = GetDiscrepancy(adjustmentVoucher.DiscrepancyID); 

            if (discrepancy.DiscrepancyStatus == "Pending")
            {
                foreach (AdjustmentVoucherDetail item in listAdjustmentVoucherDetail)
                {
                    ctx.AdjustmentVoucherDetail.Add(item);
                    AdjustInventory(item.InventoryID, item.AdjustmentQuantity);
                }

                discrepancy.DiscrepancyStatus = "Adjusted";
                ctx.SaveChanges();

                // get all store clerk emails and currentUser name
                List<SSISUser> storeClerkList = ctx.SSISUser.Where(p => p.RoleID == 3).ToList();
                List<string> storeClerkEmailList = new List<string>();
                foreach (var storeclerk in storeClerkList)
                    storeClerkEmailList.Add(storeclerk.Email);
                string currentUserName = ctx.SSISUser.Single(p => p.Email == currentUser).PersonName;
                // 

                // send email to all store clerks on inventory adjustment
                string mailSubject = "New Adjustment Voucher Issued";
                string mailBody = currentUserName + " has just issued Adjustment Voucher ID: " + adjustmentVoucher.AdjustmentVoucherID + " for Discrepancy ID: "+ adjustmentVoucher.DiscrepancyID + ".";
                foreach (string storeClerkEmail in storeClerkEmailList)
                    cfl.SendEmail(currentUser, storeClerkEmail, @"123qwe!@#QWE", mailSubject, mailBody);
            }


            
            //1. update status of discrepancy, 
            //2. update inventory quantity, 
            //3. If discrepnacy status = adjusted --> can not add new adjustment voucher.
         
            Discrepancy d = new Discrepancy();
            d.DiscrepancyStatus = "Adjusted";
            Inventory i = new Inventory();
            DiscrepancyDetail dd = new DiscrepancyDetail();
            i.Quantity = dd.ActualQuantity;
            ctx.SaveChanges();
        }

        public List<Discrepancy> UpdateDiscrepancyStatus ()
        {
            return ctx.Discrepancy.Where(p => p.DiscrepancyStatus == "Adjusted").ToList<Discrepancy>();
           
        }

        public List <Inventory> UpdateInventoryQuantity()
        {
            DiscrepancyDetail dd = new DiscrepancyDetail();
            return ctx.Inventory.Where(p => p.Quantity == dd.ActualQuantity).ToList<Inventory>();
        }



        // -------------------- DISCREPANCY

        public List<Discrepancy> GetAllDiscrepancies() // get all discrepanies
        {
            return ctx.Discrepancy.ToList<Discrepancy>();
        }

        public Discrepancy GetDiscrepancy(int? id) // get 1 discrepancy
        {
            Discrepancy d1 = ctx.Discrepancy.Where(p => p.DiscrepancyID == id).FirstOrDefault();
            return (d1);
        }

        public List<Discrepancy> GetPendingDiscrepancies() // get all pending discrepancies
        {
            return ctx.Discrepancy.Where(p => p.DiscrepancyStatus == "Pending").ToList<Discrepancy>();
        }


        // -------------------- DISCREPANCY DETAIL

        public List<DiscrepancyDetail> GetAllDiscrepancyDetail() // get all discrepancy detail for all discrepancies
        {
            return ctx.DiscrepancyDetail.ToList();
        }

        public List<DiscrepancyDetail> GetDiscrepancyDetail(int id) // get all discrepancy detail for 1 discrepancy
        {
            return ctx.DiscrepancyDetail.Where(p => p.DiscrepancyID == id).ToList();
        }


        // sum discrepancy amount (both positive and negative) for all discrepancy detail for 1 discrepancy
        public decimal SumDiscrepancyAmount(List<DiscrepancyDetail> discrepancyDetailList)
        {
            decimal a = 0, sum = 0;
            for (int i = 0; i < (discrepancyDetailList.Count); i++)
            {
                a = Convert.ToDecimal(Math.Abs((decimal)discrepancyDetailList[i].DiscrepancyAmount));
                sum = sum + a;
            }
            return sum;

        }
        public int GetUserRoleID(string email)
        {
            var query =
                from ur in ctx.SSISUser
                where ur.Email == email
                select ur.RoleID;
           
          int cts = Convert.ToInt32(query.FirstOrDefault());
            return cts ;
        }
    }
}
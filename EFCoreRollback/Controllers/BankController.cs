using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCoreRollback.Models;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreRollback.Controllers
{
    public class BankController : Controller
    {
        private readonly BankContext _bankContext;
        public BankController(BankContext context)
        {
            _bankContext = context;
        }
        /// <summary>
        /// 数据初始化
        /// </summary>
        [HttpGet]
        [Route("bangk/InitData")]
        public string InitData()
        {
            if (_bankContext.Wallets.ToList().Count == 0)
            {
                Wallet AUser = new Wallet()
                {
                    Name = "A",
                    Money = 100
                };
                Wallet BUser = new Wallet()
                {
                    Name = "B",
                    Money = 100
                };
                _bankContext.Wallets.Add(AUser);
                _bankContext.Wallets.Add(BUser);
                _bankContext.SaveChanges();
            }
            return "Success";
        }
        /// <summary>
        /// 进行转账
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("bank/TransferAccounts")]
        public string TransferAccounts()
        {
            using (var transaction = _bankContext.Database.BeginTransaction())
            {
                try
                {
                    AAction();
                    BAction();

                    //如果未执行到Commit()就执行失败遇到异常了，EF Core会自动进行数据回滚（前提是使用Using）
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // TODO: Handle failure
                    return ex.Message;
                }
            }
            return "success";
        }
        /// <summary>
        /// 从A的账户里面减掉10元
        /// </summary>
        private void AAction()
        {
            var AUser = _bankContext.Wallets.Where(u => u.Name == "A").FirstOrDefault();
            AUser.Money -= 10;
            _bankContext.SaveChanges();
        }
        /// <summary>
        /// 从B的账户里面加上10元
        /// </summary>
        private void BAction()
        {
            var BUser = _bankContext.Wallets.Where(u => u.Name == "B").FirstOrDefault();
            BUser.Money += 10;
            throw new Exception("B的数据在保存前出现异常了"); //使用该方法模拟出现数据保存异常
            _bankContext.SaveChanges();
        }

        /// <summary>
        /// 展示钱包账户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("bank/Show")]
        public List<Wallet> ShowWallets()
        {
            return _bankContext.Wallets.ToList();
        }



    }
}

using System;
using System.Web.Http;
using System.Web.Http.Cors;
using Bank.BusinessLogic;
using Bank.Commons.Concretes.Helpers;
using Bank.Commons.Concretes.Logger;
using Bank.Models.Concretes;
using BankAppWebApi.Models;
using BankAppWebApi.Results;
using Microsoft.Ajax.Utilities;

namespace BankAppWebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Transaction")]
    public class TransactionController : ApiController
    {
        [HttpPost]
        [Route("Deposit")]
        public IHttpActionResult Deposit([FromBody] TransactionRequestTemplate transactionRequest)
        {
            // Create content object
            var content = new ResponseContent<Transactions>(null);
            // Check Post  Body is okay?
            if (CheckPostBody(transactionRequest))
                try
                {
                    using (var transactionBussiness = new TransactionBusiness())
                    {
                        // Initialize a transaction and 
                        var transaction = new Transactions
                        {
                            TransactorAccountNumber = transactionRequest.SenderId,
                            ReceiverAccountNumber = transactionRequest.SenderId,
                            TransactionAmount = transactionRequest.Amount,
                            TransactionDate = DateTime.Now,
                            isSuccess = false
                        };

                        using (var customerBussiness = new CustomersBusiness())
                        {
                            // Check the transaction is successful?
                            if (transactionBussiness.DepositMoney(transaction,
                                customerBussiness.SelectCustomerById(transactionRequest.SenderId)))
                            {
                                // Return Result
                                content.Result = "1";
                                return new StandartResult<Transactions>(content, Request);
                            }
                            // Return Result
                            return new StandartResult<Transactions>(content, Request);
                        }
                    }
                }
                catch (Exception e)
                {
                    // Log the error
                    LogHelper.Log(LogTarget.File,
                        "Deposit failed: " + transactionRequest.SenderId + "." + "\n" +
                        ExceptionHelper.ExceptionToString(e));
                    // Return Result
                    return new StandartResult<Transactions>(content, Request);
                }
            // Return Result
            return new StandartResult<Transactions>(content, Request);
        }

        [HttpPost]
        [Route("Withdraw")]
        public IHttpActionResult Withdraw([FromBody] TransactionRequestTemplate transactionRequest)
        {
            // Create content
            var content = new ResponseContent<Transactions>(null);
            // Check the post body is okay?
            if (CheckPostBody(transactionRequest))
                try
                {
                    using (var transactionBussiness = new TransactionBusiness())
                    {
                        // Initialize a transaction and 
                        var transaction = new Transactions
                        {
                            TransactorAccountNumber = transactionRequest.SenderId,
                            ReceiverAccountNumber = null,
                            TransactionAmount = transactionRequest.Amount,
                            TransactionDate = DateTime.Now,
                            isSuccess = false
                        };

                        using (var customerBussiness = new CustomersBusiness())
                        {
                            // Check the transaction is successful?
                            if (transactionBussiness.WithdrawMoney(transaction,
                                customerBussiness.SelectCustomerById(transactionRequest.SenderId)))
                            {
                                // Return Result
                                content.Result = "1";
                                return new StandartResult<Transactions>(content, Request);
                            }

                            // Return Result
                            return new StandartResult<Transactions>(content, Request);
                        }
                    }
                }
                catch (Exception e)
                {
                    // Log the errors
                    LogHelper.Log(LogTarget.File,
                        "Withdraw failed: " + transactionRequest.SenderId + "." + "\n" +
                        ExceptionHelper.ExceptionToString(e));
                    // Return Result
                    return new StandartResult<Transactions>(content, Request);
                }

            // Return Result
            return new StandartResult<Transactions>(content, Request);
        }

        [HttpPost]
        [Route("Transfer")]
        public IHttpActionResult Transfer([FromBody] TransactionRequestTemplate transactionRequest)
        {
            // Create content
            var content = new ResponseContent<Transactions>(null);
            // Check the post body is okay?
            if (CheckPostBody(transactionRequest) && !transactionRequest.ReceiverId.ToString().IsNullOrWhiteSpace())
                try
                {
                    using (var transactionBussiness = new TransactionBusiness())
                    {
                        // Initialize a transaction and 
                        var transaction = new Transactions
                        {
                            TransactorAccountNumber = transactionRequest.SenderId,
                            ReceiverAccountNumber = transactionRequest.ReceiverId,
                            TransactionAmount = transactionRequest.Amount,
                            TransactionDate = DateTime.Now,
                            isSuccess = false
                        };

                        using (var customerBussiness = new CustomersBusiness())
                        {
                            // Check the transaction is successful?
                            if (transactionBussiness.MakeTransaction(transaction,
                                customerBussiness.SelectCustomerById(transactionRequest.SenderId),
                                customerBussiness.SelectCustomerById(transactionRequest.ReceiverId)))
                            {
                                // Return Result
                                content.Result = "1";
                                return new StandartResult<Transactions>(content, Request);
                            }

                            // Return Result
                            return new StandartResult<Transactions>(content, Request);
                        }
                    }
                }
                catch (Exception e)
                {
                    // Log the errors
                    LogHelper.Log(LogTarget.File,
                        "Transfer failed betweeen: " + transactionRequest.SenderId + " and " +
                        transactionRequest.ReceiverId + "." + "\n" + ExceptionHelper.ExceptionToString(e));
                    // Return Result
                    return new StandartResult<Transactions>(content, Request);
                }

            // Return Result
            return new StandartResult<Transactions>(content, Request);
        }

        private bool CheckPostBody(TransactionRequestTemplate transactionRequest)
        {
            return !transactionRequest.SenderId.ToString().IsNullOrWhiteSpace() && 
                   !transactionRequest.Amount.ToString().IsNullOrWhiteSpace();
        }
    }
}
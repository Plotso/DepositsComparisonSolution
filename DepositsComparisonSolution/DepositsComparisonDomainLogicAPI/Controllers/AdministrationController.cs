namespace DepositsComparisonDomainLogicAPI.Controllers
{
    using System;
    using System.Threading.Tasks;
    using AutoMapper;
    using DepositsComparisonDomainLogic.Contracts;
    using DepositsComparisonDomainLogic.Contracts.Models.Deposits;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Models;
    using Services.Interfaces;

    [ApiController]
    [Route("[controller]")]
    public class AdministrationController : ControllerBase
    {
        private readonly ILogger<AdministrationController> _logger;
        private readonly IMapper _mapper;
        private readonly IDepositsService _depositsService;
        private readonly IBankService _bankService;
        private readonly IInterestService _interestService;

        public AdministrationController(ILogger<AdministrationController> logger, IMapper mapper, IDepositsService depositsService, IBankService bankService, IInterestService interestService)
        {
            _logger = logger;
            _mapper = mapper;
            _depositsService = depositsService;
            _bankService = bankService;
            _interestService = interestService;
        }

        [HttpPost(nameof(CreateDeposit))]
        public async Task<CreateDepositResponse> CreateDeposit(CreateDepositRequest request)
        {
            if (!IsDepositInfoValid(request.Deposit))
            {
                return new CreateDepositResponse
                {
                    IsSuccess = false,
                    ErrorMessage =
                        "Deposit information is invalid. The name should not be empty, the bank cannot be null and the minimal amount should be greater than or equal to 0"
                };
            }

            var bankId = _bankService.GetBankIdByName(request.Deposit.Bank.Name);
            if (bankId == null)
            {
                return new CreateDepositResponse
                {
                    IsSuccess = false,
                    ErrorMessage =
                        $"We couldn't find a bank with the provided name: {request.Deposit.Bank.Name}"
                };
            }
            
            var createDepositInput = _mapper.Map<DepositCreateInputModel>(request.Deposit);
            createDepositInput.BankId = bankId;

            try
            {
                var depositId = await _depositsService.CreateAsync(createDepositInput); // ToDo: Handle potential exception
                if (string.IsNullOrEmpty(depositId))
                {
                    _logger.LogError($"DepositId cannot be null or empty");
                    return new CreateDepositResponse
                    {
                        IsSuccess = false,
                        ErrorMessage =
                            $"Internal error occured during processing of you request. Please try again later."
                    };
                }

                foreach (var interest in request.Deposit.InterestOptions)
                {
                    try
                    {
                        await _interestService.CreateAsync(interest.Months, interest.Percentage, interest.Type, depositId);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e.Message);
                        return new CreateDepositResponse
                        {
                            IsSuccess = false,
                            ErrorMessage = e.Message
                        };
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"An unexpected error occured during CreateDeposit execution");
                return new CreateDepositResponse
                {
                    IsSuccess = false,
                    ErrorMessage = e.Message
                };
            }

            // Return success if deposit and all interests are successfully inserted
            return new CreateDepositResponse
            {
                IsSuccess = true
            };
        }

        private bool IsDepositInfoValid(DepositInfo depositInfo)
            => !string.IsNullOrEmpty(depositInfo.Name) &&
               depositInfo.Bank != null &&
               depositInfo.MinAmount >= 0.0m &&
               (!depositInfo.MaxAmount.HasValue || depositInfo.MaxAmount.Value >= depositInfo.MinAmount);
    }
}
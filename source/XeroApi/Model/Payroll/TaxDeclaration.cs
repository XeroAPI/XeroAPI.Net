using System;
using XeroApi.Model.Payroll.Enums;

namespace XeroApi.Model.Payroll
{
    public class TaxDeclaration : HasUpdatedDate
    {
        public Guid EmployeeID { get; set; }

        public bool TFNPendingOrExemptionHeld { get; set; }

        public int TaxFileNumber { get; set; }
        public EmploymentBasis EmploymentBasis { get; set; }

        public bool AustralianResidentForTaxPurposes { get; set; }
        public bool TaxFreeThresholdClaimed { get; set; }

        public bool HasHELPDebt { get; set; }
        //Is this Financial Supplement Debit?
        public bool HasSFSSDebt { get; set; }
        public bool EligibleToReceiveLeaveLoading { get; set; }

        // Tax Offset Claimed        
        // Upward Variation Requested        
        // Has approved Leave Loading
        // Exempt from Flood and Cyclone Reconstruction Levy

        public decimal? ApprovedWithholdingVariationPercentage { get; set; }
        public decimal? TaxOffsetEstimatedAmount { get; set; }
        public decimal? UpwardVariationTaxWithholdingAmount { get; set; }
    }
}
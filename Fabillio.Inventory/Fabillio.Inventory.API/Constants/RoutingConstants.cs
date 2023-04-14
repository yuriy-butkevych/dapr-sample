namespace Samvirk.Loyalty.Api.Extensions;

internal static class RoutingConstants
{
    internal static class Documentation
    {
        internal const string _baseApiName = "Samvirk.Loyalty.Api";
        internal const string _technicalApiName = "Samvirk.Loyalty.Api.Technical";
        internal const string _technicalInterface = "technical-interface";
        internal const string _clientInterface = "client-interface";
    }

    internal static class Diagnostic
    {
        internal const string _base = "diagnostic";
        internal const string _liveness = "liveness";
    }

    internal static class Messages
    {
        internal const string _base = "messages";
        internal const string _membershipAccepted = "membership-accepted-message";
        internal const string _membershipBalanceChanged = "membership-balance-changed";
        internal const string _membershipTransactionSync = "membership-transaction-sync-message";

        internal const string _contactBalanceChanged = "contact-balance-changed";
        internal const string _reservationPaymentRegistered = "reservation-payment-registered";
        internal const string _contactMonthlySavingUpdated = "contact-monthly-saving-updated";
    }

    internal static class Memberships
    {
        internal const string _base = "memberships";
        internal const string _discountById = "{membershipId:guid}/discount/self";
        internal const string _plussPlan = "{membershipId:guid}/plussplan";
    }

    internal static class CronJobs
    {
        internal const string _contactBookingDiscount = "contact-booking-discount-cron";
    }

    internal static class SamvirkApi
    {
        internal const string _accountingPath = "SamvirkApis:AccountingUrl";
        internal const string _identitiesPath = "SamvirkApis:IdentitiesUrl";
        internal const string _bookingPath = "SamvirkApis:BookingUrl";
    }

    internal static class Stars
    {
        internal const string _base = "stars";
        internal const string _samvirkPlusProgress = "{membershipId:guid}/samvirk-plus-progress";
        internal const string _bonusStarsByYear = "bonus-stars/{membershipId:guid}/{year:int}";
        internal const string _currentYearBonusStars = "bonus-stars/{membershipId:guid}/current";
    }

    internal static class ManualStars
    {
        internal const string _base = "manual-stars";
        internal const string _byMembershipId = "{membershipId:guid}";
        internal const string _byMembershipIdAndManualStarsDetailsId = "{membershipId:guid}/{manualStarsDetailsId:guid}";
    }

    internal static class Campaign
    {
        internal const string _base = "campaign2023";
        internal const string _submit = "submit";
    }
}
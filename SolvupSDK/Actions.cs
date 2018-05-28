using System;

namespace SolvupSDK
{
    public class Actions
    {
        /// <summary>
        /// Get complete list of repair cases assigned to current user, except closed repairs.
        /// Also Accessible by vendor login.
        /// </summary>
        /// <param name="datestamp"></param>
        /// <returns></returns>
        static string ListRepairs(DateTime? datestamp)
        {
            if (datestamp == null)
                return Request.Get(
                    @"/api/list_repairs/");
            else
                return Request.Get(
                $@"/api/list_repairs/{(datestamp.Value):yyyy-MM-dd}");
        }

        /// <summary>
        /// Get all known information about a single current or past repair request submission.
        /// Accessible by vendor login
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        static string GetRepair(int id)
        {
            return Request.Get(
                $@"/api/get_repair/{id}");
        }

        /// <summary>
        /// Mark an item as received from store or via pickup from customer.
        /// Current Repair status must be either of the following:
        /// Repairer to collect from customer
        /// Sent to repairer
        /// Received in store (Unexpected, means store hasn't checked out). 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vendorRa"></param>
        /// <returns></returns>
        static string RepairReceived(int id, string vendorRa = "")
        {
            return Request.Post(
                ($@"<Id>{id}</Id>
{(string.IsNullOrWhiteSpace(vendorRa) ? "" : $@"<VendorRa>{vendorRa}</VendorRa>")}").Trim()
                , @"/api/repair_received");
        }

        /// <summary>
        /// Change the liability of a repair case. Use only if the current LiabilityOfRepair is incorrect. This
        /// most likely will not be necessary, but if required should be called before quoting or self approve.
        /// Current Repair status must be either of the following:
        /// Received by repairer
        /// </summary>
        /// <param name="id"></param>
        /// <param name="liability"></param>
        /// <param name="notes"></param>
        /// <returns></returns>
        static string ChangeLiability(int id, string liability, string notes)
        {
            return Request.Post(
                $@"<Id>{id}</Id>
<Liability>{liability}</Liability>
<Notes>{notes}</Notes>"
                , @"/api/change_liability");
        }

        /// <summary>
        /// Most repairers will gain authorisation for a case outside of the system. In this instance, use 'self
        /// approve' to confirm that on first investigation the item appears to qualify for repair.
        /// LiabilityOfRepair must equal vendor to complete this action (otherwise use Create Quote).
        /// Current Repair status must be either of the following:
        /// Repairer to collect from customer
        /// Resolution approved
        /// Sent to repairer
        /// Received by repairer 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="notes"></param>
        /// <param name="repairerReferenceNumber"></param>
        /// <returns></returns>
        static string SelfApprove(int id, string notes, string repairerReferenceNumber = "")
        {
            return Request.Post(
                ($@"<Id>{id}</Id>
<Notes>{notes}</Notes>
{(string.IsNullOrWhiteSpace(repairerReferenceNumber) ? "" : $"<RepairerReferenceNumber>{repairerReferenceNumber}</RepairerReferenceNumber>")}").Trim()
                , @"/api/self_approve");
        }

        /// <summary>
        /// LiabilityOfRepair must equal customer to complete this action (otherwise use Self Approve).
        /// Repair request current status must be one of either: Received by repairer | Repairer to collect from customer | Sent to repairer        
        /// </summary>
        /// <param name="id"></param>
        /// <param name="notes"></param>
        /// <param name="quoteParts"></param>
        /// <param name="quoteLabour"></param>
        /// <param name="quoteFreight"></param>
        /// <param name="repairerReferenceNumber"></param>
        /// <returns></returns>
        static string CreateQuote(int id, string notes, float quoteParts, float quoteLabour, float quoteFreight = 0, string repairerReferenceNumber = "")
        {
            return Request.Post(
                ($@"<Id>{id}</Id>
<Notes>{notes}</Notes>
<QuoteParts>{quoteParts}</QuoteParts>
<QuoteLabour>{quoteLabour}</QuoteLabour>
{(quoteFreight == 0 ? "" : $"<QuoteFreight>{quoteFreight}</QuoteFreight>")}
{(string.IsNullOrWhiteSpace(repairerReferenceNumber) ? "" : $"<RepairerReferenceNumber>{repairerReferenceNumber}</RepairerReferenceNumber>")}").Trim()
                , @"/api/create_quote");
        }

        /// <summary>
        /// After repair is 'self approved' or quote is accepted and the item is to be replaced. Use this
        /// action to mark repair request as completed.
        /// Note: If item is not to be replaced but repaired or no fault found use alternate complete functions
        /// instead.
        /// Repair Request must be in status: Resolution approved
        /// </summary>
        /// <param name="id"></param>
        /// <param name="notes"></param>
        /// <param name="faultSource"></param>
        /// <param name="faultBattery"></param>
        /// <param name="faultScreen"></param>
        /// <param name="faultFirmware"></param>
        /// <param name="faultLogicboard"></param>
        /// <param name="faultKeyboard"></param>
        /// <param name="faultSpeaker"></param>
        /// <param name="faultHdd"></param>
        /// <param name="faultPowersupply"></param>
        /// <param name="faultOtherDescription"></param>
        /// <returns></returns>
        static string CompleteOutcomeRepaired(int id, string notes, string faultSource = "",
            string faultBattery = "no",
            string faultScreen = "no",
            string faultFirmware = "no",
            string faultLogicboard = "no",
            string faultKeyboard = "no",
            string faultSpeaker = "no",
            string faultHdd = "no",
            string faultPowersupply = "no",
            string faultOtherDescription = "no")
        {
            return Request.Post(
                ($@"<Id>{id}</Id>
<Notes>{notes}</Notes>
<FaultSource>{faultSource}</FaultSource>
{(faultBattery == "no" ? "" : $"<FaultBattery>yes</FaultBattery>")}
{(faultScreen == "no" ? "" : $"<FaultScreen>yes</FaultScreen>")}
{(faultFirmware == "no" ? "" : $"<FaultFirmware>yes</FaultFirmware>")}
{(faultLogicboard == "no" ? "" : $"<FaultLogicboard>yes</FaultLogicboard>")}
{(faultKeyboard == "no" ? "" : $"<FaultKeyboard>yes</FaultKeyboard>")}
{(faultSpeaker == "no" ? "" : $"<FaultSpeaker>yes</FaultSpeaker>")}
{(faultHdd == "no" ? "" : $"<FaultHdd>yes</FaultHdd>")}
{(faultPowersupply == "no" ? "" : $"<FaultPowersupply>yes</FaultPowersupply>")}
{(faultOtherDescription == "no" ? "" : $"<FaultOtherDescription>yes</FaultOtherDescription>")}").Trim()
                , @"/api/complete_outcome_repaired");
        }

        /// <summary>
        /// After repair is 'self approved' or quote is accepted and no fault could be found with the item.
        /// Use this action to mark repair request as case completed no fault found.
        /// Note: If item is not to be returned no fault, but repaired or replaced use alternate complete
        /// functions instead.
        /// Repair Request must be in status: Resolution approved
        /// </summary>
        /// <param name="id"></param>
        /// <param name="notes"></param>
        /// <param name="replaceBy"></param>
        /// <param name="replaceWith"></param>
        /// <param name="faultSource"></param>
        /// <param name="faultBattery"></param>
        /// <param name="faultScreen"></param>
        /// <param name="faultFirmware"></param>
        /// <param name="faultLogicboard"></param>
        /// <param name="faultKeyboard"></param>
        /// <param name="faultSpeaker"></param>
        /// <param name="faultHdd"></param>
        /// <param name="faultPowersupply"></param>
        /// <param name="faultOtherDescription"></param>
        /// <returns></returns>
        static string CompleteOutcomeReplaced(int id, string notes, string replaceBy, string replaceWith, string faultSource = "",
            string faultBattery = "no",
            string faultScreen = "no",
            string faultFirmware = "no",
            string faultLogicboard = "no",
            string faultKeyboard = "no",
            string faultSpeaker = "no",
            string faultHdd = "no",
            string faultPowersupply = "no",
            string faultOtherDescription = "no")
        {
            return Request.Post(
                ($@"<Id>{id}</Id>
<Notes>{notes}</Notes>
<ReplaceBy>{replaceBy}</ReplaceBy>
<ReplaceWith>{replaceWith}</ReplaceWith>
<FaultSource>{faultSource}</FaultSource>
{(faultBattery == "no" ? "" : $"<FaultBattery>yes</FaultBattery>")}
{(faultScreen == "no" ? "" : $"<FaultScreen>yes</FaultScreen>")}
{(faultFirmware == "no" ? "" : $"<FaultFirmware>yes</FaultFirmware>")}
{(faultLogicboard == "no" ? "" : $"<FaultLogicboard>yes</FaultLogicboard>")}
{(faultKeyboard == "no" ? "" : $"<FaultKeyboard>yes</FaultKeyboard>")}
{(faultSpeaker == "no" ? "" : $"<FaultSpeaker>yes</FaultSpeaker>")}
{(faultHdd == "no" ? "" : $"<FaultHdd>yes</FaultHdd>")}
{(faultPowersupply == "no" ? "" : $"<FaultPowersupply>yes</FaultPowersupply>")}
{(faultOtherDescription == "no" ? "" : $"<FaultOtherDescription>yes</FaultOtherDescription>")}").Trim()
                , @"/api/complete_outcome_replaced");
        }

        /// <summary>
        /// After repair is 'self approved' or quote is accepted and no fault could be found with the item.
        /// Use this action to mark repair request as case completed no fault found.
        /// Note: If item is not to be returned no fault, but repaired or replaced use alternate complete
        /// functions instead.
        /// Repair Request must be in status: Resolution approved
        /// </summary>
        /// <param name="id"></param>
        /// <param name="notes"></param>
        /// <returns></returns>
        static string CompleteOutcomeNofault(int id, string notes)
        {
            return Request.Post(
                $@"<Id>{id}</Id>
<Notes>{notes}</Notes>"
                , @"/api/complete_outcome_nofault");
        }

        /// <summary>
        /// After item is updated via one of the Case Complete actions, then it must be updated as sent to 
        /// TIC Solvup API Specification v1.4.8 - Updated 03-12-2014
        /// store when actually dispatched.
        /// Repair Request must be in status: Ready for dispatch from repairer
        /// </summary>
        /// <param name="id"></param>
        /// <param name="conNote"></param>
        /// <param name="courier"></param>
        /// <param name="notes"></param>
        /// <returns></returns>
        static string SendToStore(int id, string conNote, string courier, string notes = "")
        {
            return Request.Post(
                $@"<Id>{id}</Id>
<ConNote>{conNote}</ConNote>
<Courier>{courier}</Courier>
<Notes>{notes}</Notes>"
                , @"/api/send_to_store");
        }

        /// <summary>
        /// After item is updated via one of the Case Complete actions, then it must be updated as sent to
        /// customer when actually dispatched. Note: this is only where the item was picked up from the
        /// customers premises and not from store, otherwise use Send To Store.
        /// Repair Request must be in status: Ready for dispatch from repairer
        /// </summary>
        /// <param name="id"></param>
        /// <param name="notes"></param>
        /// <returns></returns>
        static string SendToCustomer(int id, string notes = "")
        {
            return Request.Post(
                $@"<Id>{id}</Id>
<Notes>{notes}</Notes>"
                , @"/api/send_to_customer");
        }

        /// <summary>
        /// Get the quote(s) of a repair request.
        /// *Accessible by vendor login
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        static string GetQuote(int id)
        {
            return Request.Get($@"/api/get_quote/{id}");
        }

        /// <summary>
        /// Add or update the Vendor RA number to a repair Case
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vendorRa"></param>
        /// <param name="notes"></param>
        /// <returns></returns>
        static string AddRA(int id, string vendorRa, string notes)
        {
            return Request.Post(
$@"<Id>{id}</Id>
<VendorRa>{vendorRa}</VendorRa>
<Notes>{notes}</Notes>"
                , @"/api/add_ra");
        }

        /// <summary>
        /// Add or update the repairer reference number to a repair Case
        /// </summary>
        /// <param name="id"></param>
        /// <param name="repairerReferenceNumber"></param>
        /// <param name="notes"></param>
        /// <returns></returns>
        static string AddRepairerReferenceNumber(int id, string repairerReferenceNumber, string notes = "")
        {
            return Request.Post(
$@"<Id>{id}</Id>
<RepairerReferenceNumber>{repairerReferenceNumber}</RepairerReferenceNumber>
<Notes>{notes}</Notes>"
                , @"/api/add_repairer_reference_number");
        }

        /// <summary>
        /// Add custom note to a repair Case
        /// </summary>
        /// <param name="id"></param>
        /// <param name="notes"></param>
        /// <returns></returns>
        static string AddNote(int id, string notes)
        {
            return Request.Post(
$@"<Id>{id}</Id>
<Notes>{notes}</Notes>"
                , @"/api/add_note");
        }
    }
}

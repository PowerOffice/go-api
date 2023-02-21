# PowerOffice Go API

Welcome to the PowerOffice Go API.

## Documentation

Please navigate to [api.poweroffice.net/Web/docs/index.html](https://api.poweroffice.net/Web/docs/index.html) for C# and REST documentation.
A Postman collection is available [here.](https://www.postman.com/gold-sunset-645321/workspace/poweroffice-api-v1-public/overview)

## Examples

In the Examples folder you find C# examples demonstrating common tasks.

Currently we provide the following examples:

Example              | Task
:------------------- |:---------------
AuthorizationDemo    | Connect to the authorization server, and retreive authorization.
CustomerDemo         | Find a customer by Vat number, update customer and get list of customers.
GeneralLedgerAccounts| Create and query general ledger accounts on the client.
ImportDemo           | Upload and post a Payroll Journal, Sales Order Import and CustomerInvoicesImport.
JournalEntryDemo     | Create vouchers that will appear in Journal Entry on PowerOffice Go.
OutgoingInvoice      | Creating, editing and querying outgoing invoices. Creating invoices here will make the invoice appear under drafts and users in PowerOffice Go can send it to the Customer(s).
Payroll	             | Querying pay items and uploading salary lines that will be added to the next payroll.
ProjectDemo          | Creating, Editing, Deleting and Querying Projects.
ProductDemo          | Creating, Editing, Deleting and Querying Products and ProductGroups.
RecurringInvoice     | Create recurring (repeated) invoices that will be sent automatically from PowerOffice Go.
Reporting            | Listing TrialBalance at a given date, listing all transactions on a given account between two dates and printing out Customer and Supplier Ledger reports.
TimeTracking         | Query Activities, Hour types and Time tracking Entries. Creating TimeTrackingEntries.
VoucherDemo          | Creating and posting Vouchers directly into PowerOffice Go without going through the Import Service (prefered solution for voucher import).

## Support

Developer support is provided via e-mail: [go-api@poweroffice.no](mailto:go-api@poweroffice.no)

Bugs and issues can be reported using the [github issue tracker](https://github.com/PowerOffice/go-api/issues)

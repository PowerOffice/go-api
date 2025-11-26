using GoApi;
using GoApi.Common;
using GoApi.Core;
using GoApi.Core.Global;
using GoApi.Invoices;
using GoApi.Party;

namespace DebtCollectionDemo
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            await RunDemo();
        }

        /// <summary>
        /// The purpose of this demo is to show how debt collection cases can be managed through the API.
        /// This includes creating test data (customer and invoices), creating cases, adding invoices to cases,
        /// querying matched items, updating case status, and cleaning up test data.
        /// </summary>
        private static async Task RunDemo()
        {
            Customer? testCustomer = null;
            Guid? testInvoiceId1 = null;
            Guid? testInvoiceId2 = null;
            Guid? testCaseId = null;

            try
            {
                // Set up authorization settings
                var authorizationSettings = new AuthorizationSettings
                {
                    ApplicationKey = "<You Application Key Here>",
                    ClientKey = "<PowerOffice Go Client Key Here>",
                    TokenStore = new BasicInMemoryTokenStore(),
                    EndPointHost = Settings.EndPointMode.Production //For authorization against the demo environment - Change this to Settings.EndPointMode.Demo
                };

                // Initialize the PowerOffice Go API and request authorization
                var api = await Go.CreateAsync(authorizationSettings);
                
                // 1. Check if debt collection is active for the client
                Console.WriteLine("1. Checking if debt collection is active...");
                var isActive = api.DebtCollection.IsActiveDebtCollection();
                Console.WriteLine("   Debt Collection Active: " + isActive);

                if (!isActive)
                {
                    Console.WriteLine("   WARNING: Debt collection is not active for this client.");
                    Console.WriteLine("   Some operations may not work as expected.\n");
                }
                else
                {
                    Console.WriteLine();
                }

                // 2. Create a test customer
                Console.WriteLine("2. Creating test customer for demo...");
                testCustomer = new Customer
                {
                    Name = "Test Debt Collection Customer " + DateTime.Now.Ticks,
                    VatNumber = "999" + DateTime.Now.Ticks.ToString().Substring(0, 6),
                    EmailAddress = "test@debtcollection.demo",
                    // Set a valid invoice delivery type (Print used in other demos)
                    InvoiceDeliveryType = InvoiceDeliveryType.Print
                };
                testCustomer = api.Customer.Save(testCustomer);
                Console.WriteLine("   Created customer: Code=" + testCustomer.Code + ", Name=" + testCustomer.Name +
                                  "\n");

                // 3. Create test invoices that will become overdue
                Console.WriteLine("3. Creating test invoices (overdue)...");

                // First invoice - overdue
                var invoice1 = new OutgoingInvoice
                {
                    CustomerCode = testCustomer.Code,
                    OrderDate = DateTime.Now.AddDays(-60),
                    CurrencyCode = "NOK",
                    CustomerReference = "Debt Collection Test 1"
                };
                invoice1.OutgoingInvoiceLines.Add(new OutgoingInvoiceLine
                {
                    LineType = VoucherLineType.Normal,
                    Description = "Test Product 1",
                    Quantity = 2,
                    UnitPrice = 1000m,
                    ExemptVat = false,
                    SortOrder = 0
                });
                invoice1 = api.OutgoingInvoice.Save(invoice1);
                testInvoiceId1 = invoice1.Id;
                Console.WriteLine("   Created invoice 1: ID=" + invoice1.Id + ", Amount=" + invoice1.TotalAmount +
                                  " NOK");

                // Second invoice - also overdue
                var invoice2 = new OutgoingInvoice
                {
                    CustomerCode = testCustomer.Code,
                    OrderDate = DateTime.Now.AddDays(-45),
                    CurrencyCode = "NOK",
                    CustomerReference = "Debt Collection Test 2"
                };
                invoice2.OutgoingInvoiceLines.Add(new OutgoingInvoiceLine
                {
                    LineType = VoucherLineType.Normal,
                    Description = "Test Product 2",
                    Quantity = 1,
                    UnitPrice = 2500m,
                    ExemptVat = false,
                    SortOrder = 0
                });
                invoice2 = api.OutgoingInvoice.Save(invoice2);
                testInvoiceId2 = invoice2.Id;

                Console.WriteLine("   Created invoice 2: ID=" + invoice2.Id + ", Amount=" + invoice2.TotalAmount +
                                  " NOK\n");

                // 4. Send invoices to make them available for debt collection
                Console.WriteLine("4. Sending invoices to customer...");
                try
                {
                    if (testInvoiceId1.HasValue && testInvoiceId2.HasValue)
                    {
                        var srInvoice1 = new SendInvoiceRequest()
                        {
                            InvoiceId = testInvoiceId1.Value,
                            DeliveryType = SendInvoiceDeliveryType.PdfByEmail
                        };
                        api.OutgoingInvoice.SendInvoice(srInvoice1);
                        Console.WriteLine("   Sent invoice 1");
                        var srInvoice2 = new SendInvoiceRequest()
                        {
                            InvoiceId = testInvoiceId2.Value,
                            DeliveryType = SendInvoiceDeliveryType.PdfByEmail
                        };
                        api.OutgoingInvoice.SendInvoice(srInvoice2);
                        Console.WriteLine("   Sent invoice 2\n");
                    }
                }
                catch (ApiException ex)
                {
                    Console.WriteLine(
                        "   Note: Could not send invoices (may require email setup): " + ex.Message + "\n");
                }

                // 5. Get available invoices for debt collection
                Console.WriteLine("5. Getting available invoices for debt collection...");
                var availableInvoices = api.DebtCollection.GetAvailableInvoices().ToArray();
                Console.WriteLine("   Found " + availableInvoices.Length + " available invoices in total");

                var ourTestInvoices = availableInvoices.Where(inv =>
                    inv.Id == testInvoiceId1 || inv.Id == testInvoiceId2).ToArray();

                if (ourTestInvoices.Length > 0)
                {
                    Console.WriteLine("   Our test invoices are available:");
                    foreach (var invoice in ourTestInvoices)
                    {
                        Console.WriteLine("   - Invoice: " + invoice.InvoiceNo +
                                          ", Customer: " + invoice.CustomerCode +
                                          ", Remaining: " + invoice.RemainingAmount + " " + invoice.CurrencyCode +
                                          ", Due: " + invoice.DueDate.ToString("yyyy-MM-dd"));
                    }
                }
                else
                {
                    Console.WriteLine(
                        "   Note: Our test invoices may not be available yet (they may need to be processed)");
                }

                Console.WriteLine();

                // 6. Create a debt collection case (initially without invoices)
                Console.WriteLine("6. Creating a debt collection case (no invoices yet)...");
                if (testCustomer?.Code.HasValue == true)
                {
                    try
                    {
                        var newCase = new GoApi.DebtCollection.DebtCollectionCase(code: testCustomer.Code.ToString(),
                            invoices: ourTestInvoices);

                        var createdCase = api.DebtCollection.CreateCase(newCase);
                        testCaseId = createdCase.Id;

                        Console.WriteLine("   ✓ Created case: ID=" + createdCase.Id);
                        Console.WriteLine("     Code: " + createdCase.Code);
                        Console.WriteLine("     Status: " + createdCase.Status);
                        Console.WriteLine();
                    }
                    catch (ApiException ex)
                    {
                        Console.WriteLine("   ✗ Error creating case: " + ex.Message + "\n");
                    }
                }
                else
                {
                    Console.WriteLine("   Skipped - customer not created\n");
                }

                // 7. Add both invoices to the case
                if (testCaseId.HasValue && testInvoiceId1.HasValue && testInvoiceId2.HasValue)
                {
                    Console.WriteLine("7. Adding invoices to the case...");
                    try
                    {
                        var request = new GoApi.DebtCollection.DebtCollectionAddInvoicesToCaseRequest(
                            testCaseId.Value,
                            new[] { testInvoiceId1.Value, testInvoiceId2.Value }
                        );

                        var updatedCase = api.DebtCollection.AddInvoicesToCase(request);
                        Console.WriteLine("   ✓ Added invoices to case");

                        if (updatedCase.Invoices != null)
                        {
                            var invoiceCount = updatedCase.Invoices.ToList().Count;
                            Console.WriteLine("     Total invoices in case: " + invoiceCount);
                        }

                        Console.WriteLine();
                    }
                    catch (ApiException ex)
                    {
                        Console.WriteLine("   ✗ Error adding invoices: " + ex.Message + "\n");
                    }
                }

                // 8. List all debt collection cases
                Console.WriteLine("8. Listing all debt collection cases...");
                try
                {
                    var allCases = api.DebtCollection.Get().ToArray();
                    Console.WriteLine("   Found " + allCases.Length + " cases total");

                    var displayCount = Math.Min(allCases.Length, 5);
                    for (int i = 0; i < displayCount; i++)
                    {
                        var debtCase = allCases[i];
                        var invoiceCount = debtCase.Invoices != null ? debtCase.Invoices.ToList().Count : 0;
                        Console.WriteLine("   - Case: " + debtCase.Code +
                                          ", ID: " + debtCase.Id +
                                          ", Status: " + debtCase.Status +
                                          ", Invoices: " + invoiceCount);
                    }

                    Console.WriteLine();
                }
                catch (ApiException ex)
                {
                    Console.WriteLine("   Error listing cases: " + ex.Message + "\n");
                }

                // 9. Get details of our test case
                if (testCaseId.HasValue)
                {
                    Console.WriteLine("9. Getting details for our test case...");
                    try
                    {
                        var caseDetails = api.DebtCollection.Get(testCaseId.Value);
                        Console.WriteLine("   Case ID: " + caseDetails.Id);
                        Console.WriteLine("   Code: " + caseDetails.Code);
                        Console.WriteLine("   Status: " + caseDetails.Status);
                        Console.WriteLine("   Balance Agency: " + caseDetails.BalanceAgency);
                        Console.WriteLine("   Last Changed: " + caseDetails.LastChanged);

                        if (caseDetails.Invoices != null)
                        {
                            var invoiceList = caseDetails.Invoices.ToList();
                            if (invoiceList.Count > 0)
                            {
                                Console.WriteLine("   Invoices in case:");
                                foreach (var inv in invoiceList)
                                {
                                    Console.WriteLine("     - Invoice #" + inv.InvoiceNo +
                                                      ", Customer: " + inv.CustomerCode +
                                                      ", Amount: " + inv.OriginalAmount + " " + inv.CurrencyCode +
                                                      ", Remaining: " + inv.RemainingAmount);
                                }
                            }
                        }

                        Console.WriteLine();
                    }
                    catch (ApiException ex)
                    {
                        Console.WriteLine("   Error getting case details: " + ex.Message + "\n");
                    }
                }

                // 10. Test GetAllMatchedItems - this retrieves payment/credit matches
                Console.WriteLine("10. Testing GetAllMatchedItems (payments matched to invoices)...");
                try
                {
                    var fromDate = DateTimeOffset.Now.AddYears(-1);
                    Console.WriteLine("    Querying matched items from: " + fromDate.ToString("yyyy-MM-dd"));

                    var matchedItemsResult = api.DebtCollection.GetAllMatchedItems(fromDate).ToArray();
                    Console.WriteLine("    ✓ Found " + matchedItemsResult.Length + " case(s) with matches");

                    if (matchedItemsResult.Length > 0)
                    {
                        var displayCount = Math.Min(matchedItemsResult.Length, 3);
                        for (int i = 0; i < displayCount; i++)
                        {
                            var caseWithMatches = matchedItemsResult[i];
                            if (caseWithMatches.MatchedItems != null)
                            {
                                var matchList = caseWithMatches.MatchedItems.ToList();
                                if (matchList.Count > 0)
                                {
                                    Console.WriteLine("    Case with " + matchList.Count + " matched items:");
                                    var itemCount = Math.Min(matchList.Count, 3);
                                    for (int j = 0; j < itemCount; j++)
                                    {
                                        var match = matchList[j];
                                        Console.WriteLine("      - Customer: " + match.CustomerCode +
                                                          ", Voucher Date: " +
                                                          match.VoucherDate.ToString("yyyy-MM-dd") +
                                                          ", Amount: " + match.Amount +
                                                          ", Voucher Type: " + match.VoucherType);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("    No matched items found (this is normal if no payments have been made)");
                    }

                    Console.WriteLine();
                }
                catch (ApiException ex)
                {
                    Console.WriteLine("    ✗ Error getting matched items: " + ex.Message + "\n");
                }

                // 11. Update case status
                if (testCaseId.HasValue)
                {
                    Console.WriteLine("11. Updating case status...");
                    try
                    {
                        var statusRequest = new GoApi.DebtCollection.DebtCollectionStatusUpdateRequest
                        {
                            CaseId = testCaseId.Value,
                            StatusText = GoApi.DebtCollection.DebtCollectionCaseStatus.Closed.ToString()
                        };

                        api.DebtCollection.UpdateStatus(statusRequest);
                        Console.WriteLine("    ✓ Updated case status to Closed\n");
                    }
                    catch (ApiException ex)
                    {
                        Console.WriteLine("    ✗ Error updating status: " + ex.Message + "\n");
                    }
                }

                // 12. Demonstrate case merging (info only)
                Console.WriteLine("12. Case merging capability");
                Console.WriteLine("    To merge multiple cases into one, use:");
                Console.WriteLine(
                    "    var request = new DebtCollectionMergeCasesRequest(targetCaseId, sourceCaseIds);");
                Console.WriteLine("    var mergedCase = api.DebtCollection.MergeCases(request);");
                Console.WriteLine();

                Console.WriteLine("=== Demo completed successfully ===\n");
            }
            catch (ApiException apiEx)
            {
                Console.WriteLine("\n✗ API Error occurred: " + apiEx.Message);
                if (apiEx.InnerException != null)
                {
                    Console.WriteLine("   Inner exception: " + apiEx.InnerException.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n✗ Error occurred: " + ex.Message);
                Console.WriteLine("   Stack trace: " + ex.StackTrace);
            }
            finally
            {
                // Clean up - delete test data
                if (testCustomer != null || testInvoiceId1 != null || testCaseId != null)
                {
                    Console.WriteLine("=== Cleaning up test data ===");

                    try
                    {
                        var api = await Go.CreateAsync(new AuthorizationSettings
                        {
                            ApplicationKey = "<You Application Key Here>",
                            ClientKey = "<PowerOffice Go Client Key Here>",
                            TokenStore = new BasicInMemoryTokenStore(),
                            EndPointHost = Settings.EndPointMode.Production //For authorization against the demo environment - Change this to Settings.EndPointMode.Demo
                        });

                        // Delete debt collection case
                        if (testCaseId.HasValue)
                        {
                            try
                            {
                                api.DebtCollection.Delete(testCaseId.Value);
                                Console.WriteLine("✓ Deleted test debt collection case");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("✗ Could not delete case: " + ex.Message);
                            }
                        }

                        // Delete test invoices
                        if (testInvoiceId1.HasValue)
                        {
                            try
                            {
                                var inv1 = api.OutgoingInvoice.Get(testInvoiceId1.Value);
                                if (inv1 != null)
                                {
                                    api.OutgoingInvoice.Delete(inv1);
                                    Console.WriteLine("✓ Deleted test invoice 1");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("✗ Could not delete invoice 1: " + ex.Message);
                            }
                        }

                        if (testInvoiceId2.HasValue)
                        {
                            try
                            {
                                var inv2 = api.OutgoingInvoice.Get(testInvoiceId2.Value);
                                if (inv2 != null)
                                {
                                    api.OutgoingInvoice.Delete(inv2);
                                    Console.WriteLine("✓ Deleted test invoice 2");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("✗ Could not delete invoice 2: " + ex.Message);
                            }
                        }

                        // Delete test customer
                        if (testCustomer != null)
                        {
                            try
                            {
                                api.Customer.Delete(testCustomer);
                                Console.WriteLine("✓ Deleted test customer");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("✗ Could not delete customer: " + ex.Message);
                            }
                        }

                        Console.WriteLine();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error during cleanup: " + ex.Message);
                    }
                }
            }

            // Wait for user input
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}